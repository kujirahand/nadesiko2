using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Libnako.Parser.Node
{
    public class NakoNodeLet : NakoNode
    {
        public NakoNodeVariable nodeVar;

        public NakoNodeLet()
        {
            type = NodeType.LET;
        }
    }
}
