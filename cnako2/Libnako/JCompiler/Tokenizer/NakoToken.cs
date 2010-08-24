
using System;
using System.Collections.Generic;
using System.Text;

namespace Libnako.JCompiler.Tokenizer
{
    public class NakoToken
    {
        public string value = null;
        public int lineno = 0;
        public int level = 0;
        public string josi = "";
        protected NakoTokenType _type = 0;
        public NakoTokenType type
        {
            get { return _type;  }
            set { _type = value; }
        }

        public NakoToken(NakoTokenType type = 0, int lineno = 0, int level = 0)
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
