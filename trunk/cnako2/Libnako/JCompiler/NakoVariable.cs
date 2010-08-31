using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Libnako.JCompiler
{
    public enum NakoVariableType
    {
        Void,
        Object,
        Int,
        Real,
        String,
        Array,
        Group,
        UserFunc,
        SystemFunc
    }
    
    public class NakoVariable
    {
		public NakoVariableType type { get; set; }
		public Object value { get; set; }
    }

    public class NakoArray
    {
        protected List<Object> list = new List<object>();
        protected Dictionary<string, int> keys = null;
        
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
