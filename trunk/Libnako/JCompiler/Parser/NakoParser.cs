using System;
using System.Collections.Generic;
using System.Text;
using Libnako.JCompiler.Node;
using Libnako.JCompiler.Tokenizer;
using Libnako.JCompiler.Function;
using Libnako.NakoAPI;
using NakoPlugin;

namespace Libnako.JCompiler.Parser
{
    /// <summary>
    /// トークンを読み込んで構文木に変換するクラス
    /// 再帰下降構文解析 LL(1)
    /// </summary>
    public class NakoParser : NakoParserBase
    {
        public NakoParser(NakoTokenList tokens) : base(tokens)
        {
        }
        /// <summary>
        /// トークンを構文解析する
        /// </summary>
        public void Parse()
        {
            _program();
        }

        //> // ノードの値 : 構文規則 ;
        //> // 選択     => a | b
        //> // 繰り返し => { a b }
        //  // 省略可   => a [ b ] c
        //> _program : empty | _blocks ... ;
        private Boolean _program()
        {
            while (!tok.IsEOF())
            {
                if (_blocks()) continue;
                break;
            }
            return true;
        }

        //> _scope : SCOPE_BEGIN _blocks SCOPE_END ;
        private Boolean _scope()
        {
            if (!Accept(NakoTokenType.SCOPE_BEGIN)) return false;
            tok.MoveNext();
            if (!_blocks()) return false;
            if (Accept(NakoTokenType.KOKOMADE)) tok.MoveNext();
            if (!Accept(NakoTokenType.SCOPE_END))
            {
                throw new NakoParserException(
                    "トークンの終端がありません。システムエラー。", tok.CurrentToken);
            }
            tok.MoveNext(); // skip SCOPE_END
            return true;
        }

        //> _blocks : empty
        //>         | { _statement | _eol }
        //>         ;
        private Boolean _blocks()
        {
            if (tok.IsEOF()) return true;

            NakoToken tBlockTop = tok.CurrentToken;
            NakoToken t;

            while (!tok.IsEOF())
            {
                // ブロックを抜けるキーワードをチェック
                if (Accept(NakoTokenType.SCOPE_END)) return true;
                if (Accept(NakoTokenType.KOKOMADE)) return true;

                // ブロック要素を繰り返し評価
                t = tok.CurrentToken;
                if (_statement()) continue;
                if (_eol()) continue;

                throw new NakoParserException("ブロックの解析エラー", t);
            }
            return true;
        }

        //> _eol : EOL ;
        private Boolean _eol()
        {
            if (tok.IsEOF()) return false;
            if (Accept(NakoTokenType.EOL))
            {
                tok.MoveNext(); return true;
            }
            return false;
        }

        //> _statement : _let
        //>            | _if_stmt
        //>            | _white
        //>            | _for
        //>            | _repeat_times
        //>            | _def_function
        //>            | _def_variable
        //>            | _callfunc_stmt
        //>            | _print
        //>            ;
        private Boolean _statement()
        {
            if (tok.IsEOF()) return true;

            if (_let()) return true;
            if (_if_stmt()) return true;
            if (_while()) return true;
            if (_for()) return true;
            if (_repeat_times()) return true;
            if (_def_function()) return true;
            if (_def_variable()) return true;
            if (_callfunc_stmt()) return true;
            if (_print()) return true;

            // 突然の字下げも構文の１つと考える
            if (Accept(NakoTokenType.SCOPE_BEGIN))
            {
                if (_scope())
                {
                    return true;
                }
            }
            
            return false;
        }

