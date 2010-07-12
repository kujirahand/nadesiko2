
using System;
using System.Collections.Generic;
using System.Text;

namespace Libnako.Parser
{
    public class TokenType
    {
        // Define Token
        public const int T_UNKNOWN = 0;
        // Literal
        public const int T_RESERVED = 1;
        public const int T_COMMENT = 2;
        public const int T_EOL = 3;
        public const int T_WORD = 4;
        public const int T_NUMBER = 5;
        public const int T_INT = 6;
        public const int T_STRING = 7;
        public const int T_STRING_EX = 8;
        // Flow Controll
        public const int T_IF = 9;
        public const int T_WHILE = 10;
        public const int T_SWITCH = 11;
        public const int T_FOR = 12;
        public const int T_FOREACH = 13;
        //
        public const int T_THEN = 14;
        public const int T_KOKOMADE = 15;
        //
        public const int T_DEF_FUNCTION = 16;
        public const int T_DEF_GROUP = 17;
        // Flags
        public const int T_EQ = 18;
        public const int T_EQ_EQ = 19;
        public const int T_NOT_EQ = 20;
        public const int T_GT = 21;
        public const int T_GT_EQ = 22;
        public const int T_LT = 23;
        public const int T_LT_EQ = 24;
        public const int T_NOT = 25;
        public const int T_AND = 26;
        public const int T_AND_AND = 27;
        public const int T_PLUS = 28;
        public const int T_MINUS = 29;
        public const int T_MUL = 30;
        public const int T_DIV = 31;
        public const int T_MOD = 32;
        public const int T_POWER = 33;
        // 角カッコ
        public const int T_BLACKETS_L = 34;
        public const int T_BLACKETS_R = 35;
        // 丸括弧
        public const int T_PARENTHESES_L = 36;
        public const int T_PARENTHESE_R = 37;
        // 波括弧(中括弧)
        public const int T_BRANCES_L = 38;
        public const int T_BRANCES_R = 39;

        // Token Description
        public static String[] TokenName = new String[] {
"T_UNKNOWN","T_RESERVED","T_COMMENT","T_EOL","T_WORD","T_NUMBER","T_INT","T_STRING","T_STRING_EX",

"T_IF","T_WHILE","T_SWITCH","T_FOR","T_FOREACH","T_THEN","T_KOKOMADE","T_DEF_FUNCTION","T_DEF_GROUP","T_EQ",
"T_EQ_EQ","T_NOT_EQ","T_GT","T_GT_EQ","T_LT","T_LT_EQ","T_NOT","T_AND","T_AND_AND","T_PLUS",
"T_MINUS","T_MUL","T_DIV","T_MOD","T_POWER","T_BLACKETS_L","T_BLACKETS_R","T_PARENTHESES_L","T_PARENTHESE_R","T_BRANCES_L",
"T_BRANCES_R",
        };
        // Description Method
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
