using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Libnako.JCompiler.Function
{
    public class NakoFuncArg
    {
        public String name;
        public List<String> josiList;

        public NakoFuncArg()
        {
            josiList = new List<String>();    
        }

        public void AddJosi(String josi)
        {
            if (!josiList.Contains(josi))
            {
                josiList.Add(josi);
            }
        }
    }
}
