
using System;
using System.Collections.Generic;
using System.Text;

namespace Libnako.Parser
{
    public enum NodeType
    {
        // Define Token
        // NODE
        N_NOP = 0,

        // Blocks
        N_BLOCK = 1,
        N_BLOCKS = 2,

        // Const
        N_INT = 3,
        N_NUMBER = 4,
        N_STRING = 5,

        // Value
        N_FORMULA = 6,
        N_VALUE = 7,
        N_CALC = 8,
        N_LD_VARIABLE = 9,

        // Let
        N_LET = 10,
        N_ST_VARIABLE = 11,

        // Function
        N_CALL_FUNCTION = 12,
        N_DEF_FUNCTION = 13,

        // DEBUG
        N_PRINT = 14,

        END_OF_NODE
    }
}
