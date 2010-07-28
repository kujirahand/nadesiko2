using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Libnako.Parser;
using NUnit.Framework;

namespace TestNako
{
    [TestFixture]    
    public class TestNakoParser
    {
        [Test]
        public void TestLet()
        {
            // 1
            NakoNamespace ns = new NakoNamespace(null);
            ns.source = "1+2*3";
            ns.Tokenize();
            ns.ParseOnlyValue();
            Assert.IsTrue(ns.TopNode.hasChildren());
            Boolean r = ns.TopNode.Children.checkNodeType(new NodeType[] {
                NodeType.CALC
            });
            // 2
            ns.source = "(1+2)*3";
            ns.Tokenize();
            ns.ParseOnlyValue();
            Assert.IsTrue(ns.TopNode.hasChildren());
            r = ns.TopNode.Children.checkNodeType(new NodeType[] {
                NodeType.CALC
            });
            // 3
            ns.source = "A=5";
            ns.Tokenize();
            ns.Parse();
            Assert.IsTrue(ns.TopNode.hasChildren());
            r = ns.TopNode.Children.checkNodeType(new NodeType[] {
                NodeType.LET
            });
        }
    }
}
