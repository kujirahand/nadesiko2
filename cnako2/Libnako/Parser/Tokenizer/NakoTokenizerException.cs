using System;
using System.Collections.Generic;
using System.Text;

namespace Libnako.Parser.Tokenizer
{
    public class NakoTokenizerException : Exception
    {
        public NakoTokenizerException(String message, NakoToken tok) : base(message + ":" + tok.ToStringForDebug())
        {
        }
    }
}
