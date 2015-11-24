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
/// connectionにPostgreSQLの例を書いています
/// SQLもPostgreSQLで動くのは確認していますが他のDBはチェックしていません。
/// </summary>
namespace NakoPluginTest
{
    [TestFixture]
    public class TestNakoPluginADO
    {
        NakoCompiler com;
        NakoInterpreter runner = new NakoInterpreter();
        string connection = "「Driver=PostgreSQL Unicode;Servername=localhost;Port=5432;Database=postgres;Username=username;Password=password;」";

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
            runner.Run(com.WriteIL(
                "A="+this.connection+"でADO開く\n" +
                "AをDB閉じる\n" +
                "Aを表示"));
            Assert.AreEqual("System.Data.Odbc.OdbcConnection", runner.PrintLog );
        }
        [Test]
        public void TestCount()
        {
            runner.Run(com.WriteIL(
                "A="+this.connection+"でADO開く\n" +
                "B=Aに『select 1 as name』をSQL実行\n" +
                "(AのDB次移動)==1の間\n" +
                "  C=Aへ『name』のDBフィールド取得\n" +
 				"  Cを表示\n" + 
                "AをDB閉じる"));
            Assert.AreEqual("1", runner.PrintLog );
        }
        [Test]
        public void TestMultiSQL()
        {
            runner.Run(com.WriteIL(
                "A="+this.connection+"でADO開く\n" +
                "B=Aに『select 1 as name』をSQL実行\n" +
                "(AのDB次移動)==1の間\n" +
                "  C=Aへ『name』のDBフィールド取得\n" +
                 "  Cを表示\n" +
                "B=Aに『select 0 as name』をSQL実行\n" +
                "(AのDB次移動)==1の間\n" +
                "  C=Aへ『name』のDBフィールド取得\n" +
                 "  Cを表示\n" +
                "AをDB閉じる"));
            Assert.AreEqual("10", runner.PrintLog );
        }
        [Test]
        public void TestSelectDate()
        {
            //TODO:64bit linuxではエラーが出ることがある。monoのパッチを当てると良いらしいが・・・
            runner.Run(com.WriteIL(
                "A="+this.connection+"でADO開く\n" +
                "B=Aに『select cast('2014-01-01' as date)』をSQL実行\n" +
                "(AのDB次移動)==1の間\n" +
                "  C=Aへ『date』のDBフィールド取得\n" +
                "  Cを表示\n" +
                "AをDB閉じる"));
            Assert.AreEqual("2014/01/01 0:00:00", runner.PrintLog );
        }

     }
}
