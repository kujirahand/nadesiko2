using System;
using System.Collections.Generic;
using System.Text;

namespace Libnako.JCompiler.Tokenizer
{
    /// <summary>
    /// トークン解析機の基本メソッドを提供する
    /// </summary>
    public class NakoTokenizerBase
    {
        protected int cur;
        protected int level;
        protected int indentCount;
        protected int lineno;
        protected NakoDicReservedWord dic;
        protected NakoTokenType last_token_type;

        protected string source;
        public string Source
        {
            set { Init(); this.source = value; }
            get { return this.source; }
        }

        protected NakoTokenList tokens;
        public NakoTokenList Tokens
        {
            get { return this.tokens; }
        }

        public void Init()
        {
            cur = 0;
            level = 0;
            lineno = 0;
            indentCount = 0;
            last_token_type = NakoTokenType.UNKNOWN;
            tokens = new NakoTokenList();
            dic = NakoDicReservedWord.Instance;
        }

        public Boolean CheckTokenType(NakoTokenType[] checker)
        {
            return Tokens.CheckTokenType(checker);
        }

        public Boolean IsEOF()
        {
            return (cur >= source.Length);
        }

        public Char CurrentChar
        {
            get
            {
                if (IsEOF()) return '\0';
                return NakoHalfFlag.ConvertChar(source[cur]);
            }
        }
        
        public Char NextChar
        {
            get
            {
                if ((cur + 1) >= source.Length)
                {
                    return '\0';
                }
                return NakoHalfFlag.ConvertChar(source[cur + 1]);
            }
        }

        public Boolean CompareStr(String str)
        {
            if (source.Length < (cur + str.Length)) { return false;  }
            return (source.Substring(cur, str.Length) == str);
        }

        public String GetToSplitter(String splitter, Boolean need_splitter = false)
        {
            String r = "";
            while (!IsEOF())
            {
                if (CompareStr(splitter))
                {
                    if (need_splitter)
                    {
                        r += splitter;
                    }
                    cur += splitter.Length;
                    break;
                }
                // 全角半角変換しない
                r += source[cur];
                cur++;
            }
            return r;
        }

        public String GetToSplitter(Char splitter, Boolean need_splitter = false)
        {
            String r = "";
            while (!IsEOF())
            {
                Char c = source[cur];
                if (c == splitter)
                {
                    if (need_splitter)
                    {
                        r += splitter;
                    }
                    cur++;
                    break;
                }
                r += c;
                cur++;
            }
            return r;
        }
    }
}
