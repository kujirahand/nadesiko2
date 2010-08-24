using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Libnako.JCompiler.Parser;

namespace Libnako.JCompiler.Node
{
    public class NakoNodeCalc : NakoNode
    {
        public NakoNode nodeL;
        public NakoNode nodeR;
        public CalcType calc_type = CalcType.NOP;

        public NakoNodeCalc()
        {
            type = NakoNodeType.CALC;
        }

        public override String ToTypeString()
        {
            String r = "";
            r += "(";
            r += base.ToTypeString() + ":";
            r += calc_type.ToString() + " ";
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
