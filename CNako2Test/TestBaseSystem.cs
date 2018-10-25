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

        [Test]
        public void TestSubstitute()
        {
            runner.Run (com.WriteIL(
                "-3.14をAに代入\n" +
                "Aを継続表示\n" +
                ""));
            Assert.AreEqual("-3.14", runner.PrintLog);

            runner.Run (com.WriteIL(
                "A=10\n" +
                "AをBに代入\n" +
                "Bを継続表示\n" +
                ""));
            Assert.AreEqual("10", runner.PrintLog);
        }

        [Test]
        public void TestIntVal()
        {
            runner.Run (com.WriteIL(
                "-3.14をAに代入\n" +
                "Aの整数部分を継続表示\n" +
                ""));
            Assert.AreEqual("-3", runner.PrintLog);

            runner.Run (com.WriteIL(
                "A=0.001\n" +
                "Aの整数部分を継続表示\n" +
                ""));
            Assert.AreEqual("0", runner.PrintLog);
        }

        [Test]
        public void TestFloatVal()
        {
            runner.Run (com.WriteIL(
                "-3.14をAに代入\n" +
                "Aの小数部分を継続表示\n" +
                ""));
            Assert.AreEqual("0.14", runner.PrintLog);
            runner.Run (com.WriteIL(
                "A=0.001\n" +
                "Aの小数部分を継続表示\n" +
                ""));
            Assert.AreEqual("0.001", runner.PrintLog);
            runner.Run (com.WriteIL(
                "A=105\n" +
                "Aの小数部分を継続表示\n" +
                ""));
            Assert.AreEqual("0", runner.PrintLog);
        }

        [Test]
        public void TestRound()
        {
            runner.Run (com.WriteIL(
                "-5.14をAに代入\n" +
                "Aを1で四捨五入して継続表示\n" +
                ""));
            Assert.AreEqual("-10", runner.PrintLog);

            runner.Run (com.WriteIL(
                "A=10.0501\n" +
                "Aを「0.1」で四捨五入して継続表示\n" +
                ""));
            Assert.AreEqual("10.1", runner.PrintLog);

//            runner.Run (com.WriteIL(
//                "0.162をAに代入\n" +
//                "Aを-1で四捨五入して継続表示\n" +//TODO:何故か-1が引数として取り出せないので例外が出る。
//                ""));
//            Assert.AreEqual("0.2", runner.PrintLog);
            runner.Run (com.WriteIL(
                "0.162をAに代入\n" +
                "B=-1\n" +
                "AをBで四捨五入して継続表示\n" +//TODO:何故か-1が引数として取り出せないので例外が出る。
                ""));
            Assert.AreEqual("0.2", runner.PrintLog);
        }

        [Test]
        public void TestCeil()
        {
            runner.Run (com.WriteIL(
                "-15.14をAに代入\n" +
                "Aを1で切り下げして継続表示\n" +
                ""));
            Assert.AreEqual("-10", runner.PrintLog);

            runner.Run (com.WriteIL(
                "A=10.0501\n" +
                "Aを「0.1」で切り下げして継続表示\n" +
                ""));
            Assert.AreEqual("10", runner.PrintLog);
            runner.Run (com.WriteIL(
                "A=10.000001\n" +
                "B=(Aを「0.000001」で切り下げ)+0.000001\n" +
                "Bを継続表示\n" +
                ""));
            Assert.AreEqual("10.000002", runner.PrintLog);

//            runner.Run (com.WriteIL(
//                "0.162をAに代入\n" +
//                "Aを-1で切り下げして継続表示\n" +//TODO:何故か-1が引数として取り出せないので例外が出る。
//                ""));
//            Assert.AreEqual("0.1", runner.PrintLog);

            runner.Run (com.WriteIL(
                "0.162をAに代入\n" +
                "B=-1\n" +
                "AをBで切り下げして継続表示\n" +//TODO:何故か-1が引数として取り出せないので例外が出る。
                ""));
            Assert.AreEqual("0.1", runner.PrintLog);
        }

