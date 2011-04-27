using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Libnako.JPNCompiler;
using Libnako.Interpreter;
using Libnako.NakoAPI;

using Libnako.JPNCompiler.ILWriter;

using NakoPluginADO;

namespace TestNako
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
        public void TestOpenClose()
        {
            com.DirectSource = 
                "HANDLE=「Driver=PostgreSQL Unicode;Server=localhost;Database=test;UID=postgres;PWD=bommer;Port=5432;」でADO開く\n" +
                "HANDLEをDB閉じる";
            runner.Run(com.Codes);
//            Assert.AreEqual("なでしこ", runner.PrintLog );
        }

        [Test]
        public void TestFind()
        {
            com.DirectSource = 
                "HANDLE=「Driver=PostgreSQL Unicode;Server=localhost;Database=test;UID=postgres;PWD=bommer;Port=5432;」でADO開く\n" +
                "HANDLEに「select * from test_table;」をSQL実行\n"+
                "HANDLEのDBデータ有りの間\n"+
                "\tF=HANDLEへ「hoge」のDBフィールド取得\n"+
                "\tFを表示\n"+
                "\tHANDLEのDB次移動\n"+
                "HANDLEをDB閉じる";
            runner.Run(com.Codes);
            Assert.AreEqual("なでしこ", runner.PrintLog );
        }

     }
}
