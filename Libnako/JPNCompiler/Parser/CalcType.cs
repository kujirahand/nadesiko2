
using System;
using System.Collections.Generic;
using System.Text;

namespace Libnako.JPNCompiler.Parser
{
    public enum CalcType
    {
        NOP,
        ADD,
        SUB,
        MUL,
        DIV,
        MOD,    // %
        POWER,  // ^
        ADD_STR,// &
        EQ,     // =  or ==
        NOT_EQ, // != or <>
        GT,
        GT_EQ,
        LT,
        LT_EQ,
        AND,    // &&
        OR,     // ||
        XOR,    // ~~
        NEG     // !
    }
}