        //> _def_variable : WORD (DIM_VARIABLE|DIM_NUMBER|DIM_INT|DIM_STRING|DIM_ARRAY) [=_value]
        //>               ;
        private Boolean _def_variable()
        {
            if (!Accept(NakoTokenType.WORD)) return false;
            NakoVarType st = NakoVarType.Object;
            switch (tok.NextTokenType)
            {
                case NakoTokenType.DIM_VARIABLE:
                    st = NakoVarType.Object;
                    break;
                case NakoTokenType.DIM_NUMBER:
                    st = NakoVarType.Double;
                    break;
                case NakoTokenType.DIM_INT:
                    st = NakoVarType.Int;
                    break;
                case NakoTokenType.DIM_STRING:
                    st = NakoVarType.String;
                    break;
                case NakoTokenType.DIM_ARRAY:
                    st = NakoVarType.Array;
                    break;
                default:
                    return false;
            }
            NakoToken t = tok.CurrentToken;
            int varNo = localVar.CreateVar(t.getValueAsName());
            NakoVariable v = localVar.GetVar(varNo);
            v.Type = st;
            tok.MoveNext(); // skip WORD
            tok.MoveNext(); // skip DIM_xxx

            // 変数の初期化があるか？
            if (!Accept(NakoTokenType.EQ)) return true; // なければ正常値として戻る
            tok.MoveNext();// skip EQ

            if (!_value())
            {
                throw new NakoParserException("変数の初期化文でエラー。", t);
            }
            // 代入文を作る
            NakoNodeLet let = new NakoNodeLet();
            let.VarNode = new NakoNodeVariable();
            let.VarNode.varNo = varNo;
            let.VarNode.scope = NakoVariableScope.Local;
            let.ValueNode.AddChild(calcStack.Pop());
            this.parentNode.AddChild(let);
            return true;
        }

        //> _scope_or_statement : _scope
        //>                     | _statement
        //>                     ;
        private NakoNode _scope_or_statement()
        {
            while (Accept(NakoTokenType.EOL)) tok.MoveNext();

            this.PushNodeState();
            NakoNode n = this.parentNode = this.lastNode = new NakoNode();
            if (Accept(NakoTokenType.SCOPE_BEGIN))
            {
                _scope();
            }
            else
            {
                _statement();
            }
            this.PopNodeState();
            return n;
        }

        //> _if_stmt : IF _value THEN [EOL] _scope_or_statement [ ELSE _scope_or_statement ]
        //>          ;
        private Boolean _if_stmt()
        {
            if (!Accept(NakoTokenType.IF)) return false;
            tok.MoveNext(); // skip IF

            NakoNodeIf ifnode = new NakoNodeIf();

            // _value
            NakoToken t = tok.CurrentToken;
            if (!_value())
            {
                throw new NakoParserException("もし文で比較式がありません。", t);
            }
            ifnode.nodeCond = calcStack.Pop();
            
            // THEN (日本語では、助詞なのでたぶんないはずだが...)
            if (Accept(NakoTokenType.THEN)) tok.MoveNext();
            while (Accept(NakoTokenType.EOL)) tok.MoveNext();

            // TRUE
            ifnode.nodeTrue = _scope_or_statement();
            while (Accept(NakoTokenType.EOL)) tok.MoveNext();

            // FALSE
            if (Accept(NakoTokenType.ELSE))
            {
                tok.MoveNext(); // skip ELSE
                while (Accept(NakoTokenType.EOL)) tok.MoveNext();
                ifnode.nodeFalse = _scope_or_statement();
            }
            this.parentNode.AddChild(ifnode);
            this.lastNode = ifnode;
            return true;
        }

        //> _while   : _value WHILE _scope_or_statement
        //>          ;
        private Boolean _while()
        {
            TokenTry();
            if (!_value())
            {
                TokenBack();
                return false;
            }
            if (!Accept(NakoTokenType.WHILE))
            {
                TokenBack();
                return false;
            }
            TokenFinally();
            tok.MoveNext();
            calcStack.Pop();
            NakoNodeWhile node_while = new NakoNodeWhile();
            
            // condition
            node_while.nodeCond = lastNode;

            // block
            node_while.nodeBlocks = _scope_or_statement();

            this.parentNode.AddChild(node_while);
            lastNode = node_while;
            return true;
        }

