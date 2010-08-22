﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;
using Libnako.JCompiler;
using Libnako.Interpreter;
using Libnako.JCompiler.ILWriter;

namespace TestNako
{
    [TestFixture]
    class TestNakoNodeFor
    {
        NakoNamespace ns = new NakoNamespace();
        NakoInterpreter runner = new NakoInterpreter();
        NakoILCodeList codes = null;

        [Test]
        public void TestNormal()
        {
            // (1) 
            codes = ns.Publish(
                "Iを１から３まで繰り返す\n"+
                "  PRINT I\n" + 
                ""
                );
            runner.Run(codes);
            Assert.AreEqual(runner.PrintLog, "123");

            // (2) 
            codes = ns.Publish(
                "FROM=2;TO=5"+
                "IをFROMからTOまで繰り返す\n" +
                "  PRINT I\n" +
                ""
                );
            runner.Run(codes);
            Assert.AreEqual(runner.PrintLog, "2345");

        }
        [Test]
        public void TestNest()
        {
            codes = ns.Publish(
                "Ｉを１から２まで繰り返す\n" +
                "　　PRINT `[`&I&`:`\n" +
                "　　Ｊを１から3まで繰り返す\n" +
                "　　　　PRINT J\n" +
                "　　PRINT `]`&I\n" +
                "\n"
                );
            runner.Run(codes);
            Assert.AreEqual(runner.PrintLog, "[1:123][2:123]");
        }
    }
}
