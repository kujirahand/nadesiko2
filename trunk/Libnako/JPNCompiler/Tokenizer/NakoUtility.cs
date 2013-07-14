using System;
using System.Collections.Generic;
using System.Text;

namespace Libnako.JPNCompiler.Tokenizer
{
    /// <summary>
    /// 
    /// </summary>
    public static class NakoUtility
    {
		/// <summary>
		/// 全角文字を半角文字に変換します。
		/// 変換できない文字の場合は変換対象文字をそのまま返します。
		/// </summary>
		/// <param name="c">変換対象文字。</param>
		/// <returns>変換された文字。</returns>
        public static char ToHalfChar(char c)
        {
            // 半角なら変換不要
            if (c <= 0xFF)
            {
                return c;
            }
            // 数字?
            if ('０' <= c && c <= '９')
            {
                return (char)('0' + c - '０');
            }
            // アルファベット?
            if ('Ａ' <= c && c <= 'Ｚ')
            {
                return (char)('A' + c - 'Ａ');
            }
            if ('ａ' <= c && c <= 'ｚ')
            {
                return (char)('a' + c - 'ａ');
            }
            // 変換の可能性
            switch (c)
            {
                case '　': return  ' ';
				case '●': return '*';
				case '＊': return '*';
				case '！': return '!';
				case '＃': return '#';
				case '／': return '/';
				case '％': return '%';
				case '＆': return '&';
				case '’': return '\'';
				case '（': return '(';
				case '）': return ')';
				case '＝': return '=';
				case '－': return '-';
				case '～': return '~';
				case '＾': return '^';
				case '｜': return '|';
				case '￥': return '\\';
				case '｛': return '{';
				case '｝': return '}';
				case '【': return '[';
				case '】': return ']';
				case '［': return '[';
				case '］': return ']';
				case '：': return ':';
				case '；': return ';';
				case '＋': return '+';
				case '＜': return '<';
				case '＞': return '>';
				case '？': return '?';
				case '。': return ';';
				case '、': return ',';
				case '，': return ',';
				case '．': return  '.';
				case '＿': return  '_';
				case '※': return  '#';
            }
            return c;
        }
        /// <summary>
        /// アルファベットか
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool IsAlpha(char c)
        {
            return ('a' <= c && c <= 'z') || ('A' <= c && c <= 'Z');
        }
        /// <summary>
        /// 数字か
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool IsNumber(char c)
        {
            return ('0' <= c && c <= '9');
        }
        /// <summary>
        /// ひらがなか
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool IsHiragana(char c)
        {
            return ('ぁ' <= c && c <= 'ん');
        }
    }
}
