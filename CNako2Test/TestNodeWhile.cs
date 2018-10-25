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
    public class TestNakoNodeWhile
    {
        [Test]
        public void TestNormal()
        {
            NakoCompiler ns = new NakoCompiler();
            NakoInterpreter runner = new NakoInterpreter();
            NakoILCodeList codes = null;

            // (1) 
            codes = ns.WriteIL(
                "N=3;(N>=0)の間\n"+
                "  PRINT N\n" + 
                "  N=N-1\n;"
                );
            runner.Run(codes);
            Assert.AreEqual(runner.PrintLog, "3210");

            // (2) 
            codes = ns.WriteIL(
                "N=1;(N<5)の間\n" +
                "  PRINT N\n" +
                "  N=N+1\n;"
                );
            runner.Run(codes);
            Assert.AreEqual(runner.PrintLog, "1234");

            // (3) 
            codes = ns.WriteIL(
                "A=1;B=3\n"+
                "(A <= B)の間\n" +
                "  PRINT A; PRINT B;\n" +
                "  A=A+1;B=B-1\n;"
                );
            runner.Run(codes);
            Assert.AreEqual(runner.PrintLog, "1322");

            codes = ns.WriteIL(
                "A=1\n" +
                "(A <= 2)の間\n" +
                "  PRINT `A`&A;\n" +
                "  A=A+1;\n" +
                "  B=1\n" +
                "  (B <= 3)の間\n" +
                "    PRINT `B`&B;\n"+
                "    B=B+1\n" +
                "\n"
                );
            runner.Run(codes);
            Assert.AreEqual(runner.PrintLog, "A1B1B2B3A2B1B2B3");
        }
        [Test]
        public void TestNestl()
        {
            NakoCompiler ns = new NakoCompiler();
            NakoInterpreter runner = new NakoInterpreter();
            NakoILCodeList codes = null;

            codes = ns.WriteIL(
                "A=1\n" +
                "(A <= 2)の間\n" +
                "  PRINT `A`&A;\n" +
                "  A=A+1;\n" +
                "  B=1\n" +
                "  (B <= 3)の間\n" +
                "    PRINT `B`&B;\n"+
                "    B=B+1\n" +
                "\n"
                );
            runner.Run(codes);
            Assert.AreEqual(runner.PrintLog, "A1B1B2B3A2B1B2B3");
        }
    }
}
