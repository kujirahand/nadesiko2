using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NakoPlugin
{
    /// <summary>
    /// なでしこの配列型(配列とハッシュを扱える)
    /// </summary>
    public class NakoVarArray : NakoVariable
    {
        protected List<NakoVariable> list = new List<NakoVariable>();

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
                NakoVariable v = list[i];
                r[i] = v.key;
            }
            return r;
        }

        public void Clear()
        {
            list = new List<NakoVariable>();
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

        public int GetIndexFromKey(string key)
        {
            // TODO: ここにキャッシュ検索を入れると早くなる!!
            for (int i = 0; i < list.Count; i++)
            {
                NakoVariable v = list[i];
                if (v == null) continue;
                if (key == v.key) return i;
            }
            return -1;
        }

        public int FindValue(Object v)
        {
            for (int i = 0; i < list.Count; i++)
            {
                NakoVariable item = list[i];
                if (v == null) continue;
                if (v.Equals(item.Body)) return i;
            }
            return -1;
        }

        public NakoVariable GetVarFromKey(string key)
        {
            int index = GetIndexFromKey(key);
            if (index < 0) return null;
            return list[index];
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
            NakoVariable v = GetVarFromKey(key);
            if (v != null) return v.Body;
            return null;
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

        public void SetVarFromKey(string key, NakoVariable var)
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
            string[] splitter = new string[] { "\r\n" };
            String[] a = str.Split(splitter, StringSplitOptions.None);
            int i = 0;
            foreach (String n in a)
            {
                SetValue(i++, n);
            }
        }

        public void RemoveAt(int index)
        {
            list.RemoveAt(index);
        }

        public void Add(NakoVariable item)
        {
            list.Add(item);
        }

        public NakoVariable Pop()
        {
            if (list.Count == 0) return null;
            int last = list.Count - 1;
            NakoVariable last_o = list[last];
            list.RemoveAt(last);
            return last_o;
        }

        public void Insert(int index, NakoVariable item)
        {
            list.Insert(index, item);
        }

        public void Reverse()
        {
            list.Reverse();
        }

        public override string ToString()
        {
            string r = "";
            NakoVariable v;
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
