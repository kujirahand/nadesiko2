using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Libnako.JCompiler.Node
{
    public class NakoNodeCallFunction : NakoNode
    {
        public NakoNodeCallFunction()
        {
            type = NakoNodeType.CALL_FUNCTION;
        }
    }
}
