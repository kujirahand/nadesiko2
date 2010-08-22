using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;
using Libnako.JCompiler.Node;

namespace Libnako.JCompiler.ILWriter
{
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
            // JUMP/BRANCH_TRUE/BRANCH_FALSE を解決する
            for (int i = 0; i < result.Count; i++)
            {
                NakoILCode code = result[i];
                switch (code.type)
                {
                    case NakoILType.JUMP:
                    case NakoILType.BRANCH_TRUE:
                    case NakoILType.BRANCH_FALSE:
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

        protected void Write_r(NakoNode node)
        {
            if (node == null) return;
            switch (node.type)
            {
                case NodeType.NOP:
                    result.Add(NakoILCode.newNop());
                    break;
                case NodeType.CALC:
                    newCalc((NakoNodeCalc)node);
                    return;
                case NodeType.INT:
                    result.Add(new NakoILCode(NakoILType.LD_CONST_INT, node.value));
                    return;
                case NodeType.NUMBER:
                    result.Add(new NakoILCode(NakoILType.LD_CONST_REAL, node.value));
                    return;
                case NodeType.STRING:
                    result.Add(new NakoILCode(NakoILType.LD_CONST_STR, node.value));
                    return;
                case NodeType.PRINT:
                    _print(node);
                    return;
                case NodeType.ST_VARIABLE:
                    _setVariable((NakoNodeVariable)node);
                    return;
                case NodeType.LET:
                    _let((NakoNodeLet)node);
                    return;
                case NodeType.LD_VARIABLE:
                    _getVariable((NakoNodeVariable)node);
                    return;
                case NodeType.IF:
                    _if((NakoNodeIf)node);
                    return;
                case NodeType.WHILE:
                    _while((NakoNodeWhile)node);
                    return;
            }
            // ---
            if (!node.hasChildren()) return;
            Write_list(node.Children);
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
            result.Add(new NakoILCode(NakoILType.BRANCH_FALSE, label_while_end));
            // (3) ループブロックを書き込む
            Write_r(node.nodeBlocks);
            result.Add(createJUMP(label_while_begin));
            result.Add(label_while_end);
        }

        private void _let(NakoNodeLet node)
        {
            NakoNodeVariable var = node.nodeVar;
            NakoNode value = node.Children[0];
            NakoILCode st = new NakoILCode();

            if (var.useElement)
            {
                // TODO: 配列アクセス
            }
            else
            {
                Write_r(value);
                if (var.scope == NakoVariableScope.Global)
                {
                    st.type = NakoILType.ST_GLOBAL;
                }
                else
                {
                    st.type = NakoILType.ST_LOCAL;
                }
                st.value = var.varNo;
                result.Add(st);
            }
        }

        private void _setVariable(NakoNodeVariable node)
        {
            // _let() で処理されるのでここでは何もしない
        }

        private void _getVariable(NakoNodeVariable node)
        {
            NakoILCode ld = new NakoILCode();
            if (node.useElement)
            {
                // TODO: 配列アクセス
            }
            else
            {
                if (node.scope == NakoVariableScope.Global)
                {
                    ld.type = NakoILType.LD_GLOBAL;
                }
                else
                {
                    ld.type = NakoILType.LD_LOCAL;
                }
                ld.value = node.varNo;
                result.Add(ld);
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
    }

    public class NakoILWriterException : Exception
    {
        public NakoILWriterException(String message) : base(message) { }
    }
}
