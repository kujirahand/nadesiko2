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
        public int kaisuVarNo { get; set; }

        public NakoNodeFor()
        {
            type = NakoNodeType.FOR;
        }

        /// <summary>
        /// タイプ文字列を得る
        /// </summary>
        /// <returns></returns>
        public override string ToTypeString()
        {
            string r = type.ToString() + "\n";
            r += "  |-- FROM: " + nodeFrom.ToTypeString() + "\n";
            r += "  |-- TO  : " + nodeTo.ToTypeString() + "\n";
            r += "  |-- BLOCKS:\n" + nodeBlocks.ToTypeString() + "\n";
            return r;
        }
    }
}
