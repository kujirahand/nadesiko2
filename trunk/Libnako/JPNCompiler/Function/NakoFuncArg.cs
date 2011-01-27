using System;
using System.Collections.Generic;
using System.Text;

namespace Libnako.JPNCompiler.Function
{
    public enum VarByType { ByVal/*値渡し*/, ByRef/*参照渡し*/ };

    /// <summary>
    /// なでしこ関数の引数の１つを表わすクラス
    /// </summary>
    public class NakoFuncArg
    {
		/// <summary>
		/// 引数の名前
		/// </summary>
        public String name { get; set; }
        /// <summary>
        /// 助詞リスト
        /// </summary>
		public List<String> josiList { get; set; }
        /// <summary>
        /// 値渡しか参照渡しか
        /// </summary>
		public VarByType varBy { get; set; }

        public NakoFuncArg()
        {
			varBy = VarByType.ByVal;
			josiList = new List<String>();    
        }

        public void AddJosi(String josi)
        {
            if (!josiList.Contains(josi))
            {
                josiList.Add(josi);
            }
        }
    }
}
