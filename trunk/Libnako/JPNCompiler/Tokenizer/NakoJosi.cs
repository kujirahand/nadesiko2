using System;
using System.Collections.Generic;
using System.Text;

namespace Libnako.JPNCompiler.Tokenizer
{
    /// <summary>
    /// なでしこ助詞リスト
    /// </summary>
    public class NakoJosi : IList<String>
    {
		private List<String> _list = new List<string>();
        
        /// <summary>
        /// 助詞リストの唯一のインスタンスを返す(Singleton)
        /// </summary>
        public static NakoJosi Instance { get { return _instance; } }
        private static NakoJosi _instance = new NakoJosi();
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
            Add("とは"); // 変数定義
            Add("して");
            Add("とは");
            Add("だけ");
            Add("より");
            Add("ほど");
            Add("など");
            Add("って");
            Add("では");
            Add("は"); // 代入
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

        /// <summary>
        /// 文字数によってソート
        /// </summary>
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

        /// <summary>
        /// ソート
        /// </summary>
        /// <param name="comparison"></param>
		public void Sort(Comparison<string> comparison)
		{
			_list.Sort(comparison);
		}

		#region IList<string> メンバー

        /// <summary>
        /// 検索
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
		public int IndexOf(string item)
		{
			return _list.IndexOf(item);
		}

        /// <summary>
        /// 挿入
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
		public void Insert(int index, string item)
		{
			_list.Insert(index, item);
		}

        /// <summary>
        /// 削除
        /// </summary>
        /// <param name="index"></param>
		public void RemoveAt(int index)
		{
			_list.RemoveAt(index);
		}

        /// <summary>
        /// 要素を得る
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
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
        /// <summary>
        /// 追加
        /// </summary>
        /// <param name="item"></param>
		public void Add(string item)
		{
			_list.Add(item);
		}
        /// <summary>
        /// 削除
        /// </summary>
		public void Clear()
		{
			_list.Clear();
		}
        /// <summary>
        /// 含むか
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
		public bool Contains(string item)
		{
			return _list.Contains(item);
		}

        /// <summary>
        /// コピー
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
		public void CopyTo(string[] array, int arrayIndex)
		{
			_list.CopyTo(array, arrayIndex);
		}
        /// <summary>
        /// かぞえる
        /// </summary>
		public int Count
		{
			get { return _list.Count; }
		}

        /// <summary>
        /// 読み取り専用か
        /// </summary>
		bool ICollection<string>.IsReadOnly
		{
			get { throw new NotImplementedException(); }
		}

        /// <summary>
        /// 削除
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
		public bool Remove(string item)
		{
			return _list.Remove(item);
		}

		#endregion

		#region IEnumerable<string> メンバー

        /// <summary>
        /// 列挙
        /// </summary>
        /// <returns></returns>
		public IEnumerator<string> GetEnumerator()
		{
			return _list.GetEnumerator();
		}

		#endregion

		#region IEnumerable メンバー

        /// <summary>
        /// 列挙
        /// </summary>
        /// <returns></returns>
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return _list.GetEnumerator();
		}

		#endregion
	}
}
