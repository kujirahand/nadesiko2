using System;
using System.Collections.Generic;
using System.Text;

namespace Libnako.JPNCompiler.Tokenizer
{
    /// <summary>
    /// トークンの一覧を保持するリスト
    /// </summary>
    public class NakoTokenList : IList<NakoToken>
    {
        private List<NakoToken> _list = new List<NakoToken>();
        /// <summary>
        /// 現在解析しているトークンの位置を返す
        /// </summary>
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
            return curStack.Pop();
        }

        /// <summary>
        /// Save() で記憶した位置をクリアする
        /// </summary>
        /// <returns></returns>
        public int RemoveTop()
        {
            return Restore();
        }

        /// <summary>
        /// 現在調査中のトークン
        /// </summary>
        public NakoToken CurrentToken
        {
            get
            {
                if (IsEOF()) { return null; }
                return this[cur];
            }
        }

        /// <summary>
        /// 現在のトークンを削除
        /// </summary>
        public void RemoveCurrentToken()
        {
            if (_list.Count > 0)
            {
                _list.RemoveAt(cur);
            }
        }

        /// <summary>
        /// 引数tのトークンを可憐とトークンの後ろに挿入する
        /// </summary>
        /// <param name="t"></param>
        public void InsertAfterCurrentToken(NakoToken t)
        {
            this.Insert(cur + 1, t);
        }

        /// <summary>
        /// 現在のトークンが、指定したトークンタイプtと合致するか
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public Boolean Accept(NakoTokenType t)
        {
            return (t == CurrentTokenType);
        }

        /// <summary>
        /// 次のトークンを返す
        /// </summary>
        public NakoToken NextToken
        {
            get
            {
                if ((cur + 1) >= this.Count) return null;
                return this[cur + 1];
            }
        }

        /// <summary>
        /// 現在のトークンタイプを返す
        /// </summary>
        public NakoTokenType CurrentTokenType
        {
            get {
                NakoToken t = CurrentToken;
                if (t == null) return NakoTokenType.UNKNOWN;
                return t.type;
            }
        }

        /// <summary>
        /// 次のトークンのタイプを返す
        /// </summary>
        public NakoTokenType NextTokenType
        {
            get
            {
                NakoToken t = NextToken;
                if (t == null) return NakoTokenType.UNKNOWN;
                return t.type;
            }
        }
        /// <summary>
        /// トップへ移動
        /// </summary>
        public void MoveTop()
        {
            cur = 0;
        }

        /// <summary>
        /// 次のトークンに移動
        /// </summary>
        public void MoveNext()
        {
            cur++;
        }

        /// <summary>
        /// 終端か?
        /// </summary>
        /// <returns></returns>
        public Boolean IsEOF()
        {
            return (cur >= this.Count);
        }

        /// <summary>
        /// トークンを探す
        /// </summary>
        /// <param name="keytype"></param>
        /// <returns></returns>
        public bool SearchToken(NakoTokenType keytype)
        {
            return SearchToken(keytype, false);
        }
        /// <summary>
        /// 現在のカーソル位置から keytype のトークンがないか調べる (EOLStop=trueのときはEOLまで)
        /// </summary>
        /// <param name="keytype"></param>
        /// <param name="EOLStop"></param>
        /// <returns></returns>
        public bool SearchToken(NakoTokenType keytype, Boolean EOLStop)
        {
            NakoToken t;
            int par_nest = 0;
            int bla_nest = 0;
            for (int i = cur; i < Count; i++ )
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
            }
            return false;
        }

        /// <summary>
        /// トークンのタイプを表す文字列を返す
        /// </summary>
        /// <returns></returns>
        public string toTypeString()
        {
            string s = "";
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
        public bool CheckTokens(NakoToken[] checker)
        {
            // 要素数が異なる
            if (checker.Length != this.Count) return false;
            // タイプを判断
            for (int i = 0; i < checker.Length; i++)
            {
                NakoToken chk = checker[i];
                NakoToken tok = this[i];
                if (!(chk.type == tok.type && chk.value == tok.value))
                {
                    return false;
                }
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

        /// <summary>
        /// 検索
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
		public int IndexOf(NakoToken item)
		{
			return _list.IndexOf(item);
		}
        
        /// <summary>
        /// 挿入
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
		public void Insert(int index, NakoToken item)
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

        /// <summary>
        /// 追加
        /// </summary>
        /// <param name="item"></param>
		public void Add(NakoToken item)
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
        /// 要素にitemを含んでいるか
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
		public bool Contains(NakoToken item)
		{
			return _list.Contains(item);
		}
        /// <summary>
        /// コピーする
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
		public void CopyTo(NakoToken[] array, int arrayIndex)
		{
			_list.CopyTo(array, arrayIndex);
		}
        /// <summary>
        /// 要素の数
        /// </summary>
		public int Count
		{
			get { return _list.Count; }
		}

        /// <summary>
        /// 読み取り専用か
        /// </summary>
		bool ICollection<NakoToken>.IsReadOnly
		{
			get { throw new NotImplementedException(); }
		}

        /// <summary>
        /// Remove
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
		public bool Remove(NakoToken item)
		{
			return _list.Remove(item);
		}

		#endregion

		#region IEnumerable<NakoToken> メンバー
        
        /// <summary>
        /// 列挙型を返す
        /// </summary>
        /// <returns></returns>
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
