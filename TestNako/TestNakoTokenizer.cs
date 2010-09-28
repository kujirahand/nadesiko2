using System;
using System.Collections.Generic;
using System.Text;
using Libnako.JCompiler;
using NUnit.Framework;
using Libnako.JCompiler.Tokenizer;

namespace TestNako
{
    [TestFixture]
    class TestNakoTokenizer
    {
        [Test(Description="Tokenizer.indent")]
        public void TestCountIndent()
        {
            NakoTokenizer tok = new NakoTokenizer(null);
            NakoReservedWord.Init(tok.TokenDic);
            tok.Source = "    *";
            Assert.AreEqual(tok.CountIndent(), 4);
            tok.Source = "    ";
            Assert.AreEqual(tok.CountIndent(), 4);
            tok.Source = "\t\t*";
            Assert.AreEqual(tok.CountIndent(), 8);
            tok.Source = "\t\t";
            Assert.AreEqual(tok.CountIndent(), 8);
        }

        [Test(Description = "Tokenize method")]
        public void TestTokenize()
        {
            Boolean r;
            NakoTokenizer tok = new NakoTokenizer(null);
            NakoReservedWord.Init(tok.TokenDic);
            tok.Source = "から";
            Assert.IsTrue(tok.CompareStr("から"));
            // 2
            tok.Source = "(1+2)*3";
            tok.Tokenize();
            r = tok.Tokens.CheckTokenType(new NakoTokenType[]{
                NakoTokenType.PARENTHESES_L,
                NakoTokenType.INT,
                NakoTokenType.PLUS,
                NakoTokenType.INT,
                NakoTokenType.PARENTHESES_R,
                NakoTokenType.MUL,
                NakoTokenType.INT
            });
            Assert.IsTrue(r);

        }

        [Test(Description = "Tokenize method")]
        public void TestTokenize2()
        {
            // 1
            NakoTokenizer tok = new NakoTokenizer(null);
            NakoReservedWord.Init(tok.TokenDic);
            Boolean r;
            tok.Source = "1";
            tok.Tokenize();
            r = tok.CheckTokenType(new NakoTokenType[]{
                NakoTokenType.INT
            });
            Assert.IsTrue(r);
            // 2
            tok.Source = "1+2";
            tok.Tokenize();
            r = tok.CheckTokenType(new NakoTokenType[]{
                NakoTokenType.INT,
                NakoTokenType.PLUS,
                NakoTokenType.INT
            });
            Assert.IsTrue(r);
            // 3
            tok.Source = "1+2*3";
            tok.Tokenize();
            r = tok.CheckTokenType(new NakoTokenType[]{
                NakoTokenType.INT,
                NakoTokenType.PLUS,
                NakoTokenType.INT,
                NakoTokenType.MUL,
                NakoTokenType.INT
            });
            Assert.IsTrue(r);
        }

        [Test]
        public void TestBlockLevel()
        {
            NakoTokenizer tok = new NakoTokenizer(null);
            NakoReservedWord.Init(tok.TokenDic);
            Boolean r;
            //
            tok.Source =
                "もし、A=1ならば\n" +
                "  PRINT A\n" +
                "違えば\n" +
                "  PRINT A\n";
            tok.Tokenize();
            r = tok.CheckTokenType(new NakoTokenType[]{
                NakoTokenType.IF,
                NakoTokenType.WORD,
                NakoTokenType.EQ,
                NakoTokenType.INT,
                NakoTokenType.EOL,
                NakoTokenType.SCOPE_BEGIN,
                NakoTokenType.PRINT,
                NakoTokenType.WORD,
                NakoTokenType.EOL,
                NakoTokenType.SCOPE_END,
                NakoTokenType.ELSE,
                NakoTokenType.EOL,
                NakoTokenType.SCOPE_BEGIN,
                NakoTokenType.PRINT,
                NakoTokenType.WORD,
                NakoTokenType.EOL,
                NakoTokenType.SCOPE_END
            });
            Assert.IsTrue(r);

        }

        [Test(Description = "TestNakoHalfFlag")]
        public void TestNakoHalfFlag()
        {
            Assert.AreEqual('*', NakoHalfFlag.ConvertChar('●'));
            Assert.AreEqual('>', NakoHalfFlag.ConvertChar('＞'));
            Assert.AreEqual('#', NakoHalfFlag.ConvertChar('＃'));
            Assert.AreEqual('#', NakoHalfFlag.ConvertChar('※'));
            Assert.AreEqual('#', NakoHalfFlag.ConvertChar('#'));
        }

        [Test(Description = "Josi")]
        public void TestNakoJosi()
        {
            NakoJosi josiList = NakoJosi.Instance;
            String first_s = josiList[0];
            String last_s = josiList[josiList.Count - 1];
            Assert.IsTrue(first_s.Length > last_s.Length);
        }

        [Test]
        public void TestGetToken_Number()
        {
            // 1
            NakoTokenizer tok = new NakoTokenizer("1234");
            NakoToken t = tok.GetToken_Number();
            Assert.AreEqual(NakoTokenType.INT, t.type);
            Assert.AreEqual("1234", t.value);
            // 2
            tok.Source = "12.3456";
            t = tok.GetToken_Number();
            Assert.AreEqual(NakoTokenType.NUMBER, t.type);
            Assert.AreEqual("12.3456", t.value);
            // 3
            tok.Source = "0.123";
            t = tok.GetToken_Number();
            Assert.AreEqual(NakoTokenType.NUMBER, t.type);
            Assert.AreEqual("0.123", t.value);
            // 4
            tok.Source = "32から";
            t = tok.GetToken_Number();
            Assert.AreEqual(NakoTokenType.INT, t.type);
            Assert.AreEqual("32", t.value);
            Assert.AreEqual("から", t.josi);
        }

