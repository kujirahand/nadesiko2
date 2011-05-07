using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Libnako.JPNCompiler;
using Libnako.Interpreter;
using Libnako.NakoAPI;

using NakoPluginFile;

namespace NakoPluginTest
{
    [TestFixture]
    public class TestNakoPluginFile
    {
        NakoCompiler com;
        NakoInterpreter runner = new NakoInterpreter();

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
        }

        [Test]
        public void Test_mkdir()
        {
            string hoge = System.IO.Path.GetTempPath() + "\\hoge\\fuga";
            // ディレクトリの作成
            com.DirectSource = 
                "DIR=「"+hoge+"」\n" +
                "DIRにフォルダ作成。\n" +
                "(DIRがフォルダ存在?)を表示。\n" +
                "\n" +
                "";
            runner.Run(com.Codes);
            Assert.AreEqual("1", runner.PrintLog);
        }

        [Test]
        public void Test_rmdir()
        {
            string hoge = System.IO.Path.GetTempPath() + "\\hoge\\fuga001";
            // ディレクトリの作成
            com.DirectSource =
                "DIR=「" + hoge + "」\n" +
                "DIRにフォルダ作成。DIRのフォルダ削除。\n" +
                "(DIRがフォルダ存在?)を表示。\n" +
                "";
            runner.Run(com.Codes);
            Assert.AreEqual("0", runner.PrintLog);
        }

        [Test]
        public void Test_saveText()
        {
            string hoge = System.IO.Path.GetTempPath() + "\\hoge";
            string test_txt = hoge + "\\test001.txt";
            // ディレクトリの作成
            com.DirectSource =
                "HOGE=「"+hoge+"」。\n" +
                "HOGEにフォルダ作成。"+
                "F=「" + test_txt + "」\n" +
                "Fに「あいう」を保存。\n" +
                "Fを開く。\n" +
                "それを表示。\n" +
                "";
            runner.Run(com.Codes);
            Assert.AreEqual("あいう", runner.PrintLog);
        }

        [Test]
        public void Test_Path()
        {
            string desktop = System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop)+"\\";
            com.DirectSource =
                "デスクトップを表示。\n" +
                "";
            runner.Run(com.Codes);
            Assert.AreEqual(desktop, runner.PrintLog);
        }

        [Test]
        public void Test_enumFiles()
        {
            string tmp = System.IO.Path.GetTempPath()+"\\hoge\\fuga\\nyaa";

            com.DirectSource =
                "DIR=「"+tmp+"」\n" +
                "DIRにフォルダ作成。\n" +
                "F=DIR&「\\a.txt」;Fに「abc」を保存。\n" +
                "F=DIR&「\\b.txt」;Fに「abc」を保存。\n" +
                "DIRのファイル列挙して「/」で配列結合して表示。";
            runner.Run(com.Codes);
            Assert.AreEqual("a.txt/b.txt", runner.PrintLog);
        }
    }
}
