using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;
using Libnako.Parser;
using Libnako.Interpreter;

namespace TestNako
{
    [TestFixture]
    class TestNakoNodeIf
    {
        [Test]
        public void TestOneLine()
        {
            NakoNamespace ns = new NakoNamespace();
            NakoInterpreter runner = new NakoInterpreter();
            NakoILCodeList codes = null;

            // (1) 
            codes = ns.Publish("もし、1=1ならば、PRINT 3");
            runner.Run(codes);
            Assert.AreEqual(runner.PrintLog, "3");

            // (2)
            runner.Reset();
            codes = ns.Publish("もし、1=2ならば、PRINT 3、違えば、PRINT 5");
            runner.Run(codes);
            Assert.AreEqual(runner.PrintLog, "5");

            // (3)
            runner.Reset();
            codes = ns.Publish("A=1,B=2。もし,A=Bならば、PRINT`NG`。違えば、PRINT`OK`");
            runner.Run(codes);
            Assert.AreEqual(runner.PrintLog, "OK");
        }
        [Test]
        public void TestNormal()
        {
            NakoNamespace ns = new NakoNamespace();
            NakoInterpreter runner = new NakoInterpreter();
            NakoILCodeList codes = null;

            // (1) 
            codes = ns.Publish(
                "A=1\n" +
                "B=2\n" +
                "もし、A=Bならば\n" +
                "  PRINT`真`\n" +
                "違えば\n" +
                "  PRINT`偽`\n");
            runner.Run(codes);
            Assert.AreEqual(runner.PrintLog, "偽");

        }
    }
}
