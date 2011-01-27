﻿using System;
using System.Collections.Generic;
using System.Text;

using NakoPlugin;

namespace Libnako.JCompiler
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
        public int varNo = -1;

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
        protected List<NakoVariable> list = new List<NakoVariable>();
        protected Dictionary<string, int> keys = null;

        public NakoVarType Type { get; set; }
        public Object Body { get; set; }

        public NakoVarArray()
        {
            this.Type = NakoVarType.Array;
        }

        public NakoVariable GetVar(int index)
        {
            if (list.Count < 0) return null;
            if (list.Count <= index) return null;
            return list[index];
        }

        public Object GetValue(int index)
        {
            NakoVariable v = GetVar(index);
            if (v == null) return null;
            return v.Body;
        }

        public NakoVariable GetVarFromKey(string key)
        {
            if (keys == null) return null;
            int i = keys[key];
            return GetVar(i);
        }

        public NakoVariable GetVarFromObj(Object key)
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
            NakoVariable v = GetVarFromObj(key);
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

        public void SetVar(int index, NakoVariable value)
        {
            while (index >= list.Count) { list.Add(null); }
            list[index] = value;
        }

        public void SetVarFromObj(Object key, NakoVariable value)
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

        public void SetVarFromKey(string key, NakoVariable value)
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
    }

}