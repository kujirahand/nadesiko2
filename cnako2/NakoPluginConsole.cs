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
        //--- プラグインの宣言 ---
        string _description = "コンソール出力を行うプラグイン";
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
            //+コンソール用(cnako2.exe)
            //-コンソール入出力
            bank.AddFunc("表示", "Sを|Sと", NakoVarType.Void, _coutLine, "コンソールに出力する(改行あり)", "ひょうじ");
            bank.AddFunc("継続表示", "Sを|Sと", NakoVarType.Void, _cout, "コンソールに出力する(改行なし)", "けいぞくひょうじ");
            bank.AddFunc("入力", "Sと|Sを|Sの", NakoVarType.String, _cinLine, "コンソールに質問Sを表示し、標準入力から一行入力を取得して返す", "にゅうりょく");
            bank.AddFunc("標準入力取得", "CNTの", NakoVarType.String, _cin, "コンソールの標準入力からCNTバイト取得して返す", "ひょうじゅんにゅうりょくしゅとく");
        }
        // プラグインの初期化処理
        public void PluginInit(INakoInterpreter runner)
        {
        }
        // プラグインの終了処理
        public void PluginFin(INakoInterpreter runner)
        {
        }
        // --- カスタムフィールド
        public bool UsePrintLog = false; // 表示・継続表示の出力を PrintLog (info.WriteLog())に渡すかどうかのフラグ
        
        // Define Method
        public Object _coutLine(INakoFuncCallInfo info)
        {
            string s = info.StackPopAsString();
            if (UsePrintLog) { info.WriteLog(s+"\r\n"); } else { System.Console.WriteLine(s); }
            return null;
        }
        
        public Object _cout(INakoFuncCallInfo info)
        {
        	string s = info.StackPopAsString();
            if (UsePrintLog) { info.WriteLog(s); } else { System.Console.Write(s); }
            return null;
        }
        
        public Object _cin(INakoFuncCallInfo info)
        {
        	Int64 count = info.StackPopAsInt();
        	//TODO:標準入力の取得方法が効率が悪い
        	Int64 i = 0;
        	string r = "";
        	while (i < count)
        	{
        		int ch = System.Console.Read();
        		if (ch < 0) break;
        		r += (char)ch;
        		i++;
        	}
            return r;
        }
        
        public Object _cinLine(INakoFuncCallInfo info)
        {
        	string s = info.StackPopAsString();
        	System.Console.WriteLine(s);
        	string res = System.Console.ReadLine();
            return res;
        }
    }
}
