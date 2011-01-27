using System;
using System.Collections.Generic;

using System.Text;
using NakoPlugin;

namespace Libnako.JPNCompiler
{
    /// <summary>
    /// 変数のスコープを表わす型
    /// </summary>
    public enum NakoVariableScope
    {
        Global, Local
    }

    /// <summary>
    /// なでしこの変数を管理するクラス
    /// </summary>
    public class NakoVariableManager
    {
        /// <summary>
        /// 変数一覧をリストとして保持
        /// </summary>
        protected List<NakoVariable> list;
        
        /// <summary>
        /// 名前から変数番号を取得するための辞書
        /// </summary>
        protected Dictionary<String, int> names;

		static NakoVariableManager()
		{
		}

        public NakoVariableManager(NakoVariableScope scope)
        {
            list = new List<NakoVariable>();
            names = new Dictionary<string, int>();

            if (scope == NakoVariableScope.Global)
            {
                // 変数「それ」を登録
                NakoVariable sore = new NakoVariable();
                sore.Type = NakoVarType.Int;
                sore.Body = 0;
                list.Add(sore);
                names["それ"] = 0;
            }

        }

        public int GetIndex(string name)
        {
            if (!names.ContainsKey(name))
            {
                return -1;
            }
            return names[name];
        }

        public NakoVariable GetVar(int index)
        {
            if (index < list.Count)
            {
                return list[index];
            }
            return null;
        }

        public NakoVariable GetVar(string name)
        {
            int i = GetIndex(name);
            if (i < 0) return null;
            return list[i];
        }

        public void SetVar(int index, NakoVariable v)
        {
            // Create Var
            while (index >= list.Count)
            {
                list.Add(null);
            }
            list[index] = v;
        }

        public void SetVar(string name, NakoVariable v)
        {
            int i = GetIndex(name);
            if (i < 0)
            {
                i = CreateVar(name, v);
            }
            list[i] = v;
        }

        public int CreateVar(string name)
        {
        	return CreateVar(name, null);
        }
        public int CreateVar(string name, NakoVariable v)
        {
            if (v == null)
            {
                v = new NakoVariable();
            }
            list.Add(v);
            int i = list.Count - 1;
            names[name] = i;
            return i;
        }

        public int CreateVarNameless(NakoVariable v)
        {
            string name = ";nameless_" + list.Count; // あり得ない変数名を作る
            return CreateVar(name, v);
        }
        public int CreateVarNameless()
        {
        	return CreateVarNameless(null);
        }

        public void SetValue(int index, Object value)
        {
            NakoVariable v = GetVar(index);
            if (v == null)
            {
                v = new NakoVariable();
                SetVar(index, v);
            }
            v.Body = value;
        }

        public Object GetValue(int index)
        {
            NakoVariable v = GetVar(index);
            if (v == null)
            {
                v = new NakoVariable();
                while (index >= list.Count)
                {
                    list.Add(new NakoVariable());
                }
                SetVar(index, v);
            }
            return v.Body;
        }
    }
}
