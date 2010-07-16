using System;
using System.Collections.Generic;
using System.Text;

namespace Libnako.Parser
{
    public class NakoParserBase
    {
        public NakoNode topNode;
        protected NakoNode parentNode;
        protected NakoNode lastNode;
        protected NakoTokenList tok;

        public NakoParserBase(NakoTokenList tokens)
        {
            this.tok = tokens;
            tokens.MoveTop();
            parentNode = topNode = new NakoNode();
            lastNode = null;
        }

        protected Boolean Accept(TokenType type)
        {
            return (tok.CurrentTokenType == type);
        }



    }
}
