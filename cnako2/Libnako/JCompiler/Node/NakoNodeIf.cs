using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Libnako.JCompiler.Node
{
    public class NakoNodeIf : NakoNode
    {
		public NakoNode nodeCond { get; set; }
		public NakoNode nodeTrue { get; set; }
		public NakoNode nodeFalse { get; set; }

        public NakoNodeIf()
        {
            this.type = NakoNodeType.IF;
        }
    }
}
