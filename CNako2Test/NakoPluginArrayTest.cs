using System;
using System.Collections.Generic;
using System.Text;
using Libnako.Interpreter;
using Libnako.JPNCompiler;
using Libnako.JPNCompiler.ILWriter;
using Libnako.NakoAPI;
using NakoPlugin;
using NUnit.Framework;

namespace NakoPluginTest
{
    [TestFixture]
    public class TestNakoPluginArray
    {
        NakoCompiler com = new NakoCompiler();
        NakoInterpreter runner = new NakoInterpreter();
        [Test]
        public void TestLength()
        {
            runner.Run(com.WriteIL(
                "A=「」;" +
                "A[0]=「な」\n" +
                "A[1]=「で」\n" +
                "A[2]=「し」\n" +
                "A[3]=「こ」\n" +
                "Aの配列要素数を継続表示。"));
            Assert.AreEqual("4", runner.PrintLog);
        }
        [Test]
        public void TestLength2()
        {
            runner.Run(com.WriteIL(
                "「a,b,c」を「,」で区切る。\n" +
                "それの配列要素数を継続表示。"));
            Assert.AreEqual("3", runner.PrintLog);
        }

        [Test]
        public void TestLength3()
        {
            com.DirectSource =
                "B=「a,b,c」を「,」で区切る。\n" +
                "Bの要素数を継続表示。";
            runner.Run(com.Codes);
            Assert.AreEqual("3", runner.PrintLog);
            com.DirectSource =
                "S=「あ\nいう\nえお」\n" +
                "Sの要素数を継続表示。";
            runner.Run(com.Codes);
            Assert.AreEqual("3", runner.PrintLog);
            //TODO:現在このテストが通らない
            com.DirectSource =
                "A=「」\n" +
                "A[`na`]=「な」\n" +
                "A[`de`]=「で」\n" +
                "A[`si`]=「し」\n" +
                "A[`ko`]=「こ」\n" +
                "Aの要素数を継続表示。";
            runner.Run(com.Codes);
            Assert.AreEqual("4", runner.PrintLog);
            com.DirectSource =
                "d[`na`]=「な」\n" +
                "d[`de`]=「で」\n" +
                "d[`si`]=「し」\n" +
                "d[`ko`]=「こ」\n" +
                "d[`mu`]=「む」\n" +
                "dの要素数を継続表示。";
            runner.Run(com.Codes);
            Assert.AreEqual("5", runner.PrintLog);
        }
        [Test]
        public void TestSearch()
        {
            runner.Run(com.WriteIL(
                "A=「」;\n" +
                "A[0]=「な」\n" +
                "A[1]=「で」\n" +
                "A[2]=「し」\n" +
                "A[3]=「こ」\n" +
                "Aの0から「し」を配列検索を継続表示。"));
            Assert.AreEqual("2", runner.PrintLog);
        }
        [Test]
        public void TestReverse()
        {
            com.WriteIL(
                "A=「」;\n" +
                "A[0]=「な」\n" +
                "A[1]=「で」\n" +
                "A[2]=「し」\n" +
                "A[3]=「こ」\n" +
                "B=Aを配列逆順\n" +
                "A[0]を継続表示。");
            runner.Run(com.Codes);
            Assert.AreEqual("こ", runner.PrintLog);
        }
        [Test]
        public void TestConcat()
        {
            com.WriteIL(
                "A=「」;\n" +
                "A[0]=「な」\n" +
                "A[1]=「で」\n" +
                "A[2]=「し」\n" +
                "A[3]=「こ」\n" +
                "Aを「ん」で配列結合して継続表示。");
            runner.Run(com.Codes);
            Assert.AreEqual("なんでんしんこ", runner.PrintLog);
        }
        [Test]
        public void TestRemove()
        {
            runner.Run(com.WriteIL(
                "A=「」;\n" +
                "A[0]=「な」\n" +
                "A[1]=「で」\n" +
                "A[2]=「し」\n" +
                "A[3]=「こ」\n" +
                "Aの0を配列削除\n" +
                "Aの配列要素数を継続表示。"));
            Assert.AreEqual("3", runner.PrintLog);
        }
        //        [Test]
        //        public void TestInsert()
        //        {
        //            runner.Run(com.WriteIL("「なしこ」の1に「で」を文字挿入して表示。"));
        //            Assert.AreEqual("なでしこ", runner.PrintLog);
        //        }
        [Test]
        public void TestAppend()
        {
            runner.Run(com.WriteIL(
                "A=「」;\n" +
                "A[0]=「な」\n" +
                "A[1]=「で」\n" +
                "A[2]=「し」\n" +
                "A[3]=「こ」\n" +
                "Aに「む」を配列追加\n" +
                "A[4]を継続表示。"));
            Assert.AreEqual("む", runner.PrintLog);
        }
        [Test]
        public void TestPop()
        {
            runner.Run(com.WriteIL(
                "A=「」;\n" +
                "A[0]=「な」\n" +
                "A[1]=「で」\n" +
                "A[2]=「し」\n" +
                "A[3]=「こ」\n" +
                "Aに「む」を配列追加\n" +
                "B=Aを配列ポップ\n" +
                "Bを継続表示。\n" +
                "B=Aを配列ポップ\n" +
                "Bを継続表示。"));
            Assert.AreEqual("むこ", runner.PrintLog);
        }
        [Test]
        public void TestHashKeys()
        {
            runner.Run(com.WriteIL("Aとは変数;A[`a`]=30;A[`b`]=31;Aの配列ハッシュキー列挙。それを「,」で配列結合して継続表示。"));
            Assert.AreEqual("a,b", runner.PrintLog);
        }

        [Test]
        public void TestInsertArray()
        {
            NakoCompiler nc = new NakoCompiler();
            NakoInterpreter ni = new NakoInterpreter();
            nc.WriteIL(
                "I[0]=000\n" +
                "I[1]=333\n" +
                "I[2]=444\n" +
                "J=「111{~}222」\n" +
                "Iの1にJを配列一括挿入\n" +
                "Iの配列要素数を継続表示\n" +
                "「:」継続表示\n" +
                "I[3]を継続表示\n" +
                "");
            ni.Run(nc.Codes);
            Assert.AreEqual("5:333",ni.PrintLog);
        }
        [Test]
        public void TestHashKeysLoopAndAcessToValues()
        {
            runner.Run(com.WriteIL(
                "Aとは変数;\n"+
                "A[`a`]=30;\n"+
                "A[`b`]=31;\n"+
                "Aの配列ハッシュキー列挙して反復\n"+
                "   b=それ\n"+
                "   A[b]を継続表示。\n"+
                ""));
            Assert.AreEqual("3031", runner.PrintLog);
        }
    }
}
