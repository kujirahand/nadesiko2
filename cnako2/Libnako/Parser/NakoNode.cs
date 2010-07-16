using System;
using System.Collections.Generic;
using System.Text;

namespace Libnako.Parser
{
    public class NakoNode
    {
        protected NodeType _type = 0;
        public NodeType type
        {
            get { return _type; }
            set
            {
                _type = value;
                debug_type = NodeTypeDescriptor.GetTypeName(_type);
            }
        }
        // for DEBUG
        public String debug_type = "";

        public Object value = null;
        public String josi = "";

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
            return NodeTypeDescriptor.GetTypeName(type);
        }

        public virtual void Eval()
        {
            EvalSelf();
            EvalChildren();
        }

        protected virtual void EvalSelf()
        {
        }

        protected virtual void EvalChildren()
        {
            if (!hasChildren()) return;
        }
    }
}
