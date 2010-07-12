using System;
using System.Collections.Generic;
using System.Text;

namespace Libnako.Parser
{
    public class NakoTokenizerExcept : Exception
    {
        public NakoTokenizerExcept(String message) : base(message)
        {

        }
    }
}
