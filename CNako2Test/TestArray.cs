using System;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;
using Libnako.JPNCompiler;
using Libnako.Interpreter;

namespace NakoPluginTest
{
    [TestFixture]
    public class TestArray
    {
        NakoCompiler com;
        NakoInterpreter runner;

        public TestArray()
        {
            com = new NakoCompiler();
            runner = new NakoInterpreter();
        }


        [Test]
        public void Test_array1()
        {
            NakoCompiler nc = new NakoCompiler();
            NakoInterpreter ni = new NakoInterpreter();
            nc.DirectSource =
                "A[3]=566\n" +
                "A[3]を継続表示\n" +
                "";
            ni.Run(nc.Codes);
            Assert.AreEqual("566", ni.PrintLog);
        }

        [Test]
        public void Test_array2()
        {
            NakoCompiler nc = new NakoCompiler();
            NakoInterpreter ni = new NakoInterpreter();
            nc.DirectSource =
                "A[`a`]=566\n" +
                "A[`a`]を継続表示\n" +
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
                "A￥3を継続表示\n" +
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
                "C￥１￥２￥３を継続表示\n" +
                "";
            Console.WriteLine(nc.Codes);
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
                "C￥１￥２￥３を継続表示。\n" +
                "";
            ni.Run(nc.Codes);
            Assert.AreEqual("2222", ni.PrintLog);
        }

        [Test]
        public void Test_toString()
        {
            com.DirectSource = "Cとは変数。\n" +
                "C￥0=`a`;C￥1=`b`;C￥2=`c`;\n" +
                "Cを継続表示";
            runner.Run(com.Codes);
            Assert.AreEqual("a\r\nb\r\nc", runner.PrintLog);
        }

        [Test]
        public void Test_convArray()
        {
            com.DirectSource =
                "A=`abc`;" +
                "A[1]=`def`;" +
                "Aを継続表示;" +
                "" +
                "";
            runner.Run(com.Codes);
            Assert.AreEqual("abc\r\ndef", runner.PrintLog);
        }

        [Test]
        public void Test_convArray2()
        {
            com.DirectSource =
                "A=「」;" +
                "A[0]=`abc`;"+
                "A[1]=`def`;" +
                "Aを継続表示;" +
                "" +
                "";
            runner.Run(com.Codes);
            Assert.AreEqual("abc\r\ndef", runner.PrintLog);
        }

        [Test]
        public void Test_array4()
        {
            NakoCompiler nc = new NakoCompiler();
            NakoInterpreter ni = new NakoInterpreter();
            nc.DirectSource =
                "A=「1\r\n" +
                "10\r\n" +
                "100\r\n" +
                "566」\n" +
                "PRINT A[3]\n" +
                "";
            ni.Run(nc.Codes);
            Assert.AreEqual("566", ni.PrintLog);
        }

        [Test]
        public void Test_array_array5()
        {
            NakoCompiler nc = new NakoCompiler();
            NakoInterpreter ni = new NakoInterpreter();
            nc.DirectSource =
                "A[45]=222\n" +
                "B[3]=A\n" +
                "PRINT B[3][45]\n" +
                "";
            ni.Run(nc.Codes);
            Assert.AreEqual("222",ni.PrintLog);
        }
        [Test]
        public void Test_array_array6()
        {
            NakoCompiler nc = new NakoCompiler();
            NakoInterpreter ni = new NakoInterpreter();
            nc.DirectSource =
                "A[3][45]=222\n" +
                "B[20]=A\n" +
                "PRINT B[20][3][45]\n" +
                "";
            ni.Run(nc.Codes);
            Assert.AreEqual("222",ni.PrintLog);
        }
        [Test]
        public void Test_array_array7()
        {
            NakoCompiler nc = new NakoCompiler();
            NakoInterpreter ni = new NakoInterpreter();
            nc.DirectSource =
                "A[`a`]=222\n" +
                "B[`b`]=A\n" +
                "PRINT B[`b`][`a`]\n" +
                "";
            ni.Run(nc.Codes);
            Assert.AreEqual("222",ni.PrintLog);
        }
        [Test]
        public void Test_array_array8()
        {
            NakoCompiler nc = new NakoCompiler();
            NakoInterpreter ni = new NakoInterpreter();
            nc.DirectSource =
                "A[3][`a`]=222\n" +
                "B[`b`]=A\n" +
                "PRINT B[`b`][3][`a`]\n" +
                "";
            ni.Run(nc.Codes);
            Assert.AreEqual("222",ni.PrintLog);
        }

    }
}
