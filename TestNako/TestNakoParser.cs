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
            ns.source = "A=5";
            ns.TokenizeAndParse();
            Assert.IsTrue(ns.TopNode.hasChildren());
            Boolean r = ns.TopNode.Children.checkNodeType(new int[] {
                NodeType.N_LET
            });
        }
    }
}
