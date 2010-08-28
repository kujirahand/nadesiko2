using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Libnako.JCompiler.ILWriter
{
    public class NakoILCode
    {
		public NakoILType type { get; set; }
		public Object value { get; set; }

		public NakoILCode()
		{
			this.type = 0;
			this.value = null;
		}

        public NakoILCode(NakoILType type, Object value = null)
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
