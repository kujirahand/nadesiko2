using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Libnako.Interpreter;
using Libnako.JCompiler.Function;
using Libnako.JCompiler;
using Libnako.JCompiler.Tokenizer;
using Libnako.JCompiler.Parser;

namespace Libnako.NakoAPI
{
    /// <summary>
    /// なでしこ２のシステム関数の一覧を保持するリスト
    /// </summary>
    public class NakoAPIFuncBank
    {
        // Singleton
        public static readonly NakoAPIFuncBank Instance = new NakoAPIFuncBank();
        private NakoAPIFuncBank() { }

        public List<NakoAPIFunc> list = new List<NakoAPIFunc>();

        public void AddFunc(NakoAPIFunc s)
        {
            list.Add(s);
            s.varNo = list.Count - 1;
        }

        public void RegisterToSystem(NakoVariableManager globalVar)
        {
            // Tokenizer.NakoDic に登録
            NakoDicReservedWord dic = NakoDicReservedWord.Instance;
            for (int i = 0; i < list.Count; i++)
            {
                NakoAPIFunc call = list[i];
                dic.Add(call.name, NakoTokenType.FUNCTION_NAME);
            }

            // NakoVariables に登録
            for (int i = 0; i < list.Count; i++)
            {
                NakoVariable var = new NakoVariable();
                var.type = NakoVariableType.SystemFunc;
                var.body = i;
                NakoAPIFunc call = list[i];
                globalVar.CreateVar(call.name, var);
            }
        }
    }
}
