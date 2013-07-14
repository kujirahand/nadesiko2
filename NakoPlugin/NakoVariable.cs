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
        /// <summary>
        /// 変数のタイプ(内部で使用する)
        /// </summary>
        protected NakoVarType _type = NakoVarType.Void;

        /// <summary>
        /// 変数の値
        /// </summary>
        public object Body { 
            get { return _body; } 
        }

        /// <summary>
        /// 値を整数として参照する
        /// </summary>
        public long AsInt
        {
            get { return (long)_body; }
            set { _body = value; }
        }

        /// <summary>
        /// 変数の値(内部で使用する)
        /// </summary>
        private object _body;

        /// <summary>
        /// 変数の管理番号
        /// </summary>
        public int varNo { get; set; }

        /// <summary>
        /// 配列などでキーとして利用される
        /// </summary>
        public string key { get; set; }

        /// <summary>
        /// 変数を生成するコンストラクタ
        /// </summary>
        public NakoVariable()
        {
            varNo = -1;
        }

        /// <summary>
        /// 値と型を明示して設定する
        /// </summary>
        /// <param name="value"></param>
        /// <param name="type"></param>
        public void SetBody(object value, NakoVarType type)
        {
            _body = value;
            _type = type;
        }

        /// <summary>
        /// 値の型を自動的に判別し、値と型を設定する
        /// </summary>
        /// <param name="value"></param>
        public void SetBodyAutoType(object value)
        {
            // detect type
            if (value is int)
            {
                _type = NakoVarType.Int;
                _body = Convert.ToInt64(value);
            }
            else if (value is long)
            {
                _type = NakoVarType.Int;
                _body = value;
            }
            else if (value is double)
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
        /// <summary>
        /// 変数の内容を文字列に変換して返す
        /// </summary>
        /// <returns></returns>
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
