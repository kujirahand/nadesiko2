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
            com.DirectSource = "1でエクセル起動。エクセル終了。";
            runner.Run(com.Codes);
            Assert.AreEqual(1, 1); // dummy
        }
    }
}
