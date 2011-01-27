using System;
using System.Collections.Generic;
using System.Text;

namespace Libnako.JCompiler.Node
{
    public class NakoNodeLet : NakoNode
    {
        public NakoNodeLet()
        {
            type = NakoNodeType.LET;
            AddChild(new NakoNodeVariable());   // VarNode
            AddChild(new NakoNode());           // ValueNode
        }

        // 代入する値を表すノード
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
        
        // 代入先の変数を表すノード
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
    public class NakoNodeLetValue : NakoNode
    {
    	public NakoNodeLetValue()
    	{
            type = NakoNodeType.LET_VALUE;
    	}
    }
}
