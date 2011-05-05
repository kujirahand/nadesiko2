using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;

using Libnako.JPNCompiler;
using Libnako.Interpreter;
using Libnako.NakoAPI;

using NakoPluginOfficeExcel;

namespace NakoPluginOfficeTest
{
    [TestFixture]
    public class NakoPluginOfficeExcelTest
    {
        NakoCompiler com;
        NakoInterpreter runner;

        public NakoPluginOfficeExcelTest()
        {
            NakoCompilerLoaderInfo info = new NakoCompilerLoaderInfo();
            info.PreloadModules = new NakoPlugin.INakoPlugin[] {
                new NakoBaseSystem(),
                new NakoPluginOfficeExcel.NakoPluginOfficeExcel()
            };
            com = new NakoCompiler(info);
            runner = new NakoInterpreter();
        }

        [Test]
        public void ExcelStart()
        {
            com.DirectSource = "0でエクセル起動。エクセル終了。";
            runner.Run(com.Codes);
            Assert.AreEqual(1, 1); // dummy
        }

        [Test]
        public void ExcelVersion()
        {
            com.DirectSource = "0でエクセル起動。PRINT(エクセルバージョン)。エクセル終了。";
            runner.Run(com.Codes);
            Assert.AreNotEqual("-1", runner.PrintLog);
        }

        [Test]
        public void ExcelTitle()
        {
            com.DirectSource = "0でエクセル起動。エクセル新規ブック。「わんわん」にエクセルタイトル変更。エクセルタイトル状態を表示。エクセル終了。";
            runner.Run(com.Codes);
            Assert.AreNotEqual("", runner.PrintLog);
        }

        [Test]
        public void ExcelSetCell()
        {
            com.DirectSource = "0でエクセル起動。「C3」に「ABC」をエクセルセル設定。「C3」のエクセルセル取得。それを表示。";
            runner.Run(com.Codes);
            Assert.AreEqual("ABC", runner.PrintLog);
        }

        [Test]
        public void ExcelSetCell2()
        {
            com.DirectSource = "0でエクセル起動。「C3」に「いろは」をエクセルセル設定。「C3」のエクセルセル取得。それを表示。";
            runner.Run(com.Codes);
            Assert.AreEqual("いろは", runner.PrintLog);
        }

        [Test]
        public void ExcelFileOpen()
        {
            string tmp = System.IO.Path.GetTempPath();
            com.DirectSource =
                "TMP=「"+tmp+"test.xlsx」。\n" +
                "0でエクセル起動。「A1」に「ABC」をエクセルセル設定。TMPへエクセル保存。エクセル終了。\n" +
                "0でエクセル起動。TMPのエクセル開く。「A1」のエクセルセル取得。それを表示。\n";
            runner.Run(com.Codes);
            Assert.AreEqual("ABC", runner.PrintLog);
        }
    }
}
