﻿using System;
using System.Collections.Generic;
using System.Text;
using Libnako.JPNCompiler.Function;
using Libnako.JPNCompiler;
using NakoPlugin;

namespace Libnako.NakoAPI
{
    /// <summary>
    /// なでしこプラグイン関数を定義したもの
    /// </summary>
    public class NakoAPIFunc : NakoFunc
    {
        /// <summary>
        /// Dll Function Delegate
        /// </summary>
        public SysCallDelegate FuncDl { get; set; }
        
        /// <summary>
        /// プラグインが利用されたかどうかを表す値
        /// </summary>
        public Boolean Used { get; set; }
        
        /// <summary>
        /// INakoPlugin Instance
        /// </summary>
        public INakoPlugin PluginInstance { get; set; }

        /// <summary>
        /// なでしこのシステム関数を定義する関数
        /// </summary>
        /// <param name="name"></param>
        /// <param name="argdef"></param>
        /// <param name="resultType"></param>
        /// <param name="FuncDl"></param>
        public NakoAPIFunc(String name, String argdef, NakoVarType resultType, SysCallDelegate FuncDl)
            : base(name, argdef)
        {
            this.FuncDl = FuncDl;
            this.resultType = resultType;
            this.Used = false;
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public override void Init()
        {
            base.Init();
            funcType = NakoFuncType.SysCall;
        }

        /// <summary>
        /// 実行
        /// </summary>
        public override void Execute()
        {
        }

    }
}
