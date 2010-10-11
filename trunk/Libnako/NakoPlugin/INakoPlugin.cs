﻿using System;
using System.Collections.Generic;

using System.Text;

namespace NakoPlugin
{
    /// <summary>
    /// なでしこプラグインのインターフェイス
    /// </summary>
    public interface INakoPlugin
    {
        string      Name            { get; }
        double      PluginVersion   { get; }
        string      Description     { get; }
        bool        Used            { get; set; }
        void        DefineFunction(INakoPluginBank bank);
    }

    /// <summary>
    /// なでしこの型を表わすタイプ一覧
    /// </summary>
    public enum NakoVarType
    {
        Void,
        Object,
        Int,        // = Int64
        Double,     // = Double
        String,     // = String
        Array,      // = NakoVarArray
        Group,
        UserFunc,
        SystemFunc
    }
    /// <summary>
    /// なでしこの変数を表わすインターフェイス
    /// </summary>
    public interface INakoVariable
    {
        NakoVarType Type { get; set; }
        Object Body { get; set; }
    }

    public interface INakoVarArray
    {

    }

    /// <summary>
    /// プラグイン関数呼び出しに使うインターフェイス
    /// </summary>
    public interface INakoFuncCallInfo
    {
        Object StackPop();
        string StackPopAsString();
        Int64 StackPopAsInt();
        double StackPopAsDouble();
        void WriteLog(string s);
        INakoVariable GetVariable(string varname);
        void SetVariable(string varname, INakoVariable value);
        Object GetVariableValue(string varname);
        void SetVariableValue(string varname, Object value);
    }

    /// <summary>
    /// システム関数の型
    /// </summary>
    /// <param name="info">インタプリタの情報を受け取る</param>
    /// <returns></returns>
    public delegate Object SysCallDelegate(INakoFuncCallInfo info);

    /// <summary>
    /// プラグインにシステム関数を追加する
    /// </summary>
    /// <see cref="Libnako.NakoAPI.NakoAPIFuncBank">実際の定義</see>
    public interface INakoPluginBank
    {
        /// <summary>
        /// 関数をシステムに追加する
        /// </summary>
        /// <param name="name">関数の名前</param>
        /// <param name="argdef">引数の定義</param>
        /// <param name="resultType">関数の戻り値</param>
        /// <param name="f">実際に処理を行うC#のdelegate</param>
        /// <param name="desc">関数の説明</param>
        /// <param name="kana">命令のよみがな</param>
        void SetPluginInstance(INakoPlugin plugin);
        void AddFunc(String name, String argdef, NakoVarType resultType, SysCallDelegate f, String desc, String kana);
        void AddVar(String name, Object value, String desc, String kane);
    }

    /// <summary>
    /// プラグイン内の関数によるランタイムエラー
    /// </summary>
    public class NakoPluginRuntimeException : ApplicationException
    {
        public NakoPluginRuntimeException(string message)
            : base(message)
        {
        }
    }

}
