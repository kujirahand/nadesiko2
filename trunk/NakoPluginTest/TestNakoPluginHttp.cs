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
			//auto encode (utf-8 page)
            runner.Run(com.WriteIL(
				"「http://www.shigepon.com/test/utf8/」をHTTPデータ取得して表示。"));
            Assert.AreEqual(true,runner.PrintLog.Contains("なでしこ"));
			//auto encode (sjis page)
			runner.Run(com.WriteIL(
				"「http://www.shigepon.com/test/sjis/」をHTTPデータ取得して表示。"));
			Assert.AreEqual(true,runner.PrintLog.Contains("なでしこ"));
			//auto encode (euc-jp page)
			runner.Run(com.WriteIL(
				"「http://www.shigepon.com/test/eucjp/」をHTTPデータ取得して表示。"));
			Assert.AreEqual(true,runner.PrintLog.Contains("なでしこ"));
			//set encode to sjis (utf-8 page) 文字化けする
			runner.Run(com.WriteIL(
				"「http://www.shigepon.com/test/utf8/」を「CP932」HTTPデータ取得して表示。"));
			Assert.AreNotEqual(true,runner.PrintLog.Contains("なでしこ"));
			//set encode to utf-8 (utf-8 page)
			runner.Run(com.WriteIL(
				"「http://www.shigepon.com/test/utf8/」を「UTF-8」HTTPデータ取得して表示。"));
			Assert.AreEqual(true,runner.PrintLog.Contains("なでしこ"));
			//set encode to euc-jp (sjis page) 文字化けする
			runner.Run(com.WriteIL(
				"「http://www.shigepon.com/test/sjis/」を「EUC-JP」HTTPデータ取得して表示。"));
			Assert.AreNotEqual(true,runner.PrintLog.Contains("なでしこ"));
			//set encode to sjis (sjis page)
			runner.Run(com.WriteIL(
				"「http://www.shigepon.com/test/sjis/」を「CP932」HTTPデータ取得して表示。"));
			Assert.AreEqual(true,runner.PrintLog.Contains("なでしこ"));
			//set encode to utf-8 (euc-jp page) 文字化けする
			runner.Run(com.WriteIL(
				"「http://www.shigepon.com/test/eucjp/」を「UTF-8」HTTPデータ取得して表示。"));
			Assert.AreNotEqual(true,runner.PrintLog.Contains("なでしこ"));
			//set encode to euc-jp (euc-jp page)
			runner.Run(com.WriteIL(
				"「http://www.shigepon.com/test/eucjp/」を「EUC-JP」HTTPデータ取得して表示。"));
			Assert.AreEqual(true,runner.PrintLog.Contains("なでしこ"));
        }

        [Test]
        public void TestJsonEncode()
        {
            runner.Run(com.WriteIL(
                "J[`href`]=「/account/login.aspx」\n"+
                "J[`target`]=「_blank」\n"+
                "S=JをJSONエンコード\n"+
                "Sを表示。"));
            Assert.AreEqual("{\"href\":\"/account/login.aspx\",\"target\":\"_blank\"}",runner.PrintLog);
//            runner.Run(com.WriteIL(
//                "A[`href`]=「/account/login.aspx」\n"+
//                "A[`target`]=「_blank」\n"+
//                "S=AをJSONエンコード\n"+
//                "Sを表示。"));
//            Assert.AreEqual("{\"href\":\"/account/login.aspx\",\"target\":\"_blank\"}",runner.PrintLog);
//            runner.Run(com.WriteIL(
//                "B[`href`][`hoge`]=「/acc/login.aspx」\n"+
//                "B[`href`][`fuga`]=「/account/」\n"+
//                "B[`href`][`fuga`]を表示。"));
//                "B[`target`]=「_blank」\n"+
//                "S=BをJSONエンコード\n"+
//                "Sを表示。"));
//            Assert.AreEqual("{\"href\":\"/account/login.aspx\",\"target\":\"_blank\"}",runner.PrintLog);
//            runner.Run(com.WriteIL(
//                "B=「/account/login.aspx」\n"+
//                "C[0]=「/account/login.aspx」\n"+
//                "C[1]=「_blank」\n"+
//                "S=CをJSONエンコード\n"+
//                "Sを表示。"));
//            Assert.AreEqual("{\"href\":\"/account/login.aspx\",\"target\":\"_blank\"}",runner.PrintLog);
        }

        [Test]
        public void TestJsonDecode()
        {
            runner.Run(com.WriteIL(
                "S=『[1,2,{\"href\":\"/account/login.aspx\",\"target\":[1,\"hoge\",3]}]』\n"+
//            「{\"href\":\"/account/login.aspx\",\"target\":\"_blank\"}」\n"+
				"A=SをJSONデコード\n"+
				"A[2][`target`][1]を表示。"));
            Assert.AreEqual("hoge",runner.PrintLog);
            runner.Run(com.WriteIL(
                "S=『{\"href\":\"/account/login.aspx\",\"target\":\"_blank\"}』\n"+
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
			//auto encode
			runner.Run(com.WriteIL(
				"A=「http://www.muryou-tools.com/test/aaaa.php」へ「hoge=fuga&hage=1」をHTTPポスト。" +
				"Aを表示。"));
			Assert.AreEqual(true,runner.PrintLog.Contains("hoge=fuga"));
			Assert.AreEqual(true,runner.PrintLog.Contains("テスト"));
			//set encode to sjis
			runner.Run(com.WriteIL(
				"A=「http://www.muryou-tools.com/test/aaaa.php」へ「hoge=fuga&hage=1」を「CP932」でHTTPポスト。" +
				"Aを表示。"));
			Assert.AreEqual(true,runner.PrintLog.Contains("hoge=fuga"));
			Assert.AreEqual(true,runner.PrintLog.Contains("テスト"));
			//set encode to utf-8 (文字化けする)
            runner.Run(com.WriteIL(
				"A=「http://www.muryou-tools.com/test/aaaa.php」へ「hoge=fuga&hage=1」を「UTF-8」でHTTPポスト。" +
                "Aを表示。"));
			Console.WriteLine (runner.PrintLog);
			Assert.AreEqual(true,runner.PrintLog.Contains("hoge=fuga"));
			Assert.AreNotEqual(true,runner.PrintLog.Contains("テスト"));
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

        [Test]
        public void TestRelativeUrl()//TODO:外部サービスを使ってるので、後々簡易WEBサーバをテスト時に構築し、そこにアクセスするように変更したい
        {
            runner.Run(com.WriteIL(
                "A=「../index.html」\n"+
                "B=「http://hoge.com/fuga/foo/」\n"+
                "AをBでURL展開を表示。"));
            Assert.AreEqual("http://hoge.com/fuga/index.html",runner.PrintLog);
        }

        [Test]
        public void TestBaseOfUrl()//TODO:外部サービスを使ってるので、後々簡易WEBサーバをテスト時に構築し、そこにアクセスするように変更したい
        {
            runner.Run(com.WriteIL(
                "A=「http://hoge.com/fuga/foo/index.html」\n"+
                "AのURL基本パス抽出を表示。"));
            Assert.AreEqual("http://hoge.com/fuga/foo/",runner.PrintLog);
        }

        [Test]
        public void TestFilenameOfUrl()//TODO:外部サービスを使ってるので、後々簡易WEBサーバをテスト時に構築し、そこにアクセスするように変更したい
        {
            runner.Run(com.WriteIL(
                "A=「http://hoge.com/fuga/foo/index.html」\n"+
                "AのURLファイル名抽出を表示。"));
            Assert.AreEqual("index.html",runner.PrintLog);
        }

        [Test]
        public void TestDomainOfUrl()//TODO:外部サービスを使ってるので、後々簡易WEBサーバをテスト時に構築し、そこにアクセスするように変更したい
        {
            runner.Run(com.WriteIL(
                "A=「http://hoge.com/fuga/foo/」\n"+
                "AのURLドメイン名抽出を表示。"));
            Assert.AreEqual("hoge.com",runner.PrintLog);
        }
     }
}
