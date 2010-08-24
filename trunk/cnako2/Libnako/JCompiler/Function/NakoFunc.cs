using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Libnako.JCompiler.Function
{
    /// <summary>
    /// なでしこ関数を定義したもの
    /// </summary>
    public class NakoFunc
    {
        public int varNo;
        public String name;
        public NakoFuncArgs args;
        public NakoFuncType funcType = NakoFuncType.UserCall;
        public NakoVariableType resultType = NakoVariableType.Void;

        public NakoFunc()
        {
            Init();
        }

        public NakoFunc(String name, String argdef)
        {
            Init();
            this.name = name;
            this.args.analizeArgStr(argdef);
        }

        public virtual void Init()
        {
            args = new NakoFuncArgs();
        }

        public virtual void Execute() { }

        public int ArgCount
        {
            get
            {
                return args.Count;
            }
        }

    }

    public enum NakoFuncType
    {
        SysCall,
        UserCall
    }
}
