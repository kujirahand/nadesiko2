using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Libnako.JPNCompiler;
using Libnako.Interpreter;
using Libnako.NakoAPI;

using Libnako.JPNCompiler.ILWriter;

using NakoPluginEval;

namespace NakoPluginTest
{
	[TestFixture]
	public class TestNakoPluginEval
	{
		NakoCompiler com;
		NakoInterpreter runner = new NakoInterpreter();

		public TestNakoPluginEval()
		{
			NakoCompilerLoaderInfo info = new NakoCompilerLoaderInfo();
			info.PreloadModules = new NakoPlugin.INakoPlugin[] {
				new NakoBaseSystem(),
				new NakoPluginTag.NakoPluginTag()
			};
			com = new NakoCompiler(info);
		}

		[Test]
		public void TestEval()
		{
			runner.Run(com.WriteIL( 
				"S=「それ=1」をNadesiko\n" +
				"Sを表示。"));
			Assert.AreEqual("1",runner.PrintLog);
		}
	}
}
