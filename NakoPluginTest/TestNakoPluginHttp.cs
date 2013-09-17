using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Libnako.JPNCompiler;
using Libnako.Interpreter;
using Libnako.NakoAPI;

using Libnako.JPNCompiler.ILWriter;

using NakoPlugin;

namespace NakoPluginTest
{
    [TestFixture]
    public class TestNakoPluginHttp//TODO:簡易Webサーバを立ち上げる
    {
        NakoCompiler com;
        NakoInterpreter runner = new NakoInterpreter();

        public TestNakoPluginHttp()
        {
            NakoCompilerLoaderInfo info = new NakoCompilerLoaderInfo();
            info.PreloadModules = new NakoPlugin.INakoPlugin[] {
                new NakoBaseSystem(),
                new NakoPluginHttp.NakoPluginHttp()
            };
            com = new NakoCompiler(info);
        }

        [Test]
        public void TestUrlDecode()
        {
            runner.Run(com.WriteIL(
                "「%e3%81%aa%e3%81%a7%e3%81%97%e3%81%93」をURLデコードして表示。"));
            Assert.AreEqual("なでしこ", runner.PrintLog );
        }
        [Test]
        public void TestUrlEncode()
        {
            runner.Run(com.WriteIL(
                "「なでしこ」をURLエンコードして小文字変換して表示。"));
            Assert.AreEqual("%e3%81%aa%e3%81%a7%e3%81%97%e3%81%93", runner.PrintLog );
        }


        [Test]
        public void TestHttpGetData()
        {
            runner.Run(com.WriteIL(
                "「http://www.nadesi.com/」をHTTPデータ取得して表示。"));
            Assert.AreEqual(true,runner.PrintLog.Contains("なでしこ"));
        }

        [Test]
        public void TestJsonDecode()
        {
            runner.Run(com.WriteIL(
                "S=『{'href':'/account/login.aspx','target':'_blank'}』\n"+
				"A=SをJSONデコード\n"+
				"A[`target`]を表示。"));
            Assert.AreEqual("_blank",runner.PrintLog);
        }

        [Test]
        public void TestHttpHeader()
        {
            runner.Run(com.WriteIL(
                "「http://nadesi.com/hoge」をHTTPヘッダ取得して表示。"));
            Assert.AreEqual(true,runner.PrintLog.Contains("404 Not Found"));
            runner.Run(com.WriteIL(
                "「http://nadesi.com/」をHTTPヘッダ取得して表示。"));
            Assert.AreEqual(true,runner.PrintLog.Contains("200 OK"));
        }

        [Test]
        public void TestHttpHeaderHash()
        {
            runner.Run(com.WriteIL(
                "A=「http://www.yahoo.co.jp/」をHTTPヘッダハッシュ取得。" +
                "A[`Content-Type`]を表示。"));
            Assert.AreEqual("text/html; charset=utf-8",runner.PrintLog);
        }

        [Test]
        public void TestHttpPost()//TODO:外部サービスを使ってるので、後々簡易WEBサーバをテスト時に構築し、そこにアクセスするように変更したい
        {
            runner.Run(com.WriteIL(
                "A=「http://www.muryou-tools.com/test/aaaa.php」へ「hoge=fuga&hage=1」をHTTPポスト。" +
                "Aを表示。"));
            Assert.AreEqual(true,runner.PrintLog.Contains("hoge=fuga"));
        }

        [Test]
        public void TestHttpGet()//TODO:外部サービスを使ってるので、後々簡易WEBサーバをテスト時に構築し、そこにアクセスするように変更したい
        {
            runner.Run(com.WriteIL(
                "A=「Referer:http://localhost/nadesiko2/\n"+
                "UserAgent:Mozilla/5.0 (iPhone; U; CPU iPhone OS 4_0_1 like Mac OS X; ja-jp) AppleWebKit/532.9 (KHTML, like Gecko) Version/4.0.5 Mobile/8A306 Safari/6531.22.7」を「http://mpw.jp/requestheader/」へHTTPゲット。" +
                "Aを表示。"));
            Assert.AreEqual(true,runner.PrintLog.Contains("http://localhost/nadesiko2/"));
        }
     }
}
