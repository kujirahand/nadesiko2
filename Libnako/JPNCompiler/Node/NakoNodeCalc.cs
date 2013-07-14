using System;
using System.Collections.Generic;
using System.Text;
using Libnako.JPNCompiler.Parser;

namespace Libnako.JPNCompiler.Node
{
    /// <summary>
    /// 計算を表わすノード
    /// </summary>
    public class NakoNodeCalc : NakoNode
    {
        /// <summary>
        /// 左辺
        /// </summary>
		public NakoNode nodeL { get; set; }
        /// <summary>
        /// 右辺
        /// </summary>
		public NakoNode nodeR { get; set; }
        /// <summary>
        /// 演算のタイプ
        /// </summary>
		public CalcType calc_type { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public NakoNodeCalc()
        {
            calc_type = CalcType.NOP;
			type = NakoNodeType.CALC;
        }

        /// <summary>
        /// タイプを表す文字列
        /// </summary>
        /// <returns></returns>
        public override string ToTypeString()
        {
            string r = "";
            r += "(";
            r += base.ToTypeString() + ":";
            r += calc_type.ToString() + " ";
            if (nodeL != null)
            {
                r += "(";
                r += nodeL.ToTypeString();
                r += ")";
            }
            if (nodeR != null)
            {
                r += " (";
                r += nodeR.ToTypeString();
                r += ")";
            }
            r += ")";
            return r;
        }
    }
}
