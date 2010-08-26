using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Libnako.JCompiler;
using NUnit.Framework;
using Libnako.JCompiler.Node;

namespace TestNako
{
    [TestFixture]    
    public class TestNakoParser
    {
        [Test]
        public void TestLet()
        {
            // 1
            NakoCompiler ns = new NakoCompiler(null);
            ns.source = "1+2*3";
            ns.Tokenize();
            ns.ParseOnlyValue();
            Assert.IsTrue(ns.TopNode.hasChildren());
            Boolean r = ns.TopNode.Children.checkNodeType(new NakoNodeType[] {
                NakoNodeType.CALC
            });
            // 2
            ns.source = "(1+2)*3";
            ns.Tokenize();
            ns.ParseOnlyValue();
            Assert.IsTrue(ns.TopNode.hasChildren());
            r = ns.TopNode.Children.checkNodeType(new NakoNodeType[] {
                NakoNodeType.CALC
            });
            // 3
            ns.source = "A=5";
            ns.Tokenize();
            ns.Parse();
            Assert.IsTrue(ns.TopNode.hasChildren());
            r = ns.TopNode.Children.checkNodeType(new NakoNodeType[] {
                NakoNodeType.LET
            });
        }
    }
}
