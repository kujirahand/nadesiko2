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
		public int varNo { get; set; }
		public String name { get; set; }
		public NakoFuncArgs args { get; set; }
		public NakoFuncType funcType { get; set; }
		public NakoVariableType resultType { get; set; }

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
			funcType = NakoFuncType.UserCall;
			resultType = NakoVariableType.Void;
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
