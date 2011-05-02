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
        public NakoVarType Type { get { return _type; } }
        protected NakoVarType _type = NakoVarType.Void;

        /// <summary>
        /// 変数の値
        /// </summary>
        public Object Body { 
            get { return _body; } 
        }
        protected Object _body = null;

        /// <summary>
        /// 変数の管理番号
        /// </summary>
        public int varNo { get; set; }

        /// <summary>
        /// 配列などでキーとして利用される
        /// </summary>
        public string key { get; set; }

        public NakoVariable()
        {
            varNo = -1;
        }

        public void SetBody(Object value, NakoVarType type)
        {
            _body = value;
            _type = type;
        }

        public void SetBodyAutoType(Object value)
        {
            // detect type
            if (value is int)
            {
                _type = NakoVarType.Int;
                _body = Convert.ToInt64(value);
            }
            else if (value is Int64)
            {
                _type = NakoVarType.Int;
                _body = value;
            }
            else if (value is Double)
            {
                _type = NakoVarType.Double;
                _body = value;
            }
            else if (value is string)
            {
                _type = NakoVarType.String;
                _body = value;
            }
            else if (value is NakoVarArray)
            {
                _type = NakoVarType.Array;
                _body = value;
            }
            else
            {
                _type = NakoVarType.Object;
                _body = value;
            }
        }

        public override string ToString()
        {
            // detect type
            switch (Type)
            {
                case NakoVarType.Array:
                    return Body.ToString();
                case NakoVarType.Int:
                    return Body.ToString();
                case NakoVarType.Double:
                    return Body.ToString();
                case NakoVarType.Object:
                    return Body.ToString();
                case NakoVarType.Void:
                    return "";
                case NakoVarType.String:
                    return (string)Body;
                default:
                    return Body.ToString();
            }
        }
    }

    /// <summary>
    /// なでしこの配列型(配列とハッシュを扱える)
    /// </summary>
    public class NakoVarArray : NakoVariable, INakoVarArray
    {
        protected List<INakoVariable> list = new List<INakoVariable>();

        public NakoVarArray()
        {
            this._type = NakoVarType.Array;
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
            for (int i = 0; i < list.Count; i++)
            {
                INakoVariable v = list[i];
                r[i] = v.key;
            }
            return r;
        }

        public void Clear()
        {
            list = new List<INakoVariable>();
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

        public int GetIndexFromKey(string key)
        {
            // TODO: ここにキャッシュ検索を入れると早くなる!!
            for (int i = 0; i < list.Count; i++)
            {
                INakoVariable v = list[i];
                if (key == v.key) return i;
            }
            return -1;
        }

        public INakoVariable GetVarFromKey(string key)
        {
            int index = GetIndexFromKey(key);
            if (index < 0) return null;
            return list[index];
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
            INakoVariable v = GetVarFromKey(key);
            if (v != null) return v.Body;
            return null;
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

        public void SetVarFromKey(string key, INakoVariable var)
        {
            int i = GetIndexFromKey(key);
            if (i >= 0)
            {
                list.RemoveAt(i);
            }
            var.key = key;
            list.Add(var);
        }

        public void SetValue(int index, Object value)
        {
            NakoVariable v = new NakoVariable();
            v.SetBodyAutoType(value);
            this.SetVar(index, v);
        }

        public void SetValueFromKey(String key, Object value)
        {
            NakoVariable v = new NakoVariable();
            v.key = key;
            v.SetBodyAutoType(value);
            this.SetVarFromKey(key, v);
        }

        public void SetValuesFromString(String str)
        {
            Clear();
            string[] splitter = new string[]{"\r\n"};
            String[] a = str.Split(splitter, StringSplitOptions.None);
            int i = 0;
            foreach (String n in a)
            {
                SetValue(i++, n);
            }
        }

        public override string ToString()
        {
            string r = "";
            INakoVariable v;
            for (int i = 0; i < list.Count; i++)
            {
                if (i >= 1) r += "\r\n";
                v = list[i];
                if (v != null)
                {
                    r += v.ToString();
                }
            }
            r += "";
            return r;
        }
    }

}
