
using System;
using System.Collections.Generic;
using System.Text;

namespace Libnako.JPNCompiler.Tokenizer
{
    /// <summary>
    /// トークンの種類を表します。
    /// </summary>
    public enum NakoTokenType
    {
        // --- トークン定義 (Define Token) ---
        ///<summary>トークンの種類は不明です。</summary>
        UNKNOWN,
        // --- リテラル (Literal) ---
        ///<summary>予約されたトークンの種類です。</summary>
        RESERVED,
        ///<summary>トークンの種類はコメントです。</summary>
        COMMENT,
        ///<summary>トークンの種類は行末です。</summary>
        EOL,
        ///<summary>トークンの種類は単語です。</summary>
        WORD,
        ///<summary>トークンの種類は関数の名前です。</summary>
        FUNCTION_NAME,
        ///<summary>トークンの種類は数値です。</summary>
        NUMBER,
        ///<summary>トークンの種類は整数です。</summary>
        INT,
        ///<summary>トークンの種類は文字列です。</summary>
        STRING,
        ///<summary>トークンの種類は展開あり文字列です。</summary>
        STRING_EX,
        // --- フロー制御 (Flow Controll) ---
        ///<summary>トークンの種類は "もし" 文です。</summary>
        IF,
        ///<summary>トークンの種類は "間" 文です。</summary>
        WHILE,
        ///<summary>トークンの種類は "条件分岐" 文です。</summary>
        SWITCH,
        ///<summary>トークンの種類は "繰り返す" 文です。</summary>
        FOR,
        ///<summary>トークンの種類は "反復" 文です。</summary>
        FOREACH,
        ///<summary>トークンの種類は "回数" 文です。</summary>
        REPEAT_TIMES,
        ///<summary>トークンの種類は "抜ける" 文です。</summary>
        BREAK,
        ///<summary>トークンの種類は "続ける" 文です。</summary>
        CONTINUE,
        ///<summary>トークンの種類は "戻る" 文です。</summary>
        RETURN,
        // ---  ---
        ///<summary>トークンの種類は "Then" です。</summary>
        THEN,
        ///<summary>トークンの種類は "違えば" です。</summary>
        ELSE,
        ///<summary>トークンの種類は "ここまで" です。</summary>
        KOKOMADE,
        ///<summary>トークンの種類はスコープ開始です。</summary>
        SCOPE_BEGIN,
        ///<summary>トークンの種類はスコープ終了です。</summary>
        SCOPE_END,
        // ---  ---
        ///<summary>トークンの種類は関数定義です。</summary>
        DEF_FUNCTION,
        ///<summary>トークンの種類はグループ定義です。</summary>
        DEF_GROUP,
        // --- Flags ---
        ///<summary>トークンの種類は代入演算子 (Equal, "=") です。</summary>
        EQ,
        ///<summary>トークンの種類は等価演算子 (EqualEqual, "==") です。</summary>
        EQ_EQ,
        ///<summary>トークンの種類は非等価演算子 (NotEqual, "!=") です。</summary>
        NOT_EQ,
        ///<summary>トークンの種類は大なり演算子 (GreaterThan, "&gt;") です。</summary>
        GT,
        ///<summary>トークンの種類は大なりイコール演算子 (GreaterThanEqual, "&gt;=") です。</summary>
        GT_EQ,
        ///<summary>トークンの種類は小なり演算子 (LessThan, "&lt;") です。</summary>
        LT,
        ///<summary>トークンの種類は小なりイコール演算子 (LessThanEqual, "&lt;=") です。</summary>
        LT_EQ,
        ///<summary>トークンの種類は Not 演算子 ("!") です。</summary>
        NOT,
        ///<summary>トークンの種類は And 演算子 ("&amp;") です。</summary>
        AND,
        ///<summary>トークンの種類は AndAnd 演算子 ("&amp;&amp;") です。</summary>
        AND_AND,
        ///<summary>トークンの種類は Or 演算子 ("|") です。</summary>
        OR,
        ///<summary>トークンの種類は OrOr 演算子 ("||") です。</summary>
        OR_OR,
        ///<summary>トークンの種類は加算演算子 (Plus, "+") です。</summary>
        PLUS,
        ///<summary>トークンの種類は減算演算子 (Minus, "-") です。</summary>
        MINUS,
        ///<summary>トークンの種類は乗算演算子 (Multiplication, "*") です。</summary>
        MUL,
        ///<summary>トークンの種類は除算演算子 (Division, "/") です。</summary>
        DIV,
        ///<summary>トークンの種類は剰余算演算子 (Modulus, "%") です。</summary>
        MOD,
        ///<summary>トークンの種類は Power 演算子 ("^") です。</summary>
        POWER,
        ///<summary>トークンの種類は円記号 (Yen, "\") です。</summary>
        YEN,
        ///<summary>トークンの種類は左角括弧 (BracketsLeft, "[") です。</summary>
        BRACKETS_L,
        ///<summary>トークンの種類は右角括弧 (BracketsRight, "]") です。</summary>
        BRACKETS_R,
        ///<summary>トークンの種類は左丸括弧 (ParenthesesLeft, "(") です。</summary>
        PARENTHESES_L,
        ///<summary>トークンの種類は右丸括弧 (ParenthesesRight, ")") です。</summary>
        PARENTHESES_R,
        ///<summary>トークンの種類は左波括弧 (BracesLeft, "{") です。</summary>
        BRACES_L,
        ///<summary>トークンの種類は右波括弧 (BracesRight, "}") です。</summary>
        BRACES_R,
        // --- 数値、整数、文字列 ---
        ///<summary>トークンの種類は DIM_NUMBER です。</summary>
        DIM_NUMBER,
        ///<summary>トークンの種類は DIM_INT です。</summary>
        DIM_INT,
        ///<summary>トークンの種類は DIM_STRING です。</summary>
        DIM_STRING,
        ///<summary>トークンの種類は DIM_VARIABLE です。</summary>
        DIM_VARIABLE,
        ///<summary>トークンの種類は DIM_ARRAY です。</summary>
        DIM_ARRAY,
        // --- デバック (DEBUG) ---
        ///<summary>トークンの種類は PRINT </summary>
		PRINT,
		// --- exception ---
		TRY,
		CATCH,
		FINALLY,
		THROW,
		DEF_CLASS
    }
}
