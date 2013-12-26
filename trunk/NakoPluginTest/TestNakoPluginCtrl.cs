/*
 * NUnit Test Case for NakoPluginCtrl
 * User: kujirahand
 * Date: 2010-10-04
 * Time: 2:27
 */
using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Libnako.JPNCompiler;
using Libnako.Interpreter;
using Libnako.NakoAPI;

using NakoPluginCtrl;
using NakoPlugin;

namespace NakoPluginTest
{
    /// <summary>
    /// Test for NakoPluginCtrl.
    /// </summary>
    [TestFixture]
    public class TestNakoPluginCtrl
    {
        NakoCompiler com;
        NakoInterpreter runner = new NakoInterpreter();
        
        public TestNakoPluginCtrl()
        {
            NakoCompilerLoaderInfo info = new NakoCompilerLoaderInfo();
            info.PreloadModules = new NakoPlugin.INakoPlugin[] {
                new NakoBaseSystem(),
                new NakoPluginCtrl.NakoPluginCtrl()
            };
            com = new NakoCompiler(info);
        }
        
        [Test][STAThreadAttribute]
        public void TestClipboad()
        {
            runner.Run(com.WriteIL(
                "「abc」をコピー。\n" +
                "クリップボードを表示。"));
            Assert.AreEqual("abc", runner.PrintLog);
        }
        [Test][STAThreadAttribute]
        public void TestClipboad2()
        {
            runner.Run(com.WriteIL(
                "10をコピー。\n" +
                "クリップボードを表示。"));
            Assert.AreEqual("10", runner.PrintLog);
        }
        [Test][STAThreadAttribute]
        public void TestEnumWindows()
        {
            runner.Run(com.WriteIL(
                "窓列挙して表示。\n"));
            Assert.AreNotEqual("", runner.PrintLog);
        }
        [Test]
        public void TestGuid()//TODO:Success test
        {
            runner.Run(com.WriteIL(
                "GUID取得して表示。\n"));
            Assert.AreEqual("", runner.PrintLog);
        }
        [Test]
        public void TestPs()//TODO:Success test
        {
            runner.Run(com.WriteIL(
                "プロセス列挙して表示。\n"));
            Assert.AreEqual(true, runner.PrintLog.Contains((NWEnviroment.isWindows())? "svchost" : "init"));
        }
        public void TestAbort()//TODO:プロセス強制終了はどうやってテストしようか。ダミーのプロセスが要るか？ test
        {
            runner.Run(com.WriteIL(
                "。\n"));
            Assert.AreEqual("", runner.PrintLog);
        }
        [Test]
        public void TestTotalMemory()//TODO:Success test
        {
            runner.Run(com.WriteIL(
                "メモリ総容量取得して表示。\n"));
            Assert.Greater(int.Parse (runner.PrintLog),0);
        }
        [Test]
        public void TestAvailableMemory()//TODO:Success test
        {
            runner.Run(com.WriteIL(
                "メモリ空き容量取得して表示。\n"));
            Assert.Greater(int.Parse (runner.PrintLog),0);
        }
        [Test]
        public void TestUsagePercentageOfMemory()//TODO:Success test
        {
            runner.Run(com.WriteIL(
                "メモリ使用率取得して表示。\n"));
            Assert.Greater(int.Parse (runner.PrintLog),0);
            Assert.Less(int.Parse (runner.PrintLog),100);
        }
    }
}
