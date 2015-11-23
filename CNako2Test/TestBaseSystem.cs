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
    }
}
