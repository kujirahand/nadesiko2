using System;
using System.Collections.Generic;

using System.Text;

namespace NakoPlugin
{
    /// <summary>
    /// なでしこプラグインのインターフェイス
    /// </summary>
    public interface INakoPlugin
    {
        String Name { get; }
        Double PluginVersion { get; }
        String Description { get; }
        void DefineFunction(INakoPluginBank bank);
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

    public interface INakoFuncCallInfo
    {
        Object StackPop();
        String StackPopAsString();
        void WriteLog(string s);
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
        void AddFunc(String name, String argdef, NakoVarType resultType, SysCallDelegate f, String desc, String kana);
        void AddVar(String name, Object value, String desc, String kane);
    }


}

