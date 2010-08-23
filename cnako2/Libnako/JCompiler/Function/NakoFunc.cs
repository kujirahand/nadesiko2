using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Libnako.JCompiler.Function
{
    public class NakoFunc
    {
        public String name;
        public NakoFuncArgs args;
        public NakoFuncType funcType = NakoFuncType.UserCall; 

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
    }

    public enum NakoFuncType
    {
        SysCall,
        UserCall
    }
}
