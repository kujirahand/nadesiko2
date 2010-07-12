
using System;
using System.Collections.Generic;
using System.Text;

namespace Libnako.Parser
{
    public class NakoToken
    {
        public string value = null;
        public int lineno = 0;
        public int level = 0;
        public string josi = "";
        protected int _type = 0;
        public int type
        {
            get { return _type; }
            set { _type = value; debug_type = TokenType.GetTokenName(value); }
        }
        // for DEBUG
        public String debug_type = "";

        public NakoToken(int type = 0, int lineno = 0, int level = 0)
        {
            this.lineno = lineno;
            this.level = level;
            this.type = type;
        }
    }
}
