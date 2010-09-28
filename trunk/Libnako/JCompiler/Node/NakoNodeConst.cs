using System;
using System.Collections.Generic;

using System.Text;

namespace Libnako.JCompiler.Node
{
    public class NakoNodeConst : NakoNode
    {
        public override String ToTypeString()
        {
            String r = type.ToString();
            r += "=";
            r += Token.value;
            return r;
        }
    }

}
