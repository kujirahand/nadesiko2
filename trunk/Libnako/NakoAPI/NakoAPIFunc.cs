using System;
using System.Collections.Generic;
using System.Text;
using Libnako.JCompiler.Function;
using Libnako.JCompiler;
using NakoPlugin;

namespace Libnako.NakoAPI
{
    /// <summary>
    /// なでしこプラグイン関数を定義したもの
    /// </summary>
    public class NakoAPIFunc : NakoFunc
    {
        /// <summary>
        /// Dll Function Delegate
        /// </summary>
        public SysCallDelegate FuncDl { get; set; }
        
        public Boolean Used { get; set; }

        public NakoAPIFunc(String name, String argdef, NakoVarType resultType, SysCallDelegate FuncDl)
            : base(name, argdef)
        {
            this.FuncDl = FuncDl;
            this.resultType = resultType;
            this.Used = false;
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
