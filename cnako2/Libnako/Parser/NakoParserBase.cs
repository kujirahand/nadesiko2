using System;
using System.Collections.Generic;
using System.Text;
using Libnako.Parser.Node;

namespace Libnako.Parser
{
    public class NakoParserBase
    {
        public NakoNode topNode;
        public NakoNodeDefFunctionList funcList;
        protected NakoNode parentNode;
        protected NakoNode lastNode;
        protected NakoTokenList tok;
        protected NakoVariableNames globalVar;
        protected NakoVariableNames localVar;
        internal Stack<NakoParserFrame> frameStack;

        public NakoParserBase(NakoTokenList tokens)
        {
            this.tok = tokens;
            tokens.MoveTop();
            parentNode = topNode = new NakoNode();
            frameStack = new Stack<NakoParserFrame>();
            lastNode = null;
            globalVar = new NakoVariableNames();
            localVar = new NakoVariableNames();
        }

        protected Boolean Accept(TokenType type)
        {
            return (tok.CurrentTokenType == type);
        }

        protected void PushFrame()
        {
            NakoParserFrame f = new NakoParserFrame();
            f.lastNode = lastNode;
            f.parentNode = parentNode;
            f.localVar = localVar;
            frameStack.Push(f);
        }

        protected void PopFrame()
        {
            NakoParserFrame f = frameStack.Pop();
            lastNode = f.lastNode;
            parentNode = f.parentNode;
            localVar = f.localVar;
        }
    }
    internal class NakoParserFrame
    {
        public NakoVariableNames localVar;
        public NakoNode lastNode;
        public NakoNode parentNode;
    }
}
