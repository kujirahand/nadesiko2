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
    class TestArray
    {
        NakoCompiler ns = new NakoCompiler();
        NakoInterpreter runner = new NakoInterpreter();
        NakoILCodeList codes = null;

        [Test]
        public void Test_array1()
        {
            codes = ns.Publish(
                "A[3]=566\n"+
                "A[3]を表示\n" + 
                ""
                );
            runner.Run(codes);
            Assert.AreEqual(runner.PrintLog, "566");
        }

        [Test]
        public void Test_array2()
        {
            codes = ns.Publish(
                "A[`a`]=566\n" +
                "A[`a`]を表示\n" +
                ""
                );
            runner.Run(codes);
            Assert.AreEqual(runner.PrintLog, "566");
        }

        [Test]
        public void Test_array3()
        {
            codes = ns.Publish(
                "A[3]=566\n" +
                "PRINT A[3]\n" +
                ""
                );
            runner.Run(codes);
            Assert.AreEqual(runner.PrintLog, "566");
        }
    }
}
