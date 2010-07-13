
using System;
using System.Collections.Generic;
using System.Text;

namespace Libnako.Parser
{
    public class NakoILType
    {
        // Define Token
        // NOP
        public const int I_NOP = 0;
        // LITERAL
        public const int I_CONST_INT = 1;
        public const int I_CONST_NUM = 2;
        public const int I_CONST_STR = 3;
        // CALC
        public const int I_ADD = 4;
        public const int I_SUB = 5;
        public const int I_MUL = 6;
        public const int I_DIV = 7;
        public const int I_INC = 8;

        // Token Description
        public static String[] TypeName = new String[] {
"I_NOP","I_CONST_INT","I_CONST_NUM","I_CONST_STR","I_ADD","I_SUB","I_MUL","I_DIV","I_INC",

        };
        // Description Method
        public static String GetTypeName(int no)
        {
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
