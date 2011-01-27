using System;
using System.Collections.Generic;

using System.Text;
using Libnako.JPNCompiler.Tokenizer;

namespace Libnako.JPNCompiler.Node
{
    class NakoNodeFor : NakoNode
    {
		public NakoNodeVariable loopVar { get; set; }
		public NakoNode nodeFrom { get; set; }
		public NakoNode nodeTo { get; set; }
		public NakoNode nodeBlocks { get; set; }

        public NakoNodeFor()
        {
            type = NakoNodeType.FOR;
        }
    }
}
