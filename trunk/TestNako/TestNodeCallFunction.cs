using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;
using Libnako.JCompiler;
using Libnako.Interpreter;
using Libnako.JCompiler.ILWriter;

namespace TestNako
{
    [TestFixture]
    class TestNodeCallFunction
    {
        NakoCompiler ns = new NakoCompiler();
        NakoInterpreter runner = new NakoInterpreter();
        NakoILCodeList codes = null;

        [Test]
        public void TestSysFunc_AddAndLet()
        {
            codes = ns.Publish(
                "A=10に2を足す\n"+
                "PRINT A\n" + 
                ""
                );
            runner.Run(codes);
            Assert.AreEqual(runner.PrintLog, "12");
        }
        [Test]
        public void TestSysFunc_Add1()
        {
            codes = ns.Publish(
                "A=10;A=Aに2を足す\n" +
                "PRINT A\n" +
                ""
                );
            runner.Run(codes);
            Assert.AreEqual(runner.PrintLog, "12");
        }
        [Test]
        public void TestSysFunc_AddEx_ByRef()
        {
            codes = ns.Publish(
                "A=10;Aに2を足す!\n" +
                "PRINT A\n" +
                ""
                );
            runner.Run(codes);
            Assert.AreEqual(runner.PrintLog, "12");
        }
        [Test]
        public void TestUserFunc_Add()
        {
            codes = ns.Publish(
                "●AにBを加算\n" +
                "　　それ=A+B\n" +
                "\n" +
                "3に5を加算;PRINT それ\n" +
                "\n");
            runner.Run(codes);
            Assert.AreEqual(runner.PrintLog, "8");
        }
        [Test]
        public void TestUserFunc_Add2()
        {
            codes = ns.Publish(
                "●AにBを加算\n" +
                "　　それ=A+B\n" +
                "\n" +
                "3に5を加算して、それに8を加算する;PRINT それ\n" +
                "\n");
            runner.Run(codes);
            Assert.AreEqual(runner.PrintLog, "16");
        }
        [Test]
        public void TestSysFunc_sub()
        {
            codes = ns.Publish(
                "PRINT 5から3を引く\n" +
                "\n");
            runner.Run(codes);
            Assert.AreEqual(runner.PrintLog, "2");
        }
        [Test]
        public void TestSysFunc_subEx()
        {
            codes = ns.Publish(
                "A=10;B=3;" +
                "AからBを引く!\n" +
                "PRINT A");
            runner.Run(codes);
            Assert.AreEqual(runner.PrintLog, "7");
        }
        [Test]
        public void TestUserFunc_sub2()
        {
            codes = ns.Publish(
                "3を10から引く。\n" +
                "PRINT それ\n" +
                "\n");
            runner.Run(codes);
            Assert.AreEqual(runner.PrintLog, "7");
        }
    }
}
