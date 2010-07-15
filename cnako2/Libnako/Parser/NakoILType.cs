
using System;
using System.Collections.Generic;
using System.Text;

namespace Libnako.Parser
{
    public class NakoILType
    {
        // Define Token
        // 何もしない
        public const int NOP               = 0; // {args:0, push:0, pop:0} 

        // 定数をスタックに乗せる
        public const int LD_CONST_INT      = 1; // {args:1, push:1, pop:0} 
        public const int LD_CONST_REAL     = 2; // {args:1, push:1, pop:0} 
        public const int LD_CONST_STR      = 3; // {args:1, push:1, pop:0} 

        //  変数をスタックに乗せる load(varno)
        public const int LD_GLOBAL         = 4; // {args:1, push:1, pop:0} 
        public const int LD_LOCAL          = 5; // {args:1, push:1, pop:0} 

        // スタックから下ろして値を変数に記憶する set(varno)
        public const int ST_GLOBAL         = 6; // {args:1, push:0, pop:1} 
        public const int ST_LOCAL          = 7; // {args:1, push:0, pop:1} 

        // 配列
        public const int NEW_ARR           = 8; // { args:1, push:1, pop:0 } 
        public const int ST_ARR_ELEM       = 9; // { args:0, push:0, pop:3 } stack [var, index, value]
        public const int LD_ARR_ELEM      = 10; // { args:0, push:1, pop:2 } stack [bar, index]

        // 計算する
        public const int ADD              = 11; // { args:0, push:1, pop:2 } 
        public const int SUB              = 12; // { args:0, push:1, pop:2 } 
        public const int MUL              = 13; // { args:0, push:1, pop:2 } 
        public const int DIV              = 14; // { args:0, push:1, pop:2 } 
        public const int MOD              = 15; // { args:0, push:1, pop:2 } 
        public const int POWER            = 16; // { args:0, push:1, pop:2 } 
        // 比較する
        public const int EQ               = 17; // { args:0, push:1, pop:2 } 
        public const int NOT_EQ           = 18; // { args:0, push:1, pop:2 } 
        public const int GT               = 19; // { args:0, push:1, pop:2 } 
        public const int GT_EQ            = 20; // { args:0, push:1, pop:2 } 
        public const int LT               = 21; // { args:0, push:1, pop:2 } 
        public const int LT_EQ            = 22; // { args:0, push:1, pop:2 } 
        // スタックトップの値を増減する
        public const int INC              = 23; // { args:0, push:1, pop:1 } 
        public const int DEC              = 24; // { args:0, push:1, pop:1 } 
        public const int NEG              = 25; // { args:0, push:1, pop:1 } 
        // 演算
        public const int AND              = 26; // { args:0, push:1, pop:2 } 
        public const int OR               = 27; // { args:0, push:1, pop:2 } 
        public const int XOR              = 28; // { args:0, push:1, pop:2 } 
        public const int NOT		            = 29; // { args:0, push:1, pop:1 } 

        // アドレスジャンプ
        public const int JUMP             = 30; // { args:1, push:0, pop:0 } 
        public const int CALL             = 31; // { args:1, push:0, pop:0 } 
        public const int RET              = 32; // { args:0, push:0, pop:0 } 



        // Token Description
        public static String[] TypeName = new String[] {
"NOP","LD_CONST_INT","LD_CONST_REAL","LD_CONST_STR","LD_GLOBAL","LD_LOCAL","ST_GLOBAL","ST_LOCAL","NEW_ARR",
"ST_ARR_ELEM","LD_ARR_ELEM","ADD","SUB","MUL","DIV","MOD","POWER","EQ","NOT_EQ",
"GT","GT_EQ","LT","LT_EQ","INC","DEC","NEG","AND","OR","XOR",
"NOT		","JUMP","CALL","RET",
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
/*
        case NakoILType.NOP:
        case NakoILType.LD_CONST_INT:
        case NakoILType.LD_CONST_REAL:
        case NakoILType.LD_CONST_STR:
        case NakoILType.LD_GLOBAL:
        case NakoILType.LD_LOCAL:
        case NakoILType.ST_GLOBAL:
        case NakoILType.ST_LOCAL:
        case NakoILType.NEW_ARR:
        case NakoILType.ST_ARR_ELEM:
        case NakoILType.LD_ARR_ELEM:
        case NakoILType.ADD:
        case NakoILType.SUB:
        case NakoILType.MUL:
        case NakoILType.DIV:
        case NakoILType.MOD:
        case NakoILType.POWER:
        case NakoILType.EQ:
        case NakoILType.NOT_EQ:
        case NakoILType.GT:
        case NakoILType.GT_EQ:
        case NakoILType.LT:
        case NakoILType.LT_EQ:
        case NakoILType.INC:
        case NakoILType.DEC:
        case NakoILType.NEG:
        case NakoILType.AND:
        case NakoILType.OR:
        case NakoILType.XOR:
        case NakoILType.NOT		:
        case NakoILType.JUMP:
        case NakoILType.CALL:
        case NakoILType.RET:

*/
    }
}
