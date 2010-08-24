using System;
using System.Collections.Generic;
using System.Text;

namespace Libnako.JCompiler.Tokenizer
{
    /// <summary>
    /// なでしこ単語管理クラス
    /// </summary>
    public class NakoDic : Dictionary<string, NakoTokenType>
    {
        /// <summary>
        /// Singleton でインスタンス管理
        /// </summary>
        public static readonly NakoDic Instance = new NakoDic();
        private NakoDic()
        {
            Init();
        }

        public void Init()
        {
            this.Add("ナデシコ", NakoTokenType.RESERVED);
            this.Add("もし", NakoTokenType.IF);
            this.Add("ならば", NakoTokenType.THEN);
            this.Add("違えば", NakoTokenType.ELSE);
            this.Add("ここまで", NakoTokenType.KOKOMADE);
            this.Add("繰り返す", NakoTokenType.FOR);
            this.Add("間", NakoTokenType.WHILE);
            this.Add("回", NakoTokenType.REPEAT_TIMES);
            this.Add("条件分岐", NakoTokenType.SWITCH);
            this.Add("PRINT", NakoTokenType.PRINT);
        }

    }
}
