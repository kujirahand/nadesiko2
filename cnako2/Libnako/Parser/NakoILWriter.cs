using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            }
            // ---
            if (!node.hasChildren()) return;
            for (int i = 0; i < node.Children.Count; i++)
            {
                NakoNode n = node.Children[i];
                Write_r(n);
            }
        }

        private void newCalc(NakoNodeCalc node)
        {
            // TODO
            switch (node.calc_type)
            {
                case NakoNodeCalc.CalcType.Plus:
                case NakoNodeCalc.CalcType.Minus:
                case NakoNodeCalc.CalcType.Mul:
                case NakoNodeCalc.CalcType.Div:
                case NakoNodeCalc.CalcType.Mod:
                case NakoNodeCalc.CalcType.Eq:
                case NakoNodeCalc.CalcType.NotEq:
                    break;
            }
        }
    }
}
