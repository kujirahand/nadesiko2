using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Libnako.JCompiler.Function;
using Libnako.JCompiler.Parser;
using Libnako.JCompiler.ILWriter;

namespace Libnako.JCompiler.Node
{
    public class NakoNodeDefFunction : NakoNode
    {
        public String funcName;
        public NakoFunc func;
        public NakoVariables localVar;
        public NakoNode funcBody;
        public NakoILCode defLabel;

        public NakoNodeDefFunction()
        {
            type = NakoNodeType.DEF_FUNCTION;
            localVar = new NakoVariables(NakoVariableScope.Local);
        }

        public void RegistArgsToLocalVar()
        {
            for (int i = 0; i < func.args.Count; i++)
            {
                NakoFuncArg arg = func.args[i];
                localVar.CreateVar(arg.name);
            }
        }

    }

    public class NakoNodeDefFunctionList : IList<NakoNodeDefFunction>
    {
		private List<NakoNodeDefFunction> _list = new List<NakoNodeDefFunction>();

		#region IList<NakoNodeDefFunction> メンバー

		public int IndexOf(NakoNodeDefFunction item)
		{
			return _list.IndexOf(item);
		}

		public void Insert(int index, NakoNodeDefFunction item)
		{
			_list.Insert(index, item);
		}

		public void RemoveAt(int index)
		{
			_list.RemoveAt(index);
		}

		public NakoNodeDefFunction this[int index]
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

		#region ICollection<NakoNodeDefFunction> メンバー

		public void Add(NakoNodeDefFunction item)
		{
			_list.Add(item);
		}

		public void Clear()
		{
			_list.Clear();
		}

		public bool Contains(NakoNodeDefFunction item)
		{
			return _list.Contains(item);
		}

		public void CopyTo(NakoNodeDefFunction[] array, int arrayIndex)
		{
			_list.CopyTo(array, arrayIndex);
		}

		public int Count
		{
			get { return _list.Count; }
		}

		bool ICollection<NakoNodeDefFunction>.IsReadOnly
		{
			get { throw new NotImplementedException(); }
		}

		public bool Remove(NakoNodeDefFunction item)
		{
			return _list.Remove(item);
		}

		#endregion

		#region IEnumerable<NakoNodeDefFunction> メンバー

		public IEnumerator<NakoNodeDefFunction> GetEnumerator()
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