        //> _for     : WORD _value _value FOR _scope_or_statement
        //>          ;
        private Boolean _for()
        {
            NakoToken tokVar = null;

            TokenTry();
            
            // local variable
            if (!Accept(NakoTokenType.WORD)) return false;
            tokVar = tok.CurrentToken;
            if (!(tokVar.josi == "を" || tokVar.josi == "で")) {
                return false;
            }
            tok.MoveNext();

            // get argument * 2
            if (!_value())
            {
                TokenBack();
                return false;
            }
            if (!_value())
            {
                TokenBack();
                return false;
            }

            NakoNodeFor fornode = new NakoNodeFor();
            NakoNodeVariable v = new NakoNodeVariable();
            fornode.loopVar = v;
            v.scope = NakoVariableScope.Local;
            v.Token = tokVar;
            v.varNo = localVar.CreateVar(tokVar.value);

            fornode.nodeTo = calcStack.Pop();
            fornode.nodeFrom = calcStack.Pop();

            if (!Accept(NakoTokenType.FOR))
            {
                TokenBack();
                return false;
            }
            TokenFinally();
            tok.MoveNext();

            fornode.nodeBlocks = _scope_or_statement();
            this.parentNode.AddChild(fornode);
            lastNode = fornode;
            return true;
        }

        //> _repeat_times : _value REPEAT_TIMES _scope_or_statement
        //>               ;
        private Boolean _repeat_times()
        {
            TokenTry();
            if (!_value()) return false;
            if (!Accept(NakoTokenType.REPEAT_TIMES))
            {
                TokenBack();
                return false;
            }
            TokenFinally();
            tok.MoveNext(); // skip REPEAT_TIMES
            
            NakoNodeRepeatTimes repnode = new NakoNodeRepeatTimes();
            repnode.nodeTimes = calcStack.Pop();
            repnode.nodeBlocks = _scope_or_statement();
            repnode.loopVarNo = localVar.CreateVarNameless();

            this.parentNode.AddChild(repnode);
            lastNode = repnode;
            return true;
        }

        //> _callfunc : { [{_value}] FUNCTION_NAME }
        //>           ;
        private Boolean _callfunc_stmt()
        {
            TokenTry();
            while (!tok.IsEOF())
            {
                if (Accept(NakoTokenType.EOL))
                {
                    tok.MoveNext();
                    break;
                }
                if (!_value())
                {
                    TokenBack();
                    return false;
                }
            }
            TokenFinally();
            
            // 計算用スタックの管理
            if (calcStack.Count > 0)
            {
                while (calcStack.Count > 0)
                {
                    NakoNode n = calcStack.Shift();
                    parentNode.AddChild(n);
                    parentNode.AddChild(new NakoNodePop());
                }
            }
            // ここで本来ならば
            // スタックを空にする or 余剰なスタックがあればエラーにするのが筋だが...
            // 連続関数呼び出しに対応するためエラーにしない
            return true;
        }

        //> _arglist : '(' { _value } ')'
        //>          ;

        private Boolean _arglist(NakoNodeCallFunction node)
        {
            NakoToken firstT = tok.CurrentToken;
            int nest = 0;
            // '(' から始まるかチェック
            if (!Accept(NakoTokenType.PARENTHESES_L))
            {
                return false;
            }
            tok.MoveNext(); // skip '('
            nest++;
            
            // '(' .. ')' の間を取りだして別トークンとする
            NakoTokenList par_list = new NakoTokenList();
            while (!tok.IsEOF())
            {
                if (Accept(NakoTokenType.PARENTHESES_R))
                {
                    nest--;
                    if (nest == 0)
                    {
                        tok.MoveNext();
                        break;
                    }
                }
                else if (Accept(NakoTokenType.PARENTHESES_L))
                {
                    nest++;
                }
                par_list.Add(tok.CurrentToken);
                tok.MoveNext();
            }
            // 現在のトークン位置を保存
            tok.Save();
            NakoTokenList tmp_list = tok;
            tok = par_list;
            tok.MoveTop();
            while (!tok.IsEOF())
            {
                if (!_value())
                {
                    throw new NakoParserException("関数の引数の配置エラー。", firstT);
                }
            }
            // トークンリストを復元
            tok = tmp_list;
            tok.Restore();
            return true;
        }

