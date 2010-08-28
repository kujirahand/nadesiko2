using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Libnako.JCompiler
{

    public enum NakoVariableScope
    {
        Global, Local
    }

    public class NakoVariables
    {
        // Globals and Locals
        public static readonly NakoVariables Globals = new NakoVariables(NakoVariableScope.Global);
        public static NakoVariables Locals = new NakoVariables(NakoVariableScope.Local);

        /// <summary>
        /// 変数一覧をリストとして保持
        /// </summary>
        protected List<NakoVariable> list;
        
        /// <summary>
        /// 名前から変数番号を取得するための辞書
        /// </summary>
        protected Dictionary<String, int> names;

        public NakoVariables(NakoVariableScope scope = NakoVariableScope.Local)
        {
            list = new List<NakoVariable>();
            names = new Dictionary<string, int>();

            if (scope == NakoVariableScope.Global)
            {
                // 変数「それ」を登録
                NakoVariable sore = new NakoVariable();
                sore.type = NakoVariableType.Int;
                sore.value = 0;
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

        public int CreateVar(string name, NakoVariable v = null)
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

        public int CreateVarNameless(NakoVariable v = null)
        {
            string name = ";nameless_" + list.Count; // あり得ない変数名を作る
            return CreateVar(name, v);
        }

        public void SetValue(int index, Object value)
        {
            NakoVariable v = GetVar(index);
            if (v == null)
            {
                // Create Var
                while (index >= list.Count)
                {
                    v = new NakoVariable();
                    list.Add(v);
                }
                v = list[index];
            }
            v.value = value;
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
            return v.value;
        }
    }
}
