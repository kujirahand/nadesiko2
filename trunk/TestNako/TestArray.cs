using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;
using Libnako.JCompiler;
using Libnako.Interpreter;

namespace TestNako
{
    [TestFixture]
    class TestArray
    {
        NakoCompiler ns = new NakoCompiler();
        NakoInterpreter runner = new NakoInterpreter();

        [Test]
        public void Test_array1()
        {
            ns.DirectSource =
                "A[3]=566\n" +
                "A[3]を表示\n" +
                "";
            runner.Run(ns.Codes);
            Assert.AreEqual(runner.PrintLog, "566");
        }

        [Test]
        public void Test_array2()
        {
            ns.DirectSource =
                "A[`a`]=566\n" +
                "A[`a`]を表示\n" +
                ""
                ;
            runner.Run(ns.Codes);
            Assert.AreEqual(runner.PrintLog, "566");
        }

        [Test]
        public void Test_array3()
        {
            ns.DirectSource =
                "A[3]=566\n" +
                "PRINT A[3]\n" +
                "";
            runner.Run(ns.Codes);
            Assert.AreEqual(runner.PrintLog, "566");
        }

        [Test]
        public void Test_array_yen_access()
        {
            ns.DirectSource =
                "A￥3=999\n" +
                "A￥3を表示\n" +
                "";
            runner.Run(ns.Codes);
            Assert.AreEqual(runner.PrintLog, "999");
        }

        [Test]
        public void Test_array_array1()
        {
            ns.DirectSource =
                "B[3][45]=222\n" +
                "PRINT B[3][45]\n" +
                "";
            runner.Run(ns.Codes);
            Assert.AreEqual(runner.PrintLog, "222");
        }
        [Test]
        public void Test_array_array2()
        {
            ns.DirectSource =
                "C￥１￥２￥３=2222\n" +
                "C￥１￥２￥３を表示。\n" +
                "";
            runner.Run(ns.Codes);
            Assert.AreEqual(runner.PrintLog, "2222");
        }
    }
}
