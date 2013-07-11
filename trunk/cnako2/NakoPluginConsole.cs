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
        /// <summary>
        /// ターゲットとなるなでしこのバージョン
        /// </summary>
        public double TargetNakoVersion { get { return 2.0; } }
        /// <summary>
        /// プラグインが利用されているかを判定するフラグ
        /// </summary>
        public bool Used { get; set; }
        /// <summary>
        /// プラグインのフルパス
        /// </summary>
        public string Name { get { return this.GetType().FullName; } }
        /// <summary>
        /// このプラグインのバージョン情報
        /// </summary>
        public double PluginVersion { get { return _version; } }
        /// <summary>
        /// このプラグインの説明
        /// </summary>
        public string Description { get { return _description; } }
        //--- 関数の定義 ---
        /// <summary>
        /// 関数の定義
        /// </summary>
        public void DefineFunction(INakoPluginBank bank)
        {
            //+コンソール用(cnako2.exe)
            //-コンソール入出力
            bank.AddFunc("表示", "Sを|Sと", NakoVarType.Void, _coutLine, "コンソールに出力する(改行あり)", "ひょうじ");
            bank.AddFunc("継続表示", "Sを|Sと", NakoVarType.Void, _cout, "コンソールに出力する(改行なし)", "けいぞくひょうじ");
            bank.AddFunc("入力", "Sと|Sを|Sの", NakoVarType.String, _cinLine, "コンソールに質問Sを表示し、標準入力から一行入力を取得して返す", "にゅうりょく");
            bank.AddFunc("標準入力取得", "CNTの", NakoVarType.String, _cin, "コンソールの標準入力からCNTバイト取得して返す", "ひょうじゅんにゅうりょくしゅとく");
        }
        /// <summary>
        /// プラグインの初期化処理
        /// </summary>
        /// <param name="runner"></param>
        public void PluginInit(INakoInterpreter runner)
        {
        }
        /// <summary>
        /// プラグインの終了処理
        /// </summary>
        /// <param name="runner"></param>
        public void PluginFin(INakoInterpreter runner)
        {
        }
        // --- カスタムフィールド
        /// <summary>
        /// 表示・継続表示の出力を PrintLog (info.WriteLog())に渡すかどうかのフラグ
        /// </summary>
        public bool UsePrintLog = false;
        
        // Define Method
        /// <summary>
        /// 表示
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public Object _coutLine(INakoFuncCallInfo info)
        {
            string s = info.StackPopAsString();
            if (UsePrintLog) { info.WriteLog(s+"\r\n"); } else { System.Console.WriteLine(s); }
            return null;
        }
        
        /// <summary>
        /// 継続表示
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public Object _cout(INakoFuncCallInfo info)
        {
        	string s = info.StackPopAsString();
            if (UsePrintLog) { info.WriteLog(s); } else { System.Console.Write(s); }
            return null;
        }
        
        /// <summary>
        /// 標準入力取得
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public Object _cin(INakoFuncCallInfo info)
        {
        	long count = info.StackPopAsInt();
        	//TODO:標準入力の取得方法が効率が悪い
        	long i = 0;
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
        
        /// <summary>
        /// 入力
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public Object _cinLine(INakoFuncCallInfo info)
        {
        	string s = info.StackPopAsString();
        	System.Console.WriteLine(s);
        	string res = System.Console.ReadLine();
            return res;
        }
    }
}
