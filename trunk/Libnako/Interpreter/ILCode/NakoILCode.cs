using System;
using System.Collections.Generic;
using System.Text;

using Libnako.NakoAPI;

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

        public string GetDescription()
        {
            string r = "";
            switch (type)
            {
                case NakoILType.NOP:
                    if (value is string) { r += ":" + value; }
                    break;
                case NakoILType.LD_CONST_INT:
                case NakoILType.LD_CONST_REAL:
                case NakoILType.LD_CONST_STR:
                case NakoILType.LD_GLOBAL:
                case NakoILType.ST_GLOBAL:
                case NakoILType.LD_LOCAL:
                case NakoILType.ST_LOCAL:
                case NakoILType.LD_GLOBAL_REF:
                case NakoILType.LD_LOCAL_REF:
                case NakoILType.USRCALL:
                    r += ":" + value;
                    break;
                case NakoILType.SYSCALL:
                    int funcNo = (int)value;
                    NakoAPIFunc s = NakoAPIFuncBank.Instance.FuncList[funcNo];
                    r += ":" + s.name + "@" + s.PluginInstance.Name + "(" + value + ")";
                    break;
                case NakoILType.JUMP:
                case NakoILType.BRANCH_FALSE:
                case NakoILType.BRANCH_TRUE:
                    r += "->" + String.Format("{0,0:X4}", (Int64)value);
                    break;
            }
            return r;
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
