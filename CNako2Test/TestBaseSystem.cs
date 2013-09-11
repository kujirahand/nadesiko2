using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Libnako.JPNCompiler;
using Libnako.Interpreter;

namespace NakoPluginTest
{
    [TestFixture]
    public class TestBaseSystem
    {
        NakoCompiler com = new NakoCompiler();
        NakoInterpreter runner = new NakoInterpreter();

        [Test]
        public void NakoVersion()
        {
            com.WriteIL("PRINT(ナデシコバージョン)");
            runner.Run(com.Codes);
            Assert.AreNotEqual("", runner.PrintLog);
        }

        [Test]
        public void Test_Abs()
        {
            com.WriteIL(
                "-3.14の絶対値\n" +
                "それを継続表示\n" +
                "");
            runner.Run(com.Codes);
            Assert.AreEqual("3.14", runner.PrintLog);
            //
            com.WriteIL(
                "-10の絶対値\n" +
                "それを継続表示\n" +
                "");
            runner.Run(com.Codes);
            Assert.AreEqual("10", runner.PrintLog);
            //
            com.WriteIL(
                "A=-3\n" +
                "Aの絶対値を継続表示\n" +
                "");
            runner.Run(com.Codes);
            Assert.AreEqual("3", runner.PrintLog);
        }
    }
}
