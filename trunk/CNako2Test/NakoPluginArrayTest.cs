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
    public class TestNakoPluginArray
    {
        NakoCompiler com;
        NakoInterpreter runner = new NakoInterpreter();

        public TestNakoPluginArray()
        {
            NakoCompilerLoaderInfo info = new NakoCompilerLoaderInfo();
            info.PreloadModules = new NakoPlugin.INakoPlugin[] {
                new NakoBaseSystem(),
                new NakoPluginArray(),
                new NakoPluginString()
            };
            com = new NakoCompiler(info);
        }

        [Test]
        public void TestLength()
        {
            com.DirectSource =
                "A=「」;" +
                "A[0]=「な」\n" +
                "A[1]=「で」\n" +
                "A[2]=「し」\n" +
                "A[3]=「こ」\n" +
                "Aの配列要素数を継続表示。";
            runner.Run(com.Codes);
            Assert.AreEqual("4", runner.PrintLog);
        }

        [Test]
        public void TestLength2()
        {
            com.DirectSource =
                "「a,b,c」を「,」で区切る。\n" +
                "それの配列要素数を継続表示。";
            runner.Run(com.Codes);
            Assert.AreEqual("3", runner.PrintLog);
        }

        [Test]
        public void TestSearch()
        {
            com.DirectSource =
                "A=「」;\n" +
                "A[0]=「な」\n" +
                "A[1]=「で」\n" +
                "A[2]=「し」\n" +
                "A[3]=「こ」\n" +
                "Aの0から「し」を配列検索を継続表示。";
            runner.Run(com.Codes);
            Assert.AreEqual("2", runner.PrintLog);
        }

        [Test]
        public void TestReverse()
        {
            com.DirectSource =
                "A=「」;\n" +
                "A[0]=「な」\n" +
                "A[1]=「で」\n" +
                "A[2]=「し」\n" +
                "A[3]=「こ」\n" +
                "B=Aを配列逆順\n" +
                "A[0]を継続表示。";
            runner.Run(com.Codes);
            Assert.AreEqual("こ", runner.PrintLog);
        }

        [Test]
        public void TestConcat()
        {
            com.DirectSource =
                "A=「」;\n" +
                "A[0]=「な」\n" +
                "A[1]=「で」\n" +
                "A[2]=「し」\n" +
                "A[3]=「こ」\n" +
                "Aを「ん」で配列結合して継続表示。";
            runner.Run(com.Codes);
            Assert.AreEqual("なんでんしんこ", runner.PrintLog);
        }

        [Test]
        public void TestRemove()
        {
            com.DirectSource =
                "A=「」;\n" +
                "A[0]=「な」\n" +
                "A[1]=「で」\n" +
                "A[2]=「し」\n" +
                "A[3]=「こ」\n" +
                "Aの0を配列削除\n" +
                "Aの配列要素数を継続表示。";
            runner.Run(com.Codes);
            Assert.AreEqual("3", runner.PrintLog);
        }
        //        [Test]
        //        public void TestInsert()
        //        {
        //            com.DirectSource = 
        //                "「なしこ」の1に「で」を文字挿入して表示。";
        //            runner.Run(com.Codes);
        //            Assert.AreEqual("なでしこ", runner.PrintLog);
        //        }
        [Test]
        public void TestAppend()
        {
            com.DirectSource =
                "A=「」;\n" +
                "A[0]=「な」\n" +
                "A[1]=「で」\n" +
                "A[2]=「し」\n" +
                "A[3]=「こ」\n" +
                "Aに「む」を配列追加\n" +
                "A[4]を継続表示。";
            runner.Run(com.Codes);
            Assert.AreEqual("む", runner.PrintLog);
        }
        [Test]
        public void TestHashKeys()
        {
            com.DirectSource =
                "Aとは変数;A[`a`]=30;A[`b`]=31;Aの配列ハッシュキー列挙。それを「,」で配列結合して継続表示。";
            runner.Run(com.Codes);
            Assert.AreEqual("a,b", runner.PrintLog);
        }
    }
}
