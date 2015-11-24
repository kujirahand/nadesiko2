using System;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;
using Libnako.JPNCompiler;
using Libnako.Interpreter;
using Libnako.JPNCompiler.ILWriter;
using Libnako.Interpreter.ILCode;

namespace NakoPluginTest
{
    [TestFixture]
    public class TestNakoNodeIf
    {
        [Test]
        public void TestOneLine()
        {
            NakoCompiler ns = new NakoCompiler();
            NakoInterpreter runner = new NakoInterpreter();
            NakoILCodeList codes = null;

            // (1) 
            codes = ns.WriteIL("もし、1=1ならば、PRINT 3");
            runner.Run(codes);
            Assert.AreEqual(runner.PrintLog, "3");

            // (2)
            runner.Reset();
            codes = ns.WriteIL("もし、1=2ならば、PRINT 3、違えば、PRINT 5");
            runner.Run(codes);
            Assert.AreEqual(runner.PrintLog, "5");

            // (3)
            runner.Reset();
            codes = ns.WriteIL("A=1,B=2。もし,A=Bならば、PRINT`NG`。違えば、PRINT`OK`");
            runner.Run(codes);
            Assert.AreEqual(runner.PrintLog, "OK");
        }
        [Test]
        public void TestNormal()
        {
            NakoCompiler ns = new NakoCompiler();
            NakoInterpreter runner = new NakoInterpreter();
            NakoILCodeList codes = null;

            // (1) 
            codes = ns.WriteIL(
                "A=1\n" +
                "B=2\n" +
                "もし、A=Bならば\n" +
                "  PRINT`真`\n" +
                "違えば\n" +
                "  PRINT`偽`\n");
            runner.Run(codes);
            Assert.AreEqual(runner.PrintLog, "偽");


            // (2)
            codes = ns.WriteIL(
                "A=2\n" +
                "もし、A=1ならば\n" +
                "  PRINT 1\n" +
                "違えば、もし、A=2ならば\n" +
                "  PRINT 2\n" +
                "違えば\n" +
                "  PRINT `*`\n"
                );
            runner.Run(codes);
            Assert.AreEqual(runner.PrintLog, "2");

            // (3)
            codes = ns.WriteIL(
                "A=1\n" +
                "もし、A=1ならば\n" +
                "  PRINT 1\n" +
                "違えば、もし、A=2ならば\n" +
                "  PRINT 2\n" +
                "違えば\n" +
                "  PRINT `*`\n"
                );
            runner.Run(codes);
            Assert.AreEqual(runner.PrintLog, "1");

            // (4)
            codes = ns.WriteIL(
                "A=3\n" +
                "もし、A=1ならば\n" +
                "  PRINT 1\n" +
                "違えば、もし、A=2ならば\n" +
                "  PRINT 2\n" +
                "違えば、もし、A=3ならば\n" +
                "  PRINT 3\n" +
                "違えば、もし、A=4ならば\n" +
                "  PRINT 4\n" +
                "違えば\n" +
                "  PRINT `*`\n"
                );
            runner.Run(codes);
            Assert.AreEqual(runner.PrintLog, "3");

            // (5)
            codes = ns.WriteIL(
                "A=8\n" +
                "もし、A=1ならば\n" +
                "  PRINT 1\n" +
                "違えば、もし、A=2ならば\n" +
                "  PRINT 2\n" +
                "違えば、もし、A=3ならば\n" +
                "  PRINT 3\n" +
                "違えば、もし、A=4ならば\n" +
                "  PRINT 4\n" +
                "違えば\n" +
                "  PRINT `*`\n"
                );
            runner.Run(codes);
            Assert.AreEqual(runner.PrintLog, "*");
        }

        [Test]
        public void TestNest()
        {
            NakoCompiler ns = new NakoCompiler();
            NakoInterpreter runner = new NakoInterpreter();
            NakoILCodeList codes = null;

            // (1) 
            codes = ns.WriteIL(
                "A=1\n" +
                "B=2\n" +
                "もし、A=1ならば\n" +
                "  もし、B=1ならば\n" +
                "    PRINT `11`\n" +
                "  違えば\n" +
                "    PRINT `12`\n" +
                "違えば\n" +
                "  もし、B=1ならば\n" +
                "    PRINT `21`\n" +
                "  違えば\n" +
                "    PRINT`22`\n"
            );
            runner.Run(codes);
            Assert.AreEqual(runner.PrintLog, "12");


            // (2) 
            codes = ns.WriteIL(
                "A=1\n" +
                "B=1\n" +
                "もし、A=1ならば\n" +
                "  もし、B=1ならば\n" +
                "    PRINT `11`\n" +
                "  違えば\n" +
                "    PRINT `12`\n" +
                "違えば\n" +
                "  もし、B=1ならば\n" +
                "    PRINT `21`\n" +
                "  違えば\n" +
                "    PRINT`22`\n"
            );
            runner.Run(codes);
            Assert.AreEqual(runner.PrintLog, "11");

            // (3) 
            codes = ns.WriteIL(
                "A=2\n" +
                "B=2\n" +
                "もし、A=1ならば\n" +
                "  もし、B=1ならば\n" +
                "    PRINT `11`\n" +
                "  違えば\n" +
                "    PRINT `12`\n" +
                "違えば\n" +
                "  もし、B=1ならば\n" +
                "    PRINT `21`\n" +
                "  違えば\n" +
                "    PRINT`22`\n"
            );
            runner.Run(codes);
            Assert.AreEqual(runner.PrintLog, "22");

        }

		[Test]
		public void TestAndOr()
		{
			NakoCompiler ns = new NakoCompiler();
			NakoInterpreter runner = new NakoInterpreter();
			NakoILCodeList codes = null;

			// AND 
			codes = ns.WriteIL(
				"A=1\n" +
				"B=2\n" +
				"C=3\n" +
				"もし、A==1 && B==1ならば\n" +
				"  PRINT `11`\n" +
				"もし、A==1 && C==3ならば\n" +
				"  PRINT `13`\n"
			);
			runner.Run(codes);
			Assert.AreEqual(runner.PrintLog, "13");

			// or
			codes = ns.WriteIL(
				"A=1\n" +
				"B=2\n" +
				"C=3\n" +
				"もし、A==1 || B==1ならば\n" +
				"  PRINT `11`\n" +
				"もし、A==2 || C==1ならば\n" +
				"  PRINT `13`\n"
			);
			runner.Run(codes);
			Assert.AreEqual(runner.PrintLog, "11");

		}
    }
}