        //> _callfunc : FUNCTION_NAME
        //>           | FUNCTION_NAME _arglist
        //>           ;
        private Boolean _callfunc()
        {
            NakoToken t = tok.CurrentToken;
            
            if (!Accept(NakoTokenType.FUNCTION_NAME))
            {
                return false;
            }
            tok.MoveNext(); // skip FUNCTION_NAME

            string fname = t.getValueAsName();
            NakoVariable var = globalVar.GetVar(fname);
            if (var == null)
            {
                throw new NakoParserException("関数『" + fname + "』が見あたりません。", t);
            }

            //
            NakoNodeCallFunction callNode = new NakoNodeCallFunction();
            NakoFunc func = null;
            callNode.Token = t;

            if (var.Type == NakoVarType.SystemFunc)
            {
                int funcNo = (int)var.Body;
                func = NakoAPIFuncBank.Instance.FuncList[funcNo];
                callNode.func = func;
            }
            else
            {
                NakoNodeDefFunction defNode = (NakoNodeDefFunction)var.Body;
                func = callNode.func = defNode.func;
                callNode.value = defNode;
            }

            // ---------------------------------
            if (Accept(NakoTokenType.PARENTHESES_L))
            {
                _arglist(callNode);
                // TODO ここで引数の数をチェックする処理
            }
            // 引数の数だけノードを取得
            for (int i = 0; i < func.ArgCount; i++)
            {
                NakoFuncArg arg = func.args[func.ArgCount - i - 1];
                NakoNode argNode = calcStack.Pop(arg);
                if (arg.varBy == VarByType.ByRef)
                {
                    if (argNode.type == NakoNodeType.LD_VARIABLE)
                    {
                        ((NakoNodeVariable)argNode).varBy = VarByType.ByRef;
                    }
                }
                callNode.AddChild(argNode);
            }

            // ---------------------------------
            // 計算スタックに関数の呼び出しを追加
            calcStack.Push(callNode);
            this.lastNode = callNode;

            return true;
        }

        //> _def_function : DEF_FUNCTION _def_function_args _scope
        //>               ;
        private Boolean _def_function()
        {
            if (!Accept(NakoTokenType.DEF_FUNCTION)) return false;
            NakoToken t = tok.CurrentToken;
            tok.MoveNext(); // '*'

            NakoFunc userFunc = new NakoFunc();
            userFunc.funcType = NakoFuncType.UserCall;

            // 引数 + 関数名の取得
            _def_function_args(userFunc);

            // ブロックの取得
            PushFrame();
            NakoNodeDefFunction funcNode = new NakoNodeDefFunction();
            funcNode.func = userFunc;
            parentNode = funcNode.funcBody = new NakoNode();
            funcNode.RegistArgsToLocalVar();
            localVar = funcNode.localVar;
            if (!_scope())
            {
                throw new NakoParserException("関数定義中のエラー。", t);
            }
            PopFrame();
            // グローバル変数に登録
            NakoVariable v = new NakoVariable();
            v.Type = NakoVarType.UserFunc;
            v.Body = funcNode;
            globalVar.CreateVar(userFunc.name, v);
            // 関数の宣言は、ノードのトップ直下に追加する
            if (!this.topNode.hasChildren())
            {
                this.topNode.AddChild(new NakoNode());
            }
            this.topNode.Children.Insert(0, funcNode);
            return true;
        }

