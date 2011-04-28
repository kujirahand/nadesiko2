﻿using System;
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
        //--- プラグインの宣言 ---
        string _description = "日付時間処理を行うプラグイン";
        double _version = 1.0;
        //--- プラグイン共通の部分 ---
        public double TargetNakoVersion { get { return 2.0; } }
        public bool Used { get; set; }
        public string Name { get { return this.GetType().FullName; } }
        public double PluginVersion { get { return _version; } }
        public string Description { get { return _description; } }
        //--- 関数の定義 ---
        public void DefineFunction(INakoPluginBank bank)
        {
            //+日付時間処理
            //-日付時間
            bank.AddFunc("秒待つ", "SEC", NakoVarType.Void, _wait, "SEC秒だけ待機する", "びょうまつ");
            bank.AddFunc("今日", "", NakoVarType.String, _today, "今日の日付を取得して返す", "きょう");
            bank.AddFunc("今", "", NakoVarType.String, _now, "今の時間を取得して返す", "いま");
            bank.AddFunc("システム時間", "", NakoVarType.Int, _systime, "(擬似的な)システム時間をミリ秒単位で取得して返す", "しすてむじかん");
            bank.AddFunc("時間差", "ATIMEとBTIMEの", NakoVarType.Int, _diffhours, "時間AとBの時間の差を求めて返す", "じかんさ");
        }
        
        // プラグインの初期化処理
        public void PluginInit(INakoInterpreter runner)
        {
        }
        // プラグインの終了処理
        public void PluginFin(INakoInterpreter runner)
        {
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
        
        public Object _diffhours(INakoFuncCallInfo info)
        {
         	String atime = info.StackPopAsString();
         	String btime = info.StackPopAsString();
         	DateTime adatetime = DateTime.Parse(atime);
         	DateTime bdatetime = DateTime.Parse(btime);
         	return adatetime.Subtract(bdatetime).TotalHours;
           
        }
        
    }
}