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
    public class TestNakoPluginSystem
    {
        NakoCompiler com;
        NakoInterpreter runner = new NakoInterpreter();

        public TestNakoPluginSystem()
        {
            NakoCompilerLoaderInfo info = new NakoCompilerLoaderInfo();
            info.PreloadModules = new NakoPlugin.INakoPlugin[] {
                new NakoBaseSystem(),
                new NakoPluginSample.NakoPluginSample()
            };
            com = new NakoCompiler(info);
        }

        [Test]
        public void TestToInt()
        {
            com.DirectSource = 
                "A=「10」\n" +
                "B=(A>3)\n" +
                "Bを表示";
            runner.Run(com.Codes);
            Assert.AreEqual("False", runner.PrintLog );
            com.DirectSource = 
                "A=「10」\n" +
                "C=Aを整数変換\n" +
                "B=(C>3)\n" +
                "Bを表示";
            runner.Run(com.Codes);
            Assert.AreEqual("True", runner.PrintLog );
        }

        [Test]
        public void TestToString()
        {
            com.DirectSource = 
                "A=10\n"+
                "B=(A>3)\n" +
                "Bを表示";
            runner.Run(com.Codes);
            Assert.AreEqual("True", runner.PrintLog );
            com.DirectSource = 
                "A=10\n" +
                "C=Aを文字列変換\n" +
                "B=(C>3)\n" +
                "Bを表示";
            runner.Run(com.Codes);
            Assert.AreEqual("False", runner.PrintLog );
        }

     }
}
