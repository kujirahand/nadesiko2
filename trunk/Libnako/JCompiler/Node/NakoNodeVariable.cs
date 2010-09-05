using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Libnako.JCompiler.Parser;
using Libnako.JCompiler.Function;
using NakoPlugin;

namespace Libnako.JCompiler.Node
{
    public class NakoNodeVariable : NakoNode
    {
        /// <summary>
        /// 変数タイプ
        /// </summary>
		public NakoVarType varType { get; set; }
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
        public Boolean useElement { get { return this.hasChildren(); } }

        public Boolean IsVarTypeSimple()
        {
            return (varType == NakoVarType.Int ||
                varType == NakoVarType.Double ||
                varType == NakoVarType.String);
        }

		public NakoNodeVariable(Object value = null) : base(value)
		{
			this.scope = NakoVariableScope.Global;
			this.varType = NakoVarType.Int;
			this.varNo = -1;
			this.varBy = VarByType.ByVal;
		}

        public override String ToTypeString()
        {
            string r = type.ToString();
            r += "(" + this.varNo + ")";
            if (useElement)
            {
                r += "[" + children.Count + "]";
            }
            r += ":" + scope.ToString();
            return r;
        }
    }
}
