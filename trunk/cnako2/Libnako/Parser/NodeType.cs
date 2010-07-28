
using System;
using System.Collections.Generic;
using System.Text;

namespace Libnako.Parser
{
    public enum NodeType
    {
        // Define Token
        // NODE
        NOP = 0,

        // Blocks
        BLOCK = 1,
        BLOCKS = 2,

        // Const
        INT = 3,
        NUMBER = 4,
        STRING = 5,

        // Value
        FORMULA = 6,
        VALUE = 7,
        CALC = 8,
        LD_VARIABLE = 9,

        // Let
        LET = 10,
        ST_VARIABLE = 11,

        // Function
        CALL_FUNCTION = 12,
        DEF_FUNCTION = 13,

        // DEBUG
        PRINT = 14,

        END_OF_NODE
    }
}
