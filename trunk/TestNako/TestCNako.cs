using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Libnako.JCompiler;
using Libnako.JCompiler.Tokenizer;

namespace TestNako
{
    [TestFixture]
    class TestCNako
    {
        [Test(Description = "First Test")]
        public void test1()
        {
            NakoDic dic = NakoDic.Instance;
            if (dic.ContainsKey("hoge_")) {
                Assert.Fail("hoge has not element!");
            }
        }
    }
}
