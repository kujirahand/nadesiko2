using System;
using System.Collections.Generic;
using System.Text;

namespace Libnako.JCompiler.ILWriter
{
    /// <summary>
    /// なでしこの仮想バイトコード(IL)を表わすクラス
    /// </summary>
    public class NakoILCode
    {
		public NakoILType type { get; set; }
		public Object value { get; set; }

		public NakoILCode()
		{
			this.type = 0;
			this.value = null;
		}

        public NakoILCode(NakoILType type, Object value)
        {
            this.type = type;
            this.value = value;
        }
        public NakoILCode(NakoILType type)
        {
            this.type = type;
            this.value = null;
        }

        public static NakoILCode newNop() 
        {
            NakoILCode c = new NakoILCode();
            c.type = NakoILType.NOP;
            return c;
        }
    }
}
