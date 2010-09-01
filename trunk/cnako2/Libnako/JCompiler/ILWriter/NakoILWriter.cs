using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;
using Libnako.JCompiler.Node;
using Libnako.JCompiler.Parser;
using Libnako.JCompiler.Function;
using Libnako.NakoAPI;

namespace Libnako.JCompiler.ILWriter
{
    /// <summary>
    /// 構文ノードから、なでしこ仮想バイトコードを書き出すクラス
    /// </summary>
    public class NakoILWriter
    {
        protected NakoNode topNode = null;
        protected NakoILCodeList result = null;
        public NakoILCodeList Result
        {
            get { return result; }
        }
        protected Dictionary<NakoILCode, int> labels = null;


        public NakoILWriter(NakoNode topNode = null)
        {
            this.topNode = topNode;
            Init();
        }

        public void Init()
        {
            this.result = new NakoILCodeList();
            this.labels = new Dictionary<NakoILCode, int>();
        }

        protected void Write_r(NakoNode node)
        {
            if (node == null) return;
            switch (node.type)
            {
                case NakoNodeType.NOP:
                    result.Add(NakoILCode.newNop());
                    break;
                case NakoNodeType.CALC:
                    newCalc((NakoNodeCalc)node);
                    return;
                case NakoNodeType.INT:
                    result.Add(new NakoILCode(NakoILType.LD_CONST_INT, node.value));
                    return;
                case NakoNodeType.NUMBER:
                    result.Add(new NakoILCode(NakoILType.LD_CONST_REAL, node.value));
                    return;
                case NakoNodeType.STRING:
                    result.Add(new NakoILCode(NakoILType.LD_CONST_STR, node.value));
                    return;
                case NakoNodeType.PRINT:
                    _print(node);
                    return;
                case NakoNodeType.ST_VARIABLE:
                    _setVariable((NakoNodeVariable)node);
                    return;
                case NakoNodeType.LET:
                    _let((NakoNodeLet)node);
                    return;
                case NakoNodeType.LD_VARIABLE:
                    _getVariable((NakoNodeVariable)node);
                    return;
                case NakoNodeType.IF:
                    _if((NakoNodeIf)node);
                    return;
                case NakoNodeType.WHILE:
                    _while((NakoNodeWhile)node);
                    return;
                case NakoNodeType.FOR:
                    _for((NakoNodeFor)node);
                    return;
                case NakoNodeType.REPEAT_TIMES:
                    _repeat_times((NakoNodeRepeatTimes)node);
                    return;
                case NakoNodeType.CALL_FUNCTION:
                    _call_function((NakoNodeCallFunction)node);
                    return;
                case NakoNodeType.DEF_FUNCTION:
                    _def_function((NakoNodeDefFunction)node);
                    return;
            }
            // ---
            if (!node.hasChildren()) return;
            Write_list(node.Children);
        }

        public void Write(NakoNode topNode = null)
        {
            if (topNode != null) { this.topNode = topNode; }
            Write_r(this.topNode);
            FixLabel();
        }

        public void FixLabel()
        {
            // 現在のラベル位置を調べる
            for (int i = 0; i < result.Count; i++)
            {
                NakoILCode code = result[i];
                if (code.type != NakoILType.NOP) continue;
                if (labels.ContainsKey(code))
                {
                    labels[code] = i;
                }
            }
            // JUMP/BRANCH_TRUE/BRANCH_FALSE/CALL/USRCALL を解決する
            for (int i = 0; i < result.Count; i++)
            {
                NakoILCode code = result[i];
                switch (code.type)
                {
                    case NakoILType.JUMP:
                    case NakoILType.BRANCH_TRUE:
                    case NakoILType.BRANCH_FALSE:
                    case NakoILType.USRCALL:
                        break;
                    default:
                        continue;
                }
                if (!(code.value is NakoILCode)) continue;
                if (code.value is NakoILCode)
                {
                    if (labels.ContainsKey( (NakoILCode)code.value) )
                    {
                        code.value = (Object)labels[(NakoILCode)code.value];
                        continue;
                    }
                    throw new NakoILWriterException("ラベルが解決できません");
                }
            }
            
        }

