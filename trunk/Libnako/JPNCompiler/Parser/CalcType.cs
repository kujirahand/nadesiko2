
using System;
using System.Collections.Generic;
using System.Text;

namespace Libnako.JPNCompiler.Parser
{
    /// <summary>
    /// 演算タイプ
    /// </summary>
    public enum CalcType
    {
        ///<summary>NOP</summary>
        NOP,
        ///<summary>ADD</summary>
        ADD,
        ///<summary>SUB</summary>
        SUB,
        ///<summary>MUL</summary>
        MUL,
        ///<summary>DIV</summary>
        DIV,
        ///<summary>MOD %</summary>
        MOD,
        ///<summary>POWER ^</summary>
        POWER,
        ///<summary>ADD_STR &amp;</summary>
        ADD_STR,
        ///<summary>=  or ==</summary>
        EQ,
        ///<summary>!= or &gt;&lt;</summary>
        NOT_EQ,
        ///<summary>GT</summary>
        GT,
        ///<summary>GT_EQ</summary>
        GT_EQ,
        ///<summary>LT</summary>
        LT,
        ///<summary>LT_EQ</summary>
        LT_EQ,
        ///<summary>&amp;&amp;</summary>
        AND,
        ///<summary>||</summary>
        OR,
        ///<summary>~~</summary>
        XOR,
        ///<summary>!</summary>
        NEG
    }
}
