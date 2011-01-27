using System;
using System.Collections.Generic;

using System.Text;
using Libnako.JPNCompiler.Tokenizer;

namespace Libnako.JPNCompiler.Node
{
    class NakoNodeRepeatTimes : NakoNode
    {
		public NakoNode nodeTimes { get; set; }
		public NakoNode nodeBlocks { get; set; }
		public int loopVarNo { get; set; }

        public NakoNodeRepeatTimes()
        {
            type = NakoNodeType.REPEAT_TIMES;
        }
    }
}
