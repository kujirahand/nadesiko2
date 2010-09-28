using System;
using System.Collections.Generic;
using System.Text;

using NakoPlugin;
using System.Windows.Forms;

namespace NakoPluginDialog
{
    public class NakoPluginDialog : INakoPlugin
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
            get { return "各種ダイアログを表示するプラグイン"; }
        }

        public void DefineFunction(INakoPluginBank bank)
        {
            bank.AddFunc("言う", "Sを|Sと|Sの", NakoVarType.Void, _say, "文字列Sをダイアログに表示する", "いう");
        }

        public Object _say(INakoFuncCallInfo info)
        {
            String s = info.StackPopAsString();
            MessageBox.Show(s, "メッセージ",
                MessageBoxButtons.OK);
            return null;
        }
    }
}
