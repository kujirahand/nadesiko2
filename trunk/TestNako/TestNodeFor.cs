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
    public class TestNakoNodeFor
    {
        NakoCompiler ns = new NakoCompiler();
        NakoInterpreter runner = new NakoInterpreter();
        NakoILCodeList codes = null;

        [Test]
        public void Test1_Normal()
        {
            // (1) 
            codes = ns.Publish(
                "Iを１から３まで繰り返す\n"+
                "  PRINT I\n" + 
                ""
                );
            runner.Run(codes);
            Assert.AreEqual(runner.PrintLog, "123");

            // (2) 
            codes = ns.Publish(
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
            codes = ns.Publish(
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
            codes = ns.Publish(
                "3回\n" +
                "　　PRINT `a`\n" +
                "\n"
                );
            runner.Run(codes);
            Assert.AreEqual(runner.PrintLog, "aaa");
        }
    }
}
