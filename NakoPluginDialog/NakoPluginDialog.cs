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
        //--- プラグインの宣言 ---
        string _description = "各種ダイアログを表示するためのプラグイン";
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
            bank.AddFunc("言う", "Sを|Sと|Sの", NakoVarType.Void, _say, "文字列Sをダイアログに表示して出す", "いう");
            bank.AddFunc("二択", "Sで|Sと|Sを", NakoVarType.Int, _yesNo, "文字列Sをダイアログに表示し、[はい]か[いいえ]で質問するダイアログを出す", "にたく");
            bank.AddFunc("三択", "Sで|Sと|Sを", NakoVarType.Int, _yesNoCancel, "文字列Sをダイアログに表示し、[はい]か[いいえ][キャンセル]で質問するダイアログを出す", "さんたく");
            bank.AddFunc("尋ねる", "Sを|Sと", NakoVarType.String, _inputBox, "文字列Sをダイアログに表示し、一行入力ダイアログを出して、結果を「それ」に返す", "たずねる");
            bank.AddFunc("ファイル選択", "Sの", NakoVarType.String, _openFileDialog, "拡張子Sのファイルを選択するダイアログを出して、ファイル名を返す。キャンセルなら空を返す。", "ふぁいるせんたく");
            bank.AddFunc("保存ファイル選択", "Sの", NakoVarType.String, _saveFileDialog, "拡張子Sの保存ファイルを選択するダイアログを出して、ファイル名を返す。キャンセルなら空を返す。", "ほぞんふぁいるせんたく");
            bank.AddFunc("フォルダ選択", "Sで｜Sの", NakoVarType.String, _directoryDialog, "初期フォルダSでフォルダを選択して返す", "ふぉるだせんたく");
        }
        // プラグインの初期化処理
        public void PluginInit(INakoInterpreter runner)
        {
        }
        // プラグインの終了処理
        public void PluginFin(INakoInterpreter runner)
        {
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

        public Object _yesNoCancel(INakoFuncCallInfo info)
        {
            String s = info.StackPopAsString();
            DialogResult res = MessageBox.Show(s, "メッセージ",
                MessageBoxButtons.YesNoCancel);
            Object r = 0;
            switch (res)
            {
                case DialogResult.Yes: r = 1; break;
                case DialogResult.No: r = 0; break;
                case DialogResult.Cancel: r = 2; break;
            }
            return r;
        }
        
        public Object _inputBox(INakoFuncCallInfo info)
        {
            String s = info.StackPopAsString();
            String res = Interaction.InputBox(s, null, "", -1, -1);
            return res;
        }

        public Object _directoryDialog(INakoFuncCallInfo info)
        {
            string s = info.StackPopAsString();

            FolderBrowserDialog fbd = new FolderBrowserDialog();

            //RootFolderには特殊フォルダしか指定できないのでとりあえずSelectedPathで代用
            fbd.SelectedPath = s;
            //ダイアログを表示する
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                //OKボタンがクリックされたとき
                return fbd.SelectedPath;
            }
            return null;
        }

        public Object _openFileDialog(INakoFuncCallInfo info)
        {
            String s = info.StackPopAsString();

            //OpenFileDialogクラスのインスタンスを作成
            OpenFileDialog ofd = new OpenFileDialog();
            //ファイル名を指定する
            // ofd.FileName = "default.html";
            //フォルダを指定する
            //指定しない（空の文字列）の時は、現在のディレクトリが表示される
            // ofd.InitialDirectory = @"C:\";
            
            //[ファイルの種類]に表示される選択肢を指定する
            //指定しないとすべてのファイルが表示される
            // ex) filter = "HTMLファイル(*.html;*.htm)|*.html;*.htm|すべてのファイル(*.*)|*.*";
            if (s.IndexOf('|') < 0)
            {
                s = "(" + s + ")|" + s + "|すべてのファイル(*.*)|*.*";
            }

            ofd.Filter = s;
            //「すべてのファイル」が選択されているようにする
            // ofd.FilterIndex = 2;
            //タイトルを設定する
            // ofd.Title = "開くファイルを選択してください";
            //ダイアログボックスを閉じる前に現在のディレクトリを復元するようにする
            ofd.RestoreDirectory = false;
            //存在しないファイルの名前が指定されたとき警告を表示する
            //デフォルトでTrueなので指定する必要はない
            ofd.CheckFileExists = true;
            //存在しないパスが指定されたとき警告を表示する
            //デフォルトでTrueなので指定する必要はない
            ofd.CheckPathExists = true;

            //ダイアログを表示する
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                //OKボタンがクリックされたとき
                return ofd.FileName;
            }
            return null;
        }

        public Object _saveFileDialog(INakoFuncCallInfo info)
        {
            String s = info.StackPopAsString();

            //OpenFileDialogクラスのインスタンスを作成
            SaveFileDialog ofd = new SaveFileDialog();
            //ファイル名を指定する
            // ofd.FileName = "default.html";
            //フォルダを指定する
            //指定しない（空の文字列）の時は、現在のディレクトリが表示される
            // ofd.InitialDirectory = @"C:\";

            //[ファイルの種類]に表示される選択肢を指定する
            //指定しないとすべてのファイルが表示される
            // ex) filter = "HTMLファイル(*.html;*.htm)|*.html;*.htm|すべてのファイル(*.*)|*.*";
            if (s.IndexOf('|') < 0)
            {
                s = "(" + s + ")|" + s + "|すべてのファイル(*.*)|*.*";
            }

            ofd.Filter = s;
            //「すべてのファイル」が選択されているようにする
            // ofd.FilterIndex = 2;
            //タイトルを設定する
            // ofd.Title = "開くファイルを選択してください";
            //ダイアログボックスを閉じる前に現在のディレクトリを復元するようにする
            ofd.RestoreDirectory = false;
            //存在しないファイルの名前が指定されたとき警告を表示する
            //デフォルトでTrueなので指定する必要はない
            ofd.CheckFileExists = false;
            //存在しないパスが指定されたとき警告を表示する
            //デフォルトでTrueなので指定する必要はない
            ofd.CheckPathExists = false;

            //ダイアログを表示する
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                //OKボタンがクリックされたとき
                return ofd.FileName;
            }
            return null;
        }
    }
}
