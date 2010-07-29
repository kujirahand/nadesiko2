using System;
using System.Collections.Generic;
using System.Text;

namespace Libnako.Parser.Node
{
    public class NakoNodeList : List<NakoNode>
    {
        public NakoNode Shift()
        {
            if (this.Count == 0)
            {
                return null;
            }
            NakoNode r = this[0];
            this.RemoveAt(0);
            return r;
        }


        public Boolean checkNodeType(NodeType[] checker)
        {
            if (checker.Length != this.Count) return false;
            for (int i = 0; i < checker.Length; i++)
            {
                NakoNode n = this[i];
                if (n.type != checker[i])
                {
                    return false;
                }
            }
            return true;
        }
        public String toTypeString(int level = 0)
        {
            String r = "";
            foreach (NakoNode n in this)
            {
                if (r != "")
                {
                    r += ",";
                }
                r += n.ToTypeString();
            }
            return r;
        }

        public NodeType[] toTypeArray()
        {
            NodeType[] r = new NodeType[this.Count];
            int i = 0;
            foreach (NakoNode n in this)
            {
                r[i++] = n.type;
            }
            return r;
        }
    }
}
