using System;
using System.Collections.Generic;
using System.Text;

using NakoPlugin;

namespace Libnako.JPNCompiler
{
    /// <summary>
    /// なでしこの変数を表わすクラス
    /// </summary>
    public class NakoVariable : INakoVariable
    {
		/// <summary>
		/// 変数のタイプ
		/// </summary>
        public NakoVarType Type { get; set; }
		
        /// <summary>
        /// 変数の値
        /// </summary>
        public Object Body { get; set; }

        /// <summary>
        /// 変数の管理番号
        /// </summary>
        public int varNo { get; set; }

        public NakoVariable()
        {
            Type = NakoVarType.Void;
            Body = null;
            varNo = -1;
        }

        public void SetBodyAutoType(Object value)
        {
            // detect type
            if (value is int)
            {
                Type = NakoVarType.Int;
                Body = Convert.ToInt64(value);
            }
            else if (value is Int64)
            {
                Type = NakoVarType.Int;
                Body = value;
            }
            else if (value is Double)
            {
                Type = NakoVarType.Double;
                Body = value;
            }
            else if (value is string)
            {
                Type = NakoVarType.String;
                Body = value;
            }
            else if (value is NakoVarArray)
            {
                Type = NakoVarType.Array;
                Body = value;
            }
            else
            {
                Body = value;
            }
        }
    }

    /// <summary>
    /// なでしこの配列型(配列とハッシュを扱える)
    /// </summary>
    public class NakoVarArray : INakoVariable, INakoVarArray
    {
        protected List<INakoVariable> list = new List<INakoVariable>();
        protected Dictionary<string, int> keys = null;

        public NakoVarType Type { get; set; }
        public Object Body { get; set; }
        public int varNo { get; set; }

        public NakoVarArray()
        {
            this.Type = NakoVarType.Array;
        }

        public int Count
        {
            get
            {
                return list.Count;
            }
        }

        public String[] GetKeys()
        {
            String[] r = new String[list.Count];
            int i = 0;
            foreach (KeyValuePair<String,int> p in keys)
            {
                r[i++] = p.Key;
            }
            return r;
        }

        public void Clear()
        {
            list = new List<INakoVariable>();
            keys = null;
        }

        public INakoVariable GetVar(int index)
        {
            if (list.Count < 0) return null;
            if (list.Count <= index) return null;
            return list[index];
        }

        public Object GetValue(int index)
        {
            INakoVariable v = GetVar(index);
            if (v == null) return null;
            return v.Body;
        }

        public INakoVariable GetVarFromKey(string key)
        {
            if (keys == null) return null;
            if (!keys.ContainsKey(key)) return null;
            int i = keys[key];
            return GetVar(i);
        }

        public INakoVariable GetVarFromObj(Object key)
        {
            if (key is string)
            {
                return GetVarFromKey((string)key);
            }
            else
            {
                int index = Convert.ToInt32(key);
                return GetVar(index);
            }
        }

        public Object GetValueFromObj(Object key)
        {
            INakoVariable v = GetVarFromObj(key);
            if (v != null)
            {
                return v.Body;
            }
            return null;
        }

        public Object GetValueFromKey(string key)
        {
            if (keys == null) return null;
            int i = keys[key];
            return GetValue(i);
        }

        public void SetVar(int index, INakoVariable value)
        {
            while (index >= list.Count) { list.Add(null); }
            list[index] = value;
        }

        public void SetVarFromObj(Object key, INakoVariable value)
        {
            if (key is string)
            {
                SetVarFromKey((string)key, value);
            }
            else
            {
                int index = Convert.ToInt32(key);
                SetVar(index, value);
            }
        }

        public void SetVarFromKey(string key, INakoVariable value)
        {
            if (keys == null)
            {
                keys = new Dictionary<string, int>();
            }
            if (!keys.ContainsKey(key))
            {
                list.Add(value);
                keys[key] = list.Count - 1;
            }
            else
            {
                int index = keys[key];
                list[index] = value;
            }
        }

        public void SetValue(int index, Object value)
        {
            NakoVariable v = new NakoVariable();
            v.Body = value;
            this.SetVar(index, v);
        }

        public void SetValueFromKey(String key, Object value)
        {
            NakoVariable v = new NakoVariable();
            v.Body = value;
            this.SetVarFromKey(key, v);
        }
        public void SetValuesFromString(String str)
        {
            Clear();
            String[] a = str.Split('\n');
            int i = 0;
            foreach (String n in a)
            {
                SetValue(i++, n);
            }
        }
    }

}
