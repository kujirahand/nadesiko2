using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Libnako.JPNCompiler;
using Libnako.Interpreter;
using Libnako.NakoAPI;

using Libnako.JPNCompiler.ILWriter;

//using NakoPluginHttp;

namespace TestNako
{
    [TestFixture]
    public class TestNakoPluginHttp
    {
        NakoCompiler com;
        NakoInterpreter runner = new NakoInterpreter();

        public TestNakoPluginHttp()
        {
            NakoCompilerLoaderInfo info = new NakoCompilerLoaderInfo();
            info.PreloadModules = new NakoPlugin.INakoPlugin[] {
                new NakoBaseSystem()
                //new NakoPluginHttp.NakoPluginHttp()
            };
            com = new NakoCompiler(info);
        }

        [Test]
        public void TestUrlDecode()
        {
            com.DirectSource = 
                "「%e3%81%aa%e3%81%a7%e3%81%97%e3%81%93」をURLデコードして表示。";
            runner.Run(com.Codes);
            Assert.AreEqual("なでしこ", runner.PrintLog );
        }

     }
}
