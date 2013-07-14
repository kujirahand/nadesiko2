using System;
using System.Collections.Generic;
using System.Text;

namespace Libnako.JPNCompiler.Tokenizer
{
    /// <summary>
    /// トークンを表します。
    /// </summary>
    public class NakoToken
    {
        /// <summary>
        /// トークンの種類を取得または設定します。
        /// </summary>
        public NakoTokenType Type { get; set; }
        /// <summary>
        /// 行番号を取得または設定します。
        /// </summary>
        public int LineNo { get; set; }
        /// <summary>
        /// インデントレベルを取得または設定します。
        /// </summary>
        public int IndentLevel { get; set; }
        /// <summary>
        /// 値を取得または設定します。
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// 助詞を取得または設定します。
        /// </summary>
        public string Josi { get; set; }
        /// <summary>
        /// NakoToken クラスの新しいインスタンスを、指定した種類を使用して初期化します。
        /// </summary>
        /// <param name="type">種類。</param>
        public NakoToken(NakoTokenType type)
            : this(type, 0, 0)
        {

        }
        /// <summary>
        /// NakoToken クラスの新しいインスタンスを、指定した種類、行番号、インデントレベルを使用して初期化します。
        /// </summary>
        /// <param name="type">種類。</param>
        /// <param name="lineNo">行番号。</param>
        /// <param name="indentLevel">インデントレベル。</param>
        public NakoToken(NakoTokenType type, int lineNo, int indentLevel)
            : this(type, lineNo, indentLevel, "")
        {

        }
        /// <summary>
        /// NakoToken クラスの新しいインスタンスを、指定した種類、行番号、インデントレベル、値を使用して初期化します。
        /// </summary>
        /// <param name="type">種類。</param>
        /// <param name="lineNo">行番号。</param>
        /// <param name="indentLevel">インデントレベル。</param>
        /// <param name="value">値。</param>
        public NakoToken(NakoTokenType type, int lineNo, int indentLevel, string value)
        {
            Type = type;
            LineNo = lineNo;
            IndentLevel = indentLevel;
            Value = value;
            Josi = "";
        }
        /// <summary>
        /// このインスタンスを、それと等価なデバック用の文字列形式に変換します。
        /// </summary>
        /// <returns>このインスタンスのデバック用の文字列形式。</returns>
        public string ToStringForDebug()
        {
            string result = "["; 0.1.ToString();
            result += Type.ToString();
            if (!string.IsNullOrEmpty(Value))
            {
                result += ":" + Value;
            }
            if (!string.IsNullOrEmpty(Josi))
            {
                result += "{" + Josi + "}";
            }
            result += "(" + (LineNo + 1) + ")";
            result += "]";
            return result;
        }
        /// <summary>
        /// 送り仮名を削除した、名前としての値を取得します。
        /// </summary>
        /// <returns>送り仮名を削除した、名前としての値。</returns>
        public string GetValueAsName()
        {
            return TrimOkurigana(Value);
        }
        /// <summary>
        /// 送り仮名を削除します。
        /// </summary>
        /// <param name="name">名前。</param>
        /// <returns>送り仮名を削除した名前。</returns>
        public static string TrimOkurigana(string name)
        {
            if (name == "") return "";
            // 1文字目がひらがななら省略を諦める
            if (NakoUtility.IsHiragana(name[0]))
            {
                return name;
            }
            string result = "";
            // 送りがなを省略する
            foreach (char c in name)
            {
                if (!NakoUtility.IsHiragana(c))
                {
                    result += c;
                }
            }
            return result;
        }
        /// <summary>
        /// このインスタンスが演算子のトークンであるかどうかを示しす値を取得します。
        /// </summary>
        /// <returns>このインスタンスが演算子のトークンの場合は true。それ以外の場合は false。</returns>
        public bool IsCalcFlag()
        {
            switch (Type) {
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
                    return true;
            }
            return false;
        }
    }
}
