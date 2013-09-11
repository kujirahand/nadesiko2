using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Libnako.JPNCompiler;
using Libnako.Interpreter;
using Libnako.NakoAPI;

using Libnako.JPNCompiler.ILWriter;

using NakoPluginHttp;

namespace NakoPluginTest
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
                new NakoBaseSystem(),
                new NakoPluginHttp.NakoPluginHttp()
            };
            com = new NakoCompiler(info);
        }

        [Test]
        public void TestUrlDecode()
        {
            runner.Run(com.WriteIL(
                "「%e3%81%aa%e3%81%a7%e3%81%97%e3%81%93」をURLデコードして表示。"));
            Assert.AreEqual("なでしこ", runner.PrintLog );
        }
        [Test]
        public void TestUrlEncode()
        {
            runner.Run(com.WriteIL(
                "「なでしこ」をURLエンコードして小文字変換して表示。"));
            Assert.AreEqual("%e3%81%aa%e3%81%a7%e3%81%97%e3%81%93", runner.PrintLog );
        }

     }
}
