
using System;
using System.Collections.Generic;
using System.Text;

namespace Libnako.Parser
{
    public class CalcType
    {
        // Define Token
        //
        public const int NOP = 0; // ???
        //
        public const int ADD = 1; // +
        public const int SUB = 2; // -
        public const int MUL = 3; // *
        public const int DIV = 4; // /
        public const int MOD = 5; // %
        public const int POWER = 6; // ^
        public const int ADD_STR = 7; // &
        // comp
        public const int EQ = 8; // ==
        public const int NOT_EQ = 9; // <> !=
        public const int GT = 10; // >
        public const int GT_EQ = 11; // >=
        public const int LT = 12; // <
        public const int LT_EQ = 13; // <=
        //
        public const int AND = 14; // &&
        public const int OR = 15; // ||
        public const int XOR = 16; // ~~
        //
        public const int NEG = 17; // !

        // Token Description
        public static String[] TypeName = new String[] {
" ???"," +"," -"," *"," /"," %","^","&"," ==",
"<> !="," >",">="," <","<="," &&"," ||"," ~~"," !",
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
/*
        case CalcType.NOP: c.type = NakoILType.NOP; break;
        case CalcType.ADD: c.type = NakoILType.ADD; break;
        case CalcType.SUB: c.type = NakoILType.SUB; break;
        case CalcType.MUL: c.type = NakoILType.MUL; break;
        case CalcType.DIV: c.type = NakoILType.DIV; break;
        case CalcType.MOD: c.type = NakoILType.MOD; break;
        case CalcType.POWER: c.type = NakoILType.POWER; break;
        case CalcType.ADD_STR: c.type = NakoILType.ADD_STR; break;
        case CalcType.EQ: c.type = NakoILType.EQ; break;
        case CalcType.NOT_EQ: c.type = NakoILType.NOT_EQ; break;
        case CalcType.GT: c.type = NakoILType.GT; break;
        case CalcType.GT_EQ: c.type = NakoILType.GT_EQ; break;
        case CalcType.LT: c.type = NakoILType.LT; break;
        case CalcType.LT_EQ: c.type = NakoILType.LT_EQ; break;
        case CalcType.AND: c.type = NakoILType.AND; break;
        case CalcType.OR: c.type = NakoILType.OR; break;
        case CalcType.XOR: c.type = NakoILType.XOR; break;
        case CalcType.NEG: c.type = NakoILType.NEG; break;

*/
}
