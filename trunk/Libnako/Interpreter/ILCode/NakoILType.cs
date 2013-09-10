
using System;
using System.Collections.Generic;
using System.Text;

namespace Libnako.Interpreter.ILCode
{
    /// <summary>
    /// なでしこ2の仮想バイトコード(IL)を定義したもの
    /// </summary>
    public enum NakoILType
    {

        // --- 何もしない ---
        ///<summary>NOP</summary>
        NOP               = 0x00, // {args:0, push:0, pop:0}

        // --- --- STACKへのPUSH系統 --- ---
        // --- 定数をスタックに乗せる ---
        ///<summary>LD_CONST_INT</summary>
        LD_CONST_INT      = 0x01, // {args:1, push:1, pop:0}
        ///<summary>LD_CONST_REAL</summary>
        LD_CONST_REAL     = 0x02, // {args:1, push:1, pop:0}
        ///<summary>LD_CONST_STR</summary>
        LD_CONST_STR      = 0x03, // {args:1, push:1, pop:0}

        // --- 変数をスタックに乗せる load(varno) ---
        ///<summary>LD_GLOBAL</summary>
        LD_GLOBAL         = 0x10, // {args:1, push:1, pop:0}
        ///<summary>LD_LOCAL</summary>
        LD_LOCAL          = 0x11, // {args:1, push:1, pop:0}
        ///<summary>LD_GLOBAL_REF</summary>
        LD_GLOBAL_REF     = 0x12, // {args:1, push:1, pop:0}
        ///<summary>LD_LOCAL_REF</summary>
        LD_LOCAL_REF      = 0x13, // {args:1, push:1, pop:0}

        // --- --- STACKからのPOP系統 --- ---
        // --- スタックから下ろして値を変数に記憶する set(varno) ---
        ///<summary>ST_GLOBAL</summary>
        ST_GLOBAL         = 0x20, // {args:1, push:0, pop:1}
        ///<summary>ST_LOCAL</summary>
        ST_LOCAL          = 0x21, // {args:1, push:0, pop:1}
        ///<summary>POP</summary>
        POP               = 0x22, // {args:1, push:0, pop:1}

        // --- --- STACKからの配列操作 --- ---
        // --- 配列 ---
        ///<summary>NEW_ARR</summary>
        NEW_ARR           = 0x30, // { args:1, push:1, pop:0 }
        ///<summary>ST_ELEM</summary>
        ST_ELEM           = 0x31, // { args:0, push:0, pop:3 }
        ///<summary>LD_ELEM</summary>
        LD_ELEM           = 0x32, // { args:0, push:1, pop:2 }
        ///<summary>LD_ELEM_REF</summary>
        LD_ELEM_REF       = 0x33, // { args:0, push:1, pop:2 }
        ///<summary>ARR_LENGTH</summary>
        ARR_LENGTH        = 0x34, // { args:0, push:1, pop:1 }

        // --- 計算する ---
        ///<summary>ADD</summary>
        ADD              = 0x40, // { args:0, push:1, pop:2 }
        ///<summary>SUB</summary>
        SUB              = 0x41, // { args:0, push:1, pop:2 }
        ///<summary>MUL</summary>
        MUL              = 0x42, // { args:0, push:1, pop:2 }
        ///<summary>DIV</summary>
        DIV              = 0x43, // { args:0, push:1, pop:2 }
        ///<summary>MOD</summary>
        MOD              = 0x44, // { args:0, push:1, pop:2 }
        ///<summary>POWER</summary>
        POWER            = 0x45, // { args:0, push:1, pop:2 }
        ///<summary>ADD_STR</summary>
        ADD_STR          = 0x46, // { args:0, push:1, pop:2 }
        // --- 比較する ---
        ///<summary>EQ</summary>
        EQ               = 0x50, // { args:0, push:1, pop:2 }
        ///<summary>NOT_EQ</summary>
        NOT_EQ           = 0x51, // { args:0, push:1, pop:2 }
        ///<summary>GT</summary>
        GT               = 0x52, // { args:0, push:1, pop:2 }
        ///<summary>GT_EQ</summary>
        GT_EQ            = 0x53, // { args:0, push:1, pop:2 }
        ///<summary>LT</summary>
        LT               = 0x54, // { args:0, push:1, pop:2 }
        ///<summary>LT_EQ</summary>
        LT_EQ            = 0x55, // { args:0, push:1, pop:2 }
        // --- スタックトップの値を増減する ---
        ///<summary>INC</summary>
        INC              = 0x56, // { args:0, push:1, pop:1 }
        ///<summary>DEC</summary>
        DEC              = 0x57, // { args:0, push:1, pop:1 }
        ///<summary>NEG</summary>
        NEG              = 0x58, // { args:0, push:1, pop:1 }

        // --- 演算 ---
        ///<summary>AND</summary>
        AND              = 0x59, // { args:0, push:1, pop:2 }
        ///<summary>OR</summary>
        OR               = 0x5A, // { args:0, push:1, pop:2 }
        ///<summary>XOR</summary>
        XOR              = 0x5B, // { args:0, push:1, pop:2 }
        ///<summary>NOT</summary>
        NOT              = 0x5C, // { args:0, push:1, pop:1 }

        // --- アドレスジャンプ ---
        ///<summary>JUMP</summary>
        JUMP             = 0x60, // { args:1, push:0, pop:0 }
        ///<summary>BRANCH_TRUE</summary>
        BRANCH_TRUE      = 0x61, // { args:1, push:0, pop:1 }
        ///<summary>BRANCH_FALSE</summary>
        BRANCH_FALSE     = 0x62, // { args:1, push:0, pop:1 }

        // --- 関数用 ---
        ///<summary>SYSCALL</summary>
        SYSCALL          = 0x70, // { args:1, push:1, pop:? }
        ///<summary>USRCALL</summary>
        USRCALL          = 0x71, // { args:1, push:1, pop:? }
        ///<summary>RET</summary>
        RET              = 0x72, // { args:0, push:0, pop:0 }

        // --- DEBUG用 ---
        ///<summary>PRINT</summary>
        PRINT            = 0x80, // { args:0, push:0, pop:1 }

        // --- ローカル変数に対する操作 ---
        ///<summary>ローカル変数の値を1増やす</summary>
        INC_LOCAL = 0x90, // { args:1, push:0, pop:0 }
        ///<summary>ローカル変数の値を1減らす</summary>
        DEC_LOCAL = 0x91, // { args:1, push:0, pop:0 }

        // --- スタックに対する操作 ---
        /// <summary>スタックの先頭要素を複製する</summary>
        DUP = 0x95, // { args:0, push:1, pop:0 }

        /// --- 内部使用(実際に書き出されることはない ---
        ///<summary>_BREAK</summary>
        _BREAK      = 0xF1, // { args:0, push:0, pop:0 }
        ///<summary>_CONTINUE</summary>
        _CONTINUE = 0xF2, // { args:0, push:0, pop:0 }
        ///<summary>_RETURN</summary>
        _RETURN = 0xF3, // { args:0, push:0, pop:0 }
    }
}
