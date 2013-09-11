using System;
using System.Collections.Generic;
using System.Text;
using Libnako.Interpreter;
using Libnako.JPNCompiler;
using Libnako.JPNCompiler.Node;
using NakoPlugin;
using NUnit.Framework;

namespace NakoPluginTest
{
    [TestFixture]    
    public class TestNakoParser
    {
        NakoCompiler compiler = new NakoCompiler();
        NakoInterpreter interpreter = new NakoInterpreter();
        [Test]
        public void TestCalc()
        {
            NakoNode topNode;
            // 1
            topNode = compiler.ParseOnlyValue("1+2*3");
            Assert.IsTrue(topNode.hasChildren());
            Assert.IsTrue(topNode.Children.checkNodeType(
                new NakoNodeType[] {
                    NakoNodeType.CALC
                }));
            // 2
            topNode = compiler.ParseOnlyValue("(1+2)*3");
            Assert.IsTrue(topNode.hasChildren());
            Assert.IsTrue(topNode.Children.checkNodeType(
                new NakoNodeType[] {
                    NakoNodeType.CALC
                }));
            // 3
            topNode = compiler.Parse("A=5");
            Assert.IsTrue(topNode.hasChildren());
            Assert.IsTrue(topNode.Children.checkNodeType(new NakoNodeType[] {
                NakoNodeType.LET
            }));
        }
        [Test]
        public void TestDef()
        {
            interpreter.Run(compiler.WriteIL(
                "Aとは変数=30" +
                "PRINT A"));
            Assert.AreEqual("30", interpreter.PrintLog);
        }
        [Test]
        public void TestCallFuncInLetSentense()
        {
            interpreter.Run(compiler.WriteIL("A=10に2を掛けて4を足す。PRINT A"));
        	Assert.AreEqual("24", interpreter.PrintLog);
        }
        [Test]
        public void TestCallFuncInLetSentense2()
        {
        	// TODO: 関数直後の演算子がパースエラーになる
            // compiler.WriteIL("A=2に3を掛けて4を足す＋8。PRINT A"); 
            interpreter.Run(compiler.WriteIL("A=(2に3を掛けて4を足す)＋8。PRINT A"));
        	Assert.AreEqual("18", interpreter.PrintLog);
        }
    }
}
