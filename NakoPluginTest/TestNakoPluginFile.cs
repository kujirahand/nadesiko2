using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Libnako.JPNCompiler;
using Libnako.Interpreter;
using Libnako.NakoAPI;

using NakoPluginFile;
using NakoPlugin;

namespace NakoPluginTest
{
    [TestFixture]
    public class TestNakoPluginFile
    {
        NakoCompiler com;
        NakoInterpreter runner = new NakoInterpreter();
        string sjisFilePath = "";
        string asciiFilePath = "";
        public TestNakoPluginFile()
        {
            NakoCompilerLoaderInfo info = new NakoCompilerLoaderInfo();
            info.PreloadModules = new NakoPlugin.INakoPlugin[] {
                new NakoBaseSystem(),
                new NakoPluginArray(),
                new NakoPluginString(),
                new NakoPluginFile.NakoPluginFile()
            };
            com = new NakoCompiler(info);
            string assemblyPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string assemblyDirectory = System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(assemblyPath)));
            this.sjisFilePath = System.IO.Path.Combine(assemblyDirectory, "SJISTEST.txt");
            this.asciiFilePath = System.IO.Path.Combine(assemblyDirectory, "ASCIITEST.txt");
        }
		
		private string ConvertPath(string s){
			return (NWEnviroment.osVersionStr().Contains("Windows"))? s.Replace("/","\\") : s.Replace ("\\","/");
		}

        [Test]
        public void Test_mkdir()
        {
            string hoge = System.IO.Path.GetTempPath() + ConvertPath("\\hoge\\fuga");
            // ディレクトリの作成
            runner.Run(com.WriteIL(
                "DIR=「"+hoge+"」\n" +
                "DIRにフォルダ作成。\n" +
                "(DIRがフォルダ存在?)を表示。\n" +
                "\n" +
                ""));
            Assert.AreEqual("1", runner.PrintLog);
        }

        [Test]
        public void Test_rmdir()
        {
            string hoge = System.IO.Path.GetTempPath() + ConvertPath("\\hoge\\fuga001");
            // ディレクトリの作成
            runner.Run(com.WriteIL(
                "DIR=「" + hoge + "」\n" +
                "DIRにフォルダ作成。DIRのフォルダ削除。\n" +
                "(DIRがフォルダ存在?)を表示。\n" +
                ""));
            Assert.AreEqual("0", runner.PrintLog);
        }

        [Test]
        public void Test_saveText()
        {
            string hoge = System.IO.Path.GetTempPath() + ConvertPath("\\hoge");
            string test_txt = hoge + ConvertPath("\\test001.txt");
            // ディレクトリの作成
            runner.Run(com.WriteIL(
                "HOGE=「"+hoge+"」。\n" +
                "HOGEにフォルダ作成。"+
                "F=「" + test_txt + "」\n" +
                "Fに「あいう」を保存。\n" +
                "Fを開く。\n" +
                "それを表示。\n" +
                ""));
            runner.Run(com.Codes);
            Assert.AreEqual("あいう", runner.PrintLog);
        }

        [Test]
        public void Test_rmText()
        {
            string tmp = System.IO.Path.GetTempPath();
            runner.Run(com.WriteIL(
                "F=「"+tmp+"hoge.txt」\n" +
				"Fに「abc」を保存\n" +
                "Fをファイル削除。\n" +
                "(Fが存在?)を表示。\n" +
                "\n"));
            Assert.AreEqual("0", runner.PrintLog);
        }

        [Test]
        public void Test_moveText()
        {
            string tmp = System.IO.Path.GetTempPath();
            string to = System.IO.Path.GetTempPath() + "hoge";
            runner.Run(com.WriteIL(
                "F=「"+tmp+"hoge.txt」\n" +
                "F2=「"+to+"fuga.txt」\n" +
				"Fに「abc」を保存\n" +
                "F2をファイル削除。\n" +
                "FからF2へファイル移動。\n" +
                "(Fが存在?)を継続表示。\n" +
                "(F2が存在?)を継続表示。\n" +
                "\n" +
                ""));
            Assert.AreEqual("01", runner.PrintLog);
        }

        [Test]
        public void Test_Path()
        {
            string desktop = NWEnviroment.AppendLastPathFlag(System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
            runner.Run(com.WriteIL(
                "デスクトップを表示。\n" +
                ""));
            Assert.AreEqual(desktop, runner.PrintLog);
			string path = ConvertPath("/tmp/hoge/fuga/piyo.txt");
            runner.Run(com.WriteIL(
                "「"+path+"」のパス抽出を表示。\n" +
                ""));
            Assert.AreEqual(ConvertPath("/tmp/hoge/fuga"), runner.PrintLog);
            runner.Run(com.WriteIL(
                "「"+path+"」の拡張子抽出を表示。\n" +
                ""));
            Assert.AreEqual(ConvertPath(".txt"), runner.PrintLog);
            runner.Run(com.WriteIL(
                "「"+path+"」のファイル名抽出を表示。\n" +
                ""));
            Assert.AreEqual(ConvertPath("piyo.txt"), runner.PrintLog);
			
        }

        [Test]
        public void Test_enumFiles()
        {
			string tmp_path,a,b;
			if(NWEnviroment.osVersionStr()=="Unix"){
				tmp_path = "/hoge/fuga/nyaa";
				a = "/a.txt";
				b = "/b.txt";
			}else{
				tmp_path = "\\hoge\\fuga\\nyaa" ;
				a = "\\a.txt";
				b = "\\b.txt";
			}
            string tmp = System.IO.Path.GetTempPath()+tmp_path;

            runner.Run(com.WriteIL(
                "DIR=「"+tmp+"」\n" +
                "DIRにフォルダ作成。\n" +
                "F=DIR&「"+a+"」;Fに「abc」を保存。\n" +
                "F=DIR&「"+b+"」;Fに「abc」を保存。\n" +
                "DIRのファイル列挙して「/」で配列結合して表示。"));
            Assert.AreEqual("a.txt/b.txt", runner.PrintLog);
        }

        [Test]
        public void Test_enumDirs()
        {
            string tmp = System.IO.Path.GetTempPath()+ConvertPath("hoge");
            runner.Run(com.WriteIL(
                "DIR=「"+tmp+"」\n" +
                "DIR&「"+ConvertPath("\\fuga")+"」にフォルダ作成。\n" +
                "DIR&「"+ConvertPath("\\piyo")+"」にフォルダ作成。\n" +
                "DIRのフォルダ列挙して「#」で配列結合して表示。"));
            Assert.AreEqual(ConvertPath("/tmp/hoge/fuga#/tmp/hoge/piyo"), runner.PrintLog);
        }

        [Test]
        public void Test_fileSize()
        {
            string tmp = System.IO.Path.GetTempPath();
            runner.Run(com.WriteIL(
                "F=「"+tmp+"hoge.txt」\n" +
				"Fに「」を保存\n" +
                "Fのファイルサイズを継続表示" +
                ""));
            Assert.AreEqual("0", runner.PrintLog);
            runner.Run(com.WriteIL(
                "F=「"+tmp+"hoge.txt」\n" +
				"Fに「abcdefg」を保存\n" +
                "Fのファイルサイズを継続表示" +
                ""));
            Assert.AreEqual("abcdefg".Length.ToString(), runner.PrintLog);
        }

        [Test]
        public void Test_append()
        {
            string tmp = System.IO.Path.GetTempPath();
            runner.Run(com.WriteIL(
                "F=「"+tmp+"hoge.txt」\n" +
				"Fに「」を保存\n" +
				"「ほげほげ」をFに追加保存\n" +
                "Fを開く\n" +
                "それを継続表示" +
                ""));
            Assert.AreEqual("ほげほげ", runner.PrintLog);
        }

        [Test]
        public void Test_currentDir()
        {
            string currentDir = System.IO.Directory.GetCurrentDirectory();
            runner.Run(com.WriteIL(
                "F=作業フォルダ取得\n" +
                "Fを継続表示\n" +
                ""));
            Assert.AreEqual(NWEnviroment.AppendLastPathFlag(currentDir), runner.PrintLog);
            currentDir = System.IO.Path.GetTempPath();
            runner.Run(com.WriteIL(
                "「"+currentDir+"」に作業フォルダ変更\n" +
                "F=作業フォルダ取得\n" +
                "Fを継続表示\n" +
                ""));
            Assert.AreEqual(NWEnviroment.AppendLastPathFlag(currentDir), runner.PrintLog);
            
        }

        [Test]
        public void Test_command()
        {
            runner.Run(com.WriteIL(
                "S=「echo 'hoge'」をコマンド実行\n" +
                "Sを継続表示\n" +
                ""));
            Assert.AreEqual("hoge\n\n", runner.PrintLog);
            
        }

		[Test]
		public void Test_read()
		{
			string tmp = System.IO.Path.GetTempPath();
			runner.Run(com.WriteIL(
				"「ほげ\r\nふが」を「"+tmp+"hoge.txt」に保存\n" +
				"「"+tmp+"hoge.txt」を読んで表示\n" +
				""));
			Assert.AreEqual("ほげ\r\nふが", runner.PrintLog);
			//read sjis file automatically
			runner.Run(com.WriteIL(
				"「"+sjisFilePath+"」を読んで表示\n" +
				""));
			Assert.AreEqual("あいえお\n", runner.PrintLog);
			//read sjis file with utf-8 encode
			runner.Run(com.WriteIL(
				"「"+sjisFilePath+"」から「UTF-8」で読んで表示\n" +
				""));
			Assert.AreNotEqual("あいえお\n", runner.PrintLog);
			//read sjis file with sjis encode
			runner.Run(com.WriteIL(
				"「"+sjisFilePath+"」から「CP932」で読んで表示\n" +
				""));
			Assert.AreEqual("あいえお\n", runner.PrintLog);
			runner.Run(com.WriteIL(
				"「"+asciiFilePath+"」を読んで表示\n" +
				""));
			Assert.AreEqual(@"1942", runner.PrintLog);
		}

        [Test]
        public void Test_readLine()
        {
            string tmp = System.IO.Path.GetTempPath();
            runner.Run(com.WriteIL(
                "「ほげ\r\nふが」を「"+tmp+"hoge.txt」に保存\n" +
                "「"+tmp+"hoge.txt」から毎行読んで反復\n" +
                "   対象を継続表示\n" +
                ""));
			Assert.AreEqual("ほげふが", runner.PrintLog);
			//read sjis file automatically
			runner.Run(com.WriteIL(
				"「"+sjisFilePath+"」から毎行読んで反復\n" +
				"   対象を継続表示\n" +
				""));
			Assert.AreEqual("あいえお", runner.PrintLog);
			//read sjis file with utf-8 encode
			runner.Run(com.WriteIL(
				"「"+sjisFilePath+"」から「UTF-8」で毎行読んで反復\n" +
				"   対象を継続表示\n" +
				""));
			Assert.AreNotEqual("あいえお", runner.PrintLog);
			//read sjis file with sjis encode
			runner.Run(com.WriteIL(
				"「"+sjisFilePath+"」から「CP932」で毎行読んで反復\n" +
				"   対象を継続表示\n" +
				""));
			Assert.AreEqual("あいえお", runner.PrintLog);
        }

        [Test]
        public void Test_driveType()
        {
            string driveName = @"C:\";
            if(!NWEnviroment.isWindows()){
                driveName = @"/";
            }
            runner.Run(com.WriteIL(
                "S=「"+driveName+"」のドライブ種類\n" +
                "Sを継続表示\n" +
                ""));
            Assert.AreEqual("Fixed", runner.PrintLog);
        }

        [Test]
        public void Test_updateDate()
        {
            string tmp = System.IO.Path.GetTempPath();
            runner.Run(com.WriteIL(
                "「ほげ\r\nふが」を「"+tmp+"hoge.txt」に保存\n" +
                "S=「"+tmp+"hoge.txt」のファイル更新日時\n" +
                "Sを継続表示\n" +
                ""));
            Assert.AreEqual(DateTime.Today.ToShortDateString(),runner.PrintLog);
        }

        [Test]
        public void Test_relativePath()
        {
            string relativePath = ConvertPath(@"..\hoge\fuga");
            string absolutePath = System.IO.Path.GetFullPath(relativePath);
            runner.Run(com.WriteIL(
                "F=「"+relativePath+"」を作業フォルダ取得で相対パス展開\n" +
                "Fを継続表示\n" +
                ""));
            Assert.AreEqual(absolutePath, runner.PrintLog);
        }

        [Test]
        public void Test_stream()
        {
            string tmp = System.IO.Path.GetTempPath();
            runner.Run(com.WriteIL(
                "「"+tmp+"hoge.txt」をファイル削除\n" +
                "F=「"+tmp+"hoge.txt」を「書」でファイルストリーム開く\n" +
                "「Stream」をFにファイルストリーム一行書く\n" +
                "Fをファイルストリーム閉じる\n" +
                "F=「"+tmp+"hoge.txt」を「読」でファイルストリーム開く\n" +
                "S=Fのファイルストリーム一行読む\n" +
                "Fをファイルストリーム閉じる\n" +
                "Sを継続表示\n" +
                ""));
            Assert.AreEqual("Stream", runner.PrintLog);
        }
    }
}
