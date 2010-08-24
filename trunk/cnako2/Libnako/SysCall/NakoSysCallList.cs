using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Libnako.Interpreter;
using Libnako.JCompiler.Function;
using Libnako.JCompiler;
using Libnako.JCompiler.Tokenizer;

namespace Libnako.SysCall
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

        public List<NakoSysCall> list = new List<NakoSysCall>();
        
        public void RegisterToSystem()
        {
            // 二重初期化を防ぐ
            if (FlagInit) return; FlagInit = true;
            //
            // Tokenizer.NakoDic に登録
            NakoDic dic = NakoDic.Instance;
            for (int i = 0; i < list.Count; i++)
            {
                NakoSysCall call = list[i];
                dic.Add(call.name, TokenType.FUNCTION_NAME);
            }

            // NakoVariables に登録
            for (int i = 0; i < list.Count; i++)
            {
                NakoVariable var = new NakoVariable();
                var.type = NakoVariableType.SysCall;
                var.value = i;
                NakoSysCall call = list[i];
                NakoVariables.Globals.CreateVar(call.name, var);
            }
        }
    }
}
