using System;
using System.Collections.Generic;
using System.Text;
using Libnako.JPNCompiler.Parser;
using Libnako.JPNCompiler.Function;
using NakoPlugin;

namespace Libnako.JPNCompiler.Node
{
    /// <summary>
    /// 変数を表すノード
    /// </summary>
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
        public bool useElement { get { return this.hasChildren(); } }

        /// <summary>
        /// 値型かどうか
        /// </summary>
        /// <returns></returns>
        public bool IsVarTypeSimple()
        {
            return (varType == NakoVarType.Int ||
                varType == NakoVarType.Double ||
                varType == NakoVarType.String);
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
		public NakoNodeVariable() : base()
		{
			Init();
		}
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="value"></param>
		public NakoNodeVariable(Object value) : base(value)
		{
			Init();
		}
		/// <summary>
		/// 初期化
		/// </summary>
		public void Init()
		{
			this.scope = NakoVariableScope.Global;
			this.varType = NakoVarType.Int;
			this.varNo = -1;
			this.varBy = VarByType.ByVal;
		}

        /// <summary>
        /// タイプを表す文字列を返す
        /// </summary>
        /// <returns></returns>
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