        protected void Write_list(NakoNodeList list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                NakoNode node = list[i];
                Write_r(node);
            }
        }

        protected NakoILCode createLABEL(String labelName = "")
        {
            NakoILCode r = NakoILCode.newNop();
            r.value = labelName;
            labels[r] = -1;
            return r;
        }
        protected NakoILCode createJUMP(NakoILCode label)
        {
            NakoILCode r = new NakoILCode(NakoILType.JUMP, label);
            return r;
        }

        private void _if(NakoNodeIf node)
        {
            // (1) 条件文をコードにする
            Write_r(node.nodeCond);
            // (2) コードの結果により分岐する
            // 分岐先をラベルとして作成
            NakoILCode label_endif = createLABEL("ENDIF");
            NakoILCode label_else = createLABEL("ELSE");
            result.Add(new NakoILCode(NakoILType.BRANCH_FALSE, label_else));
            // (3) TRUE
            if (node.nodeTrue != null)
            {
                Write_r(node.nodeTrue);
                result.Add(createJUMP(label_endif));
            }
            // (4) FALSE
            result.Add(label_else);
            if (node.nodeFalse != null)
            {
                Write_r(node.nodeFalse);
            }
            result.Add(label_endif);
        }

        private void _while(NakoNodeWhile node)
        {
            // (1) 条件をコードにする
            NakoILCode label_while_begin = createLABEL("WHILE_BEGIN");
            result.Add(label_while_begin);
            Write_r(node.nodeCond);
            // (2) コードの結果により分岐する
            // 分岐先をラベルとして作成
            NakoILCode label_while_end = createLABEL("WHILE_END");
            addNewILCode(NakoILType.BRANCH_FALSE, label_while_end);
            // (3) ループブロックを書き込む
            Write_r(node.nodeBlocks);
            result.Add(createJUMP(label_while_begin));
            result.Add(label_while_end);
        }

        private void addNewILCode(NakoILType type, Object value = null)
        {
            result.Add(new NakoILCode(type, value));
        }

        private void _for(NakoNodeFor node)
        {
            int loopVarNo = node.loopVar.varNo;

            // (0)
            NakoILCode label_for_begin = createLABEL("FOR_BEGIN");
            NakoILCode label_for_end = createLABEL("FOR_END");

            // (1) 変数を初期化する
            result.Add(label_for_begin);
            Write_r(node.nodeFrom);
            addNewILCode(NakoILType.ST_LOCAL, loopVarNo);

            // (2) 条件をコードにする
            // i <= iTo
            NakoILCode label_for_cond = createLABEL("FOR_COND");
            result.Add(label_for_cond);
            // L
            addNewILCode(NakoILType.LD_LOCAL, loopVarNo);
            // R
            Write_r(node.nodeTo); // TODO:最適化
            // LT_EQ
            addNewILCode(NakoILType.LT_EQ);
            // IF BRANCH FALSE
            addNewILCode(NakoILType.BRANCH_FALSE, label_for_end);

            // (3) 繰り返し文を実行する
            Write_r(node.nodeBlocks);

            // (4) 変数を加算する (ここ最適化できそう)
            addNewILCode(NakoILType.LD_LOCAL, loopVarNo);
            addNewILCode(NakoILType.INC);
            addNewILCode(NakoILType.ST_LOCAL, loopVarNo);

            // (5) 手順2に戻る
            result.Add(createJUMP(label_for_cond));
            result.Add(label_for_end);
        }

