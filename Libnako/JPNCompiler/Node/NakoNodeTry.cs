using System;
using System.Collections.Generic;
using System.Text;

namespace Libnako.JPNCompiler.Node
{
	/// <summary>
	/// 条件分岐「もし」ノード
	/// </summary>
	public class NakoNodeTry : NakoNode
	{
		/// <summary>
		/// tryノード
		/// </summary>
		public NakoNode nodeTry { get; set; }
		/// <summary>
		/// catchのノード
		/// </summary>
		//TODO ○○のエラーならば〜☆☆のエラーならば〜への対応
		//TODO ○○や☆☆のエラーならばへの対応
		public NakoNode nodeCatch { get; set; }
		/// <summary>
		/// finallyのノード
		/// </summary>
		public NakoNode nodeFinally { get; set; }

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public NakoNodeTry()
		{
			this.type = NakoNodeType.TRY;
		}
		/// <summary>
		/// タイプ文字列を得る
		/// </summary>
		/// <returns></returns>
		public override string ToTypeString()
		{
			string r = type.ToString() + "\n";
			r += "  |-- TRY:";
			if (nodeTry != null) r += nodeTry.ToTypeString() + "\n";
			r += "  |-- CATCH:\n";
			if (nodeCatch != null) r += nodeCatch.ToTypeString() + "\n";
			r += "  |-- FINALLY:\n";
			if (nodeFinally != null) r += nodeFinally.ToTypeString() + "\n";
			return r;
		}
	}
}
