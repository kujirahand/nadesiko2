
using System;
using System.Collections.Generic;
using System.Text;

namespace Libnako.JPNCompiler.Node
{
    // Define Node Type
    public enum NakoNodeType
    {
        // NODE
        NOP,
        POP,

        // Blocks
        BLOCK,
        BLOCKS,
        IF,
        WHILE,
        FOR,
        FOREACH,
        REPEAT_TIMES,
        BREAK,
        CONTINUE,

        // Const
        INT,
        NUMBER,
        STRING,

        // Value
        FORMULA,
        VALUE,
        CALC,
        LD_VARIABLE,

        // Let
        LET,
        LET_VALUE,
        ST_VARIABLE,

        // Function
        CALL_FUNCTION,
        DEF_FUNCTION,

        // DEBUG
        PRINT
    }
}
