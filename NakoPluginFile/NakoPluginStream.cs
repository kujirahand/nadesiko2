using System;
using System.IO;
using NakoPlugin;

namespace NakoPluginFile
{
    public class NakoPluginStream : INakoPlugin
    {
        //--- プラグインの宣言 ---
        string _description = "ファイル入出力を行うプラグイン";
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
            bank.AddFunc("ファイルストリーム開く", "AをBで", NakoVarType.Void, _open, "ファイル名AをモードB（作|読|書|排他）でストリームを開きハンドルを返す。", "ふぁいるすとりーむひらく");
            bank.AddFunc("ファイルストリーム閉じる", "Hを", NakoVarType.Void, _close, "ファイルストリームハンドルHを閉じる。", "ふぁいるすとりーむとじる");
            bank.AddFunc("ファイルストリーム一行読む", "Hで|Hの", NakoVarType.String, _readLine, "ファイルストリームハンドルHで一行読んで返す。", "ふぁいるすとりーむいちぎょうよむ");
            bank.AddFunc("ファイルストリーム一行書く", "SをHに|Hで|Hへ", NakoVarType.Void, _writeLine, "ファイルストリームハンドルHへSを一行書く。", "ふぁいるすとりーむいちぎょうかく");
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
        public object _open(INakoFuncCallInfo info) {
            string fileName = info.StackPopAsString();
            string mode = info.StackPopAsString();
            switch (mode) {
            case "作":
             return new FileStream(fileName,FileMode.Create);
            case "読":
             return new StreamReader(fileName);
            case "書":
             return new StreamWriter(fileName);
            default:
             return new StreamReader(fileName);
            }
        }
        public object _close(INakoFuncCallInfo info) {
            object stream = info.StackPop();
            if(stream is FileStream){
                FileStream filestream = (FileStream)stream;
                filestream.Close();
                filestream.Dispose();
            }
            else if(stream is StreamReader){
                StreamReader filestream = (StreamReader)stream;
                filestream.Close();
                filestream.Dispose();
            }
            else if(stream is StreamWriter){
                StreamWriter filestream = (StreamWriter)stream;
                filestream.Close();
                filestream.Dispose();
            }
            return null;
        }
        public object _readLine(INakoFuncCallInfo info) {
            object stream = info.StackPop();
            if(stream is StreamReader){
                StreamReader filestream = (StreamReader)stream;
                return filestream.ReadLine();
            }
            return null;
        }
        public object _writeLine(INakoFuncCallInfo info) {
            string s = info.StackPopAsString();
            object stream = info.StackPop();
            if(stream is StreamWriter){
                StreamWriter filestream = (StreamWriter)stream;
                filestream.WriteLine(s);
            }
            return null;
        }
    }
}

