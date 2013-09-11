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
    public class TestNakoNodeFor
    {
        NakoCompiler ns = new NakoCompiler();
        NakoInterpreter runner = new NakoInterpreter();
        NakoILCodeList codes = null;

        [Test]
        public void Test1_Normal()
        {
            // (1) 
            codes = ns.WriteIL(
                "Iを１から３まで繰り返す\n"+
                "  PRINT I\n" + 
                ""
                );
            runner.Run(codes);
            Assert.AreEqual(runner.PrintLog, "123");

            // (2) 
            codes = ns.WriteIL(
                "FROM=2;TO=5"+
                "IをFROMからTOまで繰り返す\n" +
                "  PRINT I\n" +
                ""
                );
            runner.Run(codes);
            Assert.AreEqual(runner.PrintLog, "2345");

        }
        [Test]
        public void Test2_Nest()
        {
            codes = ns.WriteIL(
                "Ｉを１から２まで繰り返す\n" +
                "　　PRINT `[`&I&`:`\n" +
                "　　Ｊを１から3まで繰り返す\n" +
                "　　　　PRINT J\n" +
                "　　PRINT `]`\n" +
                "\n"
                );
            runner.Run(codes);
            Assert.AreEqual(runner.PrintLog, "[1:123][2:123]");
        }
        [Test]
        public void Test3_RepeatTimes()
        {
            codes = ns.WriteIL(
                "3回\n" +
                "　　PRINT `a`\n" +
                "\n"
                );
            runner.Run(codes);
            Assert.AreEqual(runner.PrintLog, "aaa");
        }
        [Test]
        public void TestFOR1()
        {
            codes = ns.WriteIL(
                "Iを1から5まで繰り返す\n" +
                "　　PRINT I\n" +
                "\n"
                );
            runner.Run(codes);
            Assert.AreEqual(runner.PrintLog, "12345");
        }
        [Test]
        public void TestFOR2_BREAK()
        {
            codes = ns.WriteIL(
                "Iを1から5まで繰り返す\n" +
                "　　PRINT I\n" +
                "    もし、I=3ならば、抜ける。\n" +
                "\n"
                );
            runner.Run(codes);
            Assert.AreEqual(runner.PrintLog, "123");
        }
    }
}
