using System;
using System.Collections.Generic;
using System.Threading;

using NakoPlugin;

namespace NakoPluginConsole
{
	/// <summary>
	/// 日付時間処理を行うプラグイン
	/// </summary>
    public class NakoPluginConsole : INakoPlugin
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
            get { return "コンソール出力を行うプラグイン"; }
        }
        
        public bool Used { get; set; }

        public void DefineFunction(INakoPluginBank bank)
        {
        	//+コンソール用(cnako2.exe)
        	//-コンソール入出力
            bank.AddFunc("表示", "Sを|Sと", NakoVarType.Void, _cout, "コンソールに出力する(改行あり)", "ひょうじ");
            bank.AddFunc("継続表示", "Sを|Sと", NakoVarType.Void, _cout2, "コンソールに出力する(改行なし)", "けいぞくひょうじ");
        }
            
        // Define Method
        public Object _cout(INakoFuncCallInfo info)
        {
        	string s = info.StackPopAsString();
        	System.Console.WriteLine(s);
            return null;
        }
        
        public Object _cout2(INakoFuncCallInfo info)
        {
        	string s = info.StackPopAsString();
        	System.Console.Write(s);
            return null;
        }
        
    }
}
