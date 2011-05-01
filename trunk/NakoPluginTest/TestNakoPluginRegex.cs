using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Libnako.JPNCompiler;
using Libnako.Interpreter;
using Libnako.NakoAPI;

using Libnako.JPNCompiler.ILWriter;

using NakoPluginRegex;

namespace NakoPluginTest
{
    [TestFixture]
    public class TestNakoPluginRegex
    {
        NakoCompiler com;
        NakoInterpreter runner = new NakoInterpreter();

        public TestNakoPluginRegex()
        {
            NakoCompilerLoaderInfo info = new NakoCompilerLoaderInfo();
            info.PreloadModules = new NakoPlugin.INakoPlugin[] {
                new NakoBaseSystem(),
                new NakoPluginRegex.NakoPluginRegex()
            };
            com = new NakoCompiler(info);
        }

        [Test]
        public void TestMatch()
        {
            com.DirectSource = 
                "「なでしこ」の「^(.*?)こ$」を正規表現マッチを表示。";
            runner.Run(com.Codes);
            Assert.AreEqual( "なでしこ", runner.PrintLog);
            com.DirectSource = 
                "「なでしこ」の「^(.*?)こ$」を正規表現マッチ。\n" +
                "抽出文字列[1]を表示";
            runner.Run(com.Codes);
            Assert.AreEqual( "なでし", runner.PrintLog);
        }

        [Test]
        public void TestReplace()
        {
            com.DirectSource = 
                "「なでしここ」の「^.*?こ」を「ら」に正規表現置換を表示。";
            runner.Run(com.Codes);
            Assert.AreEqual( "らこ", runner.PrintLog);
        }
    }
}
