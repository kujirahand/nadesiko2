using System;
using System.Collections.Generic;
using System.Text;

namespace Libnako.JCompiler.Tokenizer
{
    /// <summary>
    /// なでしこ単語管理クラス
    /// </summary>
    public class NakoDicReservedWord
    {
        public static void Init(NakoTokenDic dic)
        {
            dic.AddWord("ナデシコ", NakoTokenType.RESERVED);
            dic.AddWord("もし", NakoTokenType.IF);
            dic.AddWord("ならば", NakoTokenType.THEN);
            dic.AddWord("違えば", NakoTokenType.ELSE);
            dic.AddWord("ここまで", NakoTokenType.KOKOMADE);
            dic.AddWord("繰り返す", NakoTokenType.FOR);
            dic.AddWord("間", NakoTokenType.WHILE);
            dic.AddWord("回", NakoTokenType.REPEAT_TIMES);
            dic.AddWord("条件分岐", NakoTokenType.SWITCH);
            dic.AddWord("PRINT", NakoTokenType.PRINT);
        }
    }
}
