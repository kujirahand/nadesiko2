using System;
using System.Collections.Generic;
using System.Text;

namespace Libnako.JPNCompiler.Node
{
    /// <summary>
    /// 代入ノード
    /// </summary>
    public class NakoNodeLet : NakoNode
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public NakoNodeLet()
        {
            type = NakoNodeType.LET;
            AddChild(new NakoNodeVariable());   // VarNode
            AddChild(new NakoNode());           // ValueNode
        }

        /// <summary>
        /// 代入する値を表すノード
        /// </summary>
        public NakoNode ValueNode
        {
            get
            {
                return this.children[1];
            }
            set
            {
                this.children[1] = value;
            }
        }
        
        /// <summary>
        /// 代入先の変数を表すノード
        /// </summary>
        public NakoNodeVariable VarNode
        {
            get
            {
                return (NakoNodeVariable)this.children[0];
            }
            set
            {
                this.children[0] = value;
            }
        }
    }

    /// <summary>
    /// 代入ノード
    /// </summary>
    public class NakoNodeLetValue : NakoNode
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
    	public NakoNodeLetValue()
    	{
            type = NakoNodeType.LET_VALUE;
    	}
    }
}
