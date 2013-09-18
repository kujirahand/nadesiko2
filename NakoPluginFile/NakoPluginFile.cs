using System;
using System.Collections.Generic;
using System.Text;

using NakoPlugin;

using System.Diagnostics;
using System.IO;

namespace NakoPluginFile
{
    public class NakoPluginFile : INakoPlugin
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
            //+ テキストファイルの読み書き
            bank.AddFunc("開く", "FILEを|FILEから", NakoVarType.String, _openFile, "ファイル名FILEのテキストを全部読み込んで返す。この時、自動的に文字コードを判定して読み込む。", "ひらく");
            bank.AddFunc("読む", "FILEを|FILEから", NakoVarType.String, _openFile, "ファイル名FILEのテキストを全部読み込んで返す。この時、自動的に文字コードを判定して読み込む。", "ひらく");
            bank.AddFunc("保存", "SをFILEに|FILEへ", NakoVarType.Void, _saveFile, "文字列Sをファイル名FILEへ保存する。(文字コードUTF-8で保存される)", "ほぞん");
            bank.AddFunc("毎行読む", "Fを|Fから", NakoVarType.String, _readLine, "一行ずつ読むためにファイル名Fを開いてハンドルを返す。反復と組み合わせて使う。", "まいぎょうよむ");
            //+ ファイル処理
            //-起動
            bank.AddFunc("起動", "CMDを", NakoVarType.Void, _execCommand, "コマンドCMDを起動する", "きどう");//TODO:To NakoPluginShell
            bank.AddFunc("起動待機", "CMDを", NakoVarType.Void, _execCommandWait, "コマンドCMDを起動して終了まで待機する", "きどうたいき");//TODO:To NakoPluginShell
            bank.AddFunc("隠し起動", "CMDを", NakoVarType.Void, _execCommandHidden, "コマンドCMDを隠しモード出起動する", "かくしきどう");//TODO:To NakoPluginShell
            bank.AddFunc("隠し起動待機", "CMDを", NakoVarType.Void, _execCommandHiddenWait, "コマンドCMDを隠しモード出起動して待機する", "かくしきどうたいき");//TODO:To NakoPluginShell
            bank.AddFunc("コマンド実行", "CMDを", NakoVarType.String, _execCommandAndGetResult, "ファイルSを可視オフで起動して終了まで待機する。起動したプログラムの標準出力の内容を返す。", "かくしきどうたいき");//TODO:To NakoPluginShell
            //-存在
            bank.AddFunc("存在?", "FILEが|FILEの", NakoVarType.Int, _exists, "ファイルFILEが存在するかどうか調べて結果(1:はい,0:いいえ)を返す", "そんざい");
            bank.AddFunc("フォルダ存在?", "DIRが|DIRの", NakoVarType.Int, _existsDir, "フォルダDIRが存在するかどうか調べて結果(1:はい,0:いいえ)を返す", "ふぉるだそんざい");
            //+特殊フォルダ
            //-なでしこパス
            bank.AddFunc("母艦パス", "", NakoVarType.String, _getBokanDir, "プログラムの起動したディレクトリを取得して返す", "ぼかんぱす");
            bank.AddFunc("ランタイムパス", "", NakoVarType.String, _getRuntimeDir, "ランタイムの起動したディレクトリを取得して返す", "らんたいむぱす");
            bank.AddFunc("作業フォルダ取得", "", NakoVarType.String, _getCurrentDir, "カレントディレクトリを取得して返す", "さぎょうふぉるだしゅとく");
            bank.AddFunc("作業フォルダ変更", "Sに|Sへ", NakoVarType.Void, _setCurrentDir, "カレントディレクトリをSに変更する", "さぎょうふぉるだへんこう");
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
            //-コピー移動削除作成
            bank.AddFunc("ファイルコピー", "F1からF2へ|F1をF2に", NakoVarType.Void, _copyFile, "", "ふぁいるこぴー");
            bank.AddFunc("ファイル移動", "F1からF2へ|F1をF2に", NakoVarType.Void, _moveFile, "", "ふぁいるいどう");
            bank.AddFunc("ファイル削除", "Fを|Fの", NakoVarType.Void, _removeFile, "", "ふぁいるさくじょ");
            bank.AddFunc("フォルダ作成", "Fへ|Fに|Fの", NakoVarType.Void, _makeDir, "", "ふぉるださくせい");
            bank.AddFunc("フォルダ削除", "PATHの", NakoVarType.Void, _removeDir, "", "ふぉるださくじょ");
            //-ファイル列挙
            bank.AddFunc("ファイル列挙", "PATHの", NakoVarType.Array, _enumFiles, "PATHにあるファイルを列挙する", "ふぁいるれっきょ");
            bank.AddFunc("フォルダ列挙", "PATHの", NakoVarType.Array, _enumDirs, "PATHにあるフォルダを列挙する", "ふぉるだれっきょ");
            //-パス操作
            bank.AddFunc("パス抽出", "PATHの", NakoVarType.String, _dirname, "PATHからパスを抽出して返す", "ぱすちゅうしゅつ");
            bank.AddFunc("相対パス展開", "AをBで", NakoVarType.String, _relativePath, "相対パスAを基本パスBで展開して返す。", "そうたいぱすてんかい");
            bank.AddFunc("ファイル名抽出", "PATHの", NakoVarType.String, _basename, "PATHからパスを抽出して返す", "ふぁいるめいちゅうしゅつ");
            bank.AddFunc("拡張子抽出", "PATHの", NakoVarType.String, _extname, "PATHから拡張子を抽出して返す", "かくちょうしちゅうしゅつ");
            bank.AddFunc("ファイルサイズ", "Fの", NakoVarType.Int, _sizeof, "ファイルFのサイズを返す", "ふぁいるさいず");
            bank.AddFunc("追加保存", "SをFに|Fへ", NakoVarType.Void, _append, "文字列Sの内容をファイル名Fへ追加保存する。", "ついかほぞん");
            bank.AddFunc("ファイル更新日時", "Fの", NakoVarType.String, _updateDate, "ファイルFの更新日時を返す。", "ふぁいるこうしんにちじ");
            //-ドライブ情報（別ファイルにするか）
            bank.AddFunc("ドライブ種類", "Fの", NakoVarType.String, _driveType, "ルートドライブAの種類（不明|存在しない｜取り外し可能｜固定｜ネットワーク｜CD-ROM｜RAM）を返す。", "どらいぶしゅるい");
/*
NakoPluginFile	作業フォルダ取得　ｰｰ　カレントディレクトリを取得して返す。
NakoPluginFile	{文字列}Sに|Sへ　作業フォルダ変更　ｰｰ　カレントディレクトリをSに変更する
*/
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
        public Object _openFile(INakoFuncCallInfo info)
        {
            string fileName = info.StackPopAsString();
            // Exists?
            if (!System.IO.File.Exists(fileName))
            {
                throw new NakoPluginRuntimeException("ファイル『" + fileName + "』は存在しません。");
            }
            // Load
            //string src = File.ReadAllText(fileName);
            string src = StrUnit.LoadFromFileAutoEnc(fileName);
            return src;
        }
        
