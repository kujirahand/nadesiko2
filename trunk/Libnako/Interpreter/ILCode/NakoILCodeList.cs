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
        /// <summary>
        /// グローバル変数
        /// </summary>
		public NakoVariableManager globalVar = null;

        /// <summary>
        /// タイプをチェックする
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
		public Boolean CheckTypes(NakoILType[] types)
        {
            if (types.Length != this.Count) return false;
            for (int i = 0; i < types.Length; i++)
            {
                if (types[i] != this[i].type) return false;
            }
            return true;
        }
        /// <summary>
        /// デバッグ用タイプ文字列を返す
        /// </summary>
        /// <returns></returns>
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
        /// <summary>
        /// アドレス付きの文字列
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// 検索
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
		public int IndexOf(NakoILCode item)
		{
			return _list.IndexOf(item);
		}
        /// <summary>
        /// 挿入
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
		public void Insert(int index, NakoILCode item)
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

        /// <summary>
        /// 追加
        /// </summary>
        /// <param name="item"></param>
		public void Add(NakoILCode item)
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
        /// 含む
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
		public bool Contains(NakoILCode item)
		{
			return _list.Contains(item);
		}
        /// <summary>
        /// コピー
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
		public void CopyTo(NakoILCode[] array, int arrayIndex)
		{
			_list.CopyTo(array, arrayIndex);
		}
        /// <summary>
        /// 個数
        /// </summary>
		public int Count
		{
			get { return _list.Count; }
		}

		bool ICollection<NakoILCode>.IsReadOnly
		{
			get { throw new NotImplementedException(); }
		}
        /// <summary>
        /// 削除
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
		public bool Remove(NakoILCode item)
		{
			return _list.Remove(item);
		}

		#endregion

		#region IEnumerable<NakoILCode> メンバー

        /// <summary>
        /// 数え上げ
        /// </summary>
        /// <returns></returns>
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
