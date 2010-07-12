
using System;
using System.Collections.Generic;
using System.Text;

namespace Libnako.Parser
{
    public class NodeType
    {
        // Define Token
        // NODE
        public const int N_NOP = 0;
        public const int N_BLOCK = 1;
        public const int N_VARIABLE = 2;
        public const int N_LET = 3;
        public const int N_FORMULA = 4;
        public const int N_VALUE = 5;
        public const int N_INT = 6;
        public const int N_NUMBER = 7;
        public const int N_STRING = 8;
        public const int N_CALC = 9;

        // Token Description
        public static String[] NodeName = new String[] {
"N_NOP","N_BLOCK","N_VARIABLE","N_LET","N_FORMULA","N_VALUE","N_INT","N_NUMBER","N_STRING",
"N_CALC",
        };
        // Description Method
        public static String GetNodeName(int no)
        {
            if (NodeName.Length > no) {
                return NodeName[no];
            }
            else
            {
                return "UNKNOWN";
            }
        }

    }
}
