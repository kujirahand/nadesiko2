using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Libnako.Interpreter;
using Libnako.JCompiler.Function;
using Libnako.JCompiler;
using Libnako.JCompiler.Tokenizer;
using Libnako.JCompiler.Parser;
using NakoPlugin;

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

        public List<NakoAPIFunc> FuncList = new List<NakoAPIFunc>();
        public Dictionary<string, Object> VarList = new Dictionary<string, Object>();

        public void AddFunc(NakoAPIFunc s)
        {
            FuncList.Add(s);
            s.varNo = FuncList.Count - 1;
        }

        public void AddVar(string name, Object value)
        {
            VarList.Add(name, value);
        }

        public void RegisterToSystem(NakoTokenDic dic, NakoVariableManager globalVar)
        {
            // --- 関数
            // Tokenizer に登録
            for (int i = 0; i < FuncList.Count; i++)
            {
                NakoAPIFunc call = FuncList[i];
                dic.Add(call.name, NakoTokenType.FUNCTION_NAME);
            }

            // NakoVariables に登録
            for (int i = 0; i < FuncList.Count; i++)
            {
                NakoVariable var = new NakoVariable();
                var.Type = NakoVarType.SystemFunc;
                var.Body = i;
                NakoAPIFunc call = FuncList[i];
                globalVar.CreateVar(call.name, var);
            }

            // --- 変数
            foreach (string name in VarList.Keys)
            {
                NakoVariable var = new NakoVariable();
                var.SetBodyAutoType(VarList[name]);
                globalVar.CreateVar(name, var);
            }

        }
    }
}