        //> _def_function_args : empty
        //>                    | '(' { WORD } ')' FUNCTION_NAME
        //>                    | { WORD } FUNCTION_NAME
        //>                    | FUNCTION_NAME '(' { WORD } ')'
        //>                    ;
        private Boolean _def_function_args(NakoFunc func)
        {
            NakoToken firstT = tok.CurrentToken;
            NakoTokenList argTokens = new NakoTokenList();
            Boolean argMode = false;
            NakoToken funcName = null;

            // 関数の引数宣言を取得する
            while (!tok.IsEOF())
            {
                // '(' .. ')' の中は全部、関数の引数です
                if (argMode)
                {
                    if (Accept(NakoTokenType.PARENTHESES_R))
                    {
                        argMode = false;
                        tok.MoveNext();
                        continue;
                    }
                    argTokens.Add(tok.CurrentToken);
                    tok.MoveNext();
                    continue;
                }
                if (Accept(NakoTokenType.PARENTHESES_L))
                {
                    tok.MoveNext();
                    argMode = true;
                    continue;
                }
                if (Accept(NakoTokenType.SCOPE_BEGIN)) break;
                if (Accept(NakoTokenType.FUNCTION_NAME))
                {
                    funcName = tok.CurrentToken;
                    tok.MoveNext();
                    continue;
                }
                argTokens.Add(tok.CurrentToken);
                tok.MoveNext();
            }
            if (funcName == null) { throw new NakoParserException("関数名がありません。", firstT); }
            func.name = funcName.getValueAsName();
            func.args.analizeArgTokens(argTokens);
            return true;
        }


        //> _print : PRINT _value
        private Boolean _print()
        {
            if (tok.CurrentTokenType != NakoTokenType.PRINT)
            {
                return false;
            }
            NakoNode n = new NakoNode();
            n.Token = tok.CurrentToken;
            tok.MoveNext();
            if (!_value())
            {
                throw new NakoParserException("PRINT の後に値がありません。", n.Token);
            }
            n.type = NakoNodeType.PRINT;
            n.AddChild(calcStack.Pop());
            lastNode = n;
            this.parentNode.AddChild(n);
            return true;
        }

        //> _let : _setVariable EQ _value
        private Boolean _let()
        {
            TokenTry();
            if (!_setVariable())
            {
                TokenBack();
                return false;
            }
            if (!Accept(NakoTokenType.EQ))
            {
                TokenBack();
                return false;
            }

            NakoNodeLet node = new NakoNodeLet();
            node.VarNode = (NakoNodeVariable)lastNode; // _setVariable のノード
            tok.MoveNext();
            if (!_value())
            {
                throw new NakoParserException("代入文で値がありません。", tok.CurrentToken);
            }
            TokenFinally();

            node.ValueNode = calcStack.Pop();
            parentNode.AddChild(node);
            lastNode = node;

            return true;
        }

        private void _variable__detectVariable(NakoNodeVariable n, String name)
        {
            int varno;
            // local ?
            varno = localVar.GetIndex(name);
            if (varno >= 0)
            {
                n.scope = NakoVariableScope.Local;
                n.varNo = varno;
                return;
            }
            // global ?
            varno = globalVar.GetIndex(name);
            if (varno >= 0)
            {
                n.scope = NakoVariableScope.Global;
                n.varNo = varno;
                return;
            }
            // Create variable
            n.scope = NakoVariableScope.Global;
            n.varNo = globalVar.CreateVar(name);
        }

        //> _setVariable : WORD _variable_elements
        //>              | WORD ;
        private Boolean _setVariable()
        {
            if (!Accept(NakoTokenType.WORD)) return false;
            // 設定用変数の取得
            NakoNodeVariable n = new NakoNodeVariable();
            n.type = NakoNodeType.ST_VARIABLE;
            n.Token = tok.CurrentToken;
            // 変数アクセス
            String name = (String)tok.CurrentToken.value;
            _variable__detectVariable(n, name);
            tok.MoveNext();// skip WORD

            // 要素へのアクセスがあるか
            if (Accept(NakoTokenType.BLACKETS_L) || Accept(NakoTokenType.YEN))
            {
                // _value を読む前に setVariable のフラグをたてる
                flag_set_variable = true;
                _variable_elements(n);
                flag_set_variable = false;
            }

            lastNode = n;
            return true;
        }

