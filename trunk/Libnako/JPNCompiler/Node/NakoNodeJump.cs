using System;
using System.Collections.Generic;
using System.Text;

using Libnako.Interpreter.ILCode;

namespace Libnako.JPNCompiler.Node
{
    class NakoNodeJump : NakoNode
    {
        public NakoILCode label;

        public NakoNodeJump()
        {
            type = NakoNodeType.JUMP;
        }
    }
}
