using System;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;
using Libnako.JCompiler;
using Libnako.Interpreter;
using Libnako.JCompiler.ILWriter;

namespace TestNako
{
    [TestFixture]
    public class TestNakoCalc
    {
        NakoCompiler ns = new NakoCompiler();
        NakoInterpreter runner = new NakoInterpreter();
        NakoILCodeList codes = null;

        [Test]
        public void TestNormal()
        {
            // (1) 
            codes = ns.Publish("PRINT 1+2*3");
            runner.Run(codes);
            Assert.AreEqual(runner.PrintLog, "7");
        }
        [Test]
        public void TestCalcStr()
        {
            // (2) 
            codes = ns.Publish("PRINT `abc`&`def`");
            runner.Run(codes);
            Assert.AreEqual(runner.PrintLog, "abcdef");

            // (3) 
            codes = ns.Publish("PRINT 10 + 20 & `a`");
            runner.Run(codes);
            Assert.AreEqual(runner.PrintLog, "30a");

            // (4) 
            codes = ns.Publish("PRINT 10 * 20 & `a`");
            runner.Run(codes);
            Assert.AreEqual(runner.PrintLog, "200a");
        }
        [Test]
        public void TestCalcStr2()
        {
            // (7) 「+」は数値同士の加算
            codes = ns.Publish("PRINT `30`+30");
            runner.Run(codes);
            Assert.AreEqual(runner.PrintLog, "60");

            // (8) 「&」は文字列同士の加算
            codes = ns.Publish("PRINT `30`&30");
            runner.Run(codes);
            Assert.AreEqual(runner.PrintLog, "3030");

        }

        [Test]
        public void TestCalc2()
        {
            // (5) 
            codes = ns.Publish("PRINT (1+2)*3");
            runner.Run(codes);
            Assert.AreEqual(runner.PrintLog, "9");

            // (6) 
            codes = ns.Publish("PRINT 2^3");
            runner.Run(codes);
            Assert.AreEqual(runner.PrintLog, "8");

        }
        [Test]
        public void TestRenzokuCalc()
        {
            // (1) 
            codes = ns.Publish("PRINT 1+2+3+4");
            runner.Run(codes);
            Assert.AreEqual(runner.PrintLog, "10");

            // (2) 
            codes = ns.Publish("PRINT 1*2*3*4");
            runner.Run(codes);
            Assert.AreEqual(runner.PrintLog, "24");

        }
    }
}
