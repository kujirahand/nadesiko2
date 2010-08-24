using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Libnako.JCompiler.Parser
{
    public enum NakoVariableType
    {
        Object,
        Int,
        Real,
        String,
        Array,
        Group,
        UserFunc,
        SysCall
    }
}
