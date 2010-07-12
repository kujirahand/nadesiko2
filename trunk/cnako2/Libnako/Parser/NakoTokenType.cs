
using System;
using System.Collections.Generic;
using System.Text;

namespace Libnako.Parser
{
    public class TokenType
    {
        // Define Token
        public static int T_RESERVED = 0;
        public static int T_UNKNOWN = 1;
        public static int T_COMMENT = 2;
        public static int T_EOL = 3;
        public static int T_WORD = 4;
        public static int T_NUMBER = 5;
        public static int T_INT = 6;
        public static int T_STRING = 7;
        public static int T_STRING_EX = 8;
        //
        public static int T_IF = 10;
        public static int T_WHILE = 11;
        public static int T_SWITCH = 12;
        public static int T_FOR = 13;
        public static int T_FOREACH = 14;
        //
        public static int T_THEN = 16;
        public static int T_KOKOMADE = 17;
        //
        public static int T_DEF_FUNCTION = 19;
        public static int T_DEF_GROUP = 20;
        //
        public static int T_EQ = 22;
        public static int T_EQ_EQ = 23;
        public static int T_NOT_EQ = 24;
        public static int T_GT = 25;
        public static int T_GT_EQ = 26;
        public static int T_LT = 27;
        public static int T_LT_EQ = 28;
        public static int T_NOT = 29;
        public static int T_AND = 30;
        public static int T_AND_AND = 31;

        // Token Description
        public static String[] TokenName = new String[] {
"T_RESERVED","T_UNKNOWN","T_COMMENT","T_EOL","T_WORD","T_NUMBER","T_INT","T_STRING","T_STRING_EX",
"T_IF","T_WHILE","T_SWITCH","T_FOR","T_FOREACH","T_THEN","T_KOKOMADE","T_DEF_FUNCTION",
"T_DEF_GROUP","T_EQ","T_EQ_EQ","T_NOT_EQ","T_GT","T_GT_EQ","T_LT","T_LT_EQ","T_NOT",
"T_AND","T_AND_AND",
        };
        //
        public string value = null;
        public int lineno = 0;
        public int level = 0;
        public int type = 0;
        public string josi = "";

        public NakoToken(int type = 0, int lineno = 0, int level = 0)
        {
            this.lineno = lineno;
            this.level = level;
            this.type = type;
        }
        
        public static String GetTokenName(int no)
        {
            if (TokenName.Length > no) {
                return TokenName[no];
            }
            else
            {
                return "UNKNOWN";
            }
        }

    }
}
