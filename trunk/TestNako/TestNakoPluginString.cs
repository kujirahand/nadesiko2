using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Libnako.JPNCompiler;
using Libnako.Interpreter;
using Libnako.NakoAPI;

using Libnako.JPNCompiler.ILWriter;

using NakoPluginString;
//using NakoPluginSample;

namespace TestNako
{
    [TestFixture]
    public class TestNakoPluginString
    {
        NakoCompiler com;
        NakoInterpreter runner = new NakoInterpreter();

        public TestNakoPluginString()
        {
            NakoCompilerLoaderInfo info = new NakoCompilerLoaderInfo();
            info.PreloadModules = new NakoPlugin.INakoPlugin[] {
                new NakoBaseSystem(),
                new NakoPluginString.NakoPluginString()/*,
                new NakoPluginSample.NakoPluginSample()*/
            };
            com = new NakoCompiler(info);
        }

        [Test]
        public void TestLength()
        {
            com.DirectSource = 
                "「なでしこ」の文字数を表示。";
            runner.Run(com.Codes);
            Assert.AreEqual(runner.PrintLog, "4");
        }

        [Test]
        public void TestSearch()
        {
            com.DirectSource = 
                "「なでしこ」の「し」を文字列検索を表示。";
            runner.Run(com.Codes);
            Assert.AreEqual(runner.PrintLog, "3");
        }

        [Test]
        public void TestReplace()
        {
            com.DirectSource = 
                "「なでしこなでなで」の「なで」を「えら」に置換を表示。";
            runner.Run(com.Codes);
            Assert.AreEqual(runner.PrintLog, "えらしこえらえら");
        }

        [Test]
        public void TestReplaceA()
        {
            com.DirectSource = 
                "「なでしこなでなで」の「なで」を「えら」に単置換を表示。";
            runner.Run(com.Codes);
            Assert.AreEqual(runner.PrintLog, "えらしこなでなで");
        }

        [Test]
        public void TestLeft()
        {
            com.DirectSource = 
                "「なでしこ」の2文字左部分を表示。";
            runner.Run(com.Codes);
            Assert.AreEqual(runner.PrintLog, "なで");
        }

        [Test]
        public void TestTrim()
        {
            com.DirectSource = 
                "「　　なでしこ　　」をトリムを表示。";
            runner.Run(com.Codes);
            Assert.AreEqual(runner.PrintLog, "なでしこ");
        }


        [Test]
        public void TestRight()
        {
            com.DirectSource = 
                "「なでしこ」2文字右部分を表示。";
            runner.Run(com.Codes);
            Assert.AreEqual(runner.PrintLog, "しこ");
        }
        [Test]
        public void TestCut()
        {
//            com.DirectSource = 
//                "「なでしこ」から「し」まで切り取って表示。";
//            runner.Run(com.Codes);
//            Assert.AreEqual("なで", runner.PrintLog);
            com.DirectSource = 
                "S=「なでしこ」\n" +
                "Sから「し」まで切り取る\n" +
                "Sを表示。";
            runner.Run(com.Codes);
            Assert.AreEqual("こ", runner.PrintLog);
        }
        [Test]
        public void TestExtract()
        {
            com.DirectSource = 
                "「なでしこ」の1から2文字抜き出して表示。";
            runner.Run(com.Codes);
            Assert.AreEqual(runner.PrintLog, "なで");
        }
        [Test]
        public void TestEm()
        {
            com.DirectSource = 
                "「なでしこ」の全角か判定を表示。";
            runner.Run(com.Codes);
            Assert.AreEqual(runner.PrintLog, "1");
        }
        [Test]
        public void TestRemove()
        {
            com.DirectSource = 
                "S=「なでしこ」\n" +
                "Sの2から1文字削除\n" +
                "Sを表示。";
            runner.Run(com.Codes);
            Assert.AreEqual("なしこ",runner.PrintLog );
        }
        [Test]
        public void TestInsert()
        {
            com.DirectSource = 
                "「なしこ」の2に「で」を文字挿入して表示。";
            runner.Run(com.Codes);
            Assert.AreEqual("なでしこ", runner.PrintLog);
        }
        [Test]
        public void TestDegrade()
        {
            com.DirectSource = 
                "S=「なでしこ」を文字列分解\n" +
                "S[0]を表示。";
            runner.Run(com.Codes);
            Assert.AreEqual("な", runner.PrintLog);
        }
        [Test]
        public void TestExplode()
        {
            com.DirectSource = 
                "S=「なでしこ」を「し」で区切る\n" +
                "S[0]を表示。";
            runner.Run(com.Codes);
            Assert.AreEqual("なで", runner.PrintLog);
        }
        [Test]
        public void TestNum()
        {
            com.DirectSource = 
                "「0なでしこ」が数字か判定を表示。";
            runner.Run(com.Codes);
            Assert.AreEqual("1",runner.PrintLog);
        }
        [Test]
        public void TestAppend()
        {
            com.DirectSource = 
                "「なでし」に「こ」を追加して表示。";
            runner.Run(com.Codes);
            Assert.AreEqual("なでしこ",runner.PrintLog);
        }
        [Test]
        public void TestR()
        {
            com.DirectSource = 
                "S=「あ」\n" +
                "Sに「い」を接続!\n" +
                "Sを表示";
            runner.Run(com.Codes);
            Assert.AreEqual("あい",runner.PrintLog);
        }
    }
}
