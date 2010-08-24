using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Libnako.Interpreter;
using Libnako.JCompiler.Function;
using Libnako.JCompiler.Parser;
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

        public List<NakoSysCall> list = new List<NakoSysCall>();
        public Dictionary<int, int> dic = new Dictionary<int, int>();
        
        public void RegisterToParser(NakoParser parser)
        {
            // Parser に登録
            for (int i = 0; i < list.Count; i++)
            {
                NakoSysCall call = list[i];
                NakoVarialbeNamesValue v = parser.globalVar.createName(call.name);
                call.varNo = v.no;
                v.type = NakoVariableType.SysCall;
                // 辞書に入れて呼び出し時にすぐメソッド番号を参照できるようにする
                dic[call.varNo] = i;
            }
        }

        public void RegisterToTokenizer()
        {
            NakoDic dic = NakoDic.Instance;
            
            // Tokenizer.NakoDic に登録
            for (int i = 0; i < list.Count; i++)
            {
                NakoSysCall call = list[i];
                dic.Add(call.name, TokenType.FUNCTION_NAME);
            }
        }
    }
}
