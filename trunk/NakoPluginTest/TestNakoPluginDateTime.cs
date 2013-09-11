using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Libnako.JPNCompiler;
using Libnako.Interpreter;
using Libnako.NakoAPI;

using Libnako.JPNCompiler.ILWriter;

using NakoPluginDateTime;

namespace NakoPluginTest
{
    [TestFixture]
    public class TestNakoPluginDateTime
    {
        NakoCompiler com;
        NakoInterpreter runner = new NakoInterpreter();

        public TestNakoPluginDateTime()
        {
            NakoCompilerLoaderInfo info = new NakoCompilerLoaderInfo();
            info.PreloadModules = new NakoPlugin.INakoPlugin[] {
                new NakoBaseSystem(),
                new NakoPluginDateTime.NakoPluginDateTime()
            };
            com = new NakoCompiler(info);
        }

        [Test]
        public void TestToday()
        {
            runner.Run(com.WriteIL(
                "今日を表示。"));
            Assert.AreEqual(runner.PrintLog, DateTime.Today.ToString("yyyy-MM-dd"));
        }

        [Test]
        public void TestNow()
        {
            runner.Run(com.WriteIL(
                "今を表示。"));
            Assert.AreEqual(runner.PrintLog, DateTime.Now.ToString("HH:mm:ss"));
        }
        
        [Test]
        public void TestDiffHours()
        {
            runner.Run(com.WriteIL(
                "「2001-11-11 01:00:00」と「2001-11-11 02:00:00」の時間差を表示。"));
            DateTime d1 = DateTime.Parse("2001-11-11 01:00:00");
            DateTime d2 = DateTime.Parse("2001-11-11 02:00:00");
            TimeSpan diff = d1.Subtract(d2);
            Assert.AreEqual(runner.PrintLog, diff.TotalHours.ToString());
        }
        [Test, Timeout(2500)]
        public void TestSleep()
        {
            runner.Run(com.WriteIL(
                "2秒待つ。"));
        }

		[Test]
        public void TestAdd()
        {
            runner.Run(com.WriteIL(
                "「2001-11-11 01:00:00」に「+0001/01/01」を日付加算を表示。"));
            Assert.AreEqual("2002/12/12 1:00:00",runner.PrintLog);
        }

		[Test]
        public void TestUnixtimeToDatetime()
        {
            runner.Run(com.WriteIL(
                "1378652400をUNIXTIME_日時変換を表示。"));
			DateTime hoge = new DateTime(2013,9,9,0,0,0,DateTimeKind.Local);
            Assert.AreEqual(hoge.ToString(),runner.PrintLog);
        }
     }
}