        //> _variable_elements : '[' _value ']'
        //>                    | '\' _value
        //>                    ;
        private Boolean _variable_elements(NakoNodeVariable n)
        {
            NakoToken firstT = tok.CurrentToken;
            // 配列アクセス?
            if (!Accept(NakoTokenType.BLACKETS_L) && !Accept(NakoTokenType.YEN))
            {
                return false;
            }
            NakoTokenType t;
            while (!tok.IsEOF())
            {
                t = tok.CurrentTokenType;

                if (t == NakoTokenType.BLACKETS_L)
                {
                    tok.MoveNext(); // skip ']'
                    if (!_value())
                    {
                        throw new NakoParserException("変数要素へのアクセスで要素式にエラー。", firstT);
                    }
                    if (!Accept(NakoTokenType.BLACKETS_R))
                    {
                        throw new NakoParserException("変数要素へのアクセスで閉じ角カッコがありません。", firstT);
                    }
                    tok.MoveNext(); // skip ']'
                    n.AddChild(calcStack.Pop());
                }
                else // t == NakoTokenType.YEN
                {
                    tok.MoveNext(); // skip '\'
                    if (!_value(false))
                    {
                        throw new NakoParserException("変数要素へのアクセスで要素式にエラー。", firstT);
                    }
                    n.AddChild(calcStack.Pop());
                }
                // 引き続き、変数要素へのアクセスがあるかどうか
                if (Accept(NakoTokenType.BLACKETS_L)) continue;
                if (Accept(NakoTokenType.YEN)) continue;
                break;
            }
            return true;
        }


        //> _variable : WORD '[' VALUE ']'
        //>           | WORD ;
        private Boolean _variable()
        {
            if (!Accept(NakoTokenType.WORD)) return false;
            
            // 変数アクセス
            NakoNodeVariable n = new NakoNodeVariable();
            n.type = NakoNodeType.LD_VARIABLE;
            n.Token = tok.CurrentToken;

            String name = (String)tok.CurrentToken.value;
            _variable__detectVariable(n, name);
            lastNode = n;
            tok.MoveNext();

            // 要素へのアクセスがあるか
            if (Accept(NakoTokenType.BLACKETS_L) || Accept(NakoTokenType.YEN))
            {
                _variable_elements(n);
            }

            calcStack.Push(n);
            return true;
        }

        //> _value : FUNCTION_NAME | _calc_fact ;
        protected override Boolean _value(Boolean canCallJFunction = true)
        {
            // TODO:
            // _value は再帰が多くコストが高いのであり得る値だけチェックする
            switch (tok.CurrentTokenType)
            {
                case NakoTokenType.PARENTHESES_L:
                case NakoTokenType.INT:
                case NakoTokenType.NUMBER:
                case NakoTokenType.WORD:
                case NakoTokenType.STRING:
                    break;
                case NakoTokenType.FUNCTION_NAME:
                    if (_callfunc()) return true;
                    break;
                default:
                    return false;
            }
            
            // 例外的な処理
            // パーサーの副作用回避のため、
            // 配列アクセスで円マークがある時、通常の関数呼び出しはしない
            // (例) A\3を表示 → この場合、円マーク以降は明らかに関数呼び出しではない
            if (canCallJFunction)
            {
                // 関数があるか調べる
                if (tok.SearchToken(NakoTokenType.FUNCTION_NAME, true))
                {
                    TokenTry();
                    while (!tok.IsEOF())
                    {
                        if (Accept(NakoTokenType.EOL)) break;
                        if (Accept(NakoTokenType.FUNCTION_NAME))
                        {
                            if (!_callfunc())
                            {
                                TokenBack();
                                return false;
                            }
                            TokenFinally();
                            return true;
                        }
                        if (!_calc_fact())
                        {
                            TokenBack(); return false;
                        }
                    }
                }
            }            
            // 計算式を評価
            if (_calc_fact()) return true;
            return false;
        }

