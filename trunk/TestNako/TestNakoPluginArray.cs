using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Libnako.JPNCompiler;
using Libnako.Interpreter;
using Libnako.NakoAPI;

using Libnako.JPNCompiler.ILWriter;

using NakoPluginArray;
using NakoPluginSample;

namespace TestNako
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
                new NakoPluginArray.NakoPluginArray(),
                new NakoPluginSample.NakoPluginSample()
            };
            com = new NakoCompiler(info);
        }

        [Test]
        public void TestLength()
        {
            com.DirectSource = 
                "A[0]=「な」\n" +
                "A[1]=「で」\n" +
                "A[2]=「し」\n" +
                "A[3]=「こ」\n" +
                "Aの配列要素数を表示。";
            runner.Run(com.Codes);
            Assert.AreEqual("4",runner.PrintLog);
        }

        [Test]
        public void TestSearch()
        {
            com.DirectSource = 
                "A[0]=「な」\n" +
                "A[1]=「で」\n" +
                "A[2]=「し」\n" +
                "A[3]=「こ」\n" +
                "Aの0から「し」を配列検索を表示。";
            runner.Run(com.Codes);
            Assert.AreEqual("2",runner.PrintLog);
        }

        [Test]
        public void TestReverse()
        {
            com.DirectSource = 
                "A[0]=「な」\n" +
                "A[1]=「で」\n" +
                "A[2]=「し」\n" +
                "A[3]=「こ」\n" +
                "B=Aを配列逆順\n"+
                "A[0]を表示。";
            runner.Run(com.Codes);
            Assert.AreEqual("こ",runner.PrintLog);
        }

        [Test]
        public void TestConcat()
        {
            com.DirectSource = 
                "A[0]=「な」\n" +
                "A[1]=「で」\n" +
                "A[2]=「し」\n" +
                "A[3]=「こ」\n" +
                "Aを「ん」配列結合して表示。";
            runner.Run(com.Codes);
            Assert.AreEqual("なんでんしんこ", runner.PrintLog);
        }

        [Test]
        public void TestRemove()
        {
            com.DirectSource = 
                "A[0]=「な」\n" +
                "A[1]=「で」\n" +
                "A[2]=「し」\n" +
                "A[3]=「こ」\n" +
                "Aの0を配列削除\n" +
                "Aの配列要素数を表示。";
            runner.Run(com.Codes);
            Assert.AreEqual("3",runner.PrintLog );
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
                "A[0]=「な」\n" +
                "A[1]=「で」\n" +
                "A[2]=「し」\n" +
                "A[3]=「こ」\n" +
                "Aに「む」を配列追加\n" +
                "A[4]を表示。";
            runner.Run(com.Codes);
            Assert.AreEqual("む",runner.PrintLog);
        }
    }
}
