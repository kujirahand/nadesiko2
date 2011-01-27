using System;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;
using Libnako.JPNCompiler;
using Libnako.Interpreter;
using Libnako.JPNCompiler.ILWriter;

namespace TestNako
{
    [TestFixture]
    public class TestNodeCallFunction
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
            NakoCompiler nc = new NakoCompiler();
            NakoInterpreter ni = new NakoInterpreter();
            nc.Publish(
                "3を10から引く。\n" +
                "PRINT それ\n" +
                "\n");
            ni.Run(nc.Codes);
            Assert.AreEqual("7", ni.PrintLog);
        }
        [Test]
        public void TestStackCall_noJosi()
        {
            // memo:
            // 敢えて、FORTH っぽく、助詞がなくても動くようにしたい!!
            codes = ns.Publish(
                "100,10,5,引く,足す,表示\n" +
                "\n");
            runner.Run(codes);
            Assert.AreEqual("105",runner.PrintLog);
        }
        [Test]
        public void TestCall_likeBASIC1()
        {
            // memo: BASICの関数のように関数をコール
            codes = ns.Publish(
                "PRINT 引く(10,4)\n" +
                "\n");
            runner.Run(codes);
            Assert.AreEqual("6",runner.PrintLog);
        }
        [Test]
        public void TestCall_likeBASIC2()
        {
            // memo: BASICの関数のように関数をコール
            codes = ns.Publish(
                "引く(10,4)を表示\n" +
                "\n");
            runner.Run(codes);
            Assert.AreEqual("6", runner.PrintLog);
        }
        [Test]
        public void TestCall_likeBASIC3()
        {
            // memo: BASICの関数のように関数をコール
            codes = ns.Publish(
                "引く(足す(3,8),4)を表示\n" +
                "\n");
            runner.Run(codes);
            Assert.AreEqual(runner.PrintLog, "7");
        }
        [Test]
        public void TestCall_likeBASIC4()
        {
            // memo: BASICの関数のように関数をコール
            codes = ns.Publish(
                "1+2に3を足して表示\n" +
                "\n");
            runner.Run(codes);
            Assert.AreEqual(runner.PrintLog, "6");
        }
    }
}
