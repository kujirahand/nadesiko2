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
        	//-日付時間
            bank.AddFunc("秒待つ", "SEC", NakoVarType.Void, _wait, "SEC秒だけ待機する", "びょうまつ");
            bank.AddFunc("今日", "", NakoVarType.String, _today, "今日の日付を取得して返す", "きょう");
            bank.AddFunc("今", "", NakoVarType.String, _now, "今の時間を取得して返す", "いま");
            bank.AddFunc("システム時間", "", NakoVarType.Int, _systime, "(擬似的な)システム時間をミリ秒単位で取得して返す", "しすてむじかん");
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
        public Object _today(INakoFuncCallInfo info)
        {
        	DateTime d = DateTime.Today;
        	string s = String.Format("{0:D4}-{1:D2}-{2:D2}",
        	                        d.Year,
        	                       	d.Month,
        	                       	d.Day);
            return s;
        }
        public Object _now(INakoFuncCallInfo info)
        {
        	DateTime d = DateTime.Now;
        	string s = String.Format("{0:D2}:{1:D2}:{2:D2}",
        	                        d.Hour,
        	                       	d.Minute,
        	                       	d.Second);
            return s;
        }
        
        System.Diagnostics.Stopwatch stopwatch_systime = null;
        public Object _systime(INakoFuncCallInfo info)
        {
        	if (stopwatch_systime == null)
        	{
        		stopwatch_systime = new System.Diagnostics.Stopwatch();
        		stopwatch_systime.Start();
        	}
            return stopwatch_systime.ElapsedMilliseconds;
        }
        
        
    }
}
