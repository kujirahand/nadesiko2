using System;
using System.Collections.Generic;
using System.Text;

namespace Libnako.Parser
{
    /// <summary>
    /// なでしこ単語管理クラス
    /// </summary>
    public class NakoDic : Dictionary<string, TokenType>
    {
        /// <summary>
        /// Singleton でインスタンス管理
        /// </summary>
        /// <returns>唯一のNakoDicインスタンス</returns>
        public static NakoDic Instance
        {
            get
            {
                if (NakoDic.instance == null)
                {
                    NakoDic.instance = new NakoDic();
                }
                return NakoDic.instance;
            }
        }
        private static NakoDic instance = null;

        private NakoDic()
        {
            Init();
        }

        public void Init()
        {
            this.Add("ナデシコ", TokenType.RESERVED);
            this.Add("もし", TokenType.IF);
            this.Add("ならば", TokenType.THEN);
            this.Add("ここまで", TokenType.KOKOMADE);
            this.Add("繰り返す", TokenType.FOR);
            this.Add("間", TokenType.WHILE);
            this.Add("条件分岐", TokenType.SWITCH);
            this.Add("PRINT", TokenType.PRINT);
        }

    }
}
