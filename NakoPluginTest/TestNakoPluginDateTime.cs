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

        [Test]
        public void TestDaysDifference()
        {
            runner.Run(com.WriteIL(
                "「2013/1/1」と「2013/2/1」の日数差を表示。"));
            Assert.AreEqual("31",runner.PrintLog);
        }

        [Test]
        public void TestToJapanese()
        {
            runner.Run(com.WriteIL(
                "「2013/1/1」を和暦変換して表示。"));
            Assert.AreEqual("平成25年1月1日",runner.PrintLog);
        }

        [Test]
        public void TestFormat()
        {
            runner.Run(com.WriteIL(
                "「2013/1/1」を「gyyyy年MM月dd日(dddd)」に日時形式変換して表示。"));
            Assert.AreEqual("A.D.2013年01月01日(火曜日)",runner.PrintLog);
        }
     }
}
