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

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="interpreter"></param>
        public NakoFuncCallInfo(NakoInterpreter interpreter)
        {
            this._runner = interpreter;
        }

        /// <summary>
        /// インタプリタを返す
        /// </summary>
        public NakoInterpreter Runner
        {
            get { return _runner; }
        }

        /// <summary>
        /// LOGを書き出す
        /// </summary>
        /// <param name="s"></param>
        public void WriteLog(string s)
        {
            Runner.AddPrintLog(s);
        }

        /// <summary>
        /// 引数スタックからオブジェクトを取得する
        /// </summary>
        /// <returns></returns>
        public object StackPop()
        {
            return _runner.StackPop();
        }
        /// <summary>
        /// 引数スタックから文字列を得る
        /// </summary>
        /// <returns></returns>
        public string StackPopAsString()
        {
            object s = _runner.StackPop();
            if (s is NakoVariable)
            {
                return s.ToString();
            }
            else
            {
                return Convert.ToString(s);
            }
        }
        /// <summary>
        /// 引数スタックから整数を得る
        /// </summary>
        /// <returns></returns>
        public long StackPopAsInt()
        {
            object o = _runner.StackPop();
            return Convert.ToInt64(o);
        }
        /// <summary>
        /// 引数スタックからdoubleを得る
        /// </summary>
        /// <returns></returns>
        public double StackPopAsDouble()
        {
            object o = _runner.StackPop();
            return Convert.ToDouble(o);
        }
        /// <summary>
        /// 変数を得る
        /// </summary>
        /// <param name="varname"></param>
        /// <returns></returns>
       public NakoVariable GetVariable(string varname)
       {
       		return _runner.globalVar.GetVar(varname);
       }
        /// <summary>
        /// 変数をセット
        /// </summary>
        /// <param name="varname"></param>
        /// <param name="value"></param>
        public void SetVariable(string varname, NakoVariable value)
        {
       	    _runner.globalVar.SetVar(varname, (NakoVariable)value);
        }
        /// <summary>
        /// 変数から値を得る
        /// </summary>
        /// <param name="varname"></param>
        /// <returns></returns>
        public object GetVariableValue(string varname)
        {
        	int index = _runner.globalVar.GetIndex(varname);
        	return _runner.globalVar.GetValue(index);
        }
        /// <summary>
        /// 変数に値をセット
        /// </summary>
        /// <param name="varname"></param>
        /// <param name="value"></param>
        public void SetVariableValue(string varname, object value)
        {
        	int index = _runner.globalVar.GetIndex(varname);
            if (index < 0)
            {
                index = _runner.globalVar.CreateVar(varname);
            }
        	_runner.globalVar.SetValue(index, value);
        }
        // --- 値を作成する
        /// <summary>
        /// 配列変数を生成して返す
        /// </summary>
        /// <returns></returns>
        public NakoVarArray CreateArray()
        {
            NakoVarArray v = new NakoVarArray();
            return v;
        }
        /// <summary>
        /// Gets the callback.
        /// </summary>
        /// <returns>The callback.</returns>
        /// <param name="func_name">Func name.</param>
        /// <param name="args">Arguments.</param>
        public EventHandler GetCallback(string func_name, params object[] args){
            return new EventHandler(delegate(object sender, EventArgs e) {
                Runner.CallUserFunc(func_name, sender);
            });
        }

    }
}
