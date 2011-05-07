using System;
using System.Collections.Generic;
using System.Text;
using Libnako.JPNCompiler.Function;


namespace Libnako.JPNCompiler.Node
{
    /// <summary>
    /// なでしこの関数呼び出しを行うノード
    /// </summary>
    public class NakoNodeCallFunction : NakoNode
    {
        /// <summary>
        /// 関数を表すオブジェクト
        /// </summary>
        public NakoFunc func = new NakoFunc();

        /// <summary>
        /// 関数呼び出し
        /// </summary>
        public NakoNodeCallFunction()
        {
            type = NakoNodeType.CALL_FUNCTION;
        }
        /// <summary>
        /// 関数の引数リスト
        /// </summary>
        public NakoNodeList argNodes
        {
            get { return this.Children; }
        }

        /// <summary>
        /// タイプ文字列を得る
        /// </summary>
        /// <returns></returns>
        public override String ToTypeString()
        {
            string r = type.ToString();
            r += "(" + this.func.name + "{args:" + func.args.Count + "})";
            return r;
        }
    }
}