        //> _calc_value : _const | _variable 
        //>             ;
        private Boolean _calc_value()
        {
            if (_const()) return true;
            if (_variable()) return true;
            return false;
        }

        //> _calc_formula : PARENTHESES_L _value PARENTHESES_R 
        //>               | _calc_value
        //>               ;
        private Boolean _calc_formula()
        {
            // Check '(' *** ')'
            if (Accept(NakoTokenType.PARENTHESES_L))
            {
                TokenTry();
                tok.MoveNext();
                if (!_value())
                {
                    TokenBack();
                    return false;
                }
                if (Accept(NakoTokenType.PARENTHESES_R))
                {
                    tok.MoveNext();
                    TokenFinally();
                    return true;
                }
                TokenBack();
                return false;
            }
            return _calc_value();
        }

        //> _calc_power : _calc_formula { POWER _calc_formula }
        //>             ;
        private Boolean _calc_power()
        {
            if (!_calc_formula())
            {
                return false;
            }
            while (Accept(NakoTokenType.POWER))
            {
                TokenTry();
                tok.MoveNext();
                if (!_calc_formula())
                {
                    TokenBack();
                    return false;
                }
                TokenFinally();
                
                NakoNodeCalc node = new NakoNodeCalc();
                node.calc_type = CalcType.POWER;
                node.nodeR = calcStack.Pop();
                node.nodeL = calcStack.Pop();
                lastNode = node;
                calcStack.Push(node);
            }
            return true;
        }

        //> _calc_expr : _calc_power { (MUL|DIV|MOD) _calc_power }
        //>            ;
        private Boolean _calc_expr()
        {
            if (!_calc_power()) { return false; }
            
            while (Accept(NakoTokenType.MUL) || Accept(NakoTokenType.DIV) || Accept(NakoTokenType.MOD))
            {
                NakoNodeCalc node = new NakoNodeCalc();
                switch (tok.CurrentTokenType)
                {
                    case NakoTokenType.MUL: node.calc_type = CalcType.MUL; break;
                    case NakoTokenType.DIV: node.calc_type = CalcType.DIV; break;
                    case NakoTokenType.MOD: node.calc_type = CalcType.MOD; break;
                }
                TokenTry();
                tok.MoveNext();
                if (!_calc_power())
                {
                    TokenBack();
                    return false;
                }
                TokenFinally();
                node.nodeR = calcStack.Pop();
                node.nodeL = calcStack.Pop();
                calcStack.Push(node);
                lastNode = node;
            }
            return true;
        }

        //> _calc_term : _calc_expr { (PLUS|MINUS) _calc_expr }
        //>            ;
        private Boolean _calc_term()
        {
            if (!_calc_expr())
            {
                return false;
            }
            while (Accept(NakoTokenType.PLUS) || Accept(NakoTokenType.MINUS) || Accept(NakoTokenType.AND))
            {
                NakoNodeCalc node = new NakoNodeCalc();
                switch (tok.CurrentTokenType)
                {
                    case NakoTokenType.PLUS:
                        node.calc_type = CalcType.ADD; break;
                    case NakoTokenType.MINUS:
                        node.calc_type = CalcType.SUB; break;
                    case NakoTokenType.AND:
                        node.calc_type = CalcType.ADD_STR; break;
                    default:
                        throw new Exception("System Error");
                }
                TokenTry();
                tok.MoveNext();
                if (!_calc_expr())
                {
                    TokenBack();
                    return false;
                }
                TokenFinally();
                node.nodeR = calcStack.Pop();
                node.nodeL = calcStack.Pop();
                calcStack.Push(node);
                lastNode = node;
            }
            return true;
        }

