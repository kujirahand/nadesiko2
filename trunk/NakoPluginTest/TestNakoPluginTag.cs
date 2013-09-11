using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Libnako.JPNCompiler;
using Libnako.Interpreter;
using Libnako.NakoAPI;

using Libnako.JPNCompiler.ILWriter;

using NakoPluginTag;

namespace NakoPluginTest
{
    [TestFixture]
    public class TestNakoPluginTag
    {
        NakoCompiler com;
        NakoInterpreter runner = new NakoInterpreter();

        public TestNakoPluginTag()
        {
            NakoCompilerLoaderInfo info = new NakoCompilerLoaderInfo();
            info.PreloadModules = new NakoPlugin.INakoPlugin[] {
                new NakoBaseSystem(),
                new NakoPluginTag.NakoPluginTag()
            };
            com = new NakoCompiler(info);
        }

        [Test]
        public void TestExtract()
        {
            runner.Run(com.WriteIL( 
                "S=「<html><body><b>な</b><b>で</b><h1>し</h1><h2>こ</h2></body></html>」\n" +
                "TAG=Sから「b」のタグ切り出し\n" +
                "TAG[0]を表示。"));
            Assert.AreEqual("な",runner.PrintLog);
            runner.Run(com.WriteIL(
                "S=「<b>な</b><b>で</b><h1>し</h1><h2>こ</h2>」\n" +
                "TAG=Sから「b」のタグ切り出し\n" +
                "TAG[1]を表示。"));
            Assert.AreEqual("で",runner.PrintLog);
            runner.Run(com.WriteIL(
                "S=「<な>な</な><で>で</で><し>し</し><こ>こ</こ>」\n" +
                "TAG=Sから「で」のタグ切り出し\n" +
                "TAG[0]を表示。"));
            Assert.AreEqual("で",runner.PrintLog);
        }

        [Test]
        public void TestRemove()
        {
            runner.Run(com.WriteIL(
                "S=「<html><body><b>な</b><b>で</b><h1>し</h1><h2>こ</h2></body></html>」\n" +
                "TAG=Sのタグ削除\n" +
                "TAGを表示。"));
            Assert.AreEqual("なでしこ",runner.PrintLog);
            runner.Run(com.WriteIL(
                "S=「<b>な</b><b>で</b><h1>し</h1><h2>こ</h2>」\n" +
                "TAG=Sからタグ削除\n" +
                "TAGを表示。"));
            Assert.AreEqual("なでしこ",runner.PrintLog);
            runner.Run(com.WriteIL(
                "S=「<な>な</な><で>で</で><し>し</し><こ>こ</こ>」\n" +
                "TAG=Sからタグ削除\n" +
                "TAGを表示。"));
            Assert.AreEqual("なでしこ",runner.PrintLog);
        }
        [Test]
        public void TestAttr()
        {
            runner.Run(com.WriteIL(
                "S=「<html><body><b class='na'>な</b><b class='de'>で</b><h1 class='si'>し</h1><h2 class='ko'>こ</h2></body></html>」\n" +
                "TAG=Sの「h1」から「class」をタグ属性取得\n" +
                "TAG[0]を表示。"));
            Assert.AreEqual("si",runner.PrintLog);
        }
        [Test]
        public void TestLink()
        {
            runner.Run(com.WriteIL(
                "S=「<html><body><a href='http://nadesi.com'>な</a><a href='http://www.eznavi.net'>で</a><a href='http://d.aoikujira.com/blog/'>し</a><h2 class='ko'>こ</h2></body></html>」\n" +
                "LINKS=SからHTMLリンク抽出\n" +
                "LINKS[0]を表示\n" +
                "LINKS[2]を表示。"));
            Assert.AreEqual("http://nadesi.comhttp://d.aoikujira.com/blog/",runner.PrintLog);
        }
    }
}
