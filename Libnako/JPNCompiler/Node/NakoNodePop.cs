using System;
using System.Collections.Generic;
using System.Text;
using Libnako.JPNCompiler.Tokenizer;

namespace Libnako.JPNCompiler.Node
{
    /// <summary>
    /// なでしこの構文ノードを表わすクラス
    /// </summary>
    public class NakoNodePop : NakoNode
    {
        /// <summary>
        /// constructor
        /// </summary>
        public NakoNodePop()
        {
            type = NakoNodeType.POP;
        }
    }
}
