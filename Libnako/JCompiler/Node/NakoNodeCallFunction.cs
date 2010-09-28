using System;
using System.Collections.Generic;
using System.Text;
using Libnako.JCompiler.Function;


namespace Libnako.JCompiler.Node
{
    /// <summary>
    /// なでしこの関数呼び出しを行うノード
    /// </summary>
    public class NakoNodeCallFunction : NakoNode
    {
        public NakoFunc func = new NakoFunc();

        public NakoNodeCallFunction()
        {
            type = NakoNodeType.CALL_FUNCTION;
        }

        public NakoNodeList argNodes
        {
            get { return this.Children; }
        }

        public override String ToTypeString()
        {
            string r = type.ToString();
            r += "(" + this.func.name + "{args:" + func.args.Count + "})";
            return r;
        }
    }
}
