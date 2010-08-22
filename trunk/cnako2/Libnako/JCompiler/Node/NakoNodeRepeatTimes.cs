using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Libnako.JCompiler.Tokenizer;

namespace Libnako.JCompiler.Node
{
    class NakoNodeRepeatTimes : NakoNode
    {
        public NakoNode nodeTimes;
        public NakoNode nodeBlocks;
        public int loopVarNo;

        public NakoNodeRepeatTimes()
        {
            type = NodeType.REPEAT_TIMES;
        }
    }
}
