using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Libnako.JCompiler.Function;
using Libnako.JCompiler.Parser;
using Libnako.JCompiler.ILWriter;

namespace Libnako.JCompiler.Node
{
    public class NakoNodeDefFunction : NakoNode
    {
        public String funcName;
        public NakoFunc func;
        public NakoVariables localVar;
        public NakoNode funcBody;
        public NakoILCode defLabel;

        public NakoNodeDefFunction()
        {
            type = NakoNodeType.DEF_FUNCTION;
            localVar = new NakoVariables(NakoVariableScope.Local);
        }

        public void RegistArgsToLocalVar()
        {
            for (int i = 0; i < func.args.Count; i++)
            {
                NakoFuncArg arg = func.args[i];
                localVar.CreateVar(arg.name);
            }
        }

    }

    public class NakoNodeDefFunctionList : List<NakoNodeDefFunction>
    {
    }
}

