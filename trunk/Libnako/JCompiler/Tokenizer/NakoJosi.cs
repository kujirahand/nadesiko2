using System;
using System.Collections.Generic;
using System.Text;

namespace Libnako.JCompiler.Tokenizer
{
    /// <summary>
    /// なでしこ助詞リスト
    /// </summary>
    public class NakoJosi : IList<String>
    {
		private List<String> _list = new List<string>();
        
        // C# Singleton
		private static NakoJosi _instance = new NakoJosi();
        public static NakoJosi Instance { get { return _instance; } }
        private NakoJosi() { Init(); }

        /// <summary>
        /// 助詞一覧を単語辞書に追加する
        /// </summary>
        protected void Init()
        {
            Add("について");
            Add("ならば");
            Add("までを");
            Add("までの");
            Add("くらい");
            Add("なのか");
            Add("なら");
            Add("から");
            Add("まで");
            Add("とは");
            Add("して");
            Add("とは");
            Add("だけ");
            Add("より");
            Add("ほど");
            Add("など");
            Add("って");
            Add("では");
            Add("は");
            Add("の");
            Add("が");
            Add("を");
            Add("に");
            Add("へ");
            Add("と");
            Add("で");
            Add("て");
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
