using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Libnako.Parser;

namespace TestNako
{
    [TestFixture]
    class TestCNako
    {
        [Test(Description = "First Test")]
        public void test1()
        {
            NakoDic dic = NakoDic.GetInstance();
            if (dic.ContainsKey("hoge_")) {
                Assert.Fail("hoge has not element!");
            }
        }
    }
}
