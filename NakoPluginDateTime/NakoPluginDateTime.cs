﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Globalization;

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
        Version _version = new Version(1, 0);
        //--- プラグイン共通の部分 ---
        public Version TargetNakoVersion { get { return new Version(2, 0); } }
        public bool Used { get; set; }
        public string Name { get { return this.GetType().FullName; } }
        public Version PluginVersion { get { return _version; } }
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
            bank.AddFunc("UNIXTIME_日時変換", "Iを", NakoVarType.String, _unixtime_to_datetime, "Unix TimeであるIをなでしこ日時形式に変換する", "ゆにっくすたいむにちじへんかん");
            bank.AddFunc("日付加算", "SにAを", NakoVarType.String, _add, "日付SにAを加えて返す。Aには「(+|-)yyyy/mm/dd」で指定する。", "ひづけかさん");
            bank.AddFunc("日時形式変換", "DATEをFORMATに|DATEから|FORMATで|FORMATへ", NakoVarType.String, _format, "日時(DATE)を指定形式(FORMAT)に変換する。フォーマットには「RSS形式」や「yyyy/mm/dd hh:nn:ss」を指定する。", "にちじけいしきへんかん");
            bank.AddFunc("日数差", "AとBの|AからBまでの", NakoVarType.Int, _daysDifference, "日付AとBの差を日数で求めて返す", "にっすうさ");
            bank.AddFunc("和暦変換", "Sを", NakoVarType.String, _toJapanese, "Sを和暦に変換する。Sは明治以降の日付が有効", "われきへんかん");
        }
        public object _format(INakoFuncCallInfo info){
            CultureInfo culture = new CultureInfo ("ja-JP", true);
            string a= info.StackPopAsString();
            string format = info.StackPopAsString();
            DateTime aDate = DateTime.Parse(a);
            return aDate.ToString(format,culture);
        }
        public object _daysDifference(INakoFuncCallInfo info){
            string a= info.StackPopAsString();
            string b= info.StackPopAsString();
            DateTime aDate = DateTime.Parse(a);
            DateTime bDate = DateTime.Parse(b);
            return bDate.Subtract(aDate).TotalDays;
        }
        CultureInfo culture = new CultureInfo("ja-JP",true);
        public object _toJapanese(INakoFuncCallInfo info){
            string a= info.StackPopAsString();
            DateTime aDate = DateTime.Parse(a);
            return aDate.ToString("ggyy年M月d日",culture).Replace("Heisei","平成").Replace("Showa","昭和").Replace("Taisho","大正").Replace("Meiji","明治");//TODO:monoだとHeiseiとか出てくるので何とかならないか？
        }
        
        // プラグインの初期化処理
        public void PluginInit(INakoInterpreter runner)
        {
            culture.DateTimeFormat.Calendar = new JapaneseCalendar();
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
         	string atime = info.StackPopAsString();
         	string btime = info.StackPopAsString();
         	DateTime adatetime = DateTime.Parse(atime);
         	DateTime bdatetime = DateTime.Parse(btime);
         	return adatetime.Subtract(bdatetime).TotalHours;
           
        }
		static readonly DateTime UnixEpoch = new DateTime(1970,1,1,0,0,0,0,DateTimeKind.Utc);
        public object _unixtime_to_datetime(INakoFuncCallInfo info){
			long unixtime = info.StackPopAsInt();
			return UnixEpoch.AddSeconds(unixtime).ToLocalTime();
		}
        public object _add(INakoFuncCallInfo info){
            CultureInfo culture = new CultureInfo ("ja-JP", true);
			string s = info.StackPopAsString();
         	string addDate = info.StackPopAsString();
			string addOrDel = addDate.Substring(0,1);
			string[] span = addDate.Substring(1).Replace('-','/').Split('/');
			if(span.Length!=3) throw new ArgumentException();
			DateTime current = DateTime.Parse(s);//TODO:ParseExactを使うか？
			return current.AddYears(int.Parse (addOrDel+span[0])).AddMonths(int.Parse (addOrDel+span[1])).AddDays (int.Parse (addOrDel+span[2])).ToString(culture);
		}
        
    }
}
