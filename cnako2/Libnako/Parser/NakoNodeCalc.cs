using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Libnako.Parser
{
    public class NakoNodeCalc : NakoNode
    {
        public enum CalcType { 
            Nop = 0, Plus, Minus, Mul, Div, Mod, Power, Neg,
            Eq, NotEq, Gt, GtEq, Lt, LtEq
        };
        public static String[] CalcTypeName = new String[] {
            "Nop", "Plus", "Minus", "Mul", "Div", "Mod", "Power", "Neg",
            "Eq","NotEq", "Gt", "GtEq", "Lt", "LtEq"
        };

        public NakoNode nodeL;
        public NakoNode nodeR;
        public CalcType calc_type = CalcType.Nop; 

        public NakoNodeCalc()
        {
            type = NodeType.N_CALC;
        }

        public override String ToTypeString()
        {
            String r = "";
            r += "(";
            r += base.ToTypeString() + ":";
            r += CalcTypeName[(int)calc_type] + " ";
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
