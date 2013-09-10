using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Libnako.JPNCompiler;
using Libnako.Interpreter;
using Libnako.NakoAPI;

using Libnako.JPNCompiler.ILWriter;

namespace NakoPluginTest
{
    [TestFixture]
    public class TestNakoPluginString
    {
        NakoCompiler com;
        NakoInterpreter runner = new NakoInterpreter();

        public TestNakoPluginString()
        {
            NakoCompilerLoaderInfo info = new NakoCompilerLoaderInfo();
            info.PreloadModules = new NakoPlugin.INakoPlugin[] {
                new NakoBaseSystem(),
                new NakoPluginString(),
                new NakoPluginArray()
            };
            com = new NakoCompiler(info);
        }

        [Test]
        public void TestLength()
        {
            com.DirectSource =
                "「なでしこ」の文字数を継続表示。";
            runner.Run(com.Codes);
            Assert.AreEqual(runner.PrintLog, "4");
        }

        [Test]
        public void TestSearch()
        {
            com.DirectSource =
                "「なでしこ」の「し」を文字列検索を継続表示。";
            runner.Run(com.Codes);
            Assert.AreEqual(runner.PrintLog, "3");
        }

        [Test]
        public void TestReplace()
        {
            com.DirectSource =
                "「なでしこなでなで」の「なで」を「えら」に置換を継続表示。";
            runner.Run(com.Codes);
            Assert.AreEqual(runner.PrintLog, "えらしこえらえら");
        }

        [Test]
        public void TestReplaceA()
        {
            com.DirectSource =
                "「なでしこなでなで」の「なで」を「えら」に単置換を継続表示。";
            runner.Run(com.Codes);
            Assert.AreEqual(runner.PrintLog, "えらしこなでなで");
        }

        [Test]
        public void TestLeft()
        {
            com.DirectSource =
                "「なでしこ」の2文字左部分を継続表示。";
            runner.Run(com.Codes);
            Assert.AreEqual(runner.PrintLog, "なで");
        }

        [Test]
        public void TestTrim()
        {
            com.DirectSource =
                "「　　なでしこ　　」をトリムを継続表示。";
            runner.Run(com.Codes);
            Assert.AreEqual(runner.PrintLog, "なでしこ");
        }


