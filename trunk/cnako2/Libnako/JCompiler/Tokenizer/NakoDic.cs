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

        protected void AddWord(string key, NakoTokenType type)
        {
            key = NakoToken.TrimOkurigana(key);
            this.Add(key, type);
        }

        public void Init()
        {
            this.AddWord("ナデシコ", NakoTokenType.RESERVED);
            this.AddWord("もし", NakoTokenType.IF);
            this.AddWord("ならば", NakoTokenType.THEN);
            this.AddWord("違えば", NakoTokenType.ELSE);
            this.AddWord("ここまで", NakoTokenType.KOKOMADE);
            this.AddWord("繰り返す", NakoTokenType.FOR);
            this.AddWord("間", NakoTokenType.WHILE);
            this.AddWord("回", NakoTokenType.REPEAT_TIMES);
            this.AddWord("条件分岐", NakoTokenType.SWITCH);
            this.AddWord("PRINT", NakoTokenType.PRINT);
        }

    }
}
