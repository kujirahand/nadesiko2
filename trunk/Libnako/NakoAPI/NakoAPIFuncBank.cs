using System;
using System.Collections.Generic;
using System.Text;
using Libnako.Interpreter;
using Libnako.JPNCompiler.Function;
using Libnako.JPNCompiler;
using Libnako.JPNCompiler.Tokenizer;
using Libnako.JPNCompiler.Parser;
using NakoPlugin;

namespace Libnako.NakoAPI
{
    /// <summary>
    /// なでしこ２のシステム関数の一覧を保持するリスト
    /// </summary>
    public class NakoAPIFuncBank : INakoPluginBank
    {
        // Singleton
        public static readonly NakoAPIFuncBank Instance = new NakoAPIFuncBank();
        private NakoAPIFuncBank() { }
        
        // FuncList & Token Dictionary
        /// <summary>
        /// なでしこ関数リスト
        /// </summary>
        public List<NakoAPIFunc> FuncList = new List<NakoAPIFunc>();
        /// <summary>
        /// なでしこ変数リスト
        /// </summary>
        public Dictionary<string, object> VarList = new Dictionary<string, object>();
        /// <summary>
        /// なでしこプラグインリスト
        /// </summary>
        public Dictionary<string, INakoPlugin> PluginList = new Dictionary<string, INakoPlugin>();
        /// <summary>
        /// (プラグインの関数定義で利用される)定義中のプラグインインスタンス
        /// </summary>
        private INakoPlugin PluginInstance = null;
        /// <summary>
        
        #region INakoPluginBank の実装
        public void SetPluginInstance(INakoPlugin plugin)
        {
            PluginInstance = plugin;
        }
        
        public void AddFunc(string name, string argdef, NakoVarType resultType, NakoPlugin.SysCallDelegate f, string desc, string kana)
        {
            name = NakoToken.TrimOkurigana(name);
            NakoAPIFunc s = new NakoAPIFunc(name, argdef, resultType, f);
            s.PluginInstance = PluginInstance;
            this.AddFuncToList(s);
        }

        public void AddVar(String name, Object value, String desc, String kane)
        {
            name = NakoToken.TrimOkurigana(name);
            this.AddVarToList(name, value);
        }
        #endregion

        private void AddFuncToList(NakoAPIFunc s)
        {
            FuncList.Add(s);
            s.varNo = FuncList.Count - 1;
        }

        private void AddVarToList(string name, Object value)
        {
            try {
                VarList.Add(name, value);
            } catch (Exception e) {
                throw new ApplicationException("NakoDic Register error:" + name + "/" + e.Message);
            }
        }

        public void RegisterToSystem(NakoTokenDic dic, NakoVariableManager globalVar)
        {
            // --- 関数
            // Tokenizer に登録
            for (int i = 0; i < FuncList.Count; i++)
            {
                NakoAPIFunc call = FuncList[i];
                if (!dic.ContainsKey(call.name))
                {
	                dic.Add(call.name, NakoTokenType.FUNCTION_NAME);
                }
                else
                {
                	dic[call.name] = NakoTokenType.FUNCTION_NAME;
                }
            }

            // NakoVariables に登録
            for (int i = 0; i < FuncList.Count; i++)
            {
                NakoVariable var = new NakoVariable();
                var.SetBody(i, NakoVarType.SystemFunc);
                NakoAPIFunc call = FuncList[i];
                globalVar.SetVar(call.name, var);
            }

            // --- 変数
            foreach (string name in VarList.Keys)
            {
                NakoVariable var = new NakoVariable();
                var.SetBodyAutoType(VarList[name]);
                globalVar.SetVar(name, var);
            }
        }
        
        public void ResetUsedFlag()
        {
            foreach (NakoAPIFunc func in FuncList)
            {
                func.Used = false;
                func.PluginInstance.Used = false;
            }
        }

    }
}
