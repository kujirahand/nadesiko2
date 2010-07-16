using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Libnako.Parser
{
    public class NakoILCode
    {
        public NakoILType type = 0;
        public Object value = null;

        public NakoILCode()
        {
        }

        public NakoILCode(NakoILType type, Object value)
        {
            this.type = type;
            this.value = value;
        }

        public static NakoILCode newNop() 
        {
            NakoILCode c = new NakoILCode();
            c.type = NakoILType.NOP;
            return c;
        }
    }
}
