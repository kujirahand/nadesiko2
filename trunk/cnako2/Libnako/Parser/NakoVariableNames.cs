using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Libnako.Parser
{
    public class NakoVariableNames : Dictionary<String, int>
    {
        public int createName(String name)
        {
            if (!this.ContainsKey(name))
            {
                int i = this.Count + 1;
                this[name] = i;
                return i;
            }
            return this[name];
        }
    }
}
