using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Libnako.Interpreter
{
    /// <summary>
    /// Objectの値を変換するコンバーター
    /// </summary>
    public class NakoValueConveter
    {
        protected Object value = null;

        public NakoValueConveter(Object v)
        {
            this.value = v;
        }

        public static Int32 ToInt(Object value)
        {
            Type t = value.GetType();
            if (t == typeof(Int32))
            {
                return (Int32)value;
            }
            if (t == typeof(Boolean))
            {
                return (Boolean)value ? 1 : 0;
            }
            if (t == typeof(Double))
            {
                return (Int32)((Double)value);
            }
            if (t == typeof(String))
            {
                Int32 i;
                if (Int32.TryParse((String)value, out i)) { return i; } else { return 0; }
            }
            return 0;
        }

        public static Double ToDouble(Object value)
        {
            Type t = value.GetType();
            if (t == typeof(Int32))
            {
                int tmpi = (Int32)value;
                Double tmpd = (Double)tmpi;
                return tmpd;
            }
            if (t == typeof(Double))
            {
                return (Double)value;
            }
            if (t == typeof(Boolean))
            {
                return (Boolean)value ? 1 : 0;
            }
            if (t == typeof(String))
            {
                Double i;
                if (Double.TryParse((String)value, out i)) { return i; } else { return 0; }
            }
            return 0;
        }

        public static String ToString(Object value)
        {
            return value.ToString();
        }

        public Object Value
        {
            get { return value; }
        }

    }
}
