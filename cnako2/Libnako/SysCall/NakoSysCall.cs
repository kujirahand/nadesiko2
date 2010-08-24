using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Libnako.JCompiler.Function;

namespace Libnako.SysCall
{
    public delegate void SysCallDelegate(NakoFuncCallInfo info);

    public class NakoSysCall : NakoFunc
    {
        public SysCallDelegate FuncDl;

        public NakoSysCall(String name, String argdef, SysCallDelegate FuncDl)
            : base(name, argdef)
        {
            this.FuncDl = FuncDl;
        }

        public override void Init()
        {
            base.Init();
            funcType = NakoFuncType.SysCall;
        }
    }
}
