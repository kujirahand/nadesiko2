using System;
using System.Collections.Generic;

using System.Text;
using Libnako.JPNCompiler.Tokenizer;

namespace Libnako.JPNCompiler.Node
{
    class NakoNodeForeach : NakoNode
    {
		public NakoNode nodeValue {get; set; }
		public NakoNode nodeBlocks { get; set; }
        public int loopVarNo { get; set; }
        public int lenVarNo { get; set; }
        public int valueVarNo { get; set; }
        public int taisyouVarNo { get; set; }
        public int kaisuVarNo { get; set; }

        public NakoNodeForeach()
        {
            type = NakoNodeType.FOREACH;
        }
    }
}
