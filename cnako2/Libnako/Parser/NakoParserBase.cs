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
        protected NakoVariableNames globalVar;
        protected NakoVariableNames localVar;

        public NakoParserBase(NakoTokenList tokens)
        {
            this.tok = tokens;
            tokens.MoveTop();
            parentNode = topNode = new NakoNode();
            lastNode = null;
            globalVar = new NakoVariableNames();
            localVar = new NakoVariableNames();
        }

        protected Boolean Accept(TokenType type)
        {
            return (tok.CurrentTokenType == type);
        }



    }
}
