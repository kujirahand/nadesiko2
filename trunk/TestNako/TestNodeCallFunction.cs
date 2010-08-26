using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;
using Libnako.JCompiler;
using Libnako.Interpreter;
using Libnako.JCompiler.ILWriter;

namespace TestNako
{
    [TestFixture]
    class TestNodeCallFunction
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
    }
}
