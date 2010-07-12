using System;
using System.Collections.Generic;
using System.Text;

namespace Libnako.Parser
{
    public class NakoNamespace
    {
        public String name;
        public String fullpath;
        public String source;
        // token
        protected NakoTokenList tokens = null;
        public NakoTokenList Tokens
        {
            get { return tokens; }
        }
        // node
        protected NakoNode topNode = null;
        public NakoNode TopNode
        {
            get { return topNode; }
        }

        public NakoNamespace(String source = null)
        {
            this.source = source;
        }

        public void Tokenize()
        {
            NakoTokenizer tok = new NakoTokenizer(source);
            tok.Tokenize();
            tokens = tok.Tokens;
        }

        public void Parse()
        {
            NakoParser paser = new NakoParser(tokens);
            paser.Parse();
        }

        public void TokenizeAndParse()
        {
            this.Tokenize();
            this.Parse();
        }
    }
}
