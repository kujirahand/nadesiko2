using System;
using System.Collections.Generic;
using System.Text;

namespace Libnako.JPNCompiler.Tokenizer
{
    /// <summary>
    /// 全角文字を半角文字に変換するクラス
    /// </summary>
	public static class NakoHalfFlag
    {
		private static Dictionary<Char, Char> _dictionary;

		/// <summary>
		/// 全角文字を半角文字に変換する。
		/// 変換できない文字の場合は変換対象文字をそのまま返す。
		/// </summary>
		/// <param name="c">変換対象文字</param>
		/// <returns>変換された文字</returns>
        public static Char ConvertChar(Char c)
        {
            // 半角なら変換不要
            if (c <= 0xFF)
            {
                return c;
            }
            // 数字?
            if ('０' <= c && c <= '９') {
                return (Char)('0' + c - '０');
            }
            // アルファベット?
            if ('Ａ' <= c && c <= 'Ｚ') {
                return (Char)('A' + c - 'Ａ');
            }
            if ('ａ' <= c && c <= 'ｚ')
            {
                return (Char)('a' + c - 'ａ');
            }

            // 変換の可能性
            if (_dictionary.ContainsKey(c))
			{
				return _dictionary[c];
            }
            return c;
        }

        static NakoHalfFlag()
        {
			_dictionary = new Dictionary<char, char>{
				{'　', ' '},
				{'●','*'},
				{'＊','*'},
				{'！','!'},
				{'＃','#'},
				{'／','/'},
				{'％','%'},
				{'＆','&'},
				{'’','\''},
				{'（','('},
				{'）',')'},
				{'＝','='},
				{'－','-'},
				{'～','~'},
				{'＾','^'},
				{'｜','|'},
				{'￥','\\'},
				{'｛','{'},
				{'｝','}'},
				{'【','['},
				{'】',']'},
				{'［','['},
				{'］',']'},
				{'：',':'},
				{'；',';'},
				{'＋','+'},
				{'＜','<'},
				{'＞','>'},
				{'？','?'},
				{'。',';'},
				{'、',','},
				{'，',','},
				{'．', '.'},
				{'＿', '_'},
				{'※', '#'},
			};
        }
    }
}
