using System;
using System.Collections.Generic;
using System.Text;
using Libnako.JPNCompiler.Tokenizer;

namespace Libnako.JPNCompiler.Parser
{
    public class NakoParserException : ApplicationException
    {
        public NakoParserException(String message, NakoToken tok) : base(message + ":" + tok.ToStringForDebug())
        {
        }
    }
}
