using System;
using System.Collections.Generic;
using System.Text;
using Libnako.JCompiler.Function;
using Libnako.JCompiler;
using NakoPlugin;

namespace Libnako.NakoAPI
{
    public class NakoAPIFunc : NakoFunc
    {
		public SysCallDelegate FuncDl { get; set; }

        public NakoAPIFunc(String name, String argdef, NakoVarType resultType, SysCallDelegate FuncDl)
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

        public override void Execute()
        {
        }

    }
}
