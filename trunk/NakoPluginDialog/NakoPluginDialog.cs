using System;
using System.Collections.Generic;
using System.Text;

using NakoPlugin;
using System.Windows.Forms;
using Microsoft.VisualBasic;

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
            bank.AddFunc("言う", "Sを|Sと|Sの", NakoVarType.Void, _say, "文字列Sをダイアログに表示して出す", "いう");
            bank.AddFunc("二択", "Sで|Sと|Sを", NakoVarType.Int, _yesNo, "文字列Sをダイアログに表示し、[はい]か[いいえ]で質問するダイアログを出す", "にたく");
            bank.AddFunc("尋ねる", "Sを|Sと", NakoVarType.String, _inputBox, "文字列Sをダイアログに表示し、一行入力ダイアログを出して、結果を「それ」に返す", "たずねる");
        }

        public Object _say(INakoFuncCallInfo info)
        {
            String s = info.StackPopAsString();
            MessageBox.Show(s, "メッセージ",
                MessageBoxButtons.OK);
            return null;
        }

        public Object _yesNo(INakoFuncCallInfo info)
        {
            String s = info.StackPopAsString();
            DialogResult res = MessageBox.Show(s, "メッセージ",
                MessageBoxButtons.YesNo);
            Object result = (res == DialogResult.Yes) ? 1 : 0;
            return result;
        }

        public Object _inputBox(INakoFuncCallInfo info)
        {
            String s = info.StackPopAsString();
            String res = Interaction.InputBox(s);
            return res;
        }
    }
}
