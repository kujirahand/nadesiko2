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
    }
}
