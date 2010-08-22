using System;
using System.Collections.Generic;
using System.Text;
using Libnako.JCompiler.Node;
using Libnako.JCompiler.Tokenizer;


namespace Libnako.JCompiler.Parser
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
        protected NakoNodeList calcStack;
        protected Stack<int> calcStackCounters;
        internal Stack<NakoParserFrame> frameStack;
        internal Stack<NakoParserNodeState> stateStack;

        public NakoParserBase(NakoTokenList tokens)
        {
            this.tok = tokens;
            tokens.MoveTop();
            parentNode = topNode = new NakoNode();
            frameStack = new Stack<NakoParserFrame>();
            stateStack = new Stack<NakoParserNodeState>();
            calcStack = new NakoNodeList();
            calcStackCounters = new Stack<int>();
            lastNode = null;
            globalVar = new NakoVariableNames();
            localVar = new NakoVariableNames();
        }

        /// <summary>
        /// 値を１つだけ解析したい場合
        /// </summary>
        /// <returns></returns>
        public Boolean ParseOnlyValue()
        {
            lastNode = null;
            if (tok.IsEOF()) return false;
            if (!_value()) return false;
            topNode.AddChild(lastNode);
            return true;
        }

        protected virtual Boolean _value() { return true; }

        protected Boolean Accept(TokenType type)
        {
            return (tok.CurrentTokenType == type);
        }

        protected void TokenTry()
        {
            tok.Save();
            calcStackCounters.Push(calcStack.Count);
        }
        protected void TokenBack()
        {
            tok.Restore();
            int c = calcStackCounters.Pop();
            while (c > calcStack.Count)
            {
                calcStack.Pop();
            }
        }
        protected void TokenFinally()
        {
            tok.RemoveTop();
            calcStackCounters.Pop();
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

        protected void PushNodeState()
        {
            NakoParserNodeState s = new NakoParserNodeState();
            s.lastNode = this.lastNode;
            s.parentNode = this.parentNode;
            stateStack.Push(s);
        }

        protected void PopNodeState()
        {
            NakoParserNodeState s = stateStack.Pop();
            this.lastNode = s.lastNode;
            this.parentNode = s.parentNode;
        }
    }

    internal class NakoParserNodeState
    {
        public NakoNode parentNode;
        public NakoNode lastNode;
    }

    internal class NakoParserFrame
    {
        public NakoVariableNames localVar;
        public NakoNode lastNode;
        public NakoNode parentNode;
    }
}
