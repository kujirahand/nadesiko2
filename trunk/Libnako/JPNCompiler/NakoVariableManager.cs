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
        /// <summary>
        /// グローバル
        /// </summary>
        Global, 
        /// <summary>
        /// ローカル
        /// </summary>
        Local
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


        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="scope"></param>
        public NakoVariableManager(NakoVariableScope scope)
        {
            list = new List<NakoVariable>();
            names = new Dictionary<string, int>();

            if (scope == NakoVariableScope.Global)
            {
                // 変数「それ」を登録
                NakoVariable sore = new NakoVariable();
                sore.SetBody(0, NakoVarType.Int);
                list.Add(sore);
                names["それ"] = 0;
            }

        }

        /// <summary>
        /// 変数名から管理番号を返す
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public int GetIndex(string name)
        {
            if (!names.ContainsKey(name))
            {
                return -1;
            }
            return names[name];
        }

        /// <summary>
        /// 変数番号から変数を返す
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public NakoVariable GetVar(int index)
        {
            if (index < list.Count)
            {
                return list[index];
            }
            return null;
        }

        /// <summary>
        /// 変数名から変数を返す
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public NakoVariable GetVar(string name)
        {
            int i = GetIndex(name);
            if (i < 0) return null;
            return list[i];
        }

        /// <summary>
        /// 変数をセット
        /// </summary>
        /// <param name="index"></param>
        /// <param name="v"></param>
        public void SetVar(int index, NakoVariable v)
        {
            // Create Var
            while (index >= list.Count)
            {
                list.Add(null);
            }
            list[index] = v;
        }

        /// <summary>
        /// 変数をセット
        /// </summary>
        /// <param name="name"></param>
        /// <param name="v"></param>
        public void SetVar(string name, NakoVariable v)
        {
            int i = GetIndex(name);
            if (i < 0)
            {
                i = CreateVar(name, v);
            }
            list[i] = v;
        }

        /// <summary>
        /// 変数を作成
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public int CreateVar(string name)
        {
        	return CreateVar(name, null);
        }
        /// <summary>
        /// 変数を作成
        /// </summary>
        /// <param name="name"></param>
        /// <param name="v"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 名前のない変数を作成
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public int CreateVarNameless(NakoVariable v)
        {
            string name = ";nameless_" + list.Count; // あり得ない変数名を作る
            return CreateVar(name, v);
        }
        /// <summary>
        /// 名前のない変数を作成
        /// </summary>
        /// <returns></returns>
        public int CreateVarNameless()
        {
        	return CreateVarNameless(null);
        }
        /// <summary>
        /// 変数に値を設定
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        public void SetValue(int index, Object value)
        {
            NakoVariable v = GetVar(index);
            if (v == null)
            {
                v = new NakoVariable();
                SetVar(index, v);
            }
            v.SetBodyAutoType(value);
        }
        /// <summary>
        /// 変数の値を取得
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
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
