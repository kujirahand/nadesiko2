
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
        T_FUNCTION_NAME = 5,
        T_NUMBER = 6,
        T_INT = 7,
        T_STRING = 8,
        T_STRING_EX = 9,
        // Flow Controll
        T_IF = 10,
        T_WHILE = 11,
        T_SWITCH = 12,
        T_FOR = 13,
        T_FOREACH = 14,
        //
        T_THEN = 15,
        T_KOKOMADE = 16,
        T_SCOPE_BEGIN = 17,
        T_SCOPE_END = 18,
        //
        T_DEF_FUNCTION = 19,
        T_DEF_GROUP = 20,
        // Flags
        T_EQ = 21,
        T_EQ_EQ = 22,
        T_NOT_EQ = 23,
        T_GT = 24,
        T_GT_EQ = 25,
        T_LT = 26,
        T_LT_EQ = 27,
        T_NOT = 28,
        T_AND = 29,
        T_AND_AND = 30,
        T_OR = 31,
        T_OR_OR = 32,
        T_PLUS = 33,
        T_MINUS = 34,
        T_MUL = 35,
        T_DIV = 36,
        T_MOD = 37,
        T_POWER = 38,
        // 角カッコ
        T_BLACKETS_L = 39,
        T_BLACKETS_R = 40,
        // 丸括弧
        T_PARENTHESES_L = 41,
        T_PARENTHESES_R = 42,
        // 波括弧(中括弧)
        T_BRACES_L = 43,
        T_BRACES_R = 44,
        // DEBUG
        T_PRINT = 45,

        END_OF_TOKEN
    }
}
