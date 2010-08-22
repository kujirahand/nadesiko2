using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Libnako.JCompiler.Node
{
    class NakoNodeWhile : NakoNode
    {
        public NakoNode nodeCond;
        public NakoNode nodeBlocks;

        public NakoNodeWhile()
        {
            type = NodeType.WHILE;
        }
    }
}
