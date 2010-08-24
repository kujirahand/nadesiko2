using System;
using System.Collections.Generic;
using System.Text;
using Libnako.JCompiler.Node;
using Libnako.JCompiler.Tokenizer;
using Libnako.SysCall;

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
            if (!Accept(TokenType.SCOPE_BEGIN)) return false;
            tok.MoveNext();
            if (!_blocks()) return false;
            if (Accept(TokenType.KOKOMADE)) tok.MoveNext();
            if (!Accept(TokenType.SCOPE_END))
            {
                throw new NakoParserException("トークンの終端がありません。システムエラー。", tok.CurrentToken);
            }
            tok.MoveNext(); // skip SCOPE_END
            return true;
        }

        //> _blocks : empty
        //>         | _statement ... 
        //>         | _eol
        //>         ;
        private Boolean _blocks()
        {
            if (tok.IsEOF()) return true;

            NakoToken tBlockTop = tok.CurrentToken;
            NakoToken t;

            while (!tok.IsEOF())
            {
                // ブロックを抜けるキーワードをチェック
                if (Accept(TokenType.SCOPE_END)) return true;
                if (Accept(TokenType.KOKOMADE)) return true;

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
            if (Accept(TokenType.EOL))
            {
                tok.MoveNext(); return true;
            }
            return false;
        }

        //> _statement : _def_function
        //>            | _if_stmt
        //>            | _white
        //>            | _let
        //>            | _for
        //>            | _callfunc
        //>            | _repeat_times
        //>            | _print
        //>            ;
        private Boolean _statement()
        {
            if (tok.IsEOF()) return true;

            if (_def_function()) return true;
            if (_if_stmt()) return true;
            if (_while()) return true;
            if (_let()) return true;
            if (_for()) return true;
            if (_repeat_times()) return true;
            if (_callfunc()) return true;
            if (_print()) return true;
            
            return false;
        }

        // _scope_or_statement : _scope
        //                     | _statement
        //                     ;
        private NakoNode _scope_or_statement()
        {
            while (Accept(TokenType.EOL)) tok.MoveNext();

            this.PushNodeState();
            NakoNode n = this.parentNode = this.lastNode = new NakoNode();
            if (Accept(TokenType.SCOPE_BEGIN))
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
            if (!Accept(TokenType.IF)) return false;
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
            if (Accept(TokenType.THEN)) tok.MoveNext();
            while (Accept(TokenType.EOL)) tok.MoveNext();

            // TRUE
            ifnode.nodeTrue = _scope_or_statement();

            // FALSE
            if (Accept(TokenType.ELSE))
            {
                tok.MoveNext(); // skip ELSE
                while (Accept(TokenType.EOL)) tok.MoveNext();
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
            if (!Accept(TokenType.WHILE))
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
            if (!Accept(TokenType.WORD)) return false;
            tokVar = tok.CurrentToken;
            if (!(tokVar.josi == "を" || tokVar.josi == "で")) {
                return false;
            }
            tok.MoveNext();

            NakoNodeFor fornode = new NakoNodeFor();
            NakoNodeVariable v = new NakoNodeVariable();
            fornode.loopVar = v;
            v.scope = NakoVariableScope.Local;
            v.Token = tokVar;
            v.varNo = localVar.CreateVar(tokVar.value);

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

            fornode.nodeTo = calcStack.Pop();
            fornode.nodeFrom = calcStack.Pop();

            if (!Accept(TokenType.FOR))
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
            if (!Accept(TokenType.REPEAT_TIMES))
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

        //> _callfunc : _value .. FUNCTION_NAME
        //>           | FUNCTION_NAME
        //>           ;
        private Boolean _callfunc()
        {
            NakoToken t = tok.CurrentToken;
            // TODO: 関数呼び出し
            TokenTry();
            while (!tok.IsEOF())
            {
                if (tok.CurrentTokenType == TokenType.FUNCTION_NAME)
                {
                    __detect_func(tok.CurrentToken);
                    tok.MoveNext();
                    TokenFinally();
                    return true;
                }
                if (!_value()) break;

            }
            TokenBack();
            return false;
        }

        private void __detect_func(NakoToken t)
        {
            string fname = t.value;
            NakoVariable var = NakoVariables.Globals.GetVar(fname);
            if (var == null)
            {
                throw new NakoParserException("関数『" + fname + "』が見あたりません。", t);
            }
            if (var.type == NakoVariableType.SysCall)
            {
                int funcNo = (int)var.value;
                NakoSysCall sys = NakoSysCallList.Instance.list[funcNo];
                
            }
            else
            {
                // TODO
                throw new Exception("未実装");
            }
        }

        //> _def_function : DEF_FUNCTION _def_function_args FUNCTION_NAME EOL _blocks
        //>               ;
        private Boolean _def_function()
        {
            if (!Accept(TokenType.DEF_FUNCTION)) return false;
            NakoToken t = tok.CurrentToken;
            tok.MoveNext(); // '*'

            // 引数の取得
            _def_function_args();

            // 関数名
            if (!Accept(TokenType.FUNCTION_NAME))
            {
                throw new NakoParserException("関数の定義で関数名が見当たりません。", t);
            }
            tok.MoveNext(); // FUNCTION_NAME

            if (!Accept(TokenType.EOL))
            {
                throw new NakoParserException("関数の定義で改行がありません。", t);
            }
            tok.MoveNext(); // EOL

            // ブロックの取得
            PushFrame();
            NakoNodeDefFunction funcNode = new NakoNodeDefFunction();
            funcNode.type = NakoNodeType.BLOCKS;
            parentNode = funcNode;
            funcNode.RegistArgsToLocalVar();
            localVar = funcNode.localVar;
            funcList.Add(funcNode);
            if (!_blocks())
            {
                throw new NakoParserException("関数定義中のエラー。", t);
            }
            PopFrame();
            return true;
        }

        //> _def_function_args : empty
        //>                    | '(' WORD ... ')' FUNCTION_NAME _blocks
        //>                    | WORD ... FUNCTION_NAME _blocks
        //>                    ;
        private Boolean _def_function_args()
        {
            if (Accept(TokenType.PARENTHESES_L))
            {
                tok.MoveNext();
            }
            // 引数の登録
            while (!tok.IsEOF()) {
                if (Accept(TokenType.PARENTHESES_R)) {
                    tok.MoveNext();
                    break;
                }
                if (Accept(TokenType.OR))
                {
                    tok.MoveNext();
                    break;
                }
                if (Accept(TokenType.WORD))
                {
                    // TODO: 引数の登録
                    tok.MoveNext();
                    continue;
                }
                break;
            }
            return true;
        }


        //> _print : PRINT _value
        private Boolean _print()
        {
            if (tok.CurrentTokenType != TokenType.PRINT)
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
            n.AddChild(this.lastNode);
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
            if (!Accept(TokenType.EQ))
            {
                TokenBack();
                return false;
            }

            NakoNodeLet node = new NakoNodeLet();
            node.nodeVar = (NakoNodeVariable)lastNode;
            tok.MoveNext();
            if (!_value())
            {
                throw new NakoParserException("代入文で値がありません。", tok.CurrentToken);
            }
            node.AddChild(lastNode);
            parentNode.AddChild(node);
            lastNode = node;

            TokenFinally();
            calcStack.Pop();
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
            varno = NakoVariables.Globals.GetIndex(name);
            if (varno >= 0)
            {
                n.scope = NakoVariableScope.Global;
                n.varNo = varno;
                return;
            }
            // Create variable
            n.scope = NakoVariableScope.Global;
            n.varNo = NakoVariables.Globals.CreateVar(name);
        }

        //> _setVariable : WORD '[' VALUE ']'
        //>              | WORD ;
        private Boolean _setVariable()
        {
            if (!Accept(TokenType.WORD)) return false;

            // 配列アクセス
            if (tok.NextTokenType == TokenType.BLACKETS_L)
            {
                // TODO
                throw new NakoParserException("Not supported", tok.CurrentToken);
            }

            // 変数アクセス
            NakoNodeVariable n = new NakoNodeVariable();
            n.type = NakoNodeType.ST_VARIABLE;
            n.Token = tok.CurrentToken;
            String name = (String)tok.CurrentToken.value;
            _variable__detectVariable(n, name);
            lastNode = n;
            tok.MoveNext();
            return true;
        }

        //> _variable : WORD '[' VALUE ']'
        //>           | WORD ;
        private Boolean _variable()
        {
            if (!Accept(TokenType.WORD)) return false;
            
            // 配列アクセス
            if (tok.NextTokenType == TokenType.BLACKETS_L)
            {
                // TODO
                throw new NakoParserException("Not supported", tok.CurrentToken);
            }

            // 変数アクセス
            NakoNodeVariable n = new NakoNodeVariable();
            n.type = NakoNodeType.LD_VARIABLE;
            n.Token = tok.CurrentToken;

            String name = (String)tok.CurrentToken.value;
            _variable__detectVariable(n, name);
            lastNode = n;
            tok.MoveNext();
            
            calcStack.Push(n);
            return true;
        }

        //> _value : _calc_fact ;
        protected override Boolean _value()
        {
            // TODO:
            // _value は再帰が多くコストが高いのであり得る値だけチェックする
            switch (tok.CurrentTokenType)
            {
                case TokenType.PARENTHESES_L:
                case TokenType.INT:
                case TokenType.NUMBER:
                case TokenType.WORD:
                case TokenType.FUNCTION_NAME:
                case TokenType.STRING:
                    break;
                default:
                    return false;
            }

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
            if (Accept(TokenType.PARENTHESES_L))
            {
                TokenTry();
                tok.MoveNext();
                if (!_value())
                {
                    TokenBack();
                    return false;
                }
                if (Accept(TokenType.PARENTHESES_R))
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
            while (Accept(TokenType.POWER))
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
            
            while (Accept(TokenType.MUL) || Accept(TokenType.DIV) || Accept(TokenType.MOD))
            {
                NakoNodeCalc node = new NakoNodeCalc();
                switch (tok.CurrentTokenType)
                {
                    case TokenType.MUL: node.calc_type = CalcType.MUL; break;
                    case TokenType.DIV: node.calc_type = CalcType.DIV; break;
                    case TokenType.MOD: node.calc_type = CalcType.MOD; break;
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
            while (Accept(TokenType.PLUS) || Accept(TokenType.MINUS) || Accept(TokenType.AND))
            {
                NakoNodeCalc node = new NakoNodeCalc();
                switch (tok.CurrentTokenType)
                {
                    case TokenType.PLUS:
                        node.calc_type = CalcType.ADD; break;
                    case TokenType.MINUS:
                        node.calc_type = CalcType.SUB; break;
                    case TokenType.AND:
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

            while
                (
                    Accept(TokenType.GT)    ||
                    Accept(TokenType.LT)    ||
                    Accept(TokenType.GT_EQ) ||
                    Accept(TokenType.LT_EQ) ||
                    Accept(TokenType.EQ)    ||
                    Accept(TokenType.EQ_EQ) ||
                    Accept(TokenType.NOT_EQ)
                )
            {
                NakoNodeCalc node = new NakoNodeCalc();
                switch (tok.CurrentToken.type)
                {
                    case TokenType.GT: node.calc_type = CalcType.GT; break;
                    case TokenType.LT: node.calc_type = CalcType.LT; break;
                    case TokenType.GT_EQ: node.calc_type = CalcType.GT_EQ; break;
                    case TokenType.LT_EQ: node.calc_type = CalcType.LT_EQ; break;
                    case TokenType.EQ: node.calc_type = CalcType.EQ; break;
                    case TokenType.EQ_EQ: node.calc_type = CalcType.EQ; break;
                    case TokenType.NOT_EQ: node.calc_type = CalcType.NOT_EQ; break;
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
            if (Accept(TokenType.MINUS))
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

            if (Accept(TokenType.INT))
            {
                node.type = NakoNodeType.INT;
                node.value = Int32.Parse(node.Token.value);
                lastNode = node;
                tok.MoveNext();
                calcStack.Push(node);
                return true;
            }
            else if (Accept(TokenType.NUMBER))
            {
                node.type = NakoNodeType.NUMBER;
                node.value = Double.Parse(node.Token.value);
                lastNode = node;
                tok.MoveNext();
                calcStack.Push(node);
                return true;
            }
            else if (Accept(TokenType.STRING))
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
