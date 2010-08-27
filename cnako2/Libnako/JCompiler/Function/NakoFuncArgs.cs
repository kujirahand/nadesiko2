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
            Boolean optMode = false;
            ArgOpt argOpt = new ArgOpt();

            for (int i = 0; i < tokens.Count; i++)
            {
                NakoToken tok = tokens[i];
                NakoFuncArg arg = null;
                // オプション指定モード(optMode) の on/off
                if (tok.type == NakoTokenType.BRACES_L)
                {
                    // オプションの初期化
                    optMode = true;
                    argOpt.Init();
                    continue;
                }
                if (tok.type == NakoTokenType.BRACES_R)
                {
                    optMode = false; 
                    continue;
                }
                if (optMode)
                {
                    if (tok.type == NakoTokenType.WORD)
                    {
                        string opt = (string)tok.value;
                        if (opt == "参照渡し") argOpt.varBy = VarByType.ByRef;
                    }
                    continue;
                }

                // WORD
                if (tok.type == NakoTokenType.WORD)
                {
                    int idx = indexOfName(tok.value);
                    if (idx < 0)
                    {
                        arg = new NakoFuncArg();
                        arg.name = tok.value;
                        arg.varBy = argOpt.varBy;
                        arg.AddJosi(tok.josi);
                        this.Add(arg);
                        argOpt.Init();
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

    internal class ArgOpt
    {
        internal VarByType varBy = VarByType.ByVal;
        internal Object defaultValue;
        internal void Init()
        {
            varBy = VarByType.ByVal;
            defaultValue = null;
        }
    }
}
