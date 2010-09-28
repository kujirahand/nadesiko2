using System;
using System.Collections.Generic;
using System.Text;
using Libnako.JCompiler.Tokenizer;

namespace Libnako.JCompiler.Function
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
            Boolean optMode = false;
            ArgOpt argOpt = new ArgOpt();

            for (int i = 0; i < tokens.Count; i++)
            {
                NakoToken tok = tokens[i];
                NakoFuncArg arg = null;
                // オプション指定モード(optMode) の on/off
                if (tok.type == NakoTokenType.BRACES_L)
                {
                    // オプションの初期化
                    optMode = true;
                    argOpt.Init();
                    continue;
                }
                if (tok.type == NakoTokenType.BRACES_R)
                {
                    optMode = false; 
                    continue;
                }
                if (optMode)
                {
                    if (tok.type == NakoTokenType.WORD)
                    {
                        string opt = (string)tok.value;
                        if (opt == "参照渡し") argOpt.varBy = VarByType.ByRef;
                    }
                    continue;
                }

                // WORD
                if (tok.type == NakoTokenType.WORD)
                {
                    int idx = indexOfName(tok.value);
                    if (idx < 0)
                    {
                        arg = new NakoFuncArg();
                        arg.name = tok.value;
                        arg.varBy = argOpt.varBy;
                        arg.AddJosi(tok.josi);
                        this.Add(arg);
                        argOpt.Init();
                    }
                    else
                    {
                        arg = this[idx];
                        arg.AddJosi(tok.josi);
                    }
                }
                if (tok.type == NakoTokenType.OR || tok.type == NakoTokenType.OR_OR)
                {
                    continue;
                }
            }
        }

        /// <summary>
        /// 引数の定義文字列を読んで、関数の引数として登録する
        /// </summary>
        /// <param name="str"></param>
        public void analizeArgStr(String str)
        {
            NakoTokenizer tokenizer = new NakoTokenizer(str);
            tokenizer.splitWord();
            NakoTokenList tokens = tokenizer.Tokens;
            analizeArgTokens(tokens);
        }

        public int indexOfName(String name)
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

		public int IndexOf(NakoFuncArg item)
		{
			return _list.IndexOf(item);
		}

		public void Insert(int index, NakoFuncArg item)
		{
			_list.Insert(index, item);
		}

		public void RemoveAt(int index)
		{
			_list.RemoveAt(index);
		}

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

		public void Add(NakoFuncArg item)
		{
			_list.Add(item);
		}

		public void Clear()
		{
			_list.Clear();
		}

		public bool Contains(NakoFuncArg item)
		{
			return _list.Contains(item);
		}

		public void CopyTo(NakoFuncArg[] array, int arrayIndex)
		{
			_list.CopyTo(array, arrayIndex);
		}

		public int Count
		{
			get { return _list.Count; }
		}

		bool ICollection<NakoFuncArg>.IsReadOnly
		{
			get { throw new NotImplementedException(); }
		}

		public bool Remove(NakoFuncArg item)
		{
			return _list.Remove(item);
		}

		#endregion

		#region IEnumerable<NakoFuncArg> メンバー

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

    internal class ArgOpt
    {
        internal VarByType varBy = VarByType.ByVal;
        internal Object defaultValue;
        internal void Init()
        {
            varBy = VarByType.ByVal;
            defaultValue = null;
        }
    }
}
