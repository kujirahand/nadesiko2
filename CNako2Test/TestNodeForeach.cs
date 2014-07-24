using System;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;
using Libnako.JPNCompiler;
using Libnako.Interpreter;
using Libnako.Interpreter.ILCode;

namespace NakoPluginTest
{
    [TestFixture]
    public class TestNakoNodeForeach
    {
        NakoCompiler ns = new NakoCompiler();
        NakoInterpreter runner = new NakoInterpreter();
        NakoILCodeList codes = null;

        [Test]
        public void Test1_Normal()
        {
            // (1) 
            codes = ns.WriteIL(
				"A[0] = `aaa`\n"+
				"A[1] = `bbb`\n"+
				"A[2] = `ccc`\n"+
				"Aを反復\n"+
				"　　PRINT 「***{回数}:{対象}」"
                );
            runner.Run(codes);
            Assert.AreEqual(runner.PrintLog, "***1:aaa***2:bbb***3:ccc");
			codes = ns.WriteIL(
				"A[0] = `aaa`\n"+
				"A[1] = `bbb`\n"+
				"A[2] = `ccc`\n"+
				"Aを反復\n"+
				"　　PRINT 「***{回数}:{対象}」" +
				"　　続ける"
			);
			runner.Run(codes);
			Assert.AreEqual(runner.PrintLog, "***1:aaa***2:bbb***3:ccc");

        }
		[Test]
		public void TestHash()
		{
			// (1) 
			codes = ns.WriteIL(
				"A=``\n"+
				"A[`a`]=`aaa`\n"+
				"A[`b`]=`bbb`\n"+
				"A[`c`]=`ccc`\n"+
				"Aで反復\n"+
				"  「***{回数}:{対象}」を継続表示\n"+
				""
			);
			runner.Run(codes);
			Assert.AreEqual(runner.PrintLog, "***1:aaa***2:bbb***3:ccc");

		}
		[Test]
		public void TestArraykeys()
		{
			// (1) 
			// (1) 
			codes = ns.WriteIL(
				"A=``\n"+
				"A[`a`]=`aaa`\n"+
				"A[`b`]=`bbb`\n"+
				"A[`c`]=`ccc`\n"+
				"(Aの配列ハッシュキー列挙)で反復\n"+
				"  「***:{対象}」を継続表示\n"+
				""
			);
			runner.Run(codes);
			Assert.AreEqual(runner.PrintLog, "***1:a***2:b***3:c");
			codes = ns.WriteIL(
				"A=``\n"+
				"A[10]=`aaa`\n"+
				"A[1]=`bbb`\n"+
				"A[2]=`ccc`\n"+
				"(Aの配列ハッシュキー列挙)で反復\n"+
				"  「***:{対象}」を継続表示\n"+
				""
			);
			runner.Run(codes);
			Assert.AreEqual(runner.PrintLog, "***1:a***2:b***3:c");

		}
		[Test]
		public void TestCompareInsideLoop()
		{
			// (1) 
			codes = ns.WriteIL(
				"A=``\n"+
				"A[0] = `aaa`\n"+
				"A[1] = `bbb`\n"+
				"A[2] = `ccc`\n"+
				"Aを反復\n"+
				"  もし(対象 == `aaa`)なら\n"+
				"    PRINT 「***{回数}:{対象}」"
			);
			runner.Run(codes);
			Assert.AreEqual(runner.PrintLog, "***1:aaa");

		}
    }
}
