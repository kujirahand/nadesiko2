using System;
using System.Collections.Generic;
using System.Text;

using NakoPlugin;
using Libnako.NakoAPI.WrapLib;

namespace NakoPluginFile
{
    public class NakoPluginFile : INakoPlugin
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
            get { return "ファイルの入出力プラグイン"; }
        }

        public bool Used { get; set; }
        
        public void DefineFunction(INakoPluginBank bank)
        {
            //+ テキストファイルの読み書き
            bank.AddFunc("開く", "FILEを|FILEから", NakoVarType.String, _openFile, "ファイル名FILEのテキストを全部読み込んで返す。この時、自動的に文字コードを判定して読み込む。", "ひらく");
            bank.AddFunc("保存", "SをFILEに|FILEへ", NakoVarType.Void, _saveFile, "文字列Sをファイル名FILEへ保存する。(文字コードUTF-8で保存される)", "ほぞん");
            //+ ファイル処理
            bank.AddFunc("起動", "CMDを", NakoVarType.Void, _execCommand, "コマンドCMDを起動する", "きどう");
            bank.AddFunc("存在?", "FILEが|FILEの", NakoVarType.Int, _exists, "ファイルFILEが存在するかどうか調べて結果(1:はい,0:いいえ)を返す", "そんざい");
        }

        // Define Method
        public Object _openFile(INakoFuncCallInfo info)
        {
            String fileName = info.StackPopAsString();
            // Exists?
            if (!System.IO.File.Exists(fileName))
            {
                throw new NakoPluginRuntimeException("ファイル『" + fileName + "』は存在しません。");
            }
            // Load
            String src = StrUnit.LoadFromFileAutoEnc(fileName);
            return src;
        }
        public Object _saveFile(INakoFuncCallInfo info)
        {
            String s = info.StackPopAsString();
            String fileName = info.StackPopAsString();
            System.IO.File.WriteAllText(fileName, s, Encoding.UTF8);
            return null;
        }
        public Object _execCommand(INakoFuncCallInfo info)
        {
            string cmd = info.StackPopAsString();
            //TODO: _execCommand
            return cmd;
        }
        public Object _exists(INakoFuncCallInfo info)
        {
            string path = info.StackPopAsString();
            bool result = System.IO.File.Exists(path);
            return (Int64)(result ? 1 : 0);
        }
    }
}
