using System;
using System.Collections.Generic;
using System.Text;
using Libnako.JPNCompiler.Function;

namespace Libnako.JPNCompiler.Node
{
    /// <summary>
    /// ノード一覧クラス
    /// </summary>
    public class NakoNodeList : IList<NakoNode>
    {
		private List<NakoNode> _list = new List<NakoNode>();
		
        /// <summary>
        /// 先頭のノードを切り取って返す
        /// </summary>
        /// <returns></returns>
		public NakoNode Shift()
        {
            if (this.Count == 0)
            {
                return null;
            }
            NakoNode r = this[0];
            this.RemoveAt(0);
            return r;
        }
        /// <summary>
        /// ポップ
        /// </summary>
        /// <returns></returns>
        public NakoNode Pop()
        {
        	return this.Pop(null);
        }
        /// <summary>
        /// 助詞つきでポップ
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        public NakoNode Pop(NakoFuncArg arg)
        {
            // 助詞リストをチェックする
            if (arg != null)
            {
                foreach (string josi in arg.josiList)
                {
                    for (int i = 0; i < this.Count; i++)
                    {
                        int bi = Count - i - 1;
                        NakoNode cn = this[bi];
                        if (cn.josi == josi)
                        {
                            this.RemoveAt(bi);
                            return cn;
                        }
                    }
                }
            }
            // 普通にPOP
            if (this.Count > 0)
            {
                NakoNode n = this[this.Count - 1];
                this.RemoveAt(this.Count - 1);
                return n;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 末尾に追加
        /// </summary>
        /// <param name="value"></param>
        public void Push(NakoNode value)
        {
            this.Add(value);
        }

        /// <summary>
        /// ノードタイプが合致しているか調べる
        /// </summary>
        /// <param name="checker"></param>
        /// <returns></returns>
        public bool checkNodeType(NakoNodeType[] checker)
        {
            if (checker.Length != this.Count) return false;
            for (int i = 0; i < checker.Length; i++)
            {
                NakoNode n = this[i];
                if (n.type != checker[i])
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// デバッグ用にタイプ文字列を返す
        /// </summary>
        /// <returns></returns>
        public string toTypeString()
        {
        	return this.toTypeString(0);
        }
        private string toTypeString(int level)
        {
            // for indent
            string indent = "- ";
            for (int i = 0; i < level; i++)
            {
                indent += "|-- ";
            }
            // this children
            string r = "";
            foreach (NakoNode n in this)
            {
                if (n != null)
                {
                    r += indent + n.ToTypeString() + "\n";
                    if (n.hasChildren())
                    {
                        r += n.Children.toTypeString(level + 1);
                    }
                }
                else
                {
                    r += indent + "(null)" + "\n";
                }
            }
            return r;
        }

        /// <summary>
        /// 配列形式で返す
        /// </summary>
        /// <returns></returns>
        public NakoNodeType[] toTypeArray()
        {
            NakoNodeType[] r = new NakoNodeType[this.Count];
            int i = 0;
            foreach (NakoNode n in this)
            {
                r[i++] = n.type;
            }
            return r;
        }

		#region IList<NakoNode> メンバー
        /// <summary>
        /// 検索
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
		public int IndexOf(NakoNode item)
		{
			return _list.IndexOf(item);
		}
        /// <summary>
        /// 挿入
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
		public void Insert(int index, NakoNode item)
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
        /// 要素を返す
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
		public NakoNode this[int index]
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

		#region ICollection<NakoNode> メンバー

        /// <summary>
        /// 追加
        /// </summary>
        /// <param name="item"></param>
		public void Add(NakoNode item)
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
		public bool Contains(NakoNode item)
		{
			return _list.Contains(item);
		}
        /// <summary>
        /// コピー
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
		public void CopyTo(NakoNode[] array, int arrayIndex)
		{
			_list.CopyTo(array, arrayIndex);
		}
        /// <summary>
        /// カウント
        /// </summary>
		public int Count
		{
			get { return _list.Count; }
		}
        /// <summary>
        /// 読み取り専用か
        /// </summary>
		bool ICollection<NakoNode>.IsReadOnly
		{
			get { throw new NotImplementedException(); }
		}
        /// <summary>
        /// 削除
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
		public bool Remove(NakoNode item)
		{
			return _list.Remove(item);
		}

		#endregion

		#region IEnumerable<NakoNode> メンバー

        /// <summary>
        /// 列挙
        /// </summary>
        /// <returns></returns>
		public IEnumerator<NakoNode> GetEnumerator()
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
