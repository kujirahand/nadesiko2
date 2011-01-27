
using System;
using System.Collections.Generic;
using System.Text;

namespace Libnako.JPNCompiler.Tokenizer
{
    public enum NakoTokenType
    {
        // Define Token
        UNKNOWN,
        // Literal
        RESERVED,
        COMMENT,
        EOL,
        WORD,
        FUNCTION_NAME,
        NUMBER,
        INT,
        STRING,
        STRING_EX,
        // Flow Controll
        IF,
        WHILE,
        SWITCH,
        FOR,
        FOREACH,
        REPEAT_TIMES,
        //
        THEN,
        ELSE,
        KOKOMADE,
        SCOPE_BEGIN,
        SCOPE_END,
        //
        DEF_FUNCTION,
        DEF_GROUP,
        // Flags
        EQ,
        EQ_EQ,
        NOT_EQ,
        GT,
        GT_EQ,
        LT,
        LT_EQ,
        NOT,
        AND,
        AND_AND,
        OR,
        OR_OR,
        PLUS,
        MINUS,
        MUL,
        DIV,
        MOD,
        POWER,
        YEN,
        // 角カッコ
        BLACKETS_L,
        BLACKETS_R,
        // 丸括弧
        PARENTHESES_L,
        PARENTHESES_R,
        // 波括弧(中括弧)
        BRACES_L,
        BRACES_R,
        // 数値、整数、文字列
        DIM_NUMBER,
        DIM_INT,
        DIM_STRING,
        DIM_VARIABLE,
        DIM_ARRAY,
        // DEBUG
        PRINT
    }
}
