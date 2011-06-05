using System;
using System.Collections.Generic;
using System.Text;

namespace Libnako.JPNCompiler.Node
{
    class NakoNodeContinue : NakoNodeJump
    {
        public NakoNodeContinue()
        {
            type = NakoNodeType.CONTINUE;
        }
    }
}
