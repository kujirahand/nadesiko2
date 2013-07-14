using System;
using System.Collections.Generic;
using System.Text;
using Libnako.JPNCompiler.Tokenizer;

namespace Libnako.JPNCompiler.Parser
{
    /// <summary>
    /// 構文解析器のエラークラス
    /// </summary>
    public class NakoParserException : ApplicationException
    {
        /// <summary>
        /// 構文解析エラーを出す
        /// </summary>
        /// <param name="message"></param>
        /// <param name="tok"></param>
        public NakoParserException(string message, NakoToken tok) : base(message + ":" + tok.ToStringForDebug())
        {
        }
    }
}
