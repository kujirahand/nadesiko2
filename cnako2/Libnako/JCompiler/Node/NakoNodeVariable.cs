using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Libnako.JCompiler.Parser;
using Libnako.JCompiler.Function;

namespace Libnako.JCompiler.Node
{
    public class NakoNodeVariable : NakoNode
    {
        /// <summary>
        /// 変数タイプ
        /// </summary>
		public NakoVariableType varType { get; set; }
		/// <summary>
		/// 変数番号
		/// </summary>
        public int varNo { get; set; }
        /// <summary>
        /// 変数のスコープ
        /// </summary>
        public NakoVariableScope scope { get; set; }
        /// <summary>
        /// 参照か値か
        /// </summary>
        public VarByType varBy { get; set; }
        /// <summary>
        /// a[3] のように配列かプロパティへのアクセスかどうか？
        /// </summary>
        public Boolean useElement { get { return (elementNode != null); } }
        /// <summary>
        /// 配列アクセスのとき、要素へアクセスするノードの一覧
        /// この子ノードに各ノード一覧が入る
        /// </summary>
        public NakoNode elementNode = null;

        public Boolean IsVarTypeSimple()
        {
            return (varType == NakoVariableType.Int ||
                varType == NakoVariableType.Real ||
                varType == NakoVariableType.String);
        }

		public NakoNodeVariable()
		{
			this.scope = NakoVariableScope.Global;
			this.varType = NakoVariableType.Int;
			this.varNo = -1;
			this.varBy = VarByType.ByVal;
		}
    }
}