        [Test]
        public void TestRight()
        {
            com.DirectSource =
                "「なでしこ」2文字右部分を継続表示。";
            runner.Run(com.Codes);
            Assert.AreEqual(runner.PrintLog, "しこ");
        }
        [Test]
        public void TestCut()
        {
            //            com.DirectSource = 
            //                "「なでしこ」から「し」まで切り取って表示。";
            //            runner.Run(com.Codes);
            //            Assert.AreEqual("なで", runner.PrintLog);
            com.DirectSource =
                "S=「なでしこ」\n" +
                "Sから「し」まで切り取る\n" +
                "Sを継続表示。";
            runner.Run(com.Codes);
            Assert.AreEqual("こ", runner.PrintLog);
        }
        [Test]
        public void TestExtract()
        {
            com.DirectSource =
                "「なでしこ」の1から2文字抜き出して継続表示。";
            runner.Run(com.Codes);
            Assert.AreEqual(runner.PrintLog, "なで");
        }
        [Test]
        public void TestEm()
        {
            com.DirectSource =
                "「なでしこ」の全角か判定を継続表示。";
            runner.Run(com.Codes);
            Assert.AreEqual(runner.PrintLog, "1");
        }
        [Test]
        public void TestRemove()
        {
            com.DirectSource =
                "S=「なでしこ」\n" +
                "Sの2から1文字削除\n" +
                "Sを継続表示。";
            runner.Run(com.Codes);
            Assert.AreEqual("なしこ", runner.PrintLog);
        }
        [Test]
        public void TestRemoveRight()
        {
            com.DirectSource =
                "S=「なでしこ」\n" +
                "Sから1文字右端削除\n" +
                "Sを継続表示。";
            runner.Run(com.Codes);
            Assert.AreEqual("なでし", runner.PrintLog);
        }
        [Test]
        public void TestInsert()
        {
            com.DirectSource =
                "「なしこ」の2に「で」を文字挿入して継続表示。";
            runner.Run(com.Codes);
            Assert.AreEqual("なでしこ", runner.PrintLog);
        }
        [Test]
        public void TestDegrade()
        {
            com.DirectSource =
                "S=「なでしこ」を文字列分解\n" +
                "S[0]を継続表示。";
            runner.Run(com.Codes);
            Assert.AreEqual("な", runner.PrintLog);
        }
        [Test]
        public void TestExplode()
        {
            com.DirectSource =
                "S=「なでしこ」を「し」で区切る\n" +
                "S[0]を継続表示。";
            runner.Run(com.Codes);
            Assert.AreEqual("なで", runner.PrintLog);
        }
        [Test]
        public void TestNum()
        {
            com.DirectSource =
                "「0なでしこ」が数字か判定を継続表示。";
            runner.Run(com.Codes);
            Assert.AreEqual("1", runner.PrintLog);
        }
        [Test]
        public void TestAppend()
        {
            com.DirectSource =
                "S=「なでし」\n"+
				"Sに「こ」を追加\n" +
				"Sを継続表示。";
            runner.Run(com.Codes);
            Assert.AreEqual("なでしこ", runner.PrintLog);
        }
        [Test]
        public void TestAlnumToEn()
        {
            com.DirectSource =
                "S=「アイウエオ０１２３４５６７８９ａｂｃＡＢＣ」\n" +
                "Sを英数半角変換して継続表示";
            runner.Run(com.Codes);
            Assert.AreEqual("アイウエオ0123456789abcABC", runner.PrintLog);
        }
        [Test]
        public void TestToEn()
        {
            com.DirectSource =
                "S=「アイウエオ０１２３４５６７８９ａｂｃＡＢＣ」\n" +
                "S=Sを半角変換して継続表示";
            runner.Run(com.Codes);
            Assert.AreEqual("ｱｲｳｴｵ0123456789abcABC", runner.PrintLog);
        }
        public void TestToUpper()
        {
            com.DirectSource =
                "S=「アイウエオ０１２３４５６７８９ａｂｃＡＢＣabcABC」\n" +
                "S=Sを小文字変換して継続表示";
            runner.Run(com.Codes);
            Assert.AreEqual("アイウエオ０１２３４５６７８９ａｂｃａｂｃabcabc", runner.PrintLog);
        }
        public void TestToLower()
        {
            com.DirectSource =
                "S=「アイウエオ０１２３４５６７８９ａｂｃＡＢＣabcABC」\n" +
                "S=Sを大文字変換して継続表示";
            runner.Run(com.Codes);
            Assert.AreEqual("アイウエオ０１２３４５６７８９ＡＢＣＡＢＣABCABC", runner.PrintLog);
        }
        [Test]
        public void TestZerofill()
        {
            com.DirectSource =
                "S=「10」\n" +
                "S=Sを4でゼロ埋め\n" +
                "Sを継続表示";
            runner.Run(com.Codes);
            Assert.AreEqual("0010", runner.PrintLog);
        }
        [Test]
        public void Test_strpos()
        {
            NakoCompiler nc = new NakoCompiler();
            NakoInterpreter ni = new NakoInterpreter();
            nc.DirectSource =
                "`abc`で`b`が何文字目\n" +
                "それを継続表示\n" +
                "";
            ni.Run(nc.Codes);
            Assert.AreEqual("2", ni.PrintLog);
        }
        [Test]
        public void Test_strpos2()
        {
            NakoCompiler nc = new NakoCompiler();
            NakoInterpreter ni = new NakoInterpreter();
            nc.DirectSource =
                "`abc`で`d`が何文字目\n" +
                "それを継続表示\n" +
                "";
            ni.Run(nc.Codes);
            Assert.AreEqual("0", ni.PrintLog);
            //
            nc.DirectSource =
                "`あいう`で`う`が何文字目\n" +
                "それを継続表示\n" +
                "";
            ni.Run(nc.Codes);
            Assert.AreEqual("3", ni.PrintLog);
            //
            nc.DirectSource =
                "12345で4が何文字目\n" +
                "それを継続表示\n" +
                "";
            ni.Run(nc.Codes);
            Assert.AreEqual("4", ni.PrintLog);
        }
        [Test]
        public void Test_occurrence()
        {
            NakoCompiler nc = new NakoCompiler();
            NakoInterpreter ni = new NakoInterpreter();
            nc.DirectSource =
                "`abcdabcabdcab`で`ab`の出現回数\n" +
                "それを継続表示\n" +
                "";
            ni.Run(nc.Codes);
            Assert.AreEqual("4", ni.PrintLog);
            nc.DirectSource =
                "`ほげふがほbcaふがげふがほふげがふがほ`で`ふが`の出現回数\n" +
                "それを継続表示\n" +
                "";
            ni.Run(nc.Codes);
            Assert.AreEqual("4", ni.PrintLog);
        }
		
		[Test]
        public void TestKanaToRoman()
        {
			//TODO:なでしこ1の実装では英数が消えてしまうんだけどどうしようかな
			//Linuxでは失敗
            com.DirectSource =
                "S=「アイウエオ０１２３４５６７８９ａｂｃＡＢＣ」\n" +
                "Sをカナローマ字変換して継続表示";
            runner.Run(com.Codes);
            Assert.AreEqual("aiueo０１２３４５６７８９ａｂｃＡＢＣ", runner.PrintLog);
        }
		
		[Test]
        public void TestSjisToUtf8()
        {//このテスト、ちゃんとSJISで関数に送られていないような気がする。
			string s = @"ほげほげふがふが埋め込み";
			System.Text.Encoding src = System.Text.Encoding.UTF8;
			System.Text.Encoding dest = System.Text.Encoding.GetEncoding("CP932");
			byte [] temp = src.GetBytes(s);
			byte[] sjis_temp = System.Text.Encoding.Convert(src, dest, temp);
			string sjis_str = dest.GetString(sjis_temp);
            com.DirectSource =
                "S=「"+sjis_str+"」\n" +
                "SをSJIS_UTF8変換して継続表示";
            runner.Run(com.Codes);
            Assert.AreEqual("ほげほげふがふが埋め込み", sjis_str);
        }
		[Test]
        public void TestToUtf8()
        {//このテスト、ちゃんとSJISで関数に送られていないような気がする。
			string s = @"ほげほげふがふが埋め込み";
			System.Text.Encoding src = System.Text.Encoding.Unicode;
			System.Text.Encoding dest = System.Text.Encoding.GetEncoding("CP932");
			byte [] temp = src.GetBytes(s);
			byte[] sjis_temp = System.Text.Encoding.Convert(src, dest, temp);
			string sjis_str = dest.GetString(sjis_temp);
            com.DirectSource =
                "S=「"+sjis_str+"」\n" +
                "SをUTF8変換して継続表示";
            runner.Run(com.Codes);
            Assert.AreEqual("ほげほげふがふが埋め込み", sjis_str);
        }
    }
}
