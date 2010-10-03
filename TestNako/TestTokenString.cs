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
    class TestTokenString
    {
        NakoCompiler ns = new NakoCompiler();
        NakoInterpreter runner = new NakoInterpreter();

        [Test]
        public void Test_StringSimple()
        {
            ns.Publish(
                "PRINT 「123」&『456』\n"+
                ""
                );
            runner.Run(ns.Codes);
            Assert.AreEqual(runner.PrintLog, "123456");
        }
        
        [Test]
        public void Test_StrExtract1()
        {
            ns.Publish(
        		"a=30"+
                "PRINT 「**{a}**」\n"+
                ""
                );
            runner.Run(ns.Codes);
            Assert.AreEqual(runner.PrintLog, "**30**");
        }
        
        [Test]
        public void Test_StrExtract2()
        {
            ns.Publish(
        		"a=30"+
                "PRINT 「**{a*3}**」\n"+
                ""
                );
            runner.Run(ns.Codes);
            Assert.AreEqual(runner.PrintLog, "**90**");
        }
        
        [Test]
        public void Test_StrExtract3()
        {
            ns.Publish(
        		"a=`abc`;b=`def`;"+
                "PRINT 「**{a}**{b}**」\n"+
                ""
                );
            runner.Run(ns.Codes);
            Assert.AreEqual(runner.PrintLog, "**abc**def**");
        }
        
        [Test]
        public void Test_StrExtract4()
        {
        	/* todo
            ns.Publish(
                "PRINT 「ab{\\n}cd」\n"+
                ""
                );
            runner.Run(ns.Codes);
            Assert.AreEqual(runner.PrintLog, "ab\ncd");
            */
        }
    }
}
