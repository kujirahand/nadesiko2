using System;
using System.Collections.Generic;
using System.Text;
using Libnako.JPNCompiler.Tokenizer;
using Libnako.NakoAPI;

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
                     if (tok.Type == NakoTokenType.DIM_INT) {//ユーザー定義関数の「整数」はDIM INTと判定されるので、WORDに直す
                        tok.Type = NakoTokenType.WORD;
                     }
                     if (tok.Type == NakoTokenType.WORD) {
                        string opt = (string)tok.Value;
                        if (opt == "参照渡し")
                            argOpt.varBy = VarByType.ByRef;
                        if (opt == "整数") {//TODO:数値
                            NakoToken checkToken = tokens [i + 1];
                            if (checkToken.Type == NakoTokenType.EQ) {
                                checkToken = tokens [i + 2];
                                if (checkToken.Type == NakoTokenType.INT) {
                                    argOpt.defaultValue = int.Parse (checkToken.Value);
                                }
                            }
                        }
                        if (opt == "文字列") {
                            NakoToken checkToken = tokens [i + 1];
                            if (checkToken.Type == NakoTokenType.EQ) {
                                checkToken = tokens [i + 2];
                                if (checkToken.Type == NakoTokenType.STRING || checkToken.Type == NakoTokenType.STRING_EX || checkToken.Type == NakoTokenType.WORD) {
                                    argOpt.defaultValue = (string)checkToken.Value;
                                }
                            }
                        }
                        if (opt == "関数") {//関数定義が引数の場合。{関数(2)}とか表記(n)はn個引数があるという意味。初期値は無いはずなので、引数の個数をdefaultValueプロパティに入れているが、その点は修正の必要があるかもしれない。
                            argOpt.defaultValue = 0;
                            argOpt.type = NakoFuncType.UserCall.ToString();
                            for (int j = i + 1; j < tokens.Count; j++) {
                                if (tokens [j].Type == NakoTokenType.PARENTHESES_L) {
                                    j++;
                                    NakoToken argCountToken = tokens [j];
                                    argOpt.defaultValue = int.Parse(argCountToken.Value);
                                    j++;
                                }
                                if (tokens [j].Type == NakoTokenType.BRACES_R) {
                                    break;
                                }
                            }
                        }
                    } else {
                        //find type
                        string type = (string)tok.Value;
                        int index = NakoAPIFuncBank.Instance.FuncList.FindIndex (delegate(NakoAPIFunc obj) {
                            return obj.PluginInstance.Name == type;
                        });
                        if(index > 0){
                            argOpt.type = type;
                        }
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
                        arg.defaultValue = argOpt.defaultValue;
                        arg.type = argOpt.type;
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
        internal string type;
        internal object defaultValue;
        internal void Init()
        {
            varBy = VarByType.ByVal;
            type = null;
            defaultValue = null;
        }
    }
}
