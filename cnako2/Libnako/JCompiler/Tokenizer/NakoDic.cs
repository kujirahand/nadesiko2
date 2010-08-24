using System;
using System.Collections.Generic;
using System.Text;

namespace Libnako.JCompiler.Tokenizer
{
    /// <summary>
    /// なでしこ単語管理クラス
    /// </summary>
    public class NakoDic : Dictionary<string, TokenType>
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
            this.Add("ナデシコ", TokenType.RESERVED);
            this.Add("もし", TokenType.IF);
            this.Add("ならば", TokenType.THEN);
            this.Add("違えば", TokenType.ELSE);
            this.Add("ここまで", TokenType.KOKOMADE);
            this.Add("繰り返す", TokenType.FOR);
            this.Add("間", TokenType.WHILE);
            this.Add("回", TokenType.REPEAT_TIMES);
            this.Add("条件分岐", TokenType.SWITCH);
            this.Add("PRINT", TokenType.PRINT);
        }

    }
}
