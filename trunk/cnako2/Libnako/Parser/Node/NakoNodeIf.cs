using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Libnako.Parser.Node
{
    public class NakoNodeIf : NakoNode
    {
        public NakoNode nodeCond;
        public NakoNode nodeTrue;
        public NakoNode nodeFalse;
    }
}
