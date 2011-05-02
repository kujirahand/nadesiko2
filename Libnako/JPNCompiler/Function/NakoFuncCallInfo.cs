using System;
using System.Collections.Generic;
using System.Text;

using Libnako.Interpreter;
using NakoPlugin;

namespace Libnako.JPNCompiler.Function
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
            if (s is INakoVariable)
            {
                return s.ToString();
            }
            else
            {
                return Convert.ToString(s);
            }
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
       public INakoVariable GetVariable(string varname)
       {
       		return _runner.globalVar.GetVar(varname);
       }
       public void SetVariable(string varname, INakoVariable value)
       {
       		_runner.globalVar.SetVar(varname, (NakoVariable)value);
       }
        public Object GetVariableValue(string varname)
        {
        	int index = _runner.globalVar.GetIndex(varname);
        	return _runner.globalVar.GetValue(index);
        }
        public void SetVariableValue(string varname, Object value)
        {
        	int index = _runner.globalVar.GetIndex(varname);
            if (index < 0)
            {
                index = _runner.globalVar.CreateVar(varname);
            }
        	_runner.globalVar.SetValue(index, value);
        }
        // --- 値を作成する
        public INakoVarArray CreateArray()
        {
            INakoVarArray v = new NakoVarArray();
            return v;
        }

    }
}
