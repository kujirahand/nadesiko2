using System;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;
using Libnako.JPNCompiler;
using Libnako.Interpreter;
using Libnako.Interpreter.ILCode;

namespace NakoPluginTest
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
            codes = ns.WriteIL(
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
            codes = ns.WriteIL(
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
            codes = ns.WriteIL(
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
            codes = ns.WriteIL(
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
            codes = ns.WriteIL(
                "●AにBを加算\n" +
                "　　それ=A+B\n" +
                "\n" +
                "3に5を加算して、それに8を加算する;PRINT それ\n" +
                "\n");
            runner.Run(codes);
            Assert.AreEqual(runner.PrintLog, "16");
        }
        [Test]
        public void TestUserFunc_Add3()
        {
            codes = ns.WriteIL(
                "\n\n●AにBを加算\n" +
                "　　それ=A+B\n" +
                "\n" +
				"●AにBをKAKERU\n" +
				"　　それ=A*B\n" +
				"\n" +
                "3に5をKAKERU;PRINT それ\n" +
                "\n");
            runner.Run(codes);
            Assert.AreEqual(runner.PrintLog, "15");
        }
        [Test]
        public void TestSysFunc_sub()
        {
            codes = ns.WriteIL(
                "PRINT 5から3を引く\n" +
                "\n");
            runner.Run(codes);
            Assert.AreEqual(runner.PrintLog, "2");
        }
        [Test]
        public void TestSysFunc_subEx()
        {
            codes = ns.WriteIL(
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
            nc.WriteIL(
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
            codes = ns.WriteIL(
                "100,10,5,引く,足す,継続表示\n" +
                "\n");
            runner.Run(codes);
            Assert.AreEqual("105",runner.PrintLog);
        }
        [Test]
        public void TestCall_likeBASIC1()
        {
            // memo: BASICの関数のように関数をコール
            codes = ns.WriteIL(
                "PRINT 引く(10,4)\n" +
                "\n");
            runner.Run(codes);
            Assert.AreEqual("6",runner.PrintLog);
        }
        [Test]
        public void TestCall_likeBASIC2()
        {
            // memo: BASICの関数のように関数をコール
            codes = ns.WriteIL(
                "引く(10,4)を継続表示\n" +
                "\n");
            runner.Run(codes);
            Assert.AreEqual("6", runner.PrintLog);
        }
        [Test]
        public void TestCall_likeBASIC3()
        {
            // memo: BASICの関数のように関数をコール
            codes = ns.WriteIL(
                "引く(足す(3,8),4)を継続表示\n" +
                "\n");
            runner.Run(codes);
            Assert.AreEqual(runner.PrintLog, "7");
        }
        [Test]
        public void TestCall_likeBASIC4()
        {
            // memo: BASICの関数のように関数をコール
            codes = ns.WriteIL(
                "1+2に3を足して継続表示\n" +
                "\n");
            runner.Run(codes);
            Assert.AreEqual(runner.PrintLog, "6");
        }
        [Test]
        public void TestUserFunc_Ret()
        {
            runner.Run(ns.WriteIL(
                "●ほげ\n" +
                "　　それ＝「ふが」" +
                "　　戻る" +
                "　　それ＝「ぴよ」" +
                "\n" +
                "PRINT ほげ\n" +
                "\n"));
            Assert.AreEqual(runner.PrintLog, "ふが");
        }
    }
}
