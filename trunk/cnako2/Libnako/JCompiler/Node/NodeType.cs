
using System;
using System.Collections.Generic;
using System.Text;

namespace Libnako.JCompiler.Node
{
    // Define Node Type
    public enum NodeType
    {
        // NODE
        NOP,

        // Blocks
        BLOCK,
        BLOCKS,
        IF,
        WHILE,
        FOR,
        FOREACH,
        REPEAT_TIMES,

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
        ST_VARIABLE,

        // Function
        CALL_FUNCTION,
        DEF_FUNCTION,

        // DEBUG
        PRINT
    }
}
