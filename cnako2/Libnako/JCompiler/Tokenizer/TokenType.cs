
using System;
using System.Collections.Generic;
using System.Text;

namespace Libnako.Parser.Tokenizer
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
        ELSE = 16,
        KOKOMADE = 17,
        SCOPE_BEGIN = 18,
        SCOPE_END = 19,
        //
        DEF_FUNCTION = 20,
        DEF_GROUP = 21,
        // Flags
        EQ = 22,
        EQ_EQ = 23,
        NOT_EQ = 24,
        GT = 25,
        GT_EQ = 26,
        LT = 27,
        LT_EQ = 28,
        NOT = 29,
        AND = 30,
        AND_AND = 31,
        OR = 32,
        OR_OR = 33,
        PLUS = 34,
        MINUS = 35,
        MUL = 36,
        DIV = 37,
        MOD = 38,
        POWER = 39,
        // 角カッコ
        BLACKETS_L = 40,
        BLACKETS_R = 41,
        // 丸括弧
        PARENTHESES_L = 42,
        PARENTHESES_R = 43,
        // 波括弧(中括弧)
        BRACES_L = 44,
        BRACES_R = 45,
        // DEBUG
        PRINT = 46,

        END_OF_TOKEN
    }
}
