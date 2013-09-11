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
        NakoCompiler compiler = new NakoCompiler();
        NakoInterpreter interpreter = new NakoInterpreter();
        [Test]
        public void TestLength()
        {
            interpreter.Run(compiler.WriteIL(
                "A=「」;" +
                "A[0]=「な」\n" +
                "A[1]=「で」\n" +
                "A[2]=「し」\n" +
                "A[3]=「こ」\n" +
                "Aの配列要素数を継続表示。"));
            Assert.AreEqual("4", interpreter.PrintLog);
        }
        [Test]
        public void TestLength2()
        {
            interpreter.Run(compiler.WriteIL(
                "「a,b,c」を「,」で区切る。\n" +
                "それの配列要素数を継続表示。"));
            Assert.AreEqual("3", interpreter.PrintLog);
        }
        [Test]
        public void TestSearch()
        {
            interpreter.Run(compiler.WriteIL(
                "A=「」;\n" +
                "A[0]=「な」\n" +
                "A[1]=「で」\n" +
                "A[2]=「し」\n" +
                "A[3]=「こ」\n" +
                "Aの0から「し」を配列検索を継続表示。"));
            Assert.AreEqual("2", interpreter.PrintLog);
        }
        [Test]
        public void TestReverse()
        {
            compiler.WriteIL(
                "A=「」;\n" +
                "A[0]=「な」\n" +
                "A[1]=「で」\n" +
                "A[2]=「し」\n" +
                "A[3]=「こ」\n" +
                "B=Aを配列逆順\n" +
                "A[0]を継続表示。");
            interpreter.Run(compiler.Codes);
            Assert.AreEqual("こ", interpreter.PrintLog);
        }
        [Test]
        public void TestConcat()
        {
            compiler.WriteIL(
                "A=「」;\n" +
                "A[0]=「な」\n" +
                "A[1]=「で」\n" +
                "A[2]=「し」\n" +
                "A[3]=「こ」\n" +
                "Aを「ん」で配列結合して継続表示。");
            interpreter.Run(compiler.Codes);
            Assert.AreEqual("なんでんしんこ", interpreter.PrintLog);
        }
        [Test]
        public void TestRemove()
        {
            interpreter.Run(compiler.WriteIL(
                "A=「」;\n" +
                "A[0]=「な」\n" +
                "A[1]=「で」\n" +
                "A[2]=「し」\n" +
                "A[3]=「こ」\n" +
                "Aの0を配列削除\n" +
                "Aの配列要素数を継続表示。"));
            Assert.AreEqual("3", interpreter.PrintLog);
        }
        //        [Test]
        //        public void TestInsert()
        //        {
        //            runner.Run(compiler.WriteIL("「なしこ」の1に「で」を文字挿入して表示。"));
        //            Assert.AreEqual("なでしこ", runner.PrintLog);
        //        }
        [Test]
        public void TestAppend()
        {
            interpreter.Run(compiler.WriteIL(
                "A=「」;\n" +
                "A[0]=「な」\n" +
                "A[1]=「で」\n" +
                "A[2]=「し」\n" +
                "A[3]=「こ」\n" +
                "Aに「む」を配列追加\n" +
                "A[4]を継続表示。"));
            Assert.AreEqual("む", interpreter.PrintLog);
        }
        [Test]
        public void TestPop()
        {
            interpreter.Run(compiler.WriteIL(
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
            Assert.AreEqual("むこ", interpreter.PrintLog);
        }
        [Test]
        public void TestHashKeys()
        {
            interpreter.Run(compiler.WriteIL("Aとは変数;A[`a`]=30;A[`b`]=31;Aの配列ハッシュキー列挙。それを「,」で配列結合して継続表示。"));
            Assert.AreEqual("a,b", interpreter.PrintLog);
        }
    }
}
