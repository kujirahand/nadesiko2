using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

using Libnako.JCompiler.Node;
using Libnako.JCompiler;
using Libnako.Interpreter;

namespace TestNako
{
    [TestFixture]    
    public class TestNakoParser
    {
    	NakoCompiler com = new NakoCompiler();
        NakoInterpreter runner = new NakoInterpreter();
        
        [Test]
        public void TestCalc()
        {
            // 1
            NakoCompiler ns = new NakoCompiler();
            ns.source = "1+2*3";
            ns.Tokenize();
            ns.ParseOnlyValue();
            Assert.IsTrue(ns.TopNode.hasChildren());
            Boolean r = ns.TopNode.Children.checkNodeType(new NakoNodeType[] {
                NakoNodeType.CALC
            });
            Assert.IsTrue(r);
            // 2
            ns.source = "(1+2)*3";
            ns.Tokenize();
            ns.ParseOnlyValue();
            Assert.IsTrue(ns.TopNode.hasChildren());
            r = ns.TopNode.Children.checkNodeType(new NakoNodeType[] {
                NakoNodeType.CALC
            });
            Assert.IsTrue(r);
            // 3
            ns.source = "A=5";
            ns.Tokenize();
            ns.Parse();
            Assert.IsTrue(ns.TopNode.hasChildren());
            r = ns.TopNode.Children.checkNodeType(new NakoNodeType[] {
                NakoNodeType.LET
            });
            Assert.IsTrue(r);
        }

        [Test]
        public void TestDef()
        {
            NakoCompiler c = new NakoCompiler();
            NakoInterpreter i = new NakoInterpreter();
            c.DirectSource =
                "Aとは変数=30" +
                "PRINT A";
            i.Run(c.Codes);
            Assert.AreEqual("30", i.PrintLog);
        }
        
        [Test]
        public void TestCallFuncInLetSentense()
        {
        	com.DirectSource = "A=10に2を掛けて4を足す。PRINT A";
        	runner.Run(com.Codes);
        	Assert.AreEqual("24", runner.PrintLog);
        }
        
        [Test]
        public void TestCallFuncInLetSentense2()
        {
        	com.DirectSource = "A=(2に3を掛けて4を足す)＋8。PRINT A";
        	// TODO: 関数直後の演算子がパースエラーになる
        	// com.DirectSource = "A=2に3を掛けて4を足す＋8。PRINT A"; 
        	runner.Run(com.Codes);
        	Assert.AreEqual("18", runner.PrintLog);
        }
    }
}
