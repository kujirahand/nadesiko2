using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Libnako.JCompiler.Node
{
    public class NakoNodeLet : NakoNode
    {
		public NakoNodeVariable nodeVar { get; set; }

        public NakoNodeLet()
        {
            type = NakoNodeType.LET;
        }
    }
}
