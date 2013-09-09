using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Libnako.JPNCompiler;
using Libnako.Interpreter;
using Libnako.NakoAPI;

using NakoPluginADO;
using Libnako.JPNCompiler.ILWriter;

/// <summary>
/// Test nako plugin ADO.
/// 前もって必要な作業を書いておく（DB設定とか）
/// </summary>
namespace NakoPluginTest
{
    [TestFixture]
    public class TestNakoPluginADO
    {
        NakoCompiler com;
        NakoInterpreter runner = new NakoInterpreter();

        public TestNakoPluginADO()
        {
            NakoCompilerLoaderInfo info = new NakoCompilerLoaderInfo();
            info.PreloadModules = new NakoPlugin.INakoPlugin[] {
                new NakoBaseSystem(),
                new NakoPluginADO.NakoPluginADO()
            };
            com = new NakoCompiler(info);
        }

        [Test]
        public void TestConnect()
        {
            com.DirectSource = 
                "A=「Dsn=announce;」でADO開く\n" +
                "AをDB閉じる\n" +
                "Aを表示";
            runner.Run(com.Codes);
            Assert.AreEqual("System.Data.Odbc.OdbcConnection", runner.PrintLog );
        }
        [Test]
        public void TestCount()
        {
            com.DirectSource = 
                "A=「Dsn=announce;」でADO開く\n" +
                "B=Aに『select id,name from admin』をSQL実行\n" +
                "(AのDB次移動)==1の間\n" +
                "  C=Aへ『name』のDBフィールド取得\n" +
 				"  Cを表示\n" + 
                "AをDB閉じる";
            runner.Run(com.Codes);
            Assert.AreEqual("System.Data.Odbc.OdbcConnection", runner.PrintLog );
        }

     }
}
