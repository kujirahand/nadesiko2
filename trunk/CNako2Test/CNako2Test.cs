using System;
using System.Collections.Generic;
using System.Text;
using cnako2;
using Libnako.Interpreter;
using Libnako.JPNCompiler;
using NUnit.Framework;

namespace CNako2Test
{
    [TestFixture]
    public class CNako2Test
    {
        [Test]
        public void Test_setOptions()
        {
            var exec = new CNako2Executor()
            {
                UseLog = true
            };
            string line = "「あ」と継続表示";
            string[] args = {"-e", line};
            exec.SetOptions(args);
            Assert.AreEqual(false, exec.DescriptMode);
            Assert.AreEqual(false, exec.DebugMode);
            Assert.AreEqual(line, exec.source);
        }
        [Test]
        public void Test_Run_Oneliner()
        {
            var exec = new CNako2Executor()
            {
                UseLog = true
            };
            string line = "「あ」と継続表示";
            string[] args = { "-e", line };
            exec.SetOptions(args);
            exec.Run();
            Assert.AreEqual("あ", exec.PrintLog);
        }
        [Test]
        public void Test_Run_Oneliner_Calc()
        {
            var exec = new CNako2Executor()
            {
                UseLog = true
            };
            string line = "3+5*2と継続表示";
            string[] args = { "-e", line };
            exec.SetOptions(args);
            exec.Run();
            Assert.AreEqual("13", exec.PrintLog);
        }
        [Test]
        public void Test_Run_Oneliner_Calc2()
        {
            var exec = new CNako2Executor()
            {
                UseLog = true
            };
            string line = "PRINT ((1+2)*4)";
            string[] args = { "-e", line };
            exec.SetOptions(args);
            exec.Run();
            Assert.AreEqual("12", exec.PrintLog);
        }
        [Test]
        public void Test_Run_Oneliner_Comment()
        {
            var exec = new CNako2Executor()
            {
                UseLog = true
            };
            string line = "`abc`を継続表示。\n" +
                "# comment\n" +
                "`cde`を継続表示。\n" +
                "";
            string[] args = { "-e", line };
            exec.SetOptions(args);
            exec.Run();
            Assert.AreEqual("abccde", exec.PrintLog);
        }
        [Test]
        public void Test_Run_Oneliner_Comment2()
        {
            var exec = new CNako2Executor()
            {
                UseLog = true
            };
            string line = "`abc`を表示。\n" +
                "/* comment test\n --- abc def \nghi 333 222 */\n" +
                "`cde`を表示。\n" +
                "";
            string[] args = { "-e", line };
            exec.SetOptions(args);
            exec.Run();
            Assert.AreEqual("abc\r\ncde\r\n", exec.PrintLog);
        }
    }
}
