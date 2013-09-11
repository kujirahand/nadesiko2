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
            runner.Run(com.WriteIL(
                "「なでしこ」の「^(.*?)こ$」を正規表現マッチを表示。"));
            Assert.AreEqual("なでしこ", runner.PrintLog);
            runner.Run(com.WriteIL(
                "「なでしこ」の「^(.*?)こ$」を正規表現マッチ。\n" +
                "抽出文字列[1]を表示"));
            Assert.AreEqual("なでし", runner.PrintLog);
        }

        [Test]
        public void TestMatchAll()
        {
            runner.Run(com.WriteIL(
                "A=「1なでしこ2なでしこ3なでしこ」を「\\d(.*?)こ」で正規表現全マッチ。\n" +
                "A[0]を表示。"));
            Assert.AreEqual("1なでしこ", runner.PrintLog);
            runner.Run(com.WriteIL(
                "「なでしこ」の「.」を正規表現全マッチ。\n" +
                "抽出文字列の配列要素数を表示。"));
            Assert.AreEqual("4", runner.PrintLog);
        }

        [Test]
        public void TestReplace()
        {
            runner.Run(com.WriteIL(
                "「なでしここ」の「^.*?こ」を「ら」に正規表現置換を表示。"));
            Assert.AreEqual("らこ", runner.PrintLog);
        }
    }
}
