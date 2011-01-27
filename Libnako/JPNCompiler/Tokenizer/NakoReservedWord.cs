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
            dic.AddWord("条件分岐", NakoTokenType.SWITCH);
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
        }
    }
}