        //> _calc_comp : _calc_term  { (GT|GT_EQ|LT|LT_EQ|EQ|EQ_EQ|NOT_EQ) _calc_term }
        //>            ;
        private Boolean _calc_comp()
        {
            if (!_calc_term())
            {
                return false;
            }

            // パーサーの例外的な処理
            // →代入文の変数取得では、比較文の "=" は無効にし、代入の "=" と見なすこと。
            if (flag_set_variable && Accept(NakoTokenType.EQ))
            {
                return true; // "="以降は読まない
            }

            while
                (
                    Accept(NakoTokenType.GT)    ||
                    Accept(NakoTokenType.LT)    ||
                    Accept(NakoTokenType.GT_EQ) ||
                    Accept(NakoTokenType.LT_EQ) ||
                    Accept(NakoTokenType.EQ)    ||
                    Accept(NakoTokenType.EQ_EQ) ||
                    Accept(NakoTokenType.NOT_EQ)
                )
            {
                NakoNodeCalc node = new NakoNodeCalc();
                switch (tok.CurrentToken.type)
                {
                    case NakoTokenType.GT: node.calc_type = CalcType.GT; break;
                    case NakoTokenType.LT: node.calc_type = CalcType.LT; break;
                    case NakoTokenType.GT_EQ: node.calc_type = CalcType.GT_EQ; break;
                    case NakoTokenType.LT_EQ: node.calc_type = CalcType.LT_EQ; break;
                    case NakoTokenType.EQ: node.calc_type = CalcType.EQ; break;
                    case NakoTokenType.EQ_EQ: node.calc_type = CalcType.EQ; break;
                    case NakoTokenType.NOT_EQ: node.calc_type = CalcType.NOT_EQ; break;
                    default:
                        throw new Exception("[Nako System Error] Operator not set.");
                }
                TokenTry();
                tok.MoveNext();
                if (!_calc_term())
                {
                    TokenBack();
                    return false;
                }
                TokenFinally();
                node.nodeR = calcStack.Pop();
                node.nodeL = calcStack.Pop();
                calcStack.Push(node);
                lastNode = node;
            }
            return true;
        }


        //> _calc_fact : MINUS  _calc_comp
        //>            | _calc_comp
        //>            ;
        private Boolean _calc_fact()
        {
            if (Accept(NakoTokenType.MINUS))
            {
                NakoNodeCalc node = new NakoNodeCalc();
                node.Token = tok.CurrentToken;
                tok.MoveNext(); // skip '-'
                if (!_calc_comp())
                {
                    throw new NakoParserException("「-」記号の後に値がありません。", tok.CurrentToken);
                }
                node.nodeL = calcStack.Pop();
                lastNode = node;
                calcStack.Push(node);
                return true;
            }
            if (_calc_comp())
            {
                return true;
            }
            return false;
        }
        
        //> _const : INT | NUMBER | STRING ;
        private Boolean _const()
        {
            NakoNodeConst node = new NakoNodeConst();
            node.Token = tok.CurrentToken;

            if (Accept(NakoTokenType.INT))
            {
                node.type = NakoNodeType.INT;
                node.value = Int64.Parse(node.Token.value);
                lastNode = node;
                tok.MoveNext();
                calcStack.Push(node);
                return true;
            }
            else if (Accept(NakoTokenType.NUMBER))
            {
                node.type = NakoNodeType.NUMBER;
                node.value = Double.Parse(node.Token.value);
                lastNode = node;
                tok.MoveNext();
                calcStack.Push(node);
                return true;
            }
            else if (Accept(NakoTokenType.STRING))
            {
                node.type = NakoNodeType.STRING;
                node.value = node.Token.value;
                lastNode = node;
                tok.MoveNext();
                calcStack.Push(node);
                return true;
            }

            return false;
        }
    }
}
