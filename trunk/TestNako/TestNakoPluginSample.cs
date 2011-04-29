using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Libnako.JPNCompiler;
using Libnako.Interpreter;
using Libnako.NakoAPI;

using NakoPluginSample;
using Libnako.JPNCompiler.ILWriter;

namespace TestNako
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
            com.DirectSource = 
                "HOGE=「ふが」\n" +
                "「はげ」へ変数HOGE書換\n" +
                "HOGEを表示";
            runner.Run(com.Codes);
            Assert.AreEqual("はげ", runner.PrintLog );
        }

        [Test]
        public void TestAddEx()
        {
            com.DirectSource = 
                "A=「ほげ」\n"+
                "Aに「ほげ」を接続!\n" +
                "Aを表示";
            runner.Run(com.Codes);
            Assert.AreEqual("ほげほげ", runner.PrintLog );
        }

     }
}
