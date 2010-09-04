using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Libnako.JCompiler.Tokenizer;

namespace Libnako.JCompiler.Parser
{
    public class NakoParserException : Exception
    {
        public NakoParserException(String message, NakoToken tok) : base(message + ":" + tok.ToStringForDebug())
        {
        }
    }
}
