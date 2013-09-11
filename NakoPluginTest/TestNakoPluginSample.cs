using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Libnako.JPNCompiler;
using Libnako.Interpreter;
using Libnako.NakoAPI;

using NakoPluginSample;
using Libnako.JPNCompiler.ILWriter;

namespace NakoPluginTest
{
    [TestFixture]
    public class TestNakoPluginSample
    {
        NakoCompiler com;
        NakoInterpreter runner = new NakoInterpreter();

        public TestNakoPluginSample()
        {
            NakoCompilerLoaderInfo info = new NakoCompilerLoaderInfo();
            info.PreloadModules = new NakoPlugin.INakoPlugin[] {
                new NakoBaseSystem(),
                new NakoPluginSample.NakoPluginSample()
            };
            com = new NakoCompiler(info);
        }

        [Test]
        public void TestSetVariable()
        {
            runner.Run(com.WriteIL(
                "HOGE=「ふが」\n" +
                "「はげ」へ変数HOGE書換\n" +
                "HOGEを表示"));
            Assert.AreEqual("はげ", runner.PrintLog );
        }

        [Test]
        public void TestAddEx()
        {
            runner.Run(com.WriteIL(
                "A=「ほげ」\n"+
                "Aに「ほげ」を接続!\n" +
                "Aを表示"));
            Assert.AreEqual("ほげほげ", runner.PrintLog );
        }

     }
}
