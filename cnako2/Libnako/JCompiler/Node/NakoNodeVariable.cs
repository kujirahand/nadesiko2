using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Libnako.JCompiler.Parser;

namespace Libnako.JCompiler.Node
{
    public class NakoNodeVariable : NakoNode
    {
        // public NodeType type = N_LD_VARIABLE || N_ST_VARIABLE
        public NakoVariableScope scope = NakoVariableScope.Global;
        public NakoVariableType varType = NakoVariableType.Int;
        public int varNo = -1;
        public Boolean useElement = false; // a[3] のように配列かプロパティへのアクセスかどうか？

        public Boolean IsVarTypeSimple()
        {
            return (varType == NakoVariableType.Int ||
                varType == NakoVariableType.Real ||
                varType == NakoVariableType.String);
        }
    }
}
