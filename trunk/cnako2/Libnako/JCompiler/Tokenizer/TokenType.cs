
using System;
using System.Collections.Generic;
using System.Text;

namespace Libnako.JCompiler.Tokenizer
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
        REPEAT_TIMES = 15,
        //
        THEN = 16,
        ELSE = 17,
        KOKOMADE = 18,
        SCOPE_BEGIN = 19,
        SCOPE_END = 20,
        //
        DEF_FUNCTION = 21,
        DEF_GROUP = 22,
        // Flags
        EQ = 23,
        EQ_EQ = 24,
        NOT_EQ = 25,
        GT = 26,
        GT_EQ = 27,
        LT = 28,
        LT_EQ = 29,
        NOT = 30,
        AND = 31,
        AND_AND = 32,
        OR = 33,
        OR_OR = 34,
        PLUS = 35,
        MINUS = 36,
        MUL = 37,
        DIV = 38,
        MOD = 39,
        POWER = 40,
        // 角カッコ
        BLACKETS_L = 41,
        BLACKETS_R = 42,
        // 丸括弧
        PARENTHESES_L = 43,
        PARENTHESES_R = 44,
        // 波括弧(中括弧)
        BRACES_L = 45,
        BRACES_R = 46,
        // DEBUG
        PRINT = 47,

        END_OF_TOKEN
    }
}
