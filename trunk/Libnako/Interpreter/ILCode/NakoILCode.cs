using System;
using System.Collections.Generic;
using System.Text;

namespace Libnako.JPNCompiler.ILWriter
{
    /// <summary>
    /// なでしこの仮想バイトコード(IL)を表わすクラス
    /// </summary>
    public class NakoILCode : IEquatable<NakoILCode>
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

		public override int GetHashCode()
		{
			return type.GetHashCode() ^ (value == null ? 0 : value.GetHashCode());
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}

			return ((IEquatable<NakoILCode>)this).Equals(obj as NakoILCode);
		}

        #region IEquatable<NakoILCode> メンバ

		bool IEquatable<NakoILCode>.Equals(NakoILCode other)
		{
			if (other == null)
			{
				return false;
			}

			return (this.type == other.type) && (this.value == other.value);
		}

        #endregion
    }
}
