using System;
using System.Collections.Generic;
using System.Text;

namespace Libnako.JPNCompiler.Node
{
    class NakoNodeWhile : NakoNode
    {
		public NakoNode nodeCond { get; set; }
		public NakoNode nodeBlocks { get; set; }

        public NakoNodeWhile()
        {
            type = NakoNodeType.WHILE;
        }
    }
}
