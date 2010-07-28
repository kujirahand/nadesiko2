using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Libnako.Parser.Node
{
    public class NakoNodeDefFunction : NakoNode
    {
        public String funcName;
        public NakoFuncArgs args;
        public NakoVariableNames localVar;

        public NakoNodeDefFunction()
        {
            type = NodeType.N_DEF_FUNCTION;
            localVar = new NakoVariableNames();
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
