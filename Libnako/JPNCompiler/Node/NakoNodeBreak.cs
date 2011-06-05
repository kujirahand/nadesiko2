using System;
using System.Collections.Generic;
using System.Text;

namespace Libnako.JPNCompiler.Node
{
    class NakoNodeBreak : NakoNodeJump
    {
        public NakoNodeBreak()
        {
            type = NakoNodeType.BREAK;
        }
    }
}
