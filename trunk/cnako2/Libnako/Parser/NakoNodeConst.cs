using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Libnako.Parser
{
    public class NakoNodeConst : NakoNode
    {
        public override String ToTypeString()
        {
            String r = NodeType.GetNodeName(type);
            r += "=";
            r += Token.value;
            return r;
        }
    }

}
