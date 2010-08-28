using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Libnako.JCompiler.Parser;

namespace Libnako.JCompiler.Node
{
    public class NakoNodeCalc : NakoNode
    {
		public NakoNode nodeL { get; set; }
		public NakoNode nodeR { get; set; }
		public CalcType calc_type { get; set; }

        public NakoNodeCalc()
        {
            calc_type = CalcType.NOP;
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
