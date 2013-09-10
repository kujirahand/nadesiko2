using System;

namespace Libnako.JPNCompiler.Node
{
    class NakoNodeReturn : NakoNodeJump
    {
        public NakoNodeReturn()
        {
            type = NakoNodeType.RETURN;
        }
    }
}
