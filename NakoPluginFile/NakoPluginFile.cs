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
            bank.AddFunc("開く", "FILEを|FILEから", NakoVarType.String, _openFile, "ファイル名FILEのテキストを全部読み込んで返す。この時、自動的に文字コードを判定して読み込む。", "ひらく");
            bank.AddFunc("保存", "SをFILEに|FILEへ", NakoVarType.Void, _saveFile, "文字列Sをファイル名FILEへ保存する。(文字コードUTF-8で保存される)", "ほぞん");
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
    }
}
