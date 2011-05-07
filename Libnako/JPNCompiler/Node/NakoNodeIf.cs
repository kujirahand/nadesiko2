using System;
using System.Collections.Generic;
using System.Text;

namespace Libnako.JPNCompiler.Node
{
    /// <summary>
    /// 条件分岐「もし」ノード
    /// </summary>
    public class NakoNodeIf : NakoNode
    {
        /// <summary>
        /// 条件ノード
        /// </summary>
		public NakoNode nodeCond { get; set; }
        /// <summary>
        /// 真のときのノード
        /// </summary>
		public NakoNode nodeTrue { get; set; }
        /// <summary>
        /// 偽のときのノード
        /// </summary>
		public NakoNode nodeFalse { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public NakoNodeIf()
        {
            this.type = NakoNodeType.IF;
        }
    }
}