        private void _repeat_times(NakoNodeRepeatTimes node)
        {
            // (1)
            int loopVarNo = node.loopVarNo;

            // (0)
            NakoILCode label_for_begin = createLABEL("TIMES_BEGIN");
            NakoILCode label_for_end = createLABEL("TIMES_END");

            // (1) 変数を初期化する
            result.Add(label_for_begin);
            addNewILCode(NakoILType.LD_CONST_INT, 1);
            addNewILCode(NakoILType.ST_LOCAL, loopVarNo);

            // (2) 条件をコードにする
            // i <= iTo
            NakoILCode label_for_cond = createLABEL("TIMES_COND");
            result.Add(label_for_cond);
            // L
            addNewILCode(NakoILType.LD_LOCAL, loopVarNo);
            // R
            Write_r(node.nodeTimes); // TODO:最適化
            // LT_EQ
            addNewILCode(NakoILType.LT_EQ);
            // IF BRANCH FALSE
            addNewILCode(NakoILType.BRANCH_FALSE, label_for_end);

            // (3) 繰り返し文を実行する
            Write_r(node.nodeBlocks);

            // (4) 変数を加算する (ここ最適化できそう)
            addNewILCode(NakoILType.LD_LOCAL, loopVarNo);
            addNewILCode(NakoILType.INC);
            addNewILCode(NakoILType.ST_LOCAL, loopVarNo);

            // (5) 手順2に戻る
            result.Add(createJUMP(label_for_cond));
            result.Add(label_for_end);
        }

        private void _let(NakoNodeLet node)
        {
            NakoNodeVariable var = node.nodeVar;
            NakoNode value = node.Children[0];
            
            // 配列要素があるか確認
            if (!var.useElement)
            {
                // + 要素なしの代入処理
                // - 代入する値を書き込んで...
                Write_r(value);
                // - セットする
                NakoILCode st = new NakoILCode();
                st.value = var.varNo;
                st.type = (var.scope == NakoVariableScope.Global)
                    ? NakoILType.ST_GLOBAL
                    : NakoILType.ST_LOCAL;
                result.Add(st);
            }
            else // 配列要素があるとき 
            {
                // + 配列への代入処理
                // - 基本となる変数をセット
                NakoILCode ldvar = new NakoILCode();
                ldvar.value = var.varNo;
                ldvar.type = (var.scope == NakoVariableScope.Global)
                    ? NakoILType.LD_GLOBAL_REF
                    : NakoILType.LD_LOCAL_REF;
                result.Add(ldvar);
                // - アクセス要素をセット
                int cnt = var.Children.Count;
                for (int i = 0; i < cnt; i++)
                {
                    NakoNode n = var.Children[i];
                    Write_r(n); // ノードの値
                    if (i != (cnt - 1))
                    {
                        result.Add(new NakoILCode(NakoILType.LD_ELEM_REF)); // 要素
                    }
                    else
                    {
                        Write_r(value);
                        addNewILCode(NakoILType.ST_ELEM);
                    }
                }
            }
        }

        private void _setVariable(NakoNodeVariable node)
        {
            // _let() で処理されるのでここでは何もしない
        }

        /// <summary>
        /// ILコードのタイプを参照型に直す
        /// </summary>
        /// <param name="c">変更したいコード</param>
        private void _varBy_change_ref(NakoILCode c)
        {
            switch (c.type)
            {
                case NakoILType.LD_LOCAL: c.type = NakoILType.LD_LOCAL_REF; break;
                case NakoILType.LD_GLOBAL: c.type = NakoILType.LD_GLOBAL_REF; break;
                case NakoILType.LD_ELEM: c.type = NakoILType.LD_ELEM_REF; break;
            }
        }


        private void _getVariable(NakoNodeVariable node)
        {
            NakoILCode ld = new NakoILCode();
            if (!node.useElement)
            {
                // + 変数アクセス
                ld.type = (node.scope == NakoVariableScope.Global)
                    ? NakoILType.LD_GLOBAL
                    : NakoILType.LD_LOCAL;
                if (node.varBy == VarByType.ByRef) _varBy_change_ref(ld);
                ld.value = node.varNo;
                result.Add(ld);
            }
            else
            {
                // + 配列変数アクセス
                // - 変数
                ld.type = (node.scope == NakoVariableScope.Global)
                    ? NakoILType.LD_GLOBAL
                    : NakoILType.LD_LOCAL;
                ld.value = node.varNo;
                result.Add(ld);
                // - 要素
                NakoNodeList c = node.Children;
                for (int i = 0; i < c.Count; i++)
                {
                    Write_r(c[i]);
                    NakoILCode code = new NakoILCode();
                    code.type = ((c.Count - 1) == i)
                        ? NakoILType.LD_ELEM
                        : NakoILType.LD_ELEM_REF;
                    result.Add(code);
                }
            }
        }

