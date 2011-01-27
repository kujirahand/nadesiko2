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

namespace TestNako
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
            com.DirectSource = 
                "「abc」をコピー。\n" +
                "クリップボードを表示。";
            runner.Run(com.Codes);
            Assert.AreEqual("abc", runner.PrintLog);
        }
        [Test][STAThreadAttribute]
        public void TestClipboad2()
        {
            com.DirectSource = 
                "10をコピー。\n" +
                "クリップボードを表示。";
            runner.Run(com.Codes);
            Assert.AreEqual("10", runner.PrintLog);
        }
    }
}
