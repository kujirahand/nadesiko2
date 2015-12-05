using System;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;
using Libnako.JPNCompiler;
using Libnako.Interpreter;

namespace NakoPluginTest
{
    [TestFixture]
    public class TestInstance
    {
        NakoCompiler com;
        NakoInterpreter runner;

		public TestInstance()
        {
            com = new NakoCompiler();
            runner = new NakoInterpreter();
        }


        [Test]
        public void Test_instance_property()
        {
            NakoCompiler nc = new NakoCompiler();
            NakoInterpreter ni = new NakoInterpreter();
            nc.WriteIL(
				"A=カウントダウンタイマー\n" +
				"B=AのNO\n" +
                "Bを継続表示\n" +
                "");
            ni.Run(nc.Codes);
            Assert.AreEqual("10", ni.PrintLog);
        }

        [Test]
        public void Test_instance_method()
        {
            NakoCompiler nc = new NakoCompiler();
            NakoInterpreter ni = new NakoInterpreter();
            nc.WriteIL(
				"A=カウントダウンタイマー\n" +
				"A=Aを1でカウントダウン\n" +
				"B=AのNO\n" +
				"Bを継続表示\n" +
                "");
            ni.Run(nc.Codes);
            Assert.AreEqual("9", ni.PrintLog);
        }

        [Test]
        public void Test_instance_method_without_return()
        {
            NakoCompiler nc = new NakoCompiler();
            NakoInterpreter ni = new NakoInterpreter();
            nc.WriteIL(
                "A=カウントダウンタイマー\n" +
                "Aを1でカウントダウン\n" +
                "B=AのNO\n" +
                "Bを継続表示\n" +
                "");
            ni.Run(nc.Codes);
            Assert.AreEqual("9", ni.PrintLog);
        }

        [Test]
        public void Test_multi_instance()
        {
            NakoCompiler nc = new NakoCompiler();
            NakoInterpreter ni = new NakoInterpreter();
            nc.WriteIL(
				"A=カウントダウンタイマー\n" +
				"B=カウントダウンタイマー\n" +
				"A=Aを1でカウントダウン\n" +
				"B=Bを3でカウントダウン\n" +
				"C=AのNO\n" +
				"D=BのNO\n" +
				"Cを継続表示\n" +
				"Dを継続表示\n" +
                "");
            ni.Run(nc.Codes);
            Assert.AreEqual("97", ni.PrintLog);
        }

        [Test]
        public void Test_instance_argument()
        {
            NakoCompiler nc = new NakoCompiler();
            NakoInterpreter ni = new NakoInterpreter();
            nc.WriteIL(
                "●test({カウントダウンタイマー}Oを)\n" +
                "  O = Oを1でカウントダウン\n" +
                "  Oで戻る\n" +
                "A=カウントダウンタイマー\n" +
                "A=Aをtest\n" +
                "C=AのNO\n" +
                "Cを継続表示\n" +
                "");
            ni.Run(nc.Codes);
            Assert.AreEqual("9", ni.PrintLog);
        }

    }
}
