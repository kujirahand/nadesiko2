﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Libnako.Parser
{
    public class NakoNodeLet : NakoNode
    {
        public NakoNodeVariable nodeVar;

        public NakoNodeLet()
        {
            type = NodeType.N_LET;
        }
    }
}
