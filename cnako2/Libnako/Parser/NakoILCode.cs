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

        public static NakoILCode newNop() {
            NakoILCode c = new NakoILCode();
            c.type = NakoILType.I_NOP;
            return c;
        }
    }
}
