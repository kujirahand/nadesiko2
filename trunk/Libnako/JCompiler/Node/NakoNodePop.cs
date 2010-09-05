using System;
using System.Collections.Generic;
using System.Text;
using Libnako.JCompiler.Tokenizer;

namespace Libnako.JCompiler.Node
{
    /// <summary>
    /// なでしこの構文ノードを表わすクラス
    /// </summary>
    public class NakoNodePop : NakoNode
    {
        public NakoNodePop()
        {
            type = NakoNodeType.POP;
        }
    }
}
