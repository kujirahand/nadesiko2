using System;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;
using Libnako.JCompiler;
using Libnako.Interpreter;

namespace TestNako
{
    [TestFixture]
    class TestArray
    {
        [Test]
        public void Test_array1()
        {
            NakoCompiler nc = new NakoCompiler();
            NakoInterpreter ni = new NakoInterpreter();
            nc.DirectSource =
                "A[3]=566\n" +
                "A[3]を表示\n" +
                "";
            ni.Run(nc.Codes);
            Assert.AreEqual("566", ni.PrintLog);
        }

        [Test]
        public void Test_array2()
        {
            NakoCompiler nc = new NakoCompiler();
            NakoInterpreter ni = new NakoInterpreter();
            ni.debugMode = true;
            ni.UseConsoleOut = true;
            nc.DirectSource =
                "A[`a`]=566\n" +
                "A[`a`]を表示\n" +
                ""
                ;
            ni.Run(nc.Codes);
            Assert.AreEqual("566", ni.PrintLog);
        }

        [Test]
        public void Test_array3()
        {
            NakoCompiler nc = new NakoCompiler();
            NakoInterpreter ni = new NakoInterpreter();
            nc.DirectSource =
                "A[3]=566\n" +
                "PRINT A[3]\n" +
                "";
            ni.Run(nc.Codes);
            Assert.AreEqual("566", ni.PrintLog);
        }

        [Test]
        public void Test_array_yen_access()
        {
            NakoCompiler nc = new NakoCompiler();
            NakoInterpreter ni = new NakoInterpreter();
            nc.DirectSource =
                "A￥3=999\n" +
                "A￥3を表示\n" +
                "";
            ni.Run(nc.Codes);
            Assert.AreEqual("999",ni.PrintLog);
        }

        [Test]
        public void Test_array_array1()
        {
            NakoCompiler nc = new NakoCompiler();
            NakoInterpreter ni = new NakoInterpreter();
            nc.DirectSource =
                "B[3][45]=222\n" +
                "PRINT B[3][45]\n" +
                "";
            ni.Run(nc.Codes);
            Assert.AreEqual("222",ni.PrintLog);
        }
        [Test]
        public void Test_array_array2()
        {
            NakoCompiler nc = new NakoCompiler();
            NakoInterpreter ni = new NakoInterpreter();
            nc.DirectSource =
                "C￥１￥２￥３=2222\n" +
                "C￥１￥２￥３を表示\n" +
                "";
            Console.WriteLine(nc.Codes);
            ni.UseConsoleOut = true;
            ni.debugMode = true;
            ni.Run(nc.Codes);
            Assert.AreEqual("2222", ni.PrintLog);
        }
        [Test]
        public void Test_localVar_array()
        {
            NakoCompiler nc = new NakoCompiler();
            NakoInterpreter ni = new NakoInterpreter();
            nc.DirectSource =
                "Cとは変数\n"+
                "C￥１￥２￥３=2222\n" +
                "C￥１￥２￥３を表示。\n" +
                "";
            ni.Run(nc.Codes);
            Assert.AreEqual("2222", ni.PrintLog);
        }
    }
}
