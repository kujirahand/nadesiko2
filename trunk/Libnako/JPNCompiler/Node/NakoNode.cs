using System;
using System.Collections.Generic;
using System.Text;
using Libnako.JCompiler.Tokenizer;

namespace Libnako.JCompiler.Node
{
    /// <summary>
    /// なでしこの構文ノードを表わすクラス
    /// </summary>
    public class NakoNode
    {
		public NakoNodeType type { get; set; }
		public Object value { get; set; }
        private String _josi;
        public String josi
        {
            set { _josi = value; }
            get { return getJosi(); }
        }
        protected NakoNodeList children = null;
        public NakoNodeList Children
        {
            get { return children; }
        }
        public Boolean hasChildren()
        {
            if (children == null) return false;
            if (children.Count == 0) return false;
            return true;
        }
        private NakoToken token = null;
        
        protected String getJosi()
        {
            return _josi;
        }


        public NakoToken Token
        {
            get { return this.token; }
            set
            {
                this.token = value;
                this.josi = token.josi;
            }
        }

        public NakoNode()
        {
			type = NakoNodeType.NOP;
        }
        public NakoNode(Object value)
        {
			type = NakoNodeType.NOP;
			this.value = value;
        }

        public void AddChild(NakoNode child)
        {
            if (children == null)
            {
                children = new NakoNodeList();
            }
            children.Add(child);
        }

        public virtual String ToTypeString()
        {
            String r = type.ToString();
            return r;
        }

    }
}
