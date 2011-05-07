﻿using System;
using System.Collections.Generic;
using System.Text;
using NakoPlugin;

namespace Libnako.JPNCompiler.Function
{
    /// <summary>
    /// なでしこ関数を定義したもの
    /// </summary>
    public class NakoFunc
    {
        /// <summary>
        /// 関数の番号
        /// </summary>
		public int varNo { get; set; }
        /// <summary>
        /// 関数の名前
        /// </summary>
		public String name { get; set; }
        /// <summary>
        /// 引数のリスト
        /// </summary>
		public NakoFuncArgs args { get; set; }
        /// <summary>
        /// 関数のタイプ
        /// </summary>
		public NakoFuncType funcType { get; set; }
        /// <summary>
        /// 結果のタイプ
        /// </summary>
		public NakoVarType resultType { get; set; }
        /// <summary>
        /// それを更新するかどうか
        /// </summary>
		public bool updateSore { get; set; }
        /// <summary>
        /// constructor
        /// </summary>
        public NakoFunc()
        {
			Init();
        }
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="argdef"></param>
        public NakoFunc(String name, String argdef)
        {
            Init();
            this.name = name;
            this.args.analizeArgStr(argdef);
        }
        /// <summary>
        /// 初期化
        /// </summary>
        public virtual void Init()
        {
			funcType = NakoFuncType.UserCall;
			resultType = NakoVarType.Void;
            args = new NakoFuncArgs();
			updateSore = true;
        }
        /// <summary>
        /// 実行
        /// </summary>
        public virtual void Execute() { }
        /// <summary>
        /// 引数の個数
        /// </summary>
        public int ArgCount
        {
            get
            {
                return args.Count;
            }
        }

    }

    /// <summary>
    /// 関数のタイプ
    /// </summary>
    public enum NakoFuncType
    {
        /// <summary>
        /// システム関数
        /// </summary>
        SysCall,
        /// <summary>
        /// ユーザー関数
        /// </summary>
        UserCall
    }
}
