using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Libnako.JCompiler.Tokenizer;

namespace Libnako.JCompiler.Node
{
    class NakoNodeFor : NakoNode
    {
        public NakoNodeVariable loopVar;
        public NakoNode nodeFrom;
        public NakoNode nodeTo;
        public NakoNode nodeBlocks;

        public NakoNodeFor()
        {
            type = NakoNodeType.FOR;
        }
    }
}