        public Object _saveFile(INakoFuncCallInfo info)
        {
            string s = info.StackPopAsString();
            string fileName = info.StackPopAsString();

            System.Text.Encoding enc = new System.Text.UTF8Encoding(false);
            System.IO.File.WriteAllText(fileName, s, enc);
            
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
        
        public Object _execCommandAndGetResult(INakoFuncCallInfo info)
        {
			string s = info.StackPopAsString();
			string ret = "";
			if(NWEnviroment.isWindows()){
				ret = WindowsCommand.execute(s);
			}else{
				ret = LinuxCommand.execute(s);
			}
			return ret;
        }
        
        public Object _exists(INakoFuncCallInfo info)
        {
            string path = info.StackPopAsString();
            bool result = System.IO.File.Exists(path);
            return (Int64)(result ? 1 : 0);
        }

        public Object _existsDir(INakoFuncCallInfo info)
        {
            string path = info.StackPopAsString();
            bool result = System.IO.Directory.Exists(path);
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
        
        public object _getCurrentDir(INakoFuncCallInfo info){
        	return _path(Directory.GetCurrentDirectory());
		}
        public object _setCurrentDir(INakoFuncCallInfo info){
            string path = info.StackPopAsString();
        	Directory.SetCurrentDirectory(path);
        	return null;
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
        
        //------------------------------------------------------------------
        // 移動コピー削除
        public Object _copyFile(INakoFuncCallInfo info)
        {
        	string f1 = info.StackPopAsString();
        	string f2 = info.StackPopAsString();
        	File.Copy(f1, f2);
        	return null;
        }
        public Object _moveFile(INakoFuncCallInfo info)
        {
        	string f1 = info.StackPopAsString();
        	string f2 = info.StackPopAsString();
        	File.Move(f1, f2);
        	return null;
        }
        public Object _removeFile(INakoFuncCallInfo info)
        {
            string f = info.StackPopAsString();
            FileInfo finfo = new FileInfo(f);
            if(finfo.Exists){
                if((finfo.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly){
                    finfo.Attributes = FileAttributes.Normal;
                }
                File.Delete(f);
            }
            return null;
        }
        public Object _removeDir(INakoFuncCallInfo info)
        {
        	string f = info.StackPopAsString();
        	Directory.Delete(f);
        	return null;
        }
        public Object _makeDir(INakoFuncCallInfo info)
        {
        	string f = info.StackPopAsString();
        	Directory.CreateDirectory(f);
        	return null;
        }

        public Object _enumFiles(INakoFuncCallInfo info)
        {
            string path = info.StackPopAsString();
            string[] files = Directory.GetFiles(path);
            NakoVarArray res = info.CreateArray();
            for (int i = 0; i < files.Length; i++)
            {
                string f = Path.GetFileName(files[i]);
                res.SetValue(i, f);
            }
            return res;
        }

        public Object _enumDirs(INakoFuncCallInfo info)
        {
            string path = info.StackPopAsString();
            string[] files = Directory.GetDirectories(path);
            NakoVarArray res = info.CreateArray();
            for (int i = 0; i < files.Length; i++)
            {
                res.SetValue(i, files[i]);
            }
            return res;
        }
        //------------------------------------------------------------------
        // パス操作
        public Object _dirname(INakoFuncCallInfo info)
        {
            string path = info.StackPopAsString();
            string res = Path.GetDirectoryName(path);
            return res;
        }
        public Object _basename(INakoFuncCallInfo info)
        {
            string path = info.StackPopAsString();
            string res = Path.GetFileName(path);
            return res;
        }
        public Object _extname(INakoFuncCallInfo info)
        {
            string path = info.StackPopAsString();
            string res = Path.GetExtension(path);
            return res;
        }
		public object _sizeof(INakoFuncCallInfo info){
            string fp = info.StackPopAsString();
			FileInfo fi = new FileInfo(fp);
			return (int)fi.Length;
		}
		public object _append(INakoFuncCallInfo info){
            string s = info.StackPopAsString();
            string fileName = info.StackPopAsString();

            System.Text.Encoding enc = new System.Text.UTF8Encoding(false);
            System.IO.File.AppendAllText(fileName, s, enc);
            return null;
		}
        public object _relativePath(INakoFuncCallInfo info){
            string s = info.StackPopAsString();
            return System.IO.Path.GetFullPath(s);
        }
        public object _updateDate(INakoFuncCallInfo info){
            string s = info.StackPopAsString();
            return File.GetLastWriteTime(s).ToShortDateString();
        }
        public object _driveType(INakoFuncCallInfo info){
            string s = info.StackPopAsString();
            foreach(DriveInfo drive in DriveInfo.GetDrives()){
                if(drive.Name == s){
                    return drive.DriveType;
                }
            }
            return null;
        }
        public IEnumerable<object> _readLine(INakoFuncCallInfo info){//TODO:反復の今の実装では全行一旦読み込まないといけないんだけど、どうしようかな
        //TODO:それにiteratorを文字しても、ちゃんと変数に反映してくれないから、それも考えないと・・・
//            string s = info.StackPopAsString();
//            string line = "";
//            using(StreamReader sr = new StreamReader(File.OpenRead(s))){
//                while((line=sr.ReadLine())!=null){
//                    yield return line;
//                }
//            }
            return null;
        }
    }
}
