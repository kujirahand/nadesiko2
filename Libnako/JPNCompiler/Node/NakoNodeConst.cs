using System;
using System.Collections.Generic;

using System.Text;

namespace Libnako.JPNCompiler.Node
{
    /// <summary>
    /// 定数を表すノード
    /// </summary>
    public class NakoNodeConst : NakoNode
    {
        /// <summary>
        /// タイプ文字列を得る
        /// </summary>
        /// <returns></returns>
        public override String ToTypeString()
        {
            String r = type.ToString();
            r += "=";
            r += Token.value;
            return r;
        }
    }

}
