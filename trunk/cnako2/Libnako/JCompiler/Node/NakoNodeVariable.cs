using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Libnako.JCompiler.Parser;
using Libnako.JCompiler.Function;

namespace Libnako.JCompiler.Node
{
    public class NakoNodeVariable : NakoNode
    {
        // public NodeType type = N_LD_VARIABLE || N_ST_VARIABLE
		public NakoVariableScope scope { get; set; }
		public NakoVariableType varType { get; set; }
		public int varNo { get; set; }
		public VarByType varBy { get; set; }
		public Boolean useElement { get; set; } // a[3] のように配列かプロパティへのアクセスかどうか？

        public Boolean IsVarTypeSimple()
        {
            return (varType == NakoVariableType.Int ||
                varType == NakoVariableType.Real ||
                varType == NakoVariableType.String);
        }

		public NakoNodeVariable()
		{
			this.scope = NakoVariableScope.Global;
			this.varType = NakoVariableType.Int;
			this.varNo = -1;
			this.varBy = VarByType.ByVal;
			this.useElement = false;
		}
    }
}
