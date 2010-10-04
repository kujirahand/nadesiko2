/*
 * NUnit Test Case for NakoPluginCtrl
 * User: kujirahand
 * Date: 2010-10-04
 * Time: 2:27
 */
using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Libnako.JCompiler;
using Libnako.Interpreter;

using NakoPluginCtrl;

namespace TestNako
{
	/// <summary>
	/// Test for NakoPluginCtrl.
	/// </summary>
    [TestFixture]
	public class TestNakoPluginCtrl
	{
		NakoCompiler com = new NakoCompiler();
		NakoInterpreter runner = new NakoInterpreter();
		
		public TestNakoPluginCtrl()
		{
		}
		
		[Test]
		public void TestName() 
		{
			// DLLをテストするのに、リンクが必要なので、無理やり作った適当なメソッド
			string guid = NakoPluginCtrl.NakoPluginCtrl.getPluginGuid();
			Assert.AreEqual(guid, "44313FC9-22C5-457E-A523-96E4AA868BC0");
		}
		
		[Test]
		public void TestClipboad()
		{
		    /*
			com.DirectSource = 
				"「abc」をコピー。\n" +
				"クリップボードを表示。";
			runner.Run(com.Codes);
			Assert.AreEqual("abc", runner.PrintLog);
			*/
		}
	}
}
