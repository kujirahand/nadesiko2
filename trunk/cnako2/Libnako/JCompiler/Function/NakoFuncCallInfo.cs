using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Libnako.Interpreter;

namespace Libnako.JCompiler.Function
{
    public class NakoFuncCallInfo
    {
        private NakoInterpreter _runner;

        public NakoFuncCallInfo(NakoInterpreter interpreter)
        {
            this._runner = interpreter;
        }

        public NakoInterpreter Runner
        {
            get { return _runner; }
        }

        public Object StackPop()
        {
            return _runner.StackPop();
        }

        public void StackPush(Object v)
        {
            _runner.StackPush(v);
        }

    }
}
