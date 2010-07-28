using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;
using Libnako.Parser.Node;

namespace Libnako.Parser
{
    public class NakoILWriter
    {
        protected NakoNode topNode = null;
        protected NakoILCodeList result = null;
        public NakoILCodeList Result
        {
            get { return result; }
        }

        public NakoILWriter(NakoNode topNode = null)
        {
            this.topNode = topNode;
            Init();
        }

        public void Init()
        {
            this.result = new NakoILCodeList();
        }

        public void Write(NakoNode topNode = null)
        {
            if (topNode != null) { this.topNode = topNode; }
            Write_r(this.topNode);
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
            }
            // ---
            if (!node.hasChildren()) return;
            for (int i = 0; i < node.Children.Count; i++)
            {
                NakoNode n = node.Children[i];
                Write_r(n);
            }
            //
        }

        private void _if(NakoNodeIf node)
        {
            // TODO: if の実装
            // ラベルジャンプを行う
            Write_r(node.nodeCond);

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

    public class NakoILWriterExcept : Exception
    {
        public NakoILWriterExcept(String message) : base(message) { }
    }
}
