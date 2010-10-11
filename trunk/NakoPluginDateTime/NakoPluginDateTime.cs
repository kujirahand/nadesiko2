using System;
using System.Collections.Generic;
using System.Threading;

using NakoPlugin;

namespace NakoPluginDateTime
{
	/// <summary>
	/// 日付時間処理を行うプラグイン
	/// </summary>
    public class NakoPluginDateTime : INakoPlugin
    {
        public string Name
        {
            get { return this.GetType().FullName; }
        }

        public double PluginVersion
        {
            get { return 1.0; }
        }

        public string Description
        {
            get { return "日付時間の処理を行うプラグイン"; }
        }
        
        public bool Used { get; set; }

        public void DefineFunction(INakoPluginBank bank)
        {
        	//+日付時間処理
        	//-時間
            bank.AddFunc("秒待つ", "SEC", NakoVarType.Void, _wait, "SEC秒だけ待機する", "びょうまつ");
        }
            
        // Define Method
        public Object _wait(INakoFuncCallInfo info)
        {
        	Double sec = info.StackPopAsDouble();
        	sec *= 1000;
        	int msec = Convert.ToInt32(Math.Floor(sec));
        	Thread.Sleep(msec);
            return null;
        }
        
    }
}
