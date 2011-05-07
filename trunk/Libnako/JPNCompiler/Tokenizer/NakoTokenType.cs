
using System;
using System.Collections.Generic;
using System.Text;

namespace Libnako.JPNCompiler.Tokenizer
{
    /// <summary>
    /// トークンの種類を定義したもの
    /// </summary>
    public enum NakoTokenType
    {
        // --- Define Token ---
        ///<summary>UNKNOWN</summary>
        UNKNOWN,
        // --- Literal ---
        ///<summary>RESERVED</summary>
        RESERVED,
        ///<summary>COMMENT</summary>
        COMMENT,
        ///<summary>EOL</summary>
        EOL,
        ///<summary>WORD</summary>
        WORD,
        ///<summary>FUNCTION_NAME</summary>
        FUNCTION_NAME,
        ///<summary>NUMBER</summary>
        NUMBER,
        ///<summary>INT</summary>
        INT,
        ///<summary>STRING</summary>
        STRING,
        ///<summary>STRING_EX</summary>
        STRING_EX,
        // --- Flow Controll ---
        ///<summary>IF</summary>
        IF,
        ///<summary>WHILE</summary>
        WHILE,
        ///<summary>SWITCH</summary>
        SWITCH,
        ///<summary>FOR</summary>
        FOR,
        ///<summary>FOREACH</summary>
        FOREACH,
        ///<summary>REPEAT_TIMES</summary>
        REPEAT_TIMES,
        ///<summary>BREAK</summary>
        BREAK,
        ///<summary>CONTINUE</summary>
        CONTINUE,
        // ---  ---
        ///<summary>THEN</summary>
        THEN,
        ///<summary>ELSE</summary>
        ELSE,
        ///<summary>KOKOMADE</summary>
        KOKOMADE,
        ///<summary>SCOPE_BEGIN</summary>
        SCOPE_BEGIN,
        ///<summary>SCOPE_END</summary>
        SCOPE_END,
        // ---  ---
        ///<summary>DEF_FUNCTION</summary>
        DEF_FUNCTION,
        ///<summary>DEF_GROUP</summary>
        DEF_GROUP,
        // --- Flags ---
        ///<summary>EQ</summary>
        EQ,
        ///<summary>EQ_EQ</summary>
        EQ_EQ,
        ///<summary>NOT_EQ</summary>
        NOT_EQ,
        ///<summary>GT</summary>
        GT,
        ///<summary>GT_EQ</summary>
        GT_EQ,
        ///<summary>LT</summary>
        LT,
        ///<summary>LT_EQ</summary>
        LT_EQ,
        ///<summary>NOT</summary>
        NOT,
        ///<summary>AND</summary>
        AND,
        ///<summary>AND_AND</summary>
        AND_AND,
        ///<summary>OR</summary>
        OR,
        ///<summary>OR_OR</summary>
        OR_OR,
        ///<summary>PLUS</summary>
        PLUS,
        ///<summary>MINUS</summary>
        MINUS,
        ///<summary>MUL</summary>
        MUL,
        ///<summary>DIV</summary>
        DIV,
        ///<summary>MOD</summary>
        MOD,
        ///<summary>POWER</summary>
        POWER,
        ///<summary>YEN</summary>
        YEN,
        // --- 角カッコ ---
        ///<summary>BLACKETS_L</summary>
        BLACKETS_L,
        ///<summary>BLACKETS_R</summary>
        BLACKETS_R,
        // --- 丸括弧 ---
        ///<summary>PARENTHESES_L</summary>
        PARENTHESES_L,
        ///<summary>PARENTHESES_R</summary>
        PARENTHESES_R,
        // --- 波括弧(中括弧) ---
        ///<summary>BRACES_L</summary>
        BRACES_L,
        ///<summary>BRACES_R</summary>
        BRACES_R,
        // --- 数値、整数、文字列 ---
        ///<summary>DIM_NUMBER</summary>
        DIM_NUMBER,
        ///<summary>DIM_INT</summary>
        DIM_INT,
        ///<summary>DIM_STRING</summary>
        DIM_STRING,
        ///<summary>DIM_VARIABLE</summary>
        DIM_VARIABLE,
        ///<summary>DIM_ARRAY</summary>
        DIM_ARRAY,
        // --- DEBUG ---
        ///<summary>PRINT</summary>
        PRINT
    }
}
