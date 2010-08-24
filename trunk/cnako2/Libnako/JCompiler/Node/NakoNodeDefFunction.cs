using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Libnako.JCompiler.Function;
using Libnako.JCompiler.Parser;

namespace Libnako.JCompiler.Node
{
    public class NakoNodeDefFunction : NakoNode
    {
        public String funcName;
        public NakoFuncArgs args;
        public NakoVariables localVar;

        public NakoNodeDefFunction()
        {
            type = NodeType.DEF_FUNCTION;
            localVar = new NakoVariables();
        }

        public void RegistArgsToLocalVar()
        {
            // TODO: 引数をローカル変数に登録する
        }

    }

    public class NakoNodeDefFunctionList : List<NakoNodeDefFunction>
    {
    }
}
