using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using NUnit.Framework;
using Libnako.JPNCompiler;
using Libnako.Interpreter;
using Libnako.JPNCompiler.ILWriter;
using Libnako.Interpreter.ILCode;

namespace NakoPluginTest
{
	[TestFixture]
	public class TestNakoInclude
	{
		NakoCompiler ns = new NakoCompiler();
		NakoInterpreter runner = new NakoInterpreter();
		NakoILCodeList codes = null;
		[SetUp]
		public void MakeIncludeFile()
		{
			string source = @"
a[`d`]=`ddd`
a[`e`]=`eee`
a[`f`]=`fff`
";
			File.WriteAllText (@"test.nako", source);
		}
		[TearDown]
		public void DeleteIncludeFile()
		{
			File.Delete (@"test.nako");
		}
		[Test]
		public void TestInclude()
		{
			// (1) 
			codes = ns.WriteIL(
				@"
`test.nako`を取り込む
a[`a`]=`aaa`
a[`b`]=`bbb`
a[`c`]=`ccc`
aで反復
  「***{回数}:{対象}」を継続表示
");
			runner.Run(codes);
			Assert.AreEqual(runner.PrintLog, "***1:ddd***2:eee***3:fff***4:aaa***5:bbb***6:ccc");

		}
	}
}
