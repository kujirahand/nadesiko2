
using System;
using System.Collections.Generic;
using System.Text;

namespace Libnako.JPNCompiler.Tokenizer
{
    /// <summary>
    /// トークンを表すクラス
    /// </summary>
    public class NakoToken
    {
        /// <summary>
        /// トークンの値
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 行番号
        /// </summary>
        public int lineno   { get; set; }
        /// <summary>
        /// インデントレベル
        /// </summary>
        public int level    { get; set; }
        /// <summary>
        /// 助詞
        /// </summary>
        public string josi  { get; set; }
        /// <summary>
        /// トークンタイプ
        /// </summary>
        public NakoTokenType type { get; set; }

        /// <summary>
        /// トークンの生成（コンストラクタ）
        /// </summary>
        /// <param name="type"></param>
        /// <param name="lineno"></param>
        /// <param name="level"></param>
        public NakoToken(NakoTokenType type, int lineno, int level)
        {
        	Init(type, lineno, level);
        }
        /// <summary>
        /// トークンの生成
        /// </summary>
        /// <param name="type"></param>
        public NakoToken(NakoTokenType type)
        {
        	Init(type, 0, 0);
        }
        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="type"></param>
        /// <param name="lineno"></param>
        /// <param name="level"></param>
        public void Init(NakoTokenType type, int lineno, int level)
        {
            this.lineno = lineno;
            this.level = level;
            this.type = type;
			this.josi = "";
			this.value = null;
        }
        /// <summary>
        /// デバッグ用の文字列を返す
        /// </summary>
        /// <returns></returns>
        public String ToStringForDebug()
        {
            string s = "[";
            s += type.ToString();
            if (!(value == null || value == ""))
            {
                s += ":" + value;
            }
            if (!(josi == null || josi == ""))
            {
                s += "{" + josi + "}";
            }
            s += "(" + (lineno + 1) + ")";
            s += "]";
            return s;
        }

        /// <summary>
        /// 名前としての値を得る(送り仮名を削除)
        /// </summary>
        /// <returns></returns>
        public String getValueAsName()
        {
            return TrimOkurigana(value);
        }
        /// <summary>
        /// 送り仮名を削除
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static String TrimOkurigana(String name)
        {
            if (name == "") return "";
            // 一文字目がひらがななら省略は難しい
            if (NakoTokenizer.IsHira(name[0]))
            {
                return name;
            }
            string s = "";
            // 送りがなを省略する
            foreach(char c in name)
            {
                if (!NakoTokenizer.IsHira(c))
                {
                    s += c;
                }
            }
            return s;
        }
        
        /// <summary>
        /// 計算の演算子かどうか
        /// </summary>
        /// <returns></returns>
        public bool isCalcFlag()
        {
            bool result = false;
            switch (type) {
                case NakoTokenType.AND:
                case NakoTokenType.AND_AND:
                case NakoTokenType.EQ_EQ:
                case NakoTokenType.GT:
                case NakoTokenType.GT_EQ:
                case NakoTokenType.LT:
                case NakoTokenType.LT_EQ:
                case NakoTokenType.MINUS:
                case NakoTokenType.MOD:
                case NakoTokenType.MUL:
                case NakoTokenType.NOT:
                case NakoTokenType.NOT_EQ:
                case NakoTokenType.OR:
                case NakoTokenType.OR_OR:
                case NakoTokenType.PLUS:
                case NakoTokenType.POWER:
                    result = true;
                    break;
            }
            return result;
        }
    }
}
