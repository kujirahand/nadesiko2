using System;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;
using Libnako.JPNCompiler;
using Libnako.Interpreter;

using cnako2;

namespace CNako2Test
{
    [TestFixture]
    public class CNako2Test
    {
        [Test]
        public void Test_setOptions()
        {
            CNako2Executor e = new CNako2Executor();
            string line = "「あ」と表示";
            string[] args = {"-e", line};
            e.UseLog = true;
            e.setOptions(args);
            Assert.AreEqual(false, e.DescriptMode);
            Assert.AreEqual(false, e.DebugMode);
            Assert.AreEqual(line, e.source);
        }
        [Test]
        public void Test_Run_Oneliner()
        {
            CNako2Executor e = new CNako2Executor();
            string line = "「あ」と表示";
            string[] args = { "-e", line };
            e.UseLog = true;
            e.setOptions(args);
            e.Run();
            Assert.AreEqual("あ", e.PrintLog);
        }
        [Test]
        public void Test_Run_Oneliner_Calc()
        {
            CNako2Executor e = new CNako2Executor();
            string line = "3+5*2と表示";
            string[] args = { "-e", line };
            e.UseLog = true;
            e.setOptions(args);
            e.Run();
            Assert.AreEqual("13", e.PrintLog);
        }
        [Test]
        public void Test_Run_Oneliner_Calc2()
        {
            CNako2Executor e = new CNako2Executor();
            string line = "PRINT ((1+2)*4)";
            string[] args = { "-e", line };
            e.UseLog = true;
            e.setOptions(args);
            e.Run();
            Assert.AreEqual("12", e.PrintLog);
        }
    }
}
