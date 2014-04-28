using System;
using System.Collections.Generic;

using System.Text;
using Libnako.JPNCompiler;
using Libnako.JPNCompiler.ILWriter;
using Libnako.NakoAPI;
using Libnako.JPNCompiler.Function;
using Libnako.Interpreter.ILCode;

namespace Libnako
{
	/// <summary>
	/// Nako exception table.
	/// </summary>
	public class NakoExceptionTable : IList<NakoException>
	{
		private List<NakoException> _list = new List<NakoException>();//TODO:from,to,target,type

		/// <summary>
		/// Initializes a new instance of the <see cref="Libnako.NakoExceptionTable"/> class.
		/// </summary>
		public NakoExceptionTable ()
		{
		}
		/// <summary>
		/// Gets the catch line.
		/// </summary>
		/// <returns>The catch line.</returns>
		/// <param name="pos">Position.</param>
		/// <param name="e">E.</param>
		public int GetCatchLine(int pos,Object e){
			List<NakoException> res = _list.FindAll (delegate(NakoException ne) {
				//Type t = ne.Type;
				return (/*e.GetType()==t &&*/ pos>ne.from && pos<ne.to) ? true : false;
			});
			if (res.Count == 0)
				return pos;
			foreach (NakoException ne in res) {
				if (ne.Type == e.GetType())
					return ne.target;
			}
			return -1;
		}
		/// <summary>
		/// Add the specified from, to, target, e and message.
		/// </summary>
		/// <param name="from">From.</param>
		/// <param name="to">To.</param>
		/// <param name="target">Target.</param>
		/// <param name="e">E.</param>
		public void Add(int from, int to, int target, Exception e){
			_list.Add (new NakoException (from, to, target, e));
		}
		#region IList<NakoILCode> メンバー

		/// <summary>
		/// 検索
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public int IndexOf(NakoException item)
		{
			return _list.IndexOf(item);
		}
		/// <summary>
		/// 挿入
		/// </summary>
		/// <param name="index"></param>
		/// <param name="item"></param>
		public void Insert(int index, NakoException item)
		{
			_list.Insert(index, item);
		}
		/// <summary>
		/// 削除
		/// </summary>
		/// <param name="index"></param>
		public void RemoveAt(int index)
		{
			_list.RemoveAt(index);
		}
		/// <summary>
		/// 要素を得る
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public NakoException this[int index]
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

		#region ICollection<NakoILCode> メンバー

		/// <summary>
		/// 追加
		/// </summary>
		/// <param name="item"></param>
		public void Add(NakoException item)
		{
			_list.Add(item);
		}
		/// <summary>
		/// 削除
		/// </summary>
		public void Clear()
		{
			_list.Clear();
		}
		/// <summary>
		/// 含む
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public bool Contains(NakoException item)
		{
			return _list.Contains(item);
		}
		/// <summary>
		/// コピー
		/// </summary>
		/// <param name="array"></param>
		/// <param name="arrayIndex"></param>
		public void CopyTo(NakoException[] array, int arrayIndex)
		{
			_list.CopyTo(array, arrayIndex);
		}
		/// <summary>
		/// 個数
		/// </summary>
		public int Count
		{
			get { return _list.Count; }
		}

		bool ICollection<NakoException>.IsReadOnly
		{
			get { throw new NotImplementedException(); }
		}
		/// <summary>
		/// 削除
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public bool Remove(NakoException item)
		{
			return _list.Remove(item);
		}

		#endregion

		#region IEnumerable<NakoILCode> メンバー

		/// <summary>
		/// 数え上げ
		/// </summary>
		/// <returns></returns>
		public IEnumerator<NakoException> GetEnumerator()
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
	/// <summary>
	/// インタプリタの例外クラス
	/// </summary>
	public class NakoException : Exception
	{
		/// <summary> From </summary>
		public int from;
		/// <summary> fromLabel </summary>
		public NakoILCode fromLabel { get; set; }
		/// <summary> to </summary>
		public int to;
		/// <summary> target </summary>
		public int target;
		/// <summary> targetLabel </summary>
		public NakoILCode targetLabel { get; set; }
		/// <summary> Type </summary>
		public Type Type;
		/// <summary> message </summary>
		public string message;
		/// <summary>
		/// インタプリタクラスの例外を出す
		/// </summary>
		/// <param name="message"></param>
		internal NakoException(string message) : base(message)
		{
		}
		internal NakoException(int from, int to, int target, Object e)
		{
			this.from = from;
			this.to = to;
			this.target = target;
			this.Type = e.GetType ();
		}
		internal NakoException(NakoILCode tryLabel, NakoILCode catchLabel, Object e)
		{
			this.fromLabel = tryLabel;
			this.targetLabel = catchLabel;
			this.Type = e.GetType ();
		}

	}
}

