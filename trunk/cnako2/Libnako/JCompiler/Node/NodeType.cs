
using System;
using System.Collections.Generic;
using System.Text;

namespace Libnako.Parser.Node
{
    public enum NodeType
    {
        // Define Token
        // NODE
        NOP = 0,

        // Blocks
        BLOCK = 1,
        BLOCKS = 2,
        IF = 3,
        WHILE = 4,
        FOR = 5,
        FOREACH = 6,
        REPEAT_TIMES = 7,

        // Const
        INT = 8,
        NUMBER = 9,
        STRING = 10,

        // Value
        FORMULA = 11,
        VALUE = 12,
        CALC = 13,
        LD_VARIABLE = 14,

        // Let
        LET = 15,
        ST_VARIABLE = 16,

        // Function
        CALL_FUNCTION = 17,
        DEF_FUNCTION = 18,

        // DEBUG
        PRINT = 19,

        END_OF_NODE
    }
}