        [Test]
        public void TestGetToken_Word()
        {
            // 1
            NakoTokenizer tok = new NakoTokenizer("ABC");
            NakoToken t = tok.GetToken_Word();
            Assert.AreEqual(NakoTokenType.WORD, t.type);
            Assert.AreEqual("ABC", t.value);
            // 2
            tok.Source = "豆腐から";
            t = tok.GetToken_Word();
            Assert.AreEqual(NakoTokenType.WORD, t.type);
            Assert.AreEqual("豆腐", t.value);
            Assert.AreEqual("から", t.josi);
            // 3
            tok.Source = "F_豆腐から";
            t = tok.GetToken_Word();
            Assert.AreEqual(NakoTokenType.WORD, t.type);
            Assert.AreEqual("F_豆腐", t.value);
            Assert.AreEqual("から", t.josi);
            // 4
            tok.Source = "F123から";
            t = tok.GetToken_Word();
            Assert.AreEqual(NakoTokenType.WORD, t.type);
            Assert.AreEqual("F123", t.value);
            Assert.AreEqual("から", t.josi);
            // 5
            tok.Source = "__から";
            t = tok.GetToken_Word();
            Assert.AreEqual(NakoTokenType.WORD, t.type);
            Assert.AreEqual("__", t.value);
            Assert.AreEqual("から", t.josi);
            // 6
            tok.Source = "AからBへファイルコピー";
            tok.Tokenize();
            Boolean r = tok.CheckTokenType(new NakoTokenType[]{
                NakoTokenType.WORD,
                NakoTokenType.WORD,
                NakoTokenType.WORD
            });
            Assert.IsTrue(r);
            t = tok.Tokens[0];
            Assert.AreEqual("A", t.value);
            Assert.AreEqual("から", t.josi);
            t = tok.Tokens[1];
            Assert.AreEqual("B", t.value);
            Assert.AreEqual("へ", t.josi);
            t = tok.Tokens[2];
            Assert.AreEqual("ファイルコピー", t.value);
            Assert.AreEqual("", t.josi);

        }

        [Test]
        public void TestGetToken_String()
        {
            // 1
            NakoTokenizer tok = new NakoTokenizer("「ABC」");
            NakoToken t = tok.GetToken_String();
            Assert.AreEqual(NakoTokenType.STRING_EX, t.type);
            Assert.AreEqual("ABC", t.value);
            Assert.AreEqual("", t.josi);
            // 2
            tok.Source = "`豆腐`から";
            t = tok.GetToken_String();
            Assert.AreEqual(NakoTokenType.STRING, t.type);
            Assert.AreEqual("豆腐", t.value);
            Assert.AreEqual("から", t.josi);
            // 3
            tok.Source = "「「豆腐」」から";
            t = tok.GetToken_String();
            Assert.AreEqual(NakoTokenType.STRING_EX, t.type);
            Assert.AreEqual("豆腐", t.value);
            Assert.AreEqual("から", t.josi);
            // 4
            tok.Source = "『『『F123』』』へ飛ぶ";
            t = tok.GetToken_String();
            Assert.AreEqual(NakoTokenType.STRING, t.type);
            Assert.AreEqual("F123", t.value);
            Assert.AreEqual("へ", t.josi);
            // 5
            tok.Source = "「aaa\nbbb\nccc」から「豆腐」へ";
            t = tok.GetToken_String();
            Assert.AreEqual(NakoTokenType.STRING_EX, t.type);
            Assert.AreEqual("aaa\nbbb\nccc", t.value);
            Assert.AreEqual("から", t.josi);
            t = tok.GetToken_String();
            Assert.AreEqual(NakoTokenType.STRING_EX, t.type);
            Assert.AreEqual("豆腐", t.value);
            Assert.AreEqual("へ", t.josi);
            Assert.AreEqual(2, t.lineno);
            // 6
            tok.Source = "`abc\tabc`";
            t = tok.GetToken_String();
            Assert.AreEqual(NakoTokenType.STRING, t.type);
            Assert.AreEqual("abc\tabc", t.value);
            Assert.AreEqual("", t.josi);
        }

        [Test]
        public void TestGetToken_String_ex()
        {
            // 1
            NakoTokenizer tok = new NakoTokenizer("「ABC」");
            tok.Tokenize();
            Boolean r = tok.CheckTokenType(new NakoTokenType[] { NakoTokenType.STRING });
            Assert.IsTrue(r);
            // 2
            tok.Source = "「ABC{123}」";
            tok.Tokenize();
            r = tok.CheckTokenType(new NakoTokenType[] {
                NakoTokenType.STRING,
                NakoTokenType.AND,
                NakoTokenType.INT
            });
            Assert.IsTrue(r);
            // 3
            tok.Source = "「ABC{`123`}」";
            tok.Tokenize();
            r = tok.CheckTokenType(new NakoTokenType[] {
                NakoTokenType.STRING,
                NakoTokenType.AND,
                NakoTokenType.STRING
            });
            Assert.IsTrue(r);
            //4
            tok.Source = "「「ABC{`123`&123}」」";
            tok.Tokenize();
            r = tok.CheckTokenType(new NakoTokenType[] {
                NakoTokenType.STRING,
                NakoTokenType.AND,
                NakoTokenType.STRING,
                NakoTokenType.AND,
                NakoTokenType.INT
            });
            Assert.IsTrue(r);
        }
    }
}
