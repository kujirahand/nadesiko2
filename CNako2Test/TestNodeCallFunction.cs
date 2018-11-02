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
            codes = ns.WriteIL(
                "●ほげ\n" +
                "　　それ＝「ふが」" +
                "　　「bar」で戻る" +
                "　　それ＝「ぴよ」" +
                "\n" +
                "PRINT ほげ\n" +
                "\n");
            runner.Run(codes);
            Assert.AreEqual(runner.PrintLog, "bar");
        }
        [Test]
        public void TestUserFunc_Multi()
        {
            codes = ns.WriteIL(
                "●ほげ\n" +
                "　　それ＝「ふが」\n" +
                "●ほぎゃ\n" +
                "　　それ＝「bar」" +
                "\n" +
                "PRINT ほげ\n" +
                "PRINT ほぎゃ\n" +
                "\n");
            runner.Run(codes);
            Assert.AreEqual(runner.PrintLog, "ふがbar");
        }
		[Test]
		public void TestCallUserFuncInUserFunc()
		{
			codes = ns.WriteIL(
				"●ほげ\n" +
				"  それ＝「ふが」\n" +
				"●ふが\n" +
				"  ほげ" +
				"\n" +
				"PRINT ふが\n" +
				"\n");
			runner.Run(codes);
			Assert.AreEqual(runner.PrintLog, "ふが");
			//ユーザー関数内でユーザー関数を呼び出すと、それまでに定義したローカル変数をリセットしてしまうバグがあったのでテスト追加
			codes = ns.WriteIL(
				"●ほげ(Sで)\n" +
				"  それ＝「ほげ」\n" +
				"●ふが(Sで)\n" +
				"  T=Sでほげ\n" +
				"  それ=S\n" +
				"\n" +
				"PRINT `ふが`でふが\n" +
				"\n");
			runner.Run(codes);
			Assert.AreEqual(runner.PrintLog, "ふが");
            codes = ns.WriteIL(
                "37の関数A\n"+
                "●関数B (Sの)\n"+
                "  S = S / 2\n"+
                "  PRINT 「{S},」\n"+
                "  もしS==1ならば戻る\n"+
                "  m = Sを2で割った余り\n"+
                "  もしm == 0ならば\n"+
                "    Sの関数B\n"+
                "  違えば\n"+
                "    Sの関数A\n"+
                "●関数A (Sの)\n"+
                "  S=S*3+1\n"+
                "  PRINT 「{S},」\n"+
                "  m = Sを2で割った余り\n"+
                "  もしm == 0ならば\n"+
                "    Sの関数B\n"+
                "  違えば\n"+
                "    Sの関数A\n"+
                "\n");
            runner.Run(codes);
            Assert.AreEqual(runner.PrintLog, "112,56,28,14,7,22,11,34,17,52,26,13,40,20,10,5,16,8,4,2,1,");
		}
        [Test]
        public void TestCallUserFuncAndCheckGlobalVar()
        {
            codes = ns.WriteIL(
                "●ほげ\n" +
                "  A＝「ふが」\n" +
                "  Aで戻る\n" +
                "A=`hoge`\n" +
                "B = ほげ\n" +
                "PRINT A\n" +
                "\n");
            runner.Run(codes);
            Assert.AreEqual("hoge",runner.PrintLog);
        }
        [Test]
        public void TestCallUserFuncRecursive ()
        {
            codes = ns.WriteIL(
                "●フィボナッチ (Iの)\n"+
                "  もしI<2ならば\n"+
                "    Iで戻る\n"+
                "  違えば\n"+
                "    F1 = (I-2)のフィボナッチ\n" +
                "    F2 = (I - 1)のフィボナッチ\n"+
                "    F1 + F2で戻る\n"+
                "PRINT 6のフィボナッチ\n"+
                "\n");
            runner.Run(codes);
            Assert.AreEqual("8",runner.PrintLog);
        }
        [Test]
        public void TestSetUserFuncToVariable ()
        {
            codes = ns.WriteIL (
                "●関数A (Sの)\n"+
                "  S+10で戻る\n"+
                "B = 関数Aの定義\n"+
                "A [`a`]= B\n"+
                "PRINT A [`a`](10)\n"+
                "\n");
            runner.Run(codes);
            Assert.AreEqual("20",runner.PrintLog);
        }
        [Test]
        public void TestCallUserFuncWithUserFuncParameter ()
        { 
            codes = ns.WriteIL (
                "●マップ (Sを{ 関数 (1)}Fで)\n"+
                "  R = F (S)\n"+
                "  Rで戻る\n"+
                "●関数A (Sの)\n"+
                "  S+10で戻る\n"+
                "RET = マップ (10, 関数Aの定義)\n"+
                "PRINT RET\n"+
                "\n");
            runner.Run(codes);
            Assert.AreEqual("20",runner.PrintLog);
        }
        [Test]
        public void TestSetUserFuncWithDefaultValue ()
        { 
            codes = ns.WriteIL (
                "●関数A ({整数=10}Sの)\n"+
                "  S+10で戻る\n"+
                "B = 関数Aの定義\n"+
                "A [`a`]= B\n"+
                "PRINT A [`a`]()\n"+
                "\n");
            runner.Run(codes);
            Assert.AreEqual("20",runner.PrintLog);
        }
    }
}
