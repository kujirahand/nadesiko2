using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Libnako.Parser
{
    public class NakoTokenList : List<NakoToken>
    {
        protected int cur = 0;
        private Stack<int> curStack = new Stack<int>();

        public void Save()
        {
            curStack.Push(cur);
        }

        public int Restore()
        {
            cur = curStack.Pop();
            return cur;
        }

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

        public NakoToken NextToken
        {
            get
            {
                if ((cur + 1) >= this.Count) return null;
                return this[cur + 1];
            }
        }

        public TokenType CurrentTokenType
        {
            get {
                NakoToken t = CurrentToken;
                if (t == null) return TokenType.T_UNKNOWN;
                return t.type;
            }
        }

        public TokenType NextTokenType
        {
            get
            {
                NakoToken t = NextToken;
                if (t == null) return TokenType.T_UNKNOWN;
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

        public String toTypeString()
        {
            String s = "";
            foreach (NakoToken t in this)
            {
                if (s != "") { s += ","; }
                s += TokenTypeDescriptor.GetTypeName(t.type);
            }
            return s;
        }

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

        public Boolean CheckTokenType(TokenType[] checker)
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
