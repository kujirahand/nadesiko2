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
            String r = NodeTypeDescriptor.GetTypeName(type);
            r += "=";
            r += Token.value;
            return r;
        }
    }

}
