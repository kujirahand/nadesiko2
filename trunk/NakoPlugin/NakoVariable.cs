using System;
using System.Collections.Generic;
using System.Text;

namespace NakoPlugin
{
    /// <summary>
    /// なでしこの変数を表わすクラス
    /// </summary>
    public class NakoVariable
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


}
