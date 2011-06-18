using System;
using System.Collections.Generic;
using System.Text;

namespace Libnako.JPNCompiler.Tokenizer
{
    /// <summary>
    /// なでしこ単語管理クラス
    /// </summary>
    public class NakoReservedWord
    {
        /// <summary>
        /// 単語の初期化
        /// </summary>
        /// <param name="dic"></param>
        public static void Init(NakoTokenDic dic)
        {
            // 構文
            dic.AddWord("もし", NakoTokenType.IF);
            dic.AddWord("ならば", NakoTokenType.THEN);
            dic.AddWord("違えば", NakoTokenType.ELSE);
            dic.AddWord("ここまで", NakoTokenType.KOKOMADE);
            dic.AddWord("繰り返す", NakoTokenType.FOR);
            dic.AddWord("間", NakoTokenType.WHILE);
            dic.AddWord("回", NakoTokenType.REPEAT_TIMES);
            dic.AddWord("反復", NakoTokenType.FOREACH);
            dic.AddWord("条件分岐", NakoTokenType.SWITCH);
            dic.AddWord("抜ける", NakoTokenType.BREAK);
            dic.AddWord("続ける", NakoTokenType.CONTINUE);
            // デバッグ用の特殊構文
            dic.AddWord("PRINT", NakoTokenType.PRINT);
            // システムの優先予約語
            dic.AddWord("ナデシコ", NakoTokenType.RESERVED);
            // 変数定義など
            dic.AddWord("数値", NakoTokenType.DIM_NUMBER);
            dic.AddWord("整数", NakoTokenType.DIM_INT);
            dic.AddWord("文字列", NakoTokenType.DIM_STRING);
            dic.AddWord("変数", NakoTokenType.DIM_VARIABLE);
            dic.AddWord("配列変数", NakoTokenType.DIM_ARRAY);
            // 予約変数名
            dic.AddWord("それ", NakoTokenType.WORD); // SORE
            dic.AddWord("対象", NakoTokenType.WORD); // TAISYOU
            dic.AddWord("回数", NakoTokenType.WORD); // KAISU
        }

        // システム予約変数名対象
        /// <summary>
        /// 変数「対象」を文字列として定義したもの
        /// </summary>
        public const string TAISYOU = "対象";
        /// <summary>
        /// 変数「それ」を文字列として定義したもの
        /// </summary>
        public const string SORE = "それ";
        /// <summary>
        /// 変数「回数」を文字列として定義したもの
        /// </summary>
        public const string KAISU = "回数";
    }
}
