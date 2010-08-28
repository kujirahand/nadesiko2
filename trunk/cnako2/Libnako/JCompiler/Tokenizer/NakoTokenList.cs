using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Libnako.JCompiler.Tokenizer
{
    public class NakoTokenList : List<NakoToken>
    {
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
            while (i < this.Count)
            {
                t = this[i];
                if (t.type == keytype)
                {
                    return true;
                }
                if (EOLStop)
                {
                    if (t.type == NakoTokenType.EOL) break;
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

    }
}
