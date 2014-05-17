using System;
using System.Collections.Generic;
using System.Text;
using Libnako.Interpreter;
using Libnako.JPNCompiler;
using Libnako.JPNCompiler.ILWriter;
using Libnako.NakoAPI;
using NUnit.Framework;

namespace NakoPluginTest
{
    [TestFixture]
    public class TestNakoPluginString
    {
        NakoCompiler compiler = new NakoCompiler();
        NakoInterpreter interpreter = new NakoInterpreter();
        [Test]
        public void TestLength()
        {
            interpreter.Run(compiler.WriteIL(
                "「なでしこ」の文字数を継続表示。"));
            Assert.AreEqual(interpreter.PrintLog, "4");
        }
        [Test]
        public void TestSearch()
        {
            interpreter.Run(compiler.WriteIL(
                "「なでしこ」の「し」を文字列検索を継続表示。"));
            Assert.AreEqual(interpreter.PrintLog, "3");
        }
        [Test]
        public void TestReplace()
        {
            interpreter.Run(compiler.WriteIL(
                "「なでしこなでなで」の「なで」を「えら」に置換を継続表示。"));
            Assert.AreEqual(interpreter.PrintLog, "えらしこえらえら");
        }
        [Test]
        public void TestReplaceA()
        {
            interpreter.Run(compiler.WriteIL(
                "「なでしこなでなで」の「なで」を「えら」に単置換を継続表示。"));
            Assert.AreEqual(interpreter.PrintLog, "えらしこなでなで");
        }
        [Test]
        public void TestLeft()
        {
            interpreter.Run(compiler.WriteIL(
                "「なでしこ」の2文字左部分を継続表示。"));
            Assert.AreEqual(interpreter.PrintLog, "なで");
        }
        [Test]
        public void TestTrim()
        {
            interpreter.Run(compiler.WriteIL(
                "「　　なでしこ　　」をトリムを継続表示。"));
            Assert.AreEqual(interpreter.PrintLog, "なでしこ");
        }
        [Test]
        public void TestRight()
        {
            interpreter.Run(compiler.WriteIL(
                "「なでしこ」2文字右部分を継続表示。"));
            Assert.AreEqual(interpreter.PrintLog, "しこ");
        }
        [Test]
        public void TestCut()
        {
            //            interpreter.Run(compiler.WriteIL(
            //                "「なでしこ」から「し」まで切り取って表示。"));
            //            Assert.AreEqual("なで", interpreter.PrintLog);
            interpreter.Run(compiler.WriteIL(
                "S=「なでしこ」\n" +
                "Sから「し」まで切り取る\n" +
                "Sを継続表示。"));
            Assert.AreEqual("こ", interpreter.PrintLog);
        }
        [Test]
        public void TestExtract()
        {
            interpreter.Run(compiler.WriteIL(
                "「なでしこ」の1から2文字抜き出して継続表示。"));
            Assert.AreEqual(interpreter.PrintLog, "なで");
        }
        [Test]
        public void TestEm()
        {
            interpreter.Run(compiler.WriteIL(
                "「なでしこ」の全角か判定を継続表示。"));
            Assert.AreEqual(interpreter.PrintLog, "1");
        }
        [Test]
        public void TestRemove()
        {
            interpreter.Run(compiler.WriteIL(
                "S=「なでしこ」\n" +
                "Sの2から1文字削除\n" +
                "Sを継続表示。"));
            Assert.AreEqual("なしこ", interpreter.PrintLog);
        }
        [Test]
        public void TestRemoveRight()
        {
            interpreter.Run(compiler.WriteIL(
                "S=「なでしこ」\n" +
                "Sから1文字右端削除\n" +
                "Sを継続表示。"));
            Assert.AreEqual("なでし", interpreter.PrintLog);
        }
        [Test]
        public void TestInsert()
        {
            interpreter.Run(compiler.WriteIL(
                "「なしこ」の2に「で」を文字挿入して継続表示。"));
            Assert.AreEqual("なでしこ", interpreter.PrintLog);
        }
        [Test]
        public void TestDegrade()
        {
            interpreter.Run(compiler.WriteIL(
                "S=「なでしこ」を文字列分解\n" +
                "S[0]を継続表示。"));
            Assert.AreEqual("な", interpreter.PrintLog);
        }
        [Test]
        public void TestExplode()
        {
            interpreter.Run(compiler.WriteIL(
                "S=「なでしこ」を「し」で区切る\n" +
                "S[0]を継続表示。"));
            Assert.AreEqual("なで", interpreter.PrintLog);
        }
        [Test]
        public void TestNum()
        {
            interpreter.Run(compiler.WriteIL(
                "「0なでしこ」が数字か判定を継続表示。"));
            Assert.AreEqual("1", interpreter.PrintLog);
        }
        [Test]
        public void TestAppend()
        {
            interpreter.Run(compiler.WriteIL(
                "S=「なでし」\n"+
				"Sに「こ」を追加\n" +
				"Sを継続表示。"));
            Assert.AreEqual("なでしこ", interpreter.PrintLog);
        }
        [Test]
        public void TestAlnumToEn()
        {
            interpreter.Run(compiler.WriteIL(
                "S=「アイウエオ０１２３４５６７８９ａｂｃＡＢＣ」\n" +
                "Sを英数半角変換して継続表示"));
            Assert.AreEqual("アイウエオ0123456789abcABC", interpreter.PrintLog);
        }
        [Test]
        public void TestToEn()
        {
            interpreter.Run(compiler.WriteIL(
                "S=「アイウエオ０１２３４５６７８９ａｂｃＡＢＣ」\n" +
                "S=Sを半角変換して継続表示"));
            Assert.AreEqual("ｱｲｳｴｵ0123456789abcABC", interpreter.PrintLog);
        }
        public void TestToUpper()
        {
            interpreter.Run(compiler.WriteIL(
                "S=「アイウエオ０１２３４５６７８９ａｂｃＡＢＣabcABC」\n" +
                "S=Sを小文字変換して継続表示"));
            Assert.AreEqual("アイウエオ０１２３４５６７８９ａｂｃａｂｃabcabc", interpreter.PrintLog);
        }
        public void TestToLower()
        {
            interpreter.Run(compiler.WriteIL(
                "S=「アイウエオ０１２３４５６７８９ａｂｃＡＢＣabcABC」\n" +
                "S=Sを大文字変換して継続表示"));
            Assert.AreEqual("アイウエオ０１２３４５６７８９ＡＢＣＡＢＣABCABC", interpreter.PrintLog);
        }
        [Test]
        public void TestZerofill()
        {
            interpreter.Run(compiler.WriteIL(
                "S=「10」\n" +
                "S=Sを4でゼロ埋め\n" +
                "Sを継続表示"));
            Assert.AreEqual("0010", interpreter.PrintLog);
        }
        [Test]
        public void Test_strpos()
        {
            interpreter.Run(compiler.WriteIL(
                "`abc`で`b`が何文字目\n" +
                "それを継続表示\n" +
                ""));
            Assert.AreEqual("2", interpreter.PrintLog);
        }
        [Test]
        public void Test_strpos2()
        {
            interpreter.Run(compiler.WriteIL(
                "`abc`で`d`が何文字目\n" +
                "それを継続表示\n" +
                ""));
            Assert.AreEqual("0", interpreter.PrintLog);
            //
            ;
            interpreter.Run(compiler.WriteIL(
                "`あいう`で`う`が何文字目\n" +
                "それを継続表示\n" +
                ""));
            Assert.AreEqual("3", interpreter.PrintLog);
            //
            interpreter.Run(compiler.WriteIL(
                "12345で4が何文字目\n" +
                "それを継続表示\n" +
                ""));
            Assert.AreEqual("4", interpreter.PrintLog);
        }
        [Test]
        public void Test_occurrence()
        {
            NakoCompiler nc = new NakoCompiler();
            NakoInterpreter ni = new NakoInterpreter();
            ni.Run(nc.WriteIL(
                "`abcdabcabdcab`で`ab`の出現回数\n" +
                "それを継続表示\n" +
                ""));
            Assert.AreEqual("4", ni.PrintLog);
            ni.Run(nc.WriteIL(
                "`ほげふがほbcaふがげふがほふげがふがほ`で`ふが`の出現回数\n" +
                "それを継続表示\n" +
                ""));
            Assert.AreEqual("4", ni.PrintLog);
        }
		
		[Test]
        public void TestKanaToRoman()
        {
			//TODO:なでしこ1の実装では英数が消えてしまうんだけどどうしようかな
			//Linuxでは失敗
            interpreter.Run(compiler.WriteIL(
                "S=「アイウエオ０１２３４５６７８９ａｂｃＡＢＣ」\n" +
                "Sをカナローマ字変換して継続表示"));
            Assert.AreEqual("aiueo０１２３４５６７８９ａｂｃＡＢＣ", interpreter.PrintLog);
        }
        [Test]
        public void TestToKana()
        {
         string s = @"ホゲホゲﾌｶﾞﾌｶﾞ埋め込み";
            interpreter.Run(compiler.WriteIL(
                "S=「"+s+"」\n" +
                "Sをかな変換して継続表示"));
            Assert.AreEqual("ほげほげふがふが埋め込み", interpreter.PrintLog);
        }
        [Test]
        public void TestCutRange()
        {
            interpreter.Run(compiler.WriteIL(
                "S=「なでしこなでなで」\n" +
                "A=Sの「で」から「で」を範囲切り取る\n" +
                "Sを継続表示。\nAを継続表示"));
            Assert.AreEqual("ななでしこな", interpreter.PrintLog);
        }
    }
}
