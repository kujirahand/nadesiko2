
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
        IF = 3,

        // Const
        INT = 4,
        NUMBER = 5,
        STRING = 6,

        // Value
        FORMULA = 7,
        VALUE = 8,
        CALC = 9,
        LD_VARIABLE = 10,

        // Let
        LET = 11,
        ST_VARIABLE = 12,

        // Function
        CALL_FUNCTION = 13,
        DEF_FUNCTION = 14,

        // DEBUG
        PRINT = 15,

        END_OF_NODE
    }
}
