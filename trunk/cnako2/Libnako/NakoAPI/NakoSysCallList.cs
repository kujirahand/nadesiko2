using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Libnako.Interpreter;
using Libnako.JCompiler.Function;
using Libnako.JCompiler;
using Libnako.JCompiler.Tokenizer;

namespace Libnako.NakoAPI
{
    /// <summary>
    /// なでしこ２のシステム関数の一覧を保持するリスト
    /// </summary>
    public class NakoSysCallList
    {
        // Singleton
        public static readonly NakoSysCallList Instance = new NakoSysCallList();
        private NakoSysCallList() { }
        private static Boolean FlagInit = false;

        public List<NakoAPIFunc> list = new List<NakoAPIFunc>();

        public void AddFunc(NakoAPIFunc s)
        {
            list.Add(s);
            s.varNo = list.Count - 1;
        }

        public void RegisterToSystem()
        {
            // 二重初期化を防ぐ
            if (FlagInit) return; FlagInit = true;
            //
            // Tokenizer.NakoDic に登録
            NakoDic dic = NakoDic.Instance;
            for (int i = 0; i < list.Count; i++)
            {
                NakoAPIFunc call = list[i];
                dic.Add(call.name, NakoTokenType.FUNCTION_NAME);
            }

            // NakoVariables に登録
            for (int i = 0; i < list.Count; i++)
            {
                NakoVariable var = new NakoVariable();
                var.type = NakoVariableType.SysCall;
                var.value = i;
                NakoAPIFunc call = list[i];
                NakoVariables.Globals.CreateVar(call.name, var);
            }
        }
    }
}
