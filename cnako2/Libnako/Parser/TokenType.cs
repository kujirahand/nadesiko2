
using System;
using System.Collections.Generic;
using System.Text;

namespace Libnako.Parser
{
    public enum TokenType
    {
        // Define Token
        T_UNKNOWN = 0,
        // Literal
        T_RESERVED = 1,
        T_COMMENT = 2,
        T_EOL = 3,
        T_WORD = 4,
        T_NUMBER = 5,
        T_INT = 6,
        T_STRING = 7,
        T_STRING_EX = 8,
        // Flow Controll
        T_IF = 9,
        T_WHILE = 10,
        T_SWITCH = 11,
        T_FOR = 12,
        T_FOREACH = 13,
        //
        T_THEN = 14,
        T_KOKOMADE = 15,
        //
        T_DEF_FUNCTION = 16,
        T_DEF_GROUP = 17,
        // Flags
        T_EQ = 18,
        T_EQ_EQ = 19,
        T_NOT_EQ = 20,
        T_GT = 21,
        T_GT_EQ = 22,
        T_LT = 23,
        T_LT_EQ = 24,
        T_NOT = 25,
        T_AND = 26,
        T_AND_AND = 27,
        T_PLUS = 28,
        T_MINUS = 29,
        T_MUL = 30,
        T_DIV = 31,
        T_MOD = 32,
        T_POWER = 33,
        // 角カッコ
        T_BLACKETS_L = 34,
        T_BLACKETS_R = 35,
        // 丸括弧
        T_PARENTHESES_L = 36,
        T_PARENTHESES_R = 37,
        // 波括弧(中括弧)
        T_BRANCES_L = 38,
        T_BRANCES_R = 39,
        // DEBUG
        T_PRINT = 40,

        END_OF_TOKEN
    }
    public class TokenTypeDescriptor
    {
        // Token Description
        public static String[] TypeName = new String[] {
"T_UNKNOWN","T_RESERVED","T_COMMENT","T_EOL","T_WORD","T_NUMBER","T_INT","T_STRING","T_STRING_EX",

"T_IF","T_WHILE","T_SWITCH","T_FOR","T_FOREACH","T_THEN","T_KOKOMADE","T_DEF_FUNCTION","T_DEF_GROUP","T_EQ",
"T_EQ_EQ","T_NOT_EQ","T_GT","T_GT_EQ","T_LT","T_LT_EQ","T_NOT","T_AND","T_AND_AND","T_PLUS",
"T_MINUS","T_MUL","T_DIV","T_MOD","T_POWER","T_BLACKETS_L","T_BLACKETS_R","T_PARENTHESES_L","T_PARENTHESES_R","T_BRANCES_L",
"T_BRANCES_R","T_PRINT",
        };
        // Description Method
        public static String GetTypeName(TokenType t)
        {
            int no = (int)t;
            if (TypeName.Length > no) {
                return TypeName[no];
            }
            else
            {
                return "UNKNOWN";
            }
        }
    }
}
