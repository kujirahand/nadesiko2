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
            runner.Run(com.WriteIL(
                "A=「10」\n" +
                "B=(A>3)\n" +
                "Bを表示"));
            Assert.AreEqual("False", runner.PrintLog );
            runner.Run(com.WriteIL(
                "A=「10」\n" +
                "C=Aを整数変換\n" +
                "B=(C>3)\n" +
                "Bを表示"));
            Assert.AreEqual("True", runner.PrintLog );
        }

        [Test]
        public void TestToString()
        {
            runner.Run(com.WriteIL(
                "A=10\n"+
                "B=(A>3)\n" +
                "Bを表示"));
            Assert.AreEqual("True", runner.PrintLog );
            runner.Run(com.WriteIL(
                "A=10\n" +
                "C=Aを文字列変換\n" +
                "B=(C>3)\n" +
                "Bを表示"));
            Assert.AreEqual("False", runner.PrintLog );
        }

        [Test]
        public void TestToDouble()
        {
            runner.Run(com.WriteIL(
                "A=「10.05」\n"+
                "B=(A>3)\n" +
                "Bを表示"));
            Assert.AreEqual("False", runner.PrintLog );
            runner.Run(com.WriteIL(
                "A=「10.05」\n" +
                "C=Aを実数変換\n" +
                "B=(C>3)\n" +
                "Bを表示"));
            Assert.AreEqual("True", runner.PrintLog );
            runner.Run(com.WriteIL(
                "A=「10.05」\n" +
                "C=Aを実数変換\n" +
                "B=(C>10)\n" +
                "Bを表示"));
            Assert.AreEqual("True", runner.PrintLog );
        }

        [Test]
        public void TestIsZenkaku()
        {
            runner.Run(com.WriteIL(
                "「タモリ倶楽部」を全角か判定して表示"));
            Assert.AreEqual("1", runner.PrintLog );
            runner.Run(com.WriteIL(
                "「abテスト」を全角か判定して表示"));
            Assert.AreEqual("0", runner.PrintLog );
        }

        [Test]
        public void TestIsNumber()
        {
            runner.Run(com.WriteIL(
                "「タモリ倶楽部」を数字か判定して表示"));
            Assert.AreEqual("0", runner.PrintLog );
            runner.Run(com.WriteIL(
                "「abテスト」を数字か判定して表示"));
            Assert.AreEqual("0", runner.PrintLog );
            runner.Run(com.WriteIL(
                "「01テスト」を数字か判定して表示"));
            Assert.AreEqual("1", runner.PrintLog );
        }

     }
}
