using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Libnako.JCompiler.Tokenizer;

namespace Libnako.JCompiler.Function
{
    public class NakoFuncArgs : List<NakoFuncArg>
    {
        /// <summary>
        /// 引数の定義文字列を読んで、関数の引数として登録する
        /// </summary>
        /// <param name="str"></param>
        public void analizeArgStr(String str)
        {
            NakoTokenizer tokenizer = new NakoTokenizer(str);
            tokenizer.splitWord();
            NakoTokenList tokens = tokenizer.Tokens;

            for (int i = 0; i < tokens.Count; i++)
            {
                NakoToken tok = tokens[i];
                NakoFuncArg arg = null;
                if (tok.type == NakoTokenType.WORD)
                {
                    int idx = indexOfName(tok.value);
                    if (idx < 0)
                    {
                        arg = new NakoFuncArg();
                        arg.name = tok.value;
                        arg.AddJosi(tok.josi);
                        this.Add(arg);
                    }
                    else
                    {
                        arg = this[idx];
                        arg.AddJosi(tok.josi);
                    }
                }
                if (tok.type == NakoTokenType.OR || tok.type == NakoTokenType.OR_OR)
                {
                    continue;
                }

            }
        }

        public int indexOfName(String name)
        {
            for (int i = 0; i < this.Count; i++)
            {
                NakoFuncArg arg = this[i];
                if (arg.name == name)
                {
                    return i;
                }
            }
            return -1;
        }
    }
}
