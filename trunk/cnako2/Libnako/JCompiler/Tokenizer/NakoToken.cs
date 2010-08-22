
using System;
using System.Collections.Generic;
using System.Text;

namespace Libnako.Parser.Tokenizer
{
    public class NakoToken
    {
        public string value = null;
        public int lineno = 0;
        public int level = 0;
        public string josi = "";
        protected TokenType _type = 0;
        public TokenType type
        {
            get { return _type;  }
            set { _type = value; }
        }

        public NakoToken(TokenType type = 0, int lineno = 0, int level = 0)
        {
            this.lineno = lineno;
            this.level = level;
            this.type = type;
        }

        public String ToStringForDebug()
        {
            return "[" + _type.ToString() + ":" + value + "]" + josi + "(" + lineno + ")";
        }
    }
}
