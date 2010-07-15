using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Libnako.Parser
{
    public class NakoILCode
    {
        public int type = 0;
        public Object value = null;

        public NakoILCode()
        {
        }

        public NakoILCode(int type, Object value)
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
