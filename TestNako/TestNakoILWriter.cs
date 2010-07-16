using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Libnako.Parser;

namespace TestNako
{
    [TestFixture]
    public class TestNakoILWriter
    {
        [Test]
        public void TestNakoILWriter1()
        {
            NakoNamespace ns = new NakoNamespace();
            NakoILWriter writer = new NakoILWriter(null);
            Boolean r;

            // (1)
            ns.source = "1+2*3";
            ns.Tokenize();
            ns.ParseOnlyValue();
            writer.Write(ns.TopNode);
            r = writer.Result.CheckTypes(new int[] {
                NakoILType.NOP,
                NakoILType.LD_CONST_INT,
                NakoILType.LD_CONST_INT,
                NakoILType.LD_CONST_INT,
                NakoILType.MUL,
                NakoILType.ADD
            });
            Assert.IsTrue(r);
        }
    }
}
