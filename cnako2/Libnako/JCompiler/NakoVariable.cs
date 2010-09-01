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
        Int,
        Real,
        String,
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
		public NakoVariableType type { get; set; }
		public Object value { get; set; }
    }

    public class NakoVarialbeLink : NakoVariable
    {
        public Object key { get; set; }
        public NakoVarialbeLink(NakoVariable target, Object key)
        {
            this.type = NakoVariableType.Link;
            this.value = target;
            this.key = key;
        }
    }

    /// <summary>
    /// なでしこの配列型(配列とハッシュを扱える)
    /// </summary>
    public class NakoArray : NakoVariable
    {
        protected List<Object> list = new List<object>();
        protected Dictionary<string, int> keys = null;

        public NakoArray()
        {
            this.type = NakoVariableType.Array;
        }
        
        public Object GetValue(int index)
        {
            if (list.Count < 0) return null;
            if (list.Count <= index) return null;
            return list[index];
        }

        public Object GetValue(string key)
        {
            if (keys == null) return null;
            int i = keys[key];
            return GetValue(i);
        }

        public void SetValue(int index, Object value)
        {
            while (index >= list.Count) { list.Add(null); }
            list[index] = value;
        }

        public void SetValue(string key, Object value)
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
    }

}
