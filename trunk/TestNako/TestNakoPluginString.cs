using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Libnako.JPNCompiler;
using Libnako.Interpreter;
using Libnako.NakoAPI;

using Libnako.JPNCompiler.ILWriter;

using NakoPluginString;

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
                new NakoPluginString.NakoPluginString()
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
    }
}