        private void _print(NakoNode node)
        {
            NakoNode v = node.Children[0];
            Write_r(v);
            result.Add(new NakoILCode(NakoILType.PRINT, null));
        }

        private void newCalc(NakoNodeCalc node)
        {
            NakoILCode c = new NakoILCode();
            // 
            Write_r(node.nodeL);
            Write_r(node.nodeR);
            //
            switch (node.calc_type)
            {
                case CalcType.NOP: c.type = NakoILType.NOP; break; // ( ... )
                case CalcType.ADD: c.type = NakoILType.ADD; break;
                case CalcType.SUB: c.type = NakoILType.SUB; break;
                case CalcType.MUL: c.type = NakoILType.MUL; break;
                case CalcType.DIV: c.type = NakoILType.DIV; break;
                case CalcType.MOD: c.type = NakoILType.MOD; break;
                case CalcType.ADD_STR: c.type = NakoILType.ADD_STR; break;
                case CalcType.POWER: c.type = NakoILType.POWER; break;
                case CalcType.EQ: c.type = NakoILType.EQ; break;
                case CalcType.NOT_EQ: c.type = NakoILType.NOT_EQ; break;
                case CalcType.GT: c.type = NakoILType.GT; break;
                case CalcType.GT_EQ: c.type = NakoILType.GT_EQ; break;
                case CalcType.LT: c.type = NakoILType.LT; break;
                case CalcType.LT_EQ: c.type = NakoILType.LT_EQ; break;
                case CalcType.AND: c.type = NakoILType.AND; break;
                case CalcType.OR: c.type = NakoILType.OR; break;
                case CalcType.XOR: c.type = NakoILType.XOR; break;
                case CalcType.NEG: c.type = NakoILType.NEG; break;
            }
            result.Add(c);
        }

        private void _call_function(NakoNodeCallFunction node)
        {
            // push args values
            for (int i = 0; i < node.argNodes.Count; i++)
            {
                Write_r(node.argNodes[i]);
            }
            NakoILCode code = new NakoILCode();
            if (node.func.funcType == Function.NakoFuncType.SysCall)
            {
                NakoAPIFunc f = (NakoAPIFunc)node.func;
                code.type = NakoILType.SYSCALL;
                code.value = node.func.varNo;
                result.Add(code);
            }
            else // UserCall
            {
                NakoILCode defLabel = ((NakoNodeDefFunction)node.value).defLabel;
                code.type = NakoILType.USRCALL;
                code.value = defLabel;
                result.Add(code);
            }
        }

        private void _def_function(NakoNodeDefFunction node)
        {
            // 必要なラベルを定義
            NakoILCode end_of_def_func = createLABEL("END_OF_DEF_FUNC");
            NakoILCode begin_def_func = createLABEL("FUNC_" + node.func.name);
            node.defLabel = begin_def_func;
            
            // 関数の定義は実行しないので、end_of_def_func へ飛ぶ
            result.Add(createJUMP(end_of_def_func));
            // 関数の始まりラベルを定義
            result.Add(begin_def_func);
            // 引数をPOPする処理
            NakoFunc func = node.func;
            for (int i = 0; i < func.ArgCount; i++)
            {
                NakoFuncArg arg = func.args[i];
                NakoILCode c = new NakoILCode(NakoILType.ST_LOCAL, i);
                result.Add(c);
            }
            // 本文を定義
            Write_r(node.funcBody);
            // 戻り値(変数「それ」)をスタックに載せる
            result.Add(new NakoILCode(NakoILType.LD_GLOBAL, (int)0));
            result.Add(new NakoILCode(NakoILType.RET));
            // 関数の終わりを定義
            result.Add(end_of_def_func);
        }
    }
    
    public class NakoILWriterException : Exception
    {
        public NakoILWriterException(String message) : base(message) { }
    }
}
