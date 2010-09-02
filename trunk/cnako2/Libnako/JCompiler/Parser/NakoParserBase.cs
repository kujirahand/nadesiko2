using System;
using System.Collections.Generic;
using System.Text;
using Libnako.JCompiler.Node;
using Libnako.JCompiler.Tokenizer;


namespace Libnako.JCompiler.Parser
{
    /// <summary>
    /// トークンを読み込んで構文木に変換するクラス NakoParser のための下処理用クラス
    /// NakoParser には、意味解析処理自体を記述し、このクラスで意味解析に必要な下処理を書く
    /// </summary>
    public class NakoParserBase
    {
		public NakoNode topNode { get; set; }
        protected NakoNode parentNode;
        protected NakoNode lastNode;
        protected NakoTokenList tok;
        protected NakoNodeList calcStack;
        protected Stack<int> calcStackCounters;
		internal Stack<NakoParserFrame> frameStack { get; set; }
		internal Stack<NakoParserNodeState> stateStack { get; set; }
        protected Boolean flag_set_variable = false;

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

        protected virtual Boolean _value(Boolean canCallJFunction = true) { return true; }

        protected Boolean Accept(NakoTokenType type)
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
            while (c < calcStack.Count)
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
            f.localVar = NakoVariableManager.Locals;
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

        protected NakoVariableManager localVar
        {
            get
            {
                return NakoVariableManager.Locals;
            }
            set
            {
                NakoVariableManager.Locals = value;
            }
        }
    }

    internal class NakoParserNodeState
    {
		public NakoNode parentNode { get; set; }
		public NakoNode lastNode { get; set; }
    }

    internal class NakoParserFrame
    {
		public NakoVariableManager localVar { get; set; }
		public NakoNode lastNode { get; set; }
		public NakoNode parentNode { get; set; }
    }

}
