using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Libnako.JCompiler.Function;


namespace Libnako.JCompiler.Node
{
    /// <summary>
    /// なでしこの関数呼び出しを行うノード
    /// </summary>
    public class NakoNodeCallFunction : NakoNode
    {
        public List<NakoNode> argNodes = new List<NakoNode>();
        public NakoFunc func = new NakoFunc();

        public NakoNodeCallFunction()
        {
            type = NakoNodeType.CALL_FUNCTION;
        }
    }
}
