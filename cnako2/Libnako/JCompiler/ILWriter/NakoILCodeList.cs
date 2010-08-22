using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Libnako.JCompiler.ILWriter
{
    public class NakoILCodeList : List<NakoILCode>
    {
        public Boolean CheckTypes(NakoILType[] types)
        {
            if (types.Length != this.Count) return false;
            for (int i = 0; i < types.Length; i++)
            {
                if (types[i] != this[i].type) return false;
            }
            return true;
        }
        public String ToTypeString()
        {
            String r = "";
            for (int i = 0; i < this.Count; i++)
            {
                NakoILCode c;

                if (r != "") r += ",";
                c = this[i];
                r += c.type.ToString();
                if (c.type == NakoILType.LD_CONST_INT ||
                    c.type == NakoILType.LD_CONST_REAL ||
                    c.type == NakoILType.LD_CONST_STR)
                {
                    r += ":" + c.value;
                }
            }
            return r;
        }
        public String ToAddressString()
        {
            String r = "";
            for (int i = 0; i < this.Count; i++)
            {
                NakoILCode c;
                c = this[i];
                // address
                r += String.Format("{0,0:X4}:", i);
                // type
                r += c.type.ToString();
                //TODO
                switch (c.type)
                {
                    case NakoILType.NOP:
                        if (c.value is string) { r += ":" + c.value; }
                        break;
                    case NakoILType.LD_CONST_INT:
                    case NakoILType.LD_CONST_REAL:
                    case NakoILType.LD_CONST_STR:
                    case NakoILType.LD_GLOBAL:
                    case NakoILType.ST_GLOBAL:
                    case NakoILType.LD_LOCAL:
                    case NakoILType.ST_LOCAL:
                        r += ":" + c.value;
                        break;
                    case NakoILType.JUMP:
                    case NakoILType.BRANCH_FALSE:
                    case NakoILType.BRANCH_TRUE:
                        r += "->" + String.Format("{0,0:X4}", (Int32)c.value);
                        break;
                }
                r += "\n";
            }
            return r;
        }
    }
}
