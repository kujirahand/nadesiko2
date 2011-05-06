using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;

using Libnako.JPNCompiler;
using Libnako.Interpreter;
using Libnako.NakoAPI;

using NakoPluginOfficePowerPoint;

namespace NakoPluginOfficeTest
{
    [TestFixture]
    public class NakoPluginOfficePowerPointTest
    {
        NakoCompiler com;
        NakoInterpreter runner;

        public NakoPluginOfficePowerPointTest()
        {
            NakoCompilerLoaderInfo info = new NakoCompilerLoaderInfo();
            info.PreloadModules = new NakoPlugin.INakoPlugin[] {
                new NakoBaseSystem(),
                new NakoPluginOfficePowerPoint.NakoPluginOfficePowerPoint()
            };
            com = new NakoCompiler(info);
            runner = new NakoInterpreter();
        }
        [Test]
        public void PowerPointTest()
        {
            com.DirectSource =
                "0でパワポ起動。パワポ終了。\n";
            runner.Run(com.Codes);
            Assert.AreEqual(1,1); // dummy
        }
    }
}
