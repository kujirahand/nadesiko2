using System;
using System.Collections.Generic;
using System.Text;

using NakoPlugin;
using Libnako.NakoAPI.WrapLib;

using System.Diagnostics;
using System.IO;

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
            //-起動
            bank.AddFunc("起動", "CMDを", NakoVarType.Void, _execCommand, "コマンドCMDを起動する", "きどう");
            bank.AddFunc("起動待機", "CMDを", NakoVarType.Void, _execCommandWait, "コマンドCMDを起動して終了まで待機する", "きどうたいき");
            bank.AddFunc("隠し起動", "CMDを", NakoVarType.Void, _execCommandHidden, "コマンドCMDを隠しモード出起動する", "かくしきどう");
            bank.AddFunc("隠し起動待機", "CMDを", NakoVarType.Void, _execCommandHiddenWait, "コマンドCMDを隠しモード出起動して待機する", "かくしきどうたいき");
            //-存在
            bank.AddFunc("存在?", "FILEが|FILEの", NakoVarType.Int, _exists, "ファイルFILEが存在するかどうか調べて結果(1:はい,0:いいえ)を返す", "そんざい");
            //+特殊フォルダ
            //-なでしこパス
            bank.AddFunc("母艦パス", "", NakoVarType.String, _getBokanDir, "プログラムの起動したディレクトリを取得して返す", "ぼかんぱす");
            bank.AddFunc("ランタイムパス", "", NakoVarType.String, _getRuntimeDir, "ランタイムの起動したディレクトリを取得して返す", "らんたいむぱす");
            //-パス
            bank.AddFunc("SYSTEMパス", "", NakoVarType.String, _getSystemDir, "SYSTEMフォルダを取得して返す", "SYSTEMぱす");
            bank.AddFunc("テンポラリフォルダ", "", NakoVarType.String, _getTempDir, "テンポラリフォルダを取得して返す", "てんぽらりふぉるだ");
            bank.AddFunc("デスクトップ", "", NakoVarType.String, _getDesktopDir, "デスクトップのフォルダを取得して返す", "ですくとっぷ");
            bank.AddFunc("SENDTOパス", "", NakoVarType.String, _getSendToDir, "SENDTOのフォルダを取得して返す", "SENDTOぱす");
            bank.AddFunc("スタートアップ", "", NakoVarType.String, _getStartupDir, "スタートアップのフォルダを取得して返す", "すたーとあっぷ");
            bank.AddFunc("スタートメニュー", "", NakoVarType.String, _getStartmenuDir, "スタートメニューのフォルダを取得して返す", "すたーとめにゅー");
            bank.AddFunc("マイドキュメント", "", NakoVarType.String, _getMyDocument, "マイドキュメントのフォルダを取得して返す", "まいどきゅめんと");
            bank.AddFunc("マイピクチャ", "", NakoVarType.String, _getMyPicture, "マイピクチャのフォルダを取得して返す", "まいぴくちゃ");
            bank.AddFunc("マイミュージック", "", NakoVarType.String, _getMyMusic, "マイミュージックのフォルダを取得して返す", "まいみゅーじっく");
            bank.AddFunc("ユーザーホームフォルダ", "", NakoVarType.String, _getUserHomeDir, "ユーザーのホームディレクトリのフォルダを取得して返す", "ゆーざーほーむふぉるだ");
            bank.AddFunc("個人設定フォルダ", "", NakoVarType.String, _getAppDataDir, "%APPDATA%のフォルダを取得して返す", "こじんせっていふぉるだ");
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
            System.Diagnostics.Process.Start(cmd);
            return null;
        }
        
        public Object _execCommandWait(INakoFuncCallInfo info)
        {
            string cmd = info.StackPopAsString();
        	Process proc = System.Diagnostics.Process.Start(cmd);
        	proc.WaitForExit();
        	return null;
        }

        public Object _execCommandHidden(INakoFuncCallInfo info)
        {
            string cmd = info.StackPopAsString();
            Process proc = new Process();
            proc.StartInfo.FileName = cmd;
            proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            proc.Start();
        	return null;
        }
        
        public Object _execCommandHiddenWait(INakoFuncCallInfo info)
        {
            string cmd = info.StackPopAsString();
            Process proc = new Process();
            proc.StartInfo.FileName = cmd;
            proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            proc.Start();
            proc.WaitForExit();
        	return null;
        }
        
        public Object _exists(INakoFuncCallInfo info)
        {
            string path = info.StackPopAsString();
            bool result = System.IO.File.Exists(path);
            return (Int64)(result ? 1 : 0);
        }
        
        private string _path(string dir)
        {
        	return NWEnviroment.AppendLastPathFlag(dir);
        }
        
        public Object _getBokanDir(INakoFuncCallInfo info)
        {
        	//TODO:母艦パスが未実装
        	return _path(NWEnviroment.AppPath);
        }
        
        public Object _getRuntimeDir(INakoFuncCallInfo info)
        {
        	return _path(NWEnviroment.AppPath);
        }
        
        //------------------------------------------------------------------
        // システムの特殊ディレクトリ
        private string GetSpecialDir(Environment.SpecialFolder dir)
        {
        	string path = Environment.GetFolderPath(dir);
        	return _path(path);
        }
        public Object _getMyPicture(INakoFuncCallInfo info)
        {
        	return GetSpecialDir(Environment.SpecialFolder.MyPictures);
        }
        public Object _getDesktopDir(INakoFuncCallInfo info)
        {
        	return GetSpecialDir(Environment.SpecialFolder.DesktopDirectory);
        }
        public Object _getSendToDir(INakoFuncCallInfo info)
        {
        	return GetSpecialDir(Environment.SpecialFolder.SendTo);
        }
        public Object _getStartupDir(INakoFuncCallInfo info)
        {
        	return GetSpecialDir(Environment.SpecialFolder.Startup);
        }
        public Object _getStartmenuDir(INakoFuncCallInfo info)
        {
        	return GetSpecialDir(Environment.SpecialFolder.System);
        }
        public Object _getMyDocument(INakoFuncCallInfo info)
        {
        	return GetSpecialDir(Environment.SpecialFolder.MyDocuments);
        }
        public Object _getMyMusic(INakoFuncCallInfo info)
        {
        	return GetSpecialDir(Environment.SpecialFolder.MyMusic);
        }
        public Object _getUserHomeDir(INakoFuncCallInfo info)
        {
       		string homePath = 
       			(Environment.OSVersion.Platform == PlatformID.Unix || Environment.OSVersion.Platform == PlatformID.MacOSX)
    			? Environment.GetEnvironmentVariable("HOME")
    			: Environment.ExpandEnvironmentVariables("%HOMEDRIVE%%HOMEPATH%");
       		return _path(homePath);
        }
        public Object _getSystemDir(INakoFuncCallInfo info)
        {
        	return GetSpecialDir(Environment.SpecialFolder.System);
        }
        public Object _getTempDir(INakoFuncCallInfo info)
        {
        	return _path(System.IO.Path.GetTempPath());
        }
        public Object _getAppDataDir(INakoFuncCallInfo info)
        {
        	return GetSpecialDir(Environment.SpecialFolder.LocalApplicationData);
        }
        
    }
}
