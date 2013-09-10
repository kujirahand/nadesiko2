
using System;
using System.Collections.Generic;
using System.Text;

namespace Libnako.JPNCompiler.Node
{
    /// <summary>
    /// ノードの種類を定義したもの
    /// </summary>
    public enum NakoNodeType
    {
        // --- NODE ---
        ///<summary>NOP</summary>
        NOP,
        ///<summary>POP</summary>
        POP,

        // --- Blocks ---
        ///<summary>BLOCK</summary>
        BLOCK,
        ///<summary>BLOCKS</summary>
        BLOCKS,
        ///<summary>IF</summary>
        IF,
        ///<summary>WHILE</summary>
        WHILE,
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
        ///<summary>RETURN</summary>
        RETURN,

        // --- Const ---
        ///<summary>INT</summary>
        INT,
        ///<summary>NUMBER</summary>
        NUMBER,
        ///<summary>STRING</summary>
        STRING,

        // --- Value ---
        ///<summary>FORMULA</summary>
        FORMULA,
        ///<summary>VALUE</summary>
        VALUE,
        ///<summary>CALC</summary>
        CALC,
        ///<summary>LD_VARIABLE</summary>
        LD_VARIABLE,

        // --- Let ---
        ///<summary>LET</summary>
        LET,
        ///<summary>LET_VALUE</summary>
        LET_VALUE,
        ///<summary>ST_VARIABLE</summary>
        ST_VARIABLE,

        // --- Function ---
        ///<summary>CALL_FUNCTION</summary>
        CALL_FUNCTION,
        ///<summary>DEF_FUNCTION</summary>
        DEF_FUNCTION,

        // --- DEBUG ---
        ///<summary>PRINT</summary>
        PRINT,

        // --- OTHER ---
        ///<summary>JUMP</summary>
        JUMP
    }
}
