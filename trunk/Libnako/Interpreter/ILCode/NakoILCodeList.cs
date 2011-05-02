using System;
using System.Collections.Generic;

using System.Text;

namespace Libnako.JPNCompiler.ILWriter
{
    /// <summary>
    /// なでしこの仮想バイトコード一覧を管理するクラス
    /// </summary>
    public class NakoILCodeList : IList<NakoILCode>
    {
		private List<NakoILCode> _list = new List<NakoILCode>();
		public NakoVariableManager globalVar = null;

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
                r += c.GetDescription();
                r += "\n";
            }
            return r;
        }

		#region IList<NakoILCode> メンバー

		public int IndexOf(NakoILCode item)
		{
			return _list.IndexOf(item);
		}

		public void Insert(int index, NakoILCode item)
		{
			_list.Insert(index, item);
		}

		public void RemoveAt(int index)
		{
			_list.RemoveAt(index);
		}

		public NakoILCode this[int index]
		{
			get
			{
				return _list[index];
			}
			set
			{
				_list[index] = value;
			}
		}

		#endregion

		#region ICollection<NakoILCode> メンバー

		public void Add(NakoILCode item)
		{
			_list.Add(item);
		}

		public void Clear()
		{
			_list.Clear();
		}

		public bool Contains(NakoILCode item)
		{
			return _list.Contains(item);
		}

		public void CopyTo(NakoILCode[] array, int arrayIndex)
		{
			_list.CopyTo(array, arrayIndex);
		}

		public int Count
		{
			get { return _list.Count; }
		}

		bool ICollection<NakoILCode>.IsReadOnly
		{
			get { throw new NotImplementedException(); }
		}

		public bool Remove(NakoILCode item)
		{
			return _list.Remove(item);
		}

		#endregion

		#region IEnumerable<NakoILCode> メンバー

		public IEnumerator<NakoILCode> GetEnumerator()
		{
			return _list.GetEnumerator();
		}

		#endregion

		#region IEnumerable メンバー

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return _list.GetEnumerator();
		}

		#endregion

	}
}
