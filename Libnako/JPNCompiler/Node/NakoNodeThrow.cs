using System;

using Libnako.Interpreter.ILCode;

namespace Libnako.JPNCompiler.Node
{
	/// <summary>
	/// Nako node about throw exception
	/// </summary>
	public class NakoNodeThrow : NakoNode
	{
		/// <summary>
		/// Gets or sets the exception node.
		/// </summary>
		/// <value>The exception node.</value>
		public NakoNode exceptionNode { get; set; }
		/// <summary>
		/// Gets or sets the error variable no.
		/// </summary>
		/// <value>The error variable no.</value>
		public int errorVarNo { get; set; }
		/// <summary>
		/// Initializes a new instance of the <see cref="Libnako.JPNCompiler.Node.NakoNodeThrow"/> class.
		/// </summary>
		public NakoNodeThrow ()
		{
			type = NakoNodeType.THROW;
		}
		/// <summary>
		/// タイプ文字列
		/// </summary>
		/// <returns></returns>
		public override string ToTypeString()
		{
			return type.ToString() + ":" + exceptionNode.value.GetType().ToString();
		}
	}
}

