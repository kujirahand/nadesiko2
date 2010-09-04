using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Libnako.JCompiler.Tokenizer;

namespace Libnako.JCompiler.Node
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
