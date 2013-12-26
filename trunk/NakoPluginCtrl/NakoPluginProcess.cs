using System;
using NakoPlugin;

namespace NakoPluginCtrl
{
    public class NakoPluginProcess : INakoPlugin
    {
        //--- プラグインの宣言 ---
        string _description = "起動しているプロセスを扱うプラグイン";
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
//            bank.AddFunc("プロセス存在", "Sを|Sの", NakoVarType.Void, _copyToClipboard, "文字列Sをクリップボードにコピーする", "こぴー");
            bank.AddFunc("プロセス強制終了", "Sの", NakoVarType.Void, _abort, "起動しているプロセス(EXE名で指定)を強制終了させる", "ぷろせすきょうせいしゅうりょう");
            bank.AddFunc("プロセス列挙", "", NakoVarType.Array, _ps, "起動しているプロセスを列挙して返す。", "ぷろせすれっきょ");
            bank.AddFunc("GUID生成", "", NakoVarType.String, _guid, "GUIDを生成して返す。", "GUIDせいせい");
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
        public object _abort(INakoFuncCallInfo info)
        {
            string s = info.StackPopAsString();
            foreach(System.Diagnostics.Process p in System.Diagnostics.Process.GetProcessesByName(s)){
                p.Kill();
            }
            return null;
        }

        public object _ps(INakoFuncCallInfo info)
        {
            NakoVarArray result = new NakoVarArray();
            foreach(System.Diagnostics.Process p in System.Diagnostics.Process.GetProcesses()){
                result.SetValue(result.Count,p.ProcessName);
            }
            return result;
        }
        private object _guid(INakoFuncCallInfo info)
        {
            return System.Guid.NewGuid().ToString();
        }
    }
}
