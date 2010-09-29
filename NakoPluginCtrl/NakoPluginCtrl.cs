using System;
using System.Collections.Generic;
using System.Text;

using NakoPlugin;
using System.Windows.Forms;


namespace NakoPluginCtrl
{
    public class NakoPluginCtrl : INakoPlugin
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
            get { return "外部アプリとの連携を行うプラグイン"; }
        }

        public void DefineFunction(INakoPluginBank bank)
        {
            bank.AddFunc("コピー", "Sを|Sの", NakoVarType.Void, _copyToClipboard, "文字列Sをクリップボードにコピーする", "こぴー");
            bank.AddFunc("クリップボード", "", NakoVarType.Void, _getFromClipboard, "クリップボードの文字列を取得する", "くりっぷぼーど");
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

    }
}
