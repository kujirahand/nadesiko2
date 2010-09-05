using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Libnako.JCompiler.Tokenizer
{
    public class NakoTokenList : IList<NakoToken>
    {
		private List<NakoToken> _list = new List<NakoToken>();
		protected int cur = 0;
        private Stack<int> curStack = new Stack<int>();

        /// <summary>
        /// トークンの位置を記憶する
        /// </summary>
        public void Save()
        {
            curStack.Push(cur);
        }
        
        /// <summary>
        /// Save() で記憶した位置まで戻る
        /// </summary>
        /// <returns></returns>
        public int Restore()
        {
            cur = curStack.Pop();
            return cur;
        }

        /// <summary>
        /// Save() で記憶した位置をクリアする
        /// </summary>
        /// <returns></returns>
        public int RemoveTop()
        {
            return curStack.Pop();
        }

        public NakoToken CurrentToken
        {
            get
            {
                if (IsEOF()) { return null; }
                return this[cur];
            }
        }

        public void InsertAfterCurrentToken(NakoToken t)
        {
            this.Insert(cur + 1, t);
        }

        public Boolean Accept(NakoTokenType t)
        {
            return (t == CurrentTokenType);
        }

        public NakoToken NextToken
        {
            get
            {
                if ((cur + 1) >= this.Count) return null;
                return this[cur + 1];
            }
        }

        public NakoTokenType CurrentTokenType
        {
            get {
                NakoToken t = CurrentToken;
                if (t == null) return NakoTokenType.UNKNOWN;
                return t.type;
            }
        }

        public NakoTokenType NextTokenType
        {
            get
            {
                NakoToken t = NextToken;
                if (t == null) return NakoTokenType.UNKNOWN;
                return t.type;
            }
        }

        public void MoveTop()
        {
            cur = 0;
        }

        public void MoveNext()
        {
            cur++;
        }

        public Boolean IsEOF()
        {
            return (cur >= this.Count);
        }

        /// <summary>
        /// 現在のカーソル位置から keytype のトークンがないか調べる (EOLStop=trueのときはEOLまで)
        /// </summary>
        /// <param name="keytype"></param>
        /// <param name="EOLStop"></param>
        /// <returns></returns>
        public Boolean SearchToken(NakoTokenType keytype, Boolean EOLStop = false)
        {
            int i = cur;
            NakoToken t;
            int par_nest = 0;
            int bla_nest = 0;
            while (i < this.Count)
            {
                t = this[i];
                // break?
                if (t.type == keytype)
                {
                    return true;
                }
                if (EOLStop)
                {
                    if (t.type == NakoTokenType.EOL) break;
                }
                // ---
                // nest check
                if (t.type == NakoTokenType.BLACKETS_L)
                {
                    bla_nest++;
                }
                if (t.type == NakoTokenType.BLACKETS_R)
                {
                    bla_nest--;
                    if (bla_nest < 0) return false; // tokenが見つかる前に角カッコの不整合を見つけた
                }
                // ---
                if (t.type == NakoTokenType.PARENTHESES_L)
                {
                    par_nest++;
                }
                if (t.type == NakoTokenType.PARENTHESES_R)
                {
                    par_nest--;
                    if (par_nest < 0) return false; // tokenが見つかる前に丸カッコの不整合を見つけた
                }
                i++;
            }
            return false;
        }


        public String toTypeString()
        {
            String s = "";
            foreach (NakoToken t in this)
            {
                if (s != "") { s += ","; }
                s += t.type.ToString();
                if (t.value is string)
                {
                    s += "(" + (string)t.value + ")";
                }
            }
            return s;
        }

        /// <summary>
        /// デバッグ用：トークンタイプを調べて引数と一致するかチェック
        /// </summary>
        /// <param name="checker"></param>
        /// <returns></returns>
        public Boolean CheckTokens(NakoToken[] checker)
        {
            // 要素数が異なる
            if (checker.Length != this.Count) return false;
            // タイプを判断
            for (int i = 0; i < checker.Length; i++)
            {
                NakoToken chk = checker[i];
                NakoToken tok = this[i];
                if (chk.type == tok.type && chk.value == tok.value)
                {
                    continue;
                }
                return false;
            }
            return true;
        }

        /// <summary>
        /// デバッグ用：トークンタイプを調べて引数と一致するかチェック
        /// </summary>
        /// <param name="checker"></param>
        /// <returns></returns>
        public Boolean CheckTokenType(NakoTokenType[] checker)
        {
            // 要素数が異なる
            if (checker.Length != this.Count) return false;
            // タイプを判断
            for (int i = 0; i < checker.Length; i++)
            {
                NakoToken tok = this[i];
                if (tok == null) return false;
                if (tok.type == checker[i])
                {
                    continue;
                }
                return false;
            }
            return true;
        }


		#region IList<NakoToken> メンバー

		public int IndexOf(NakoToken item)
		{
			return _list.IndexOf(item);
		}

		public void Insert(int index, NakoToken item)
		{
			_list.Insert(index, item);
		}

		public void RemoveAt(int index)
		{
			_list.RemoveAt(index);
		}

		public NakoToken this[int index]
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

		#region ICollection<NakoToken> メンバー

		public void Add(NakoToken item)
		{
			_list.Add(item);
		}

		public void Clear()
		{
			_list.Clear();
		}

		public bool Contains(NakoToken item)
		{
			return _list.Contains(item);
		}

		public void CopyTo(NakoToken[] array, int arrayIndex)
		{
			_list.CopyTo(array, arrayIndex);
		}

		public int Count
		{
			get { return _list.Count; }
		}

		bool ICollection<NakoToken>.IsReadOnly
		{
			get { throw new NotImplementedException(); }
		}

		public bool Remove(NakoToken item)
		{
			return _list.Remove(item);
		}

		#endregion

		#region IEnumerable<NakoToken> メンバー

		public IEnumerator<NakoToken> GetEnumerator()
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
