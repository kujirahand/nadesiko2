using System;
using System.Collections.Generic;
using System.Text;

namespace Libnako.JCompiler.Tokenizer
{
    public class NakoJosi : IList<String>
    {
		private List<String> _list = new List<string>();
		private static NakoJosi instance = null;
        public static NakoJosi GetInstance()
        {
            if (instance == null)
            {
                instance = new NakoJosi();
            }
            return instance;
        }

        private NakoJosi()
        {
            Init();
        }

        protected void Init()
        {
            Add("ならば");
            Add("なら");
            Add("から");
            Add("まで");
            Add("とは");
            Add("は");
            Add("の");
            Add("が");
            Add("を");
            Add("に");
            Add("へ");
            Add("と");
            //
            SortAsLength();
        }

        protected void SortAsLength()
        {
            this.Sort(
                delegate (String a, String b) {
                    if (a.Length == b.Length)
                    {
                        return String.Compare(a, b);
                    }
                    return b.Length - a.Length;
                }
            );
        }


		public void Sort(Comparison<string> comparison)
		{
			_list.Sort(comparison);
		}

		#region IList<string> メンバー

		public int IndexOf(string item)
		{
			return _list.IndexOf(item);
		}

		public void Insert(int index, string item)
		{
			_list.Insert(index, item);
		}

		public void RemoveAt(int index)
		{
			_list.RemoveAt(index);
		}

		public string this[int index]
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

		#region ICollection<string> メンバー

		public void Add(string item)
		{
			_list.Add(item);
		}

		public void Clear()
		{
			_list.Clear();
		}

		public bool Contains(string item)
		{
			return _list.Contains(item);
		}

		public void CopyTo(string[] array, int arrayIndex)
		{
			_list.CopyTo(array, arrayIndex);
		}

		public int Count
		{
			get { return _list.Count; }
		}

		bool ICollection<string>.IsReadOnly
		{
			get { throw new NotImplementedException(); }
		}

		public bool Remove(string item)
		{
			return _list.Remove(item);
		}

		#endregion

		#region IEnumerable<string> メンバー

		public IEnumerator<string> GetEnumerator()
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
