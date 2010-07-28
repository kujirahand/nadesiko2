
using System;
using System.Collections.Generic;
using System.Text;

namespace Libnako.Parser
{
    public enum TokenType
    {
        // Define Token
        UNKNOWN = 0,
        // Literal
        RESERVED = 1,
        COMMENT = 2,
        EOL = 3,
        WORD = 4,
        FUNCTION_NAME = 5,
        NUMBER = 6,
        INT = 7,
        STRING = 8,
        STRING_EX = 9,
        // Flow Controll
        IF = 10,
        WHILE = 11,
        SWITCH = 12,
        FOR = 13,
        FOREACH = 14,
        //
        THEN = 15,
        KOKOMADE = 16,
        SCOPE_BEGIN = 17,
        SCOPE_END = 18,
        //
        DEF_FUNCTION = 19,
        DEF_GROUP = 20,
        // Flags
        EQ = 21,
        EQ_EQ = 22,
        NOT_EQ = 23,
        GT = 24,
        GT_EQ = 25,
        LT = 26,
        LT_EQ = 27,
        NOT = 28,
        AND = 29,
        AND_AND = 30,
        OR = 31,
        OR_OR = 32,
        PLUS = 33,
        MINUS = 34,
        MUL = 35,
        DIV = 36,
        MOD = 37,
        POWER = 38,
        // 角カッコ
        BLACKETS_L = 39,
        BLACKETS_R = 40,
        // 丸括弧
        PARENTHESES_L = 41,
        PARENTHESES_R = 42,
        // 波括弧(中括弧)
        BRACES_L = 43,
        BRACES_R = 44,
        // DEBUG
        PRINT = 45,

        END_OF_TOKEN
    }
}
