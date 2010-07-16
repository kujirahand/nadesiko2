using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Libnako.Parser
{
    public class NakoVariables : List<Object>
    {
        public Object GetValue(int no)
        {
            if (no < 0) return null;
            if (no >= this.Count) return null;
            return this[no];
        }

        public void SetValue(int no, Object value)
        {
            while (no >= this.Count)
            {
                this.Add(null);
            }
            this[no] = value;
        }
    }
}
