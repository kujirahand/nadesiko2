using System;
using System.Collections.Generic;
using System.Text;
using Libnako.JPNCompiler;
using NUnit.Framework;
using Libnako.JPNCompiler.Tokenizer;

namespace NakoPluginTest
{
    [TestFixture(Description = "なでしこソースをトークン化するテスト")]
    public class TestNakoTokenization
    {
        [Test(Description = "トークン化するテスト")]
        public void TestTokenize()
        {
            // 1番目のテスト
            Assert.IsTrue(
                NakoTokenization.Tokenize("1").CheckTokenType(new NakoTokenType[]{
                    NakoTokenType.INT
                }));
            // 2番目のテスト
            Assert.IsTrue(
                NakoTokenization.Tokenize("1+2").CheckTokenType(new NakoTokenType[]{
                    NakoTokenType.INT,
                    NakoTokenType.PLUS,
                    NakoTokenType.INT
                }));
            // 3番目のテスト
            Assert.IsTrue(
                NakoTokenization.Tokenize("1+2*3").CheckTokenType(new NakoTokenType[]{
                    NakoTokenType.INT,
                    NakoTokenType.PLUS,
                    NakoTokenType.INT,
                    NakoTokenType.MUL,
                    NakoTokenType.INT
                }));
            // 4番目のテスト
            Assert.IsTrue(
                NakoTokenization.Tokenize("(1+2)*3").CheckTokenType(new NakoTokenType[]{
                    NakoTokenType.PARENTHESES_L,
                    NakoTokenType.INT,
                    NakoTokenType.PLUS,
                    NakoTokenType.INT,
                    NakoTokenType.PARENTHESES_R,
                    NakoTokenType.MUL,
                    NakoTokenType.INT
                }));
        }
        [Test(Description = "ブロックレベルのテスト")]
        public void TestBlockLevel()
        {
            var tokenDic = new NakoTokenDic();
            NakoReservedWord.Init(tokenDic);
            Assert.IsTrue(
                NakoTokenization.Tokenize(
@"もし、A=1ならば
  PRINT A
違えば
  PRINT A
", tokenDic)
                .CheckTokenType(new NakoTokenType[]{
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
                }));
        }
        [Test(Description = "文字列トークンを取得するテスト")]
        public void TestGetStringToken()
        {
            NakoToken token;
            // 1番目のテスト
            token = NakoTokenization.Tokenize("「ABC」")[0];
            Assert.AreEqual(NakoTokenType.STRING, token.Type);
            Assert.AreEqual("ABC", token.Value);
            Assert.AreEqual("", token.Josi);
            // 2番目のテスト
            token = NakoTokenization.Tokenize("`豆腐`から")[0];
            Assert.AreEqual(NakoTokenType.STRING, token.Type);
            Assert.AreEqual("豆腐", token.Value);
            Assert.AreEqual("から", token.Josi);
            // 3番目のテスト
            token = NakoTokenization.Tokenize("「「豆腐」」から")[0];
            Assert.AreEqual(NakoTokenType.STRING, token.Type);
            Assert.AreEqual("豆腐", token.Value);
            Assert.AreEqual("から", token.Josi);
            // 4番目のテスト
            token = NakoTokenization.Tokenize("『『『F123』』』へ飛ぶ")[0];
            Assert.AreEqual(NakoTokenType.STRING, token.Type);
            Assert.AreEqual("F123", token.Value);
            Assert.AreEqual("へ", token.Josi);
            // 5番目のテスト
            var tokens = NakoTokenization.Tokenize("「aaa\nbbb\nccc」から「豆腐」へ");
            Assert.AreEqual(NakoTokenType.STRING, tokens[0].Type);
            Assert.AreEqual("aaa\nbbb\nccc", tokens[0].Value);
            Assert.AreEqual("から", tokens[0].Josi);
            Assert.AreEqual(NakoTokenType.STRING, tokens[1].Type);
            Assert.AreEqual("豆腐", tokens[1].Value);
            Assert.AreEqual("へ", tokens[1].Josi);
            Assert.AreEqual(2, tokens[1].LineNo);
            // 6番目のテスト
            token = NakoTokenization.Tokenize("`abc\tabc`")[0];
            Assert.AreEqual(NakoTokenType.STRING, token.Type);
            Assert.AreEqual("abc\tabc", token.Value);
            Assert.AreEqual("", token.Josi);
        }
        [Test(Description = "数値トークンを取得するテスト")]
        public void TestGetNumberToken()
        {
            NakoToken token;
            // 1番目のテスト
            token = NakoTokenization.Tokenize("1234")[0];
            Assert.AreEqual(NakoTokenType.INT, token.Type);
            Assert.AreEqual("1234", token.Value);
            // 2番目のテスト
            token = NakoTokenization.Tokenize("12.3456")[0];
            Assert.AreEqual(NakoTokenType.NUMBER, token.Type);
            Assert.AreEqual("12.3456", token.Value);
            // 3番目のテスト
            token = NakoTokenization.Tokenize("0.123")[0];
            Assert.AreEqual(NakoTokenType.NUMBER, token.Type);
            Assert.AreEqual("0.123", token.Value);
            // 4番目のテスト
            token = NakoTokenization.Tokenize("32から")[0];
            Assert.AreEqual(NakoTokenType.INT, token.Type);
            Assert.AreEqual("32", token.Value);
            Assert.AreEqual("から", token.Josi);
        }
        [Test(Description = "単語トークンを取得するテスト")]
        public void TestGetWordToken()
        {
            NakoToken token;
            // 1番目のテスト
            token = NakoTokenization.Tokenize("ABC")[0];
            Assert.AreEqual(NakoTokenType.WORD, token.Type);
            Assert.AreEqual("ABC", token.Value);
            // 2番目のテスト
            token = NakoTokenization.Tokenize("豆腐から")[0];
            Assert.AreEqual(NakoTokenType.WORD, token.Type);
            Assert.AreEqual("豆腐", token.Value);
            Assert.AreEqual("から", token.Josi);
            // 3番目のテスト
            token = NakoTokenization.Tokenize("F_豆腐から")[0];
            Assert.AreEqual(NakoTokenType.WORD, token.Type);
            Assert.AreEqual("F_豆腐", token.Value);
            Assert.AreEqual("から", token.Josi);
            // 4番目のテスト
            token = NakoTokenization.Tokenize("F123から")[0];
            Assert.AreEqual(NakoTokenType.WORD, token.Type);
            Assert.AreEqual("F123", token.Value);
            Assert.AreEqual("から", token.Josi);
            // 5番目のテスト
            token = NakoTokenization.Tokenize("__から")[0];
            Assert.AreEqual(NakoTokenType.WORD, token.Type);
            Assert.AreEqual("__", token.Value);
            Assert.AreEqual("から", token.Josi);
            // 6番目のテスト
            var tokens = NakoTokenization.Tokenize("AからBへファイルコピー");
            Assert.IsTrue(
                tokens.CheckTokenType(new NakoTokenType[]{
                    NakoTokenType.WORD,
                    NakoTokenType.WORD,
                    NakoTokenType.WORD
                }));
            Assert.AreEqual("A", tokens[0].Value);
            Assert.AreEqual("から", tokens[0].Josi);
            Assert.AreEqual("B", tokens[1].Value);
            Assert.AreEqual("へ", tokens[1].Josi);
            Assert.AreEqual("ファイルコピー", tokens[2].Value);
            Assert.AreEqual("", tokens[2].Josi);
        }
        [Test(Description = "展開あり文字列を取得するテスト")]
        public void TestGetExStringToken()
        {
            // 1番目のテスト
            Assert.IsTrue(
                NakoTokenization.Tokenize("「ABC」").CheckTokenType(new NakoTokenType[] { 
                    NakoTokenType.STRING
                }));
            // 2番目のテスト
            Assert.IsTrue(
                NakoTokenization.Tokenize("「ABC{123}」").CheckTokenType(new NakoTokenType[] {
                    NakoTokenType.STRING,
                    NakoTokenType.AND,
                    NakoTokenType.PARENTHESES_L,
                    NakoTokenType.INT,
                    NakoTokenType.PARENTHESES_R
                }));
            // 3番目のテスト
            Assert.IsTrue(
                NakoTokenization.Tokenize("「ABC{`123`}」").CheckTokenType(new NakoTokenType[] {
                    NakoTokenType.STRING,
                    NakoTokenType.AND,
                    NakoTokenType.PARENTHESES_L,
                    NakoTokenType.STRING,
                    NakoTokenType.PARENTHESES_R
                }));
            //4番目のテスト
            Assert.IsTrue(
                NakoTokenization.Tokenize("「「ABC{`123`&123}」」").CheckTokenType(new NakoTokenType[] {
                    NakoTokenType.STRING,
                    NakoTokenType.AND,
                    NakoTokenType.PARENTHESES_L,
                    NakoTokenType.STRING,
                    NakoTokenType.AND,
                    NakoTokenType.INT,
                    NakoTokenType.PARENTHESES_R
                }));
        }
        [Test(Description = "半角文字変換のテスト")]
        public void TestNakoHalfFlag()
        {
            Assert.AreEqual('*', NakoUtility.ToHalfChar('●'));
            Assert.AreEqual('>', NakoUtility.ToHalfChar('＞'));
            Assert.AreEqual('#', NakoUtility.ToHalfChar('＃'));
            Assert.AreEqual('#', NakoUtility.ToHalfChar('※'));
            Assert.AreEqual('#', NakoUtility.ToHalfChar('#'));
        }
        [Test(Description = "助詞のテスト")]
        public void TestNakoJosi()
        {
            var josiList = NakoJosi.Instance;
            string first = josiList[0];
            string last = josiList[josiList.Count - 1];
            Assert.IsTrue(first.Length > last.Length);
        }
    }
}