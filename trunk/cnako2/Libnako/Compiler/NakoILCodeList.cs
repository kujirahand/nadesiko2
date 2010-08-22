using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Libnako.Parser
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
                r += NakoILTypeDescriptor.GetTypeName(c.type);
                if (c.type == NakoILType.LD_CONST_INT ||
                    c.type == NakoILType.LD_CONST_REAL ||
                    c.type == NakoILType.LD_CONST_STR)
                {
                    r += ":" + c.value;
                }
            }
            return r;
        }
    }
}
