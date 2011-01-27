using System;
using System.Collections.Generic;
using System.Text;
using Libnako.JCompiler.Tokenizer;

namespace Libnako.JCompiler.Parser
{
    public class NakoParserException : ApplicationException
    {
        public NakoParserException(String message, NakoToken tok) : base(message + ":" + tok.ToStringForDebug())
        {
        }
    }
}
