using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Libnako.JPNCompiler.Tokenizer
{
    public static class NakoTokenization
    {
        public static NakoTokenList Tokenize(string source)
        {
            return Tokenize(source, new NakoTokenDic());
        }
        public static NakoTokenList Tokenize(string source, NakoTokenDic tokenDic)
        {
            return new NakoTokenizer().Tokenize(source, tokenDic);
        }
        public static NakoTokenList TokenizeSplitOnly(string source)
        {
            return TokenizeSplitOnly(source, 0, 0);
        }
        public static NakoTokenList TokenizeSplitOnly(string source, int lineNo, int indentLevel)
        {
            return new NakoTokenizer().TokenizeSplitOnly(source, lineNo, indentLevel);
        }
    }
}
