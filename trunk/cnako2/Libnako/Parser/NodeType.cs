
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
        N_BLOCK = 1,
        N_VARIABLE = 2,
        N_LET = 3,
        N_FORMULA = 4,
        N_VALUE = 5,
        N_INT = 6,
        N_NUMBER = 7,
        N_STRING = 8,
        N_CALC = 9,
        // DEBUG
        N_PRINT = 10,

        END_OF_NODE
    }
    public class NodeTypeDescriptor
    {
        public static String[] TypeName = new String[] {
"N_NOP","N_BLOCK","N_VARIABLE","N_LET","N_FORMULA","N_VALUE","N_INT","N_NUMBER","N_STRING",
"N_CALC","N_PRINT",
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
