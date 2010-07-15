using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;

namespace Libnako.Parser
{
    public class NakoILWriter
    {
        protected NakoNode topNode = null;
        protected NakoILCodeList result = null;

        public NakoILWriter(NakoNode topNode)
        {
            this.topNode = topNode;
            this.result = new NakoILCodeList();
        }

        public void Write()
        {
            Write_r(topNode);

        }

        protected void Write_r(NakoNode node)
        {
            switch (node.type)
            {
                case NodeType.N_NOP:
                    result.Add(NakoILCode.newNop());
                    break;
                case NodeType.N_CALC:
                    newCalc((NakoNodeCalc)node);
                    break;
                case NodeType.N_INT:
                    result.Add(new NakoILCode(NakoILType.LD_CONST_INT, node.value));
                    break;
                case NodeType.N_NUMBER:
                    result.Add(new NakoILCode(NakoILType.LD_CONST_REAL, node.value));
                    break;
                case NodeType.N_STRING:
                    result.Add(new NakoILCode(NakoILType.LD_CONST_STR, node.value));
                    break;
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

        private void newCalc(NakoNodeCalc node)
        {
            NakoILCode c = new NakoILCode();
            switch (node.calc_type)
            {
                case CalcType.NOP: throw new Exception("NOP");
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
}
