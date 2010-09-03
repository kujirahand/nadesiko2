using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Libnako.JCompiler
{

    /// <summary>
    /// なでしこの型を表わすタイプ一覧
    /// </summary>
    public enum NakoVariableType
    {
        Void,
        Object,
        Int,        // = Int64
        Real,       // = Double
        String,     // = String
        Array,      // = NakoArray
        Group,
        UserFunc,
        SystemFunc,
        Link
    }
    
    /// <summary>
    /// なでしこの変数を表わすクラス
    /// </summary>
    public class NakoVariable
    {
		/// <summary>
		/// 変数のタイプ
		/// </summary>
        public NakoVariableType type { get; set; }
		
        /// <summary>
        /// 変数の値
        /// </summary>
        public Object body
        {
            get { return _body; }
            set { _body = value; }
        }
        protected Object _body = null;

        public void SetBodyAutoType(Object value)
        {
            _body = value;
            // detect type
            if (value is int)
            {
                type = NakoVariableType.Int;
                _body = Convert.ToInt64(value);
            }
            else if (value is Int64)
            {
                type = NakoVariableType.Int;
            }
            else if (value is Double)
            {
                type = NakoVariableType.Real;
            }
            else if (value is string)
            {
                type = NakoVariableType.String;
            }
            else if (value is NakoArray)
            {
                type = NakoVariableType.Array;
            }
        }
        
    }

    public class NakoVarialbeLink : NakoVariable
    {
        public Object key { get; set; }
        public NakoVarialbeLink(NakoVariable target, Object key)
        {
            this.type = NakoVariableType.Link;
            this.body = target;
            this.key = key;
        }
    }

    /// <summary>
    /// なでしこの配列型(配列とハッシュを扱える)
    /// </summary>
    public class NakoArray : NakoVariable
    {
        protected List<NakoVariable> list = new List<NakoVariable>();
        protected Dictionary<string, int> keys = null;

        public NakoArray()
        {
            this.type = NakoVariableType.Array;
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
            return v.body;
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
            v.body = value;
            this.SetVar(index, v);
        }

        public void SetValueFromKey(String key, Object value)
        {
            NakoVariable v = new NakoVariable();
            v.body = value;
            this.SetVarFromKey(key, v);
        }
    }

}
