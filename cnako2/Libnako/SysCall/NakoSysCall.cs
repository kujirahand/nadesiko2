using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Libnako.JCompiler.Function;
using Libnako.JCompiler;

namespace Libnako.SysCall
{
    public delegate void SysCallDelegate(NakoFuncCallInfo info);

    public class NakoSysCall : NakoFunc
    {
        public SysCallDelegate FuncDl;

        public NakoSysCall(String name, String argdef, NakoVariableType resultType, SysCallDelegate FuncDl)
            : base(name, argdef)
        {
            this.FuncDl = FuncDl;
            this.resultType = resultType;
        }

        public override void Init()
        {
            base.Init();
            funcType = NakoFuncType.SysCall;
        }
    }
}
