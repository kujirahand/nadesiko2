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
    public class TestNakoInterpreter
    {
        [Test]
        public void TestInterpreter1()
        {
            NakoCompiler ns = new NakoCompiler();
            NakoILWriter w = new NakoILWriter();
            NakoInterpreter runner = new NakoInterpreter();
            Object o;

            // 1
            ns.source = "1+2*3";
            ns.Tokenize();
            ns.ParseOnlyValue();
            w.Write(ns.TopNode);
            runner.Run(w.Result);
            o = runner.StackTop;
            Assert.IsNotNull(o);
            Assert.IsTrue(7 == NakoValueConveter.ToLong(o));

            // 2
            ns.source = "(1+2)*3";
            ns.Tokenize();
            ns.ParseOnlyValue();
            w.Write(ns.TopNode);
            runner.Run(w.Result);
            o = runner.StackTop;
            Assert.IsNotNull(o);
            Assert.IsTrue(9 == NakoValueConveter.ToLong(o));

            // 3
            ns.source = "1+4/2";
            ns.Tokenize();
            ns.ParseOnlyValue();
            w.Write(ns.TopNode);
            runner.Run(w.Result);
            o = runner.StackTop;
            Assert.IsNotNull(o);
            Assert.IsTrue(3 == NakoValueConveter.ToLong(o));

        }
        [Test]
        public void TestInterpreter2()
        {
            NakoCompiler ns = new NakoCompiler();
            NakoILWriter w = new NakoILWriter();
            NakoInterpreter runner = new NakoInterpreter();

            // 1
            ns.source = "A=5; PRINT A";
            ns.Tokenize();
            ns.Parse();
            w.Write(ns.TopNode);
            runner.Run(w.Result);
            Assert.AreEqual("5", runner.PrintLog);

            // 2
            w.Init();
            runner.Reset();
            ns.source = "A=5; B=8; C=A+B; PRINT C";
            ns.Tokenize();
            ns.Parse();
            w.Write(ns.TopNode);
            runner.Run(w.Result);
            Assert.AreEqual("13", runner.PrintLog);
        }
        [Test]
        public void TestInterpreterCalcReal()
        {
            NakoCompiler ns = new NakoCompiler();
            NakoILWriter w = new NakoILWriter();
            NakoInterpreter runner = new NakoInterpreter();

            // 1
            ns.source = "PRINT 2 * 1.5";
            ns.Tokenize();
            ns.Parse();
            w.Write(ns.TopNode);
            runner.Run(w.Result);
            Assert.AreEqual("3", runner.PrintLog);

            // 2
            w.Init();
            runner.Reset();
            ns.source = "PRINT (1 / 2) * 4";
            ns.Tokenize();
            ns.Parse();
            w.Write(ns.TopNode);
            runner.Run(w.Result);
            Assert.AreEqual("2", runner.PrintLog);

            // 3
            w.Init();
            runner.Reset();
            ns.source = "PRINT 4 % 3";
            ns.Tokenize();
            ns.Parse();
            w.Write(ns.TopNode);
            runner.Run(w.Result);
            Assert.AreEqual("1", runner.PrintLog);

            // 4
            w.Init();
            runner.Reset();
            ns.source = "PRINT 2 ^ 3";
            ns.Tokenize();
            ns.Parse();
            w.Write(ns.TopNode);
            runner.Run(w.Result);
            Assert.AreEqual("8", runner.PrintLog);

            // 5 : べき乗の優先順位
            w.Init();
            runner.Reset();
            ns.source = "PRINT 2 * 3 ^ 3";
            ns.Tokenize();
            ns.Parse();
            w.Write(ns.TopNode);
            runner.Run(w.Result);
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
