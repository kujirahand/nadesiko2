using System;
using System.Text;
using System.Text.RegularExpressions;
using NakoPlugin;

namespace NakoPluginSystem
{
	public class NakoPluginCheck : INakoPlugin
	{
        string _description = "変数チェックプラグイン";
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
            bank.AddFunc("全角か判定", "Sが|Sの|Sを", NakoVarType.Int, _isFullwidth,"文字列Sの一文字目が全角かどうか判定して返す。", "ぜんかくかはんてい");
            bank.AddFunc("数字か判定", "Sが|Sの|Sを", NakoVarType.Int, _isNumber,"文字列Sの一文字目が数字かどうか判定して返す。", "すうじかはんてい");
//数字か判定
        }

        // プラグインの初期化処理
        public void PluginInit(INakoInterpreter runner)
        {
        }
        // プラグインの終了処理
        public void PluginFin(INakoInterpreter runner)
        {
        }
        private object _isFullwidth(INakoFuncCallInfo info){
            string s = info.StackPopAsString().Substring(0,1);
            return (s.Length*2 == Encoding.GetEncoding("Shift_JIS").GetByteCount(s))? 1 : 0;
        }

        private object _isNumber(INakoFuncCallInfo info){
            string s = info.StackPopAsString().Substring(0,1);
            return Regex.IsMatch(s,@"^[0-9]");
        }
    }
}

