using System;
using System.Collections.Generic;
using System.Text;


namespace Libnako.JPNCompiler.Function
{
    /// <summary>
    /// 引数の参照に関するタイプ
    /// </summary>
    public enum VarByType { 
        /// <summary>
        /// 値渡し
        /// </summary>
        ByVal, 
        /// <summary>
        /// 参照渡し
        /// </summary>
        ByRef
    };

    /// <summary>
    /// なでしこ関数の引数の１つを表わすクラス
    /// </summary>
    public class NakoFuncArg
    {
		/// <summary>
		/// 引数の名前
		/// </summary>
        public string name { get; set; }
        /// <summary>
        /// 助詞リスト
        /// </summary>
		public List<string> josiList { get; set; }
        /// <summary>
        /// 値渡しか参照渡しか
        /// </summary>
		public VarByType varBy { get; set; }
		/// <summary>
		/// 初期値
		/// </summary>
		public object defaultValue { get; set; }
        /// <summary>
        /// constructor
        /// </summary>
        public NakoFuncArg()
        {
			varBy = VarByType.ByVal;
			josiList = new List<string>();    
        }
        /// <summary>
        /// 助詞を追加
        /// </summary>
        /// <param name="josi"></param>
        public void AddJosi(string josi)
        {
            if (!josiList.Contains(josi))
            {
                josiList.Add(josi);
            }
        }
    }
}
