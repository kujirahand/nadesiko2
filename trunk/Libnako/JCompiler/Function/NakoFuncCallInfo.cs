using System;
using System.Collections.Generic;
using System.Text;

using Libnako.Interpreter;
using NakoPlugin;

namespace Libnako.JCompiler.Function
{
    /// <summary>
    /// なでしこのシステム関数呼び出しの引数となる情報を定義したもの
    /// </summary>
    public class NakoFuncCallInfo : INakoFuncCallInfo
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

        public void WriteLog(string s)
        {
            Runner.AddPrintLog(s);
        }

        public Object StackPop()
        {
            return _runner.StackPop();
        }

        public string StackPopAsString()
        {
            Object s = _runner.StackPop();
            return Convert.ToString(s);
        }
        
        public Int64 StackPopAsInt()
        {
            Object o = _runner.StackPop();
            return Convert.ToInt64(o);
        }
        public double StackPopAsDouble()
        {
            Object o = _runner.StackPop();
            return Convert.ToDouble(o);
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
