using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Libnako.JCompiler.Function
{
    public enum VarByType { ByVal/*値渡し*/, ByRef/*参照渡し*/ };

    public class NakoFuncArg
    {
		public String name { get; set; }
		public List<String> josiList { get; set; }
		public VarByType varBy { get; set; }

        public NakoFuncArg()
        {
			varBy = VarByType.ByVal;
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
