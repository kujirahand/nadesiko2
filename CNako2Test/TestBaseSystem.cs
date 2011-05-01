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
        [Test]
        public void Test_strpos()
        {
            NakoCompiler nc = new NakoCompiler();
            NakoInterpreter ni = new NakoInterpreter();
            nc.DirectSource =
                "`abc`で`b`が何文字目\n" +
                "それを継続表示\n" +
                "";
            ni.Run(nc.Codes);
            Assert.AreEqual("2", ni.PrintLog);
        }
        [Test]
        public void Test_strpos2()
        {
            NakoCompiler nc = new NakoCompiler();
            NakoInterpreter ni = new NakoInterpreter();
            nc.DirectSource =
                "`abc`で`d`が何文字目\n" +
                "それを継続表示\n" +
                "";
            ni.Run(nc.Codes);
            Assert.AreEqual("0", ni.PrintLog);
            //
            nc.DirectSource =
                "`あいう`で`う`が何文字目\n" +
                "それを継続表示\n" +
                "";
            ni.Run(nc.Codes);
            Assert.AreEqual("3", ni.PrintLog);
            //
            nc.DirectSource =
                "12345で4が何文字目\n" +
                "それを継続表示\n" +
                "";
            ni.Run(nc.Codes);
            Assert.AreEqual("4", ni.PrintLog);
        }
        [Test]
        public void Test_Abs()
        {
            NakoCompiler nc = new NakoCompiler();
            NakoInterpreter ni = new NakoInterpreter();
            nc.DirectSource =
                "-3.14の絶対値\n" +
                "それを継続表示\n" +
                "";
            ni.Run(nc.Codes);
            Assert.AreEqual("3.14", ni.PrintLog);
            //
            nc.DirectSource =
                "-10の絶対値\n" +
                "それを継続表示\n" +
                "";
            ni.Run(nc.Codes);
            Assert.AreEqual("10", ni.PrintLog);
            //
            nc.DirectSource =
                "A=-3\n" +
                "Aの絶対値を継続表示\n" +
                "";
            ni.Run(nc.Codes);
            Assert.AreEqual("3", ni.PrintLog);
        }
    }
}
