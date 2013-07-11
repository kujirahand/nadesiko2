﻿using System;
using System.Collections.Generic;
using System.Text;
using Libnako.JPNCompiler;
using NUnit.Framework;
using Libnako.JPNCompiler.Tokenizer;

namespace NakoPluginTest
{
    [TestFixture]
    public class TestNakoTokenizer
    {
        /*[Test(Description="Tokenizer.indent")]
        public void TestCountIndent()
        {
            var tokenizer = new NakoTokenizer();
            NakoReservedWord.Init(tokenizer.TokenDic);
            // 1
            tokenizer.Initialization();
            tokenizer.source = "    *";
            Assert.AreEqual(tokenizer.CountIndent(), 4);
            // 2
            tokenizer.Initialization();
            tokenizer.source = "    ";
            Assert.AreEqual(tokenizer.CountIndent(), 4);
            // 3
            tokenizer.Initialization();
            tokenizer.source = "\t\t*";
            Assert.AreEqual(tokenizer.CountIndent(), 8);
            // 4
            tokenizer.Initialization();
            tokenizer.source = "\t\t";
            Assert.AreEqual(tokenizer.CountIndent(), 8);
        }*/
        [Test(Description = "Tokenize method")]
        public void TestTokenize()
        {
            var tokenizer = new NakoTokenizer();
            NakoReservedWord.Init(tokenizer.TokenDic);
            // 1
            /*tokenizer.Initialization();
            tokenizer.source = "から";
            Assert.IsTrue(tokenizer.CompareStr("から"));*/
            // 2
            Assert.IsTrue(
                tokenizer.Tokenize("(1+2)*3").CheckTokenType(new NakoTokenType[]{
                    NakoTokenType.PARENTHESES_L,
                    NakoTokenType.INT,
                    NakoTokenType.PLUS,
                    NakoTokenType.INT,
                    NakoTokenType.PARENTHESES_R,
                    NakoTokenType.MUL,
                    NakoTokenType.INT
                }));
        }
        [Test(Description = "Tokenize method")]
        public void TestTokenize2()
        {
            var tokenizer = new NakoTokenizer();
            NakoReservedWord.Init(tokenizer.TokenDic);
            // 1
            Assert.IsTrue(
                tokenizer.Tokenize("1").CheckTokenType(new NakoTokenType[]{
                    NakoTokenType.INT
                }));
            // 2
            Assert.IsTrue(
                tokenizer.Tokenize("1+2").CheckTokenType(new NakoTokenType[]{
                    NakoTokenType.INT,
                    NakoTokenType.PLUS,
                    NakoTokenType.INT
                }));
            // 3
            Assert.IsTrue(
                tokenizer.Tokenize("1+2*3").CheckTokenType(new NakoTokenType[]{
                    NakoTokenType.INT,
                    NakoTokenType.PLUS,
                    NakoTokenType.INT,
                    NakoTokenType.MUL,
                    NakoTokenType.INT
                }));
        }
        [Test]
        public void TestBlockLevel()
        {
            var tokenizer = new NakoTokenizer();
            NakoReservedWord.Init(tokenizer.TokenDic);
            Assert.IsTrue(
                tokenizer.Tokenize(
                "もし、A=1ならば\n" +
                "  PRINT A\n" +
                "違えば\n" +
                "  PRINT A\n")
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
            var josiList = NakoJosi.Instance;
            string first = josiList[0];
            string last = josiList[josiList.Count - 1];
            Assert.IsTrue(first.Length > last.Length);
        }

        [Test]
        public void TestGetToken_Number()
        {
            NakoToken token;
            var tokenizer = new NakoTokenizer();
            // 1
            tokenizer.Initialization();
            tokenizer.source = "1234";
            token = tokenizer.GetToken_Number();
            Assert.AreEqual(NakoTokenType.INT, token.Type);
            Assert.AreEqual("1234", token.Value);
            // 2
            tokenizer.Initialization();
            tokenizer.source = "12.3456";
            token = tokenizer.GetToken_Number();
            Assert.AreEqual(NakoTokenType.NUMBER, token.Type);
            Assert.AreEqual("12.3456", token.Value);
            // 3
            tokenizer.Initialization();
            tokenizer.source = "0.123";
            token = tokenizer.GetToken_Number();
            Assert.AreEqual(NakoTokenType.NUMBER, token.Type);
            Assert.AreEqual("0.123", token.Value);
            // 4
            tokenizer.Initialization();
            tokenizer.source = "32から";
            token = tokenizer.GetToken_Number();
            Assert.AreEqual(NakoTokenType.INT, token.Type);
            Assert.AreEqual("32", token.Value);
            Assert.AreEqual("から", token.Josi);
        }
        [Test]
        public void TestGetToken_Word()
        {
            NakoToken token;
            var tokenizer = new NakoTokenizer();
            // 1
            tokenizer.Initialization();
            tokenizer.source = "ABC";
            token = tokenizer.GetToken_Word();
            Assert.AreEqual(NakoTokenType.WORD, token.Type);
            Assert.AreEqual("ABC", token.Value);
            // 2
            tokenizer.Initialization();
            tokenizer.source = "豆腐から";
            token = tokenizer.GetToken_Word();
            Assert.AreEqual(NakoTokenType.WORD, token.Type);
            Assert.AreEqual("豆腐", token.Value);
            Assert.AreEqual("から", token.Josi);
            // 3
            tokenizer.Initialization();
            tokenizer.source = "F_豆腐から";
            token = tokenizer.GetToken_Word();
            Assert.AreEqual(NakoTokenType.WORD, token.Type);
            Assert.AreEqual("F_豆腐", token.Value);
            Assert.AreEqual("から", token.Josi);
            // 4
            tokenizer.Initialization();
            tokenizer.source = "F123から";
            token = tokenizer.GetToken_Word();
            Assert.AreEqual(NakoTokenType.WORD, token.Type);
            Assert.AreEqual("F123", token.Value);
            Assert.AreEqual("から", token.Josi);
            // 5
            tokenizer.Initialization();
            tokenizer.source = "__から";
            token = tokenizer.GetToken_Word();
            Assert.AreEqual(NakoTokenType.WORD, token.Type);
            Assert.AreEqual("__", token.Value);
            Assert.AreEqual("から", token.Josi);
            // 6
            var tokens = tokenizer.Tokenize("AからBへファイルコピー");
            Assert.IsTrue(
                tokens.CheckTokenType(new NakoTokenType[]{
                    NakoTokenType.WORD,
                    NakoTokenType.WORD,
                    NakoTokenType.WORD
                }));
            token = tokens[0];
            Assert.AreEqual("A", token.Value);
            Assert.AreEqual("から", token.Josi);
            token = tokens[1];
            Assert.AreEqual("B", token.Value);
            Assert.AreEqual("へ", token.Josi);
            token = tokens[2];
            Assert.AreEqual("ファイルコピー", token.Value);
            Assert.AreEqual("", token.Josi);
        }
        [Test]
        public void TestGetToken_String()
        {
            NakoToken token;
            var tokenizer = new NakoTokenizer();
            // 1
            tokenizer.Initialization();
            tokenizer.source = "「ABC」";
            token = tokenizer.GetStringToken();
            Assert.AreEqual(NakoTokenType.STRING_EX, token.Type);
            Assert.AreEqual("ABC", token.Value);
            Assert.AreEqual("", token.Josi);
            // 2
            tokenizer.Initialization();
            tokenizer.source = "`豆腐`から";
            token = tokenizer.GetStringToken();
            Assert.AreEqual(NakoTokenType.STRING, token.Type);
            Assert.AreEqual("豆腐", token.Value);
            Assert.AreEqual("から", token.Josi);
            // 3
            tokenizer.Initialization();
            tokenizer.source = "「「豆腐」」から";
            token = tokenizer.GetStringToken();
            Assert.AreEqual(NakoTokenType.STRING_EX, token.Type);
            Assert.AreEqual("豆腐", token.Value);
            Assert.AreEqual("から", token.Josi);
            // 4
            tokenizer.Initialization();
            tokenizer.source = "『『『F123』』』へ飛ぶ";
            token = tokenizer.GetStringToken();
            Assert.AreEqual(NakoTokenType.STRING, token.Type);
            Assert.AreEqual("F123", token.Value);
            Assert.AreEqual("へ", token.Josi);
            // 5
            tokenizer.Initialization();
            tokenizer.source = "「aaa\nbbb\nccc」から「豆腐」へ";
            token = tokenizer.GetStringToken();
            Assert.AreEqual(NakoTokenType.STRING_EX, token.Type);
            Assert.AreEqual("aaa\nbbb\nccc", token.Value);
            Assert.AreEqual("から", token.Josi);
            token = tokenizer.GetStringToken();
            Assert.AreEqual(NakoTokenType.STRING_EX, token.Type);
            Assert.AreEqual("豆腐", token.Value);
            Assert.AreEqual("へ", token.Josi);
            Assert.AreEqual(2, token.LineNumber);
            // 6
            tokenizer.Initialization();
            tokenizer.source = "`abc\tabc`";
            token = tokenizer.GetStringToken();
            Assert.AreEqual(NakoTokenType.STRING, token.Type);
            Assert.AreEqual("abc\tabc", token.Value);
            Assert.AreEqual("", token.Josi);
        }
        [Test]
        public void TestGetToken_String_ex()
        {
            var tokenizer = new NakoTokenizer();
            // 1
            Assert.IsTrue(
                tokenizer.Tokenize("「ABC」").CheckTokenType(new NakoTokenType[] { 
                    NakoTokenType.STRING
                }));
            // 2
            Assert.IsTrue(
                tokenizer.Tokenize("「ABC{123}」").CheckTokenType(new NakoTokenType[] {
                    NakoTokenType.STRING,
                    NakoTokenType.AND,
                    NakoTokenType.PARENTHESES_L,
                    NakoTokenType.INT,
                    NakoTokenType.PARENTHESES_R
                }));
            // 3
            Assert.IsTrue(
                tokenizer.Tokenize("「ABC{`123`}」").CheckTokenType(new NakoTokenType[] {
                    NakoTokenType.STRING,
                    NakoTokenType.AND,
                    NakoTokenType.PARENTHESES_L,
                    NakoTokenType.STRING,
                    NakoTokenType.PARENTHESES_R
                }));
            //4
            Assert.IsTrue(
                tokenizer.Tokenize("「「ABC{`123`&123}」」").CheckTokenType(new NakoTokenType[] {
                    NakoTokenType.STRING,
                    NakoTokenType.AND,
                    NakoTokenType.PARENTHESES_L,
                    NakoTokenType.STRING,
                    NakoTokenType.AND,
                    NakoTokenType.INT,
                    NakoTokenType.PARENTHESES_R
                }));
        }
    }
}