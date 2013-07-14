using System;
using System.Collections.Generic;
using System.Text;
using Libnako.JPNCompiler.Tokenizer;

namespace Libnako.JPNCompiler.Function
{
    /// <summary>
    /// なでしこ関数の引数一覧を管理するクラス
    /// </summary>
    public class NakoFuncArgs : IList<NakoFuncArg>
    {
        private List<NakoFuncArg> _list = new List<NakoFuncArg>();
        /// <summary>
        /// 引数の定義文字列を読んで、関数の引数として登録する
        /// </summary>
        public void analizeArgTokens(NakoTokenList tokens)
        {
            bool optMode = false;
            ArgOpt argOpt = new ArgOpt();

            for (int i = 0; i < tokens.Count; i++)
            {
                NakoToken tok = tokens[i];
                NakoFuncArg arg = null;
                // オプション指定モード(optMode) の on/off
                if (tok.Type == NakoTokenType.BRACES_L)
                {
                    // オプションの初期化
                    optMode = true;
                    argOpt.Init();
                    continue;
                }
                if (tok.Type == NakoTokenType.BRACES_R)
                {
                    optMode = false; 
                    continue;
                }
                if (optMode)
                {
                    if (tok.Type == NakoTokenType.WORD)
                    {
                        string opt = (string)tok.Value;
                        if (opt == "参照渡し") argOpt.varBy = VarByType.ByRef;
                    }
                    continue;
                }

                // WORD
                if (tok.Type == NakoTokenType.WORD)
                {
                    int idx = indexOfName(tok.Value);
                    if (idx < 0)
                    {
                        arg = new NakoFuncArg();
                        arg.name = tok.Value;
                        arg.varBy = argOpt.varBy;
                        arg.AddJosi(tok.Josi);
                        this.Add(arg);
                        argOpt.Init();
                    }
                    else
                    {
                        arg = this[idx];
                        arg.AddJosi(tok.Josi);
                    }
                }
                if (tok.Type == NakoTokenType.OR || tok.Type == NakoTokenType.OR_OR)
                {
                    continue;
                }
            }
        }

        /// <summary>
        /// 引数の定義文字列を読んで、関数の引数として登録する
        /// </summary>
        /// <param name="str"></param>
        public void analizeArgStr(string str)
        {
            NakoTokenList tokens = NakoTokenization.TokenizeSplitOnly(str);
            analizeArgTokens(tokens);
        }

        /// <summary>
        /// 名前から引数の番号を得る
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public int indexOfName(string name)
        {
            for (int i = 0; i < this.Count; i++)
            {
                NakoFuncArg arg = this[i];
                if (arg.name == name)
                {
                    return i;
                }
            }
            return -1;
        }

		#region IList<NakoFuncArg> メンバー

        /// <summary>
        /// 検索
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
		public int IndexOf(NakoFuncArg item)
		{
			return _list.IndexOf(item);
		}
        /// <summary>
        /// 挿入
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
		public void Insert(int index, NakoFuncArg item)
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
		public NakoFuncArg this[int index]
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

		#region ICollection<NakoFuncArg> メンバー

        /// <summary>
        /// 追加
        /// </summary>
        /// <param name="item"></param>
		public void Add(NakoFuncArg item)
		{
			_list.Add(item);
		}
        /// <summary>
        /// クリア
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
		public bool Contains(NakoFuncArg item)
		{
			return _list.Contains(item);
		}
        /// <summary>
        /// コピー
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
		public void CopyTo(NakoFuncArg[] array, int arrayIndex)
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

		bool ICollection<NakoFuncArg>.IsReadOnly
		{
			get { throw new NotImplementedException(); }
		}
        /// <summary>
        /// 削除
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
		public bool Remove(NakoFuncArg item)
		{
			return _list.Remove(item);
		}

		#endregion

		#region IEnumerable<NakoFuncArg> メンバー
        /// <summary>
        /// 列挙
        /// </summary>
        /// <returns></returns>
		public IEnumerator<NakoFuncArg> GetEnumerator()
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

    /// <summary>
    /// 引数オプションを表すクラス
    /// </summary>
    internal class ArgOpt
    {
        internal VarByType varBy = VarByType.ByVal;
        internal object defaultValue;
        internal void Init()
        {
            varBy = VarByType.ByVal;
            defaultValue = null;
        }
    }
}