[Test]
public void TestEq()
{
    runner.Run (com.WriteIL (
        "A=10\n" +
        "B=10\n" +
        "もしAとBが等しいならば\n" +
        "  「等しい」と継続表示\n" +
        ""));
    Assert.AreEqual ("等しい", runner.PrintLog);
    runner.Run (com.WriteIL (
        "A=10\n" +
        "B=20\n" +
        "もしAとBが等しいならば\n" +
        "  「等しい」と継続表示\n" +
        ""));
    Assert.AreEqual ("", runner.PrintLog);
    runner.Run (com.WriteIL (
        "A=0.1234\n" +
        "B=0.1234\n" +
        "もしAとBが等しいならば\n" +
        "  「等しい」と継続表示\n" +
        ""));
    Assert.AreEqual ("等しい", runner.PrintLog);
    runner.Run (com.WriteIL (
        "A=0.1234\n" +
        "B=0.4321\n" +
        "もしAとBが等しいならば\n" +
        "  「等しい」と継続表示\n" +
        ""));
    Assert.AreEqual ("", runner.PrintLog);
    runner.Run (com.WriteIL (
        "A=「なでしこ」\n" +
        "B=「なでしこ」\n" +
        "もしAとBが等しいならば\n" +
        "  「等しい」と継続表示\n" +
        ""));
    Assert.AreEqual ("等しい", runner.PrintLog);
    runner.Run (com.WriteIL (
        "A=「なでしこ」\n" +
        "B=「なでしこジャパン」\n" +
        "もしAとBが等しいならば\n" +
        "  「等しい」と継続表示\n" +
        ""));
    Assert.AreEqual ("", runner.PrintLog);
    runner.Run (com.WriteIL (
        "A[0]=1\n" +
        "B=A\n" +
        "もしAとBが等しいならば\n" +
        "  「等しい」と継続表示\n" +
        ""));
    Assert.AreEqual ("等しい", runner.PrintLog);
    runner.Run (com.WriteIL (
        "C[0]=1\n" +
        "D[0]=1\n" +
        "もしCとDが等しいならば\n" +
        "  「等しい」と継続表示\n" +
        "違えばえば\n"+
        "  「違う」と継続表示\n" +
        ""));
    Assert.AreEqual ("違う", runner.PrintLog);
    runner.Run (com.WriteIL (
        "E[0]=1\n" +
        "F[0]=2\n" +
        "もしEとFが等しいならば\n" +
        "  「等しい」と継続表示\n" +
        ""));
    Assert.AreEqual ("", runner.PrintLog);
    runner.Run (com.WriteIL (
        "G[0]=1\n" +
        "H[0]=Gを配列コピー\n" +
        "もしGとHが等しいならば\n" +
        "  「等しい」と継続表示\n" +
        ""));
    Assert.AreEqual ("", runner.PrintLog);
}

[Test]
public void TestOverUnder ()
{
    runner.Run (com.WriteIL (
        "A=20\n" +
        "B=10\n" +
        "もしAがBより大きいならば\n" +
        "  「大きい」と継続表示\n" +
        ""));
    Assert.AreEqual ("大きい", runner.PrintLog);
    runner.Run (com.WriteIL (
        "A=10\n" +
        "B=20\n" +
        "もしAがBより大きいならば\n" +
        "  「大きい」と継続表示\n" +
        ""));
    Assert.AreEqual ("", runner.PrintLog);
    runner.Run (com.WriteIL (
        "A=10\n" +
        "B=20\n" +
        "もしAがBより小さいならば\n" +
        "  「小さい」と継続表示\n" +
        ""));
    Assert.AreEqual ("小さい", runner.PrintLog);
    runner.Run (com.WriteIL (
        "A=20\n" +
        "B=10\n" +
        "もしAがBより小さいならば\n" +
        "  「小さい」と継続表示\n" +
        ""));
    Assert.AreEqual ("", runner.PrintLog);
    runner.Run (com.WriteIL (
        "A=20\n" +
        "B=10\n" +
        "もしAがBより以上ならば\n" +
        "  「以上」と継続表示\n" +
        ""));
    Assert.AreEqual ("以上", runner.PrintLog);
    runner.Run (com.WriteIL (
        "A=10\n" +
        "B=10\n" +
        "もしAがBより以上ならば\n" +
        "  「以上」と継続表示\n" +
        ""));
    Assert.AreEqual ("以上", runner.PrintLog);
    runner.Run (com.WriteIL (
        "A=10\n" +
        "B=20\n" +
        "もしAがBより以上ならば\n" +
        "  「以上」と継続表示\n" +
        ""));
	Assert.AreEqual ("", runner.PrintLog);
}
    }
}
