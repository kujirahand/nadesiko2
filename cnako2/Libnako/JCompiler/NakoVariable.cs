using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Libnako.JCompiler
{
    public enum NakoVariableType
    {
        Void,
        Object,
        Int,
        Real,
        String,
        Array,
        Group,
        UserFunc,
        SystemFunc
    }
    
    public class NakoVariable
    {
		public NakoVariableType type { get; set; }
		public Object value { get; set; }
    }

}
