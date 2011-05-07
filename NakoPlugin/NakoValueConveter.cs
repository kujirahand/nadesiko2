using System;
using System.Collections.Generic;

using System.Text;

namespace NakoPlugin
{
    /// <summary>
    /// Objectの値を変換するコンバーター
    /// </summary>
    public class NakoValueConveter
    {
        /// <summary>
        /// 内部で使う値
        /// </summary>
        protected Object value = null;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="v"></param>
        public NakoValueConveter(Object v)
        {
            this.value = v;
        }

        /// <summary>
        /// 整数に変換
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int ToInt(Object value)
        {
            return Convert.ToInt32(value);
        }

        /// <summary>
        /// LONG型に変換
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Int64 ToLong(Object value)
        {
            if (value is Int64)
            {
                return (Int64)value;
            }
            if (value is Boolean)
            {
                return (Boolean)value ? 1 : 0;
            }
            if (value is Double)
            {
                return (Int64)((Double)value);
            }
            if (value is String)
            {
                Int64 i;
                if (Int64.TryParse((String)value, out i)) { return i; } else { return 0; }
            }
            return 0;
        }

        /// <summary>
        /// 浮動小数点数に変換
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Double ToDouble(Object value)
        {
            Type t = value.GetType();
            if (t == typeof(Int64))
            {
                Int64 tmpi = (Int64)value;
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

        /// <summary>
        /// 文字列に変換
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static String ToString(Object value)
        {
            return value.ToString();
        }

        /// <summary>
        /// 値
        /// </summary>
        public Object Value
        {
            get { return value; }
        }

    }
}
