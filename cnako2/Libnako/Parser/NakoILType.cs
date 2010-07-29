
using System;
using System.Collections.Generic;
using System.Text;

namespace Libnako.Parser
{
    public enum NakoILType
    {
        // Define Token
        // 何もしない
        NOP               = 0, // {args:0, push:0, pop:0} 

        // 定数をスタックに乗せる
        LD_CONST_INT      = 1, // {args:1, push:1, pop:0} 
        LD_CONST_REAL     = 2, // {args:1, push:1, pop:0} 
        LD_CONST_STR      = 3, // {args:1, push:1, pop:0} 

        //  変数をスタックに乗せる load(varno)
        LD_GLOBAL         = 4, // {args:1, push:1, pop:0} 
        LD_LOCAL          = 5, // {args:1, push:1, pop:0} 

        // スタックから下ろして値を変数に記憶する set(varno)
        ST_GLOBAL         = 6, // {args:1, push:0, pop:1} 
        ST_LOCAL          = 7, // {args:1, push:0, pop:1} 

        // 配列
        NEW_ARR           = 8, // { args:1, push:1, pop:0 } 
        ST_ARR_ELEM       = 9, // { args:0, push:0, pop:3 } stack [var, index, value]
        LD_ARR_ELEM      = 10, // { args:0, push:1, pop:2 } stack [bar, index]

        // 計算する
        ADD              = 11, // { args:0, push:1, pop:2 } 
        SUB              = 12, // { args:0, push:1, pop:2 } 
        MUL              = 13, // { args:0, push:1, pop:2 } 
        DIV              = 14, // { args:0, push:1, pop:2 } 
        MOD              = 15, // { args:0, push:1, pop:2 } 
        POWER            = 16, // { args:0, push:1, pop:2 } 
        ADD_STR          = 17, // { args:0, push:1, pop:2 } 
        // 比較する
        EQ               = 18, // { args:0, push:1, pop:2 } 
        NOT_EQ           = 19, // { args:0, push:1, pop:2 } 
        GT               = 20, // { args:0, push:1, pop:2 } 
        GT_EQ            = 21, // { args:0, push:1, pop:2 } 
        LT               = 22, // { args:0, push:1, pop:2 } 
        LT_EQ            = 23, // { args:0, push:1, pop:2 } 
        // スタックトップの値を増減する
        INC              = 24, // { args:0, push:1, pop:1 } 
        DEC              = 25, // { args:0, push:1, pop:1 } 
        NEG              = 26, // { args:0, push:1, pop:1 } 
        // 演算
        AND              = 27, // { args:0, push:1, pop:2 } 
        OR               = 28, // { args:0, push:1, pop:2 } 
        XOR              = 29, // { args:0, push:1, pop:2 } 
        NOT              = 30, // { args:0, push:1, pop:1 } 

        // アドレスジャンプ
        JUMP             = 31, // { args:1, push:0, pop:0 } 
        CALL             = 32, // { args:1, push:0, pop:0 } 
        RET              = 33, // { args:0, push:0, pop:0 } 
        BRANCH_TRUE      = 34, // { args:1, push:0, pop:1 } 
        BRANCH_FALSE     = 35, // { args:1, push:0, pop:1 } 

        // DEBUG用
        PRINT            = 36, // { args:0, push:0, pop:1 } 


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
        case NakoILType.ADD_STR:
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
        case NakoILType.NOT:
        case NakoILType.JUMP:
        case NakoILType.CALL:
        case NakoILType.RET:
        case NakoILType.BRANCH_TRUE:
        case NakoILType.BRANCH_FALSE:
        case NakoILType.PRINT:

*/
}
