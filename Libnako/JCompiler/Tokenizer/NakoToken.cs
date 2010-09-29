
using System;
using System.Collections.Generic;
using System.Text;

namespace Libnako.JCompiler.Tokenizer
{
    public class NakoToken
    {
		public string value { get; set; }
		public int lineno { get; set; }
		public int level { get; set; }
		public string josi { get; set; }
        protected NakoTokenType _type = 0;
        public NakoTokenType type
        {
            get { return _type;  }
            set { _type = value; }
        }

        public NakoToken(NakoTokenType type, int lineno, int level)
        {
        	Init(type, lineno, level);
        }
        public NakoToken(NakoTokenType type)
        {
        	Init(type, 0, 0);
        }
        
        public void Init(NakoTokenType type, int lineno, int level)
        {
            this.lineno = lineno;
            this.level = level;
            this.type = type;
			this.josi = "";
			this.value = null;
        }

        public String ToStringForDebug()
        {
            return "[" + _type.ToString() + ":" + value + "]" + josi + "(" + lineno + ")";
        }

        public String getValueAsName()
        {
            return TrimOkurigana(value);
        }

        public static String TrimOkurigana(String name)
        {
            String s = "";
            int cur = 0;
            Char c;
            if (name == "") return "";
            c = name[cur];

            // 一文字目がひらがななら省略は難しい
            if (NakoTokenizer.IsHira(c))
            {
                return name;
            }
            s += c;
            cur++;

            // 送りがなを省略する
            while (cur < name.Length)
            {
                c = name[cur];
                if (!NakoTokenizer.IsHira(c))
                {
                    s += c;
                }
                cur++;
            }
            return s;
        }
    }
}
