using System;
using System.Collections.Generic;
using System.Text;

namespace Libnako.JPNCompiler.Tokenizer
{
    public class NakoTokenizerException : ApplicationException
    {
        public NakoTokenizerException(String message, NakoToken tok) : base(message + ":" + tok.ToStringForDebug())
        {
        }
    }
}
