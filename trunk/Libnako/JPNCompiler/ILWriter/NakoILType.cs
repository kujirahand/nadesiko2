
using System;
using System.Collections.Generic;
using System.Text;

namespace Libnako.JPNCompiler.ILWriter
{
    /// <summary>
    /// なでしこ2の仮想バイトコード(IL)を定義したもの
    /// </summary>
    public enum NakoILType
    {
        // Define Token
        // 何もしない
        NOP               = 0x00, // {args:0, push:0, pop:0} 

        // --- STACKへのPUSH系統 ---
        // 定数をスタックに乗せる
        LD_CONST_INT      = 0x01, // {args:1, push:1, pop:0} 
        LD_CONST_REAL     = 0x02, // {args:1, push:1, pop:0} 
        LD_CONST_STR      = 0x03, // {args:1, push:1, pop:0} 

        //  変数をスタックに乗せる load(varno)
        LD_GLOBAL         = 0x10, // {args:1, push:1, pop:0} 
        LD_LOCAL          = 0x11, // {args:1, push:1, pop:0}
        LD_GLOBAL_REF     = 0x12, // {args:1, push:1, pop:0}
        LD_LOCAL_REF      = 0x13, // {args:1, push:1, pop:0}     

        // --- STACKからのPOP系統 ---
        // スタックから下ろして値を変数に記憶する set(varno)
        ST_GLOBAL         = 0x20, // {args:1, push:0, pop:1} 
        ST_LOCAL          = 0x21, // {args:1, push:0, pop:1}
        POP               = 0x22, // {args:1, push:0, pop:1}

        // --- STACKからの配列操作 ---
        // 配列
        NEW_ARR           = 0x30, // { args:1, push:1, pop:0 } 
        ST_ELEM           = 0x31, // { args:0, push:0, pop:3 }
        LD_ELEM           = 0x32, // { args:0, push:0, pop:2 }
        LD_ELEM_REF       = 0x33, // { args:0, push:0, pop:2 }

        // 計算する
        ADD              = 0x40, // { args:0, push:1, pop:2 } 
        SUB              = 0x41, // { args:0, push:1, pop:2 } 
        MUL              = 0x42, // { args:0, push:1, pop:2 } 
        DIV              = 0x43, // { args:0, push:1, pop:2 } 
        MOD              = 0x44, // { args:0, push:1, pop:2 } 
        POWER            = 0x45, // { args:0, push:1, pop:2 } 
        ADD_STR          = 0x46, // { args:0, push:1, pop:2 } 
        // 比較する
        EQ               = 0x50, // { args:0, push:1, pop:2 } 
        NOT_EQ           = 0x51, // { args:0, push:1, pop:2 } 
        GT               = 0x52, // { args:0, push:1, pop:2 } 
        GT_EQ            = 0x53, // { args:0, push:1, pop:2 } 
        LT               = 0x54, // { args:0, push:1, pop:2 } 
        LT_EQ            = 0x55, // { args:0, push:1, pop:2 } 
        // スタックトップの値を増減する
        INC              = 0x56, // { args:0, push:1, pop:1 } 
        DEC              = 0x57, // { args:0, push:1, pop:1 } 
        NEG              = 0x58, // { args:0, push:1, pop:1 } 
        // 演算
        AND              = 0x59, // { args:0, push:1, pop:2 } 
        OR               = 0x5A, // { args:0, push:1, pop:2 } 
        XOR              = 0x5B, // { args:0, push:1, pop:2 } 
        NOT              = 0x5C, // { args:0, push:1, pop:1 } 

        // アドレスジャンプ
        JUMP             = 0x60, // { args:1, push:0, pop:0 } 
        BRANCH_TRUE      = 0x61, // { args:1, push:0, pop:1 } 
        BRANCH_FALSE     = 0x62, // { args:1, push:0, pop:1 } 

        // 関数用
        SYSCALL          = 0x70, // { args:1, push:1, pop:? } 
        USRCALL          = 0x71, // { args:1, push:1, pop:? } 
        RET              = 0x72, // { args:0, push:0, pop:0 } 

        // DEBUG用
        PRINT            = 0x80, // { args:0, push:0, pop:1 } 

    }
}
