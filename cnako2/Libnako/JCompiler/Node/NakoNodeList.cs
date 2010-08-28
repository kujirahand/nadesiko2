using System;
using System.Collections.Generic;
using System.Text;

namespace Libnako.JCompiler.Node
{
    public class NakoNodeList : IList<NakoNode>
    {
		private List<NakoNode> _list = new List<NakoNode>();
		
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

        public NakoNode Pop()
        {
            NakoNode n = this[this.Count - 1];
            this.RemoveAt(this.Count - 1);
            return n;
        }

        public void Push(NakoNode value)
        {
            this.Add(value);
        }


        public Boolean checkNodeType(NakoNodeType[] checker)
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

        public NakoNodeType[] toTypeArray()
        {
            NakoNodeType[] r = new NakoNodeType[this.Count];
            int i = 0;
            foreach (NakoNode n in this)
            {
                r[i++] = n.type;
            }
            return r;
        }

		#region IList<NakoNode> メンバー

		public int IndexOf(NakoNode item)
		{
			return _list.IndexOf(item);
		}

		public void Insert(int index, NakoNode item)
		{
			_list.Insert(index, item);
		}

		public void RemoveAt(int index)
		{
			_list.RemoveAt(index);
		}

		public NakoNode this[int index]
		{
			get
			{
				return _list[index];
			}
			set
			{
				_list[index] = value;
			}
		}

		#endregion

		#region ICollection<NakoNode> メンバー

		public void Add(NakoNode item)
		{
			_list.Add(item);
		}

		public void Clear()
		{
			_list.Clear();
		}

		public bool Contains(NakoNode item)
		{
			return _list.Contains(item);
		}

		public void CopyTo(NakoNode[] array, int arrayIndex)
		{
			_list.CopyTo(array, arrayIndex);
		}

		public int Count
		{
			get { return _list.Count; }
		}

		bool ICollection<NakoNode>.IsReadOnly
		{
			get { throw new NotImplementedException(); }
		}

		public bool Remove(NakoNode item)
		{
			return _list.Remove(item);
		}

		#endregion

		#region IEnumerable<NakoNode> メンバー

		public IEnumerator<NakoNode> GetEnumerator()
		{
			return _list.GetEnumerator();
		}

		#endregion

		#region IEnumerable メンバー

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return _list.GetEnumerator();
		}

		#endregion
	}
}
