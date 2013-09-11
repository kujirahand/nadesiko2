using System;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;
using Libnako.JPNCompiler;
using Libnako.Interpreter;
using Libnako.JPNCompiler.ILWriter;
using NakoPlugin;


namespace NakoPluginTest
{
    [TestFixture]
    public class TestNakoInterpreter
    {
        [Test]
        public void TestInterpreter1()
        {
            NakoCompiler ns = new NakoCompiler();
            NakoInterpreter runner = new NakoInterpreter();
            object o;

            // 1
            ns.source = "1+2*3";
            ns.Tokenize();
            ns.ParseOnlyValue();
            runner.Run(ns.WriteIL());
            o = runner.StackTop;
            Assert.IsNotNull(o);
            Assert.IsTrue(7 == NakoValueConveter.ToLong(o));

            // 2
            ns.source = "(1+2)*3";
            ns.Tokenize();
            ns.ParseOnlyValue();
            runner.Run(ns.WriteIL());
            o = runner.StackTop;
            Assert.IsNotNull(o);
            Assert.IsTrue(9 == NakoValueConveter.ToLong(o));

            // 3
            ns.source = "1+4/2";
            ns.Tokenize();
            ns.ParseOnlyValue();
            runner.Run(ns.WriteIL());
            o = runner.StackTop;
            Assert.IsNotNull(o);
            Assert.IsTrue(3 == NakoValueConveter.ToLong(o));

        }
        [Test]
        public void TestInterpreter2()
        {
            NakoCompiler ns = new NakoCompiler();
            NakoInterpreter runner = new NakoInterpreter();

            // 1
            ns.source = "A=5; PRINT A";
            ns.Tokenize();
            ns.Parse();
            runner.Run(ns.WriteIL());
            Assert.AreEqual("5", runner.PrintLog);

            // 2
            runner.Reset();
            ns.source = "A=5; B=8; C=A+B; PRINT C";
            ns.Tokenize();
            ns.Parse();
            runner.Run(ns.WriteIL());
            Assert.AreEqual("13", runner.PrintLog);
        }
        [Test]
        public void TestInterpreterCalcReal()
        {
            NakoCompiler ns = new NakoCompiler();
            NakoInterpreter runner = new NakoInterpreter();

            // 1
            ns.source = "PRINT 2 * 1.5";
            ns.Tokenize();
            ns.Parse();
            runner.Run(ns.WriteIL());
            Assert.AreEqual("3", runner.PrintLog);

            // 2
            runner.Reset();
            ns.source = "PRINT (1 / 2) * 4";
            ns.Tokenize();
            ns.Parse();
            runner.Run(ns.WriteIL());
            Assert.AreEqual("2", runner.PrintLog);

            // 3
            runner.Reset();
            ns.source = "PRINT 4 % 3";
            ns.Tokenize();
            ns.Parse();
            runner.Run(ns.WriteIL());
            Assert.AreEqual("1", runner.PrintLog);

            // 4
            runner.Reset();
            ns.source = "PRINT 2 ^ 3";
            ns.Tokenize();
            ns.Parse();
            runner.Run(ns.WriteIL());
            Assert.AreEqual("8", runner.PrintLog);

            // 5 : べき乗の優先順位
            runner.Reset();
            ns.source = "PRINT 2 * 3 ^ 3";
            ns.Tokenize();
            ns.Parse();
            runner.Run(ns.WriteIL());
            Assert.AreEqual("54", runner.PrintLog);
        }
        [Test]
        public void TestInterpreterId()
        {
            NakoInterpreter runner1 = new NakoInterpreter();
            NakoInterpreter runner2 = new NakoInterpreter();
            Assert.AreNotEqual(runner1.InterpreterId, runner2.InterpreterId);
        }
    }
}
