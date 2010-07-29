using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Libnako.Parser.Tokenizer;

namespace Libnako.Parser
{
    public class NakoParserException : Exception
    {
        public NakoParserException(String message, NakoToken tok) : base(message + ":" + tok.ToStringForDebug())
        {
        }
    }
}
