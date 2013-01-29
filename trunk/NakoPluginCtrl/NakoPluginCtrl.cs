using System;
using System.Collections.Generic;
using System.Text;

using NakoPlugin;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace NakoPluginCtrl
{
    public class NakoPluginCtrl : INakoPlugin
    {
        //--- プラグインの宣言 ---
        string _description = "外部アプリとの連携を行うプラグイン";
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
            bank.AddFunc("コピー", "Sを|Sの", NakoVarType.Void, _copyToClipboard, "文字列Sをクリップボードにコピーする", "こぴー");
            bank.AddFunc("クリップボード", "", NakoVarType.Void, _getFromClipboard, "クリップボードの文字列を取得する", "くりっぷぼーど");
            bank.AddFunc("キー送信", "TITLEにKEYSを", NakoVarType.Void, _sendKeys, "ウィンドウのタイトルTITLEに文字列KEYSを送信する", "きーそうしん");
            bank.AddFunc("窓列挙", "", NakoVarType.String, _enumWindows, "ウィンドウのタイトルを列挙する", "まどれっきょ");
            bank.AddFunc("窓正規表現検索", "", NakoVarType.Int, _findWindowsRegExp, "ウィンドウのタイトルを正規表現で検索する", "まどせいきひょうげんけんさく");
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
        public Object _copyToClipboard(INakoFuncCallInfo info)
        {
            String s = info.StackPopAsString();
            Clipboard.SetDataObject(s, true);
            return null;
        }

        public Object _getFromClipboard(INakoFuncCallInfo info)
        {
            return Clipboard.GetText();
        }

        public Object _sendKeys(INakoFuncCallInfo info)
        {
            String title = info.StackPopAsString();
            String keys  = info.StackPopAsString();
            EnumWindows.ActivateWindow(title);
            SendKeys.Send(keys);
            return null;
        }
        
        public Object _enumWindows(INakoFuncCallInfo info)
        {
            String s = EnumWindows.GetTitle();
            return s;
        }
        
        public Object _findWindowsRegExp(INakoFuncCallInfo info)
        {
            String pattern = info.StackPopAsString();
            IntPtr hWnd = EnumWindows.FindWindowRE(pattern);
            int h = hWnd.ToInt32();
            return h;
        }
    }
}
