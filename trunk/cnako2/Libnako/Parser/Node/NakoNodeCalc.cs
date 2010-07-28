using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Libnako.Parser.Node
{
    public class NakoNodeCalc : NakoNode
    {
        public NakoNode nodeL;
        public NakoNode nodeR;
        public int calc_type = CalcType.NOP;

        public NakoNodeCalc()
        {
            type = NodeType.CALC;
        }

        public override String ToTypeString()
        {
            String r = "";
            r += "(";
            r += base.ToTypeString() + ":";
            r += CalcType.GetTypeName((int)calc_type) + " ";
            if (nodeL != null)
            {
                r += "(";
                r += nodeL.ToTypeString();
                r += ")";
            }
            if (nodeR != null)
            {
                r += " (";
                r += nodeR.ToTypeString();
                r += ")";
            }
            r += ")";
            return r;
        }
    }
}
