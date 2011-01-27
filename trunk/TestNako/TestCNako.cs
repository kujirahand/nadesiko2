using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Libnako.JPNCompiler;
using Libnako.JPNCompiler.Tokenizer;

namespace TestNako
{
    [TestFixture]
    public class TestCNako
    {
        [Test(Description = "First Test")]
        public void test1()
        {
            NakoTokenDic dic = new NakoTokenDic();
            NakoReservedWord.Init(dic);
            if (dic.ContainsKey("hoge_")) {
                Assert.Fail("hoge has not element!");
            }
        }
    }
}
