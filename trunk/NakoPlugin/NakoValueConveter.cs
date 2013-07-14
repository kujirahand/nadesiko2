using System;
using System.Collections.Generic;

using System.Text;

namespace NakoPlugin
{
    /// <summary>
    /// objectの値を変換するコンバーター
    /// </summary>
    public class NakoValueConveter
    {
        /// <summary>
        /// 内部で使う値
        /// </summary>
        protected object value = null;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="v"></param>
        public NakoValueConveter(object v)
        {
            this.value = v;
        }

        /// <summary>
        /// 整数に変換
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int ToInt(object value)
        {
            return Convert.ToInt32(value);
        }

        /// <summary>
        /// LONG型に変換
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static long ToLong(object value)
        {
            if (value is long)
            {
                return (long)value;
            }
            if (value is bool)
            {
                return (bool)value ? 1 : 0;
            }
            if (value is double)
            {
                return (long)((double)value);
            }
            if (value is string)
            {
                long i;
                if (long.TryParse((string)value, out i)) { return i; } else { return 0; }
            }
            return 0;
        }

        /// <summary>
        /// 浮動小数点数に変換
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double ToDouble(object value)
        {
            Type t = value.GetType();
            if (t == typeof(long))
            {
                long tmpi = (long)value;
                double tmpd = (double)tmpi;
                return tmpd;
            }
            if (t == typeof(double))
            {
                return (double)value;
            }
            if (t == typeof(bool))
            {
                return (bool)value ? 1 : 0;
            }
            if (t == typeof(string))
            {
                double i;
                if (double.TryParse((string)value, out i)) { return i; } else { return 0; }
            }
            return 0;
        }

        /// <summary>
        /// 文字列に変換
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToString(object value)
        {
            return value.ToString();
        }

        /// <summary>
        /// 値
        /// </summary>
        public object Value
        {
            get { return value; }
        }

    }
}
