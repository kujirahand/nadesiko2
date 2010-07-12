using System;
using System.Collections.Generic;
using System.Text;

namespace Libnako.Parser
{
    public class NakoNodeList : List<NakoNode>
    {
        public Boolean checkNodeType(int[] checker)
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
        public String toNodeTypeString(int level = 0)
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

        public int[] toNodeTypeArray()
        {
            int[] r = new int[this.Count];
            int i = 0;
            foreach (NakoNode n in this)
            {
                r[i++] = n.type;
            }
            return r;
        }
    }
}
