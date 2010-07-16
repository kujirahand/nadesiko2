
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
    public class NodeTypeDescriptor
    {
        public static String[] TypeName = new String[] {
"N_NOP","N_BLOCK","N_BLOCKS","N_INT","N_NUMBER","N_STRING","N_FORMULA","N_VALUE","N_CALC",
"N_LD_VARIABLE","N_LET","N_ST_VARIABLE","N_CALL_FUNCTION","N_DEF_FUNCTION","N_PRINT",
        };
        // Description Method
        public static String GetTypeName(NodeType n)
        {
            int no = (int)n;
            if (TypeName.Length > no) {
                return TypeName[no];
            }
            else
            {
                return "UNKNOWN";
            }
        }
    }
}
