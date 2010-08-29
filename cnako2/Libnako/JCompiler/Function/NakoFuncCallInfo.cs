using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Libnako.Interpreter;

namespace Libnako.JCompiler.Function
{
    /// <summary>
    /// なでしこのシステム関数呼び出しの引数となる情報を定義したもの
    /// </summary>
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

        /*
         * 基本的に PUSH は不要
        public void StackPush(Object v)
        {
            _runner.StackPush(v);
        }
        */

    }
}
