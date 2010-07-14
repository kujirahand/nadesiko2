using System;
using System.Collections.Generic;
using System.Text;

namespace Libnako.Parser
{
    /// <summary>
    /// なでしこ単語管理クラス
    /// </summary>
    public class NakoDic : Dictionary<string, int>
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
            this.Add("ナデシコ", TokenType.T_RESERVED);
            this.Add("もし", TokenType.T_IF);
            this.Add("ならば", TokenType.T_THEN);
            this.Add("ここまで", TokenType.T_KOKOMADE);
            this.Add("繰り返す", TokenType.T_FOR);
            this.Add("間", TokenType.T_WHILE);
            this.Add("条件分岐", TokenType.T_SWITCH);
            this.Add("●", TokenType.T_DEF_FUNCTION);
            this.Add("*", TokenType.T_DEF_FUNCTION);
            this.Add("■", TokenType.T_DEF_GROUP);
        }

    }
}
