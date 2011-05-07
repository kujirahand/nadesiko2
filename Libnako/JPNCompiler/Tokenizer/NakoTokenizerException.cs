using System;
using System.Collections.Generic;
using System.Text;

namespace Libnako.JPNCompiler.Tokenizer
{
    /// <summary>
    /// トークン解析の失敗を通知する例外クラス
    /// </summary>
    public class NakoTokenizerException : ApplicationException
    {
        /// <summary>
        /// トークン解析の例外を出す
        /// </summary>
        /// <param name="message"></param>
        /// <param name="tok"></param>
        public NakoTokenizerException(String message, NakoToken tok) : base(message + ":" + tok.ToStringForDebug())
        {
        }
    }
}
