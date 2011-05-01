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
        /// <summary>
        /// 対応プラグインのバージョン番号 (現在は必ず 2.0 を返すようにする)
        /// </summary>
        double TargetNakoVersion { get; }
        /// <summary>
        /// プラグインの名前 ( this.GetType().FullName とクラスのフルパスを指定する )
        /// </summary>
        string Name { get; }
        /// <summary>
        /// プラグインのバージョン（任意の値を返すことができる）
        /// </summary>
        double PluginVersion { get; }
        /// <summary>
        /// プラグインの説明を返す
        /// </summary>
        string Description { get; }
        /// <summary>
        /// プラグインが利用されたかどうかを判別する (自動的に設定されるので定義するだけでOK)
        /// </summary>
        bool Used { get; set; }
        /// <summary>
        /// プラグインでなでしこの関数を定義する
        /// <see cref="Libnako.NakoAPI.NakoBaseSystem.DefineFunction">登録例</see>
        /// </summary>
        /// <param name="bank">このオブジェクトに命令を登録する</param>
        void DefineFunction(INakoPluginBank bank);
        /// <summary>
        /// アプリ開始時に実行する処理
        /// </summary>
        void PluginInit(INakoInterpreter runner);
        /// <summary>
        /// アプリ終了時に実行する処理(メモリの解放など)
        /// </summary>
        void PluginFin(INakoInterpreter runner);
    }

    /// <summary>
    /// なでしこの型を表わすタイプ一覧
    /// </summary>
    public enum NakoVarType
    {
        Void,
        Object,     // = 任意のオブジェクト
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
        int varNo { get; set; }
    }

    public interface INakoVarArray
    {
        NakoVarType Type { get; set; }
        int Count { get; }
        INakoVariable GetVar(int index);
        Object GetValue(int index);
        INakoVariable GetVarFromObj(Object key);
        Object GetValueFromObj(Object key);
        Object GetValueFromKey(string key);
        void Clear();
        String[] GetKeys();
        void SetVar(int index, INakoVariable value);
        void SetVarFromObj(Object key, INakoVariable value);
        void SetVarFromKey(string key, INakoVariable value);
        void SetValue(int index, Object value);
        void SetValueFromKey(String key, Object value);
        void SetValuesFromString(String str);
    }

    /// <summary>
    /// プラグイン関数呼び出しに使うインターフェイス
    /// </summary>
    public interface INakoFuncCallInfo
    {
        // --- 関数の引数を取得するメソッド
        Object StackPop();
        string StackPopAsString();
        Int64 StackPopAsInt();
        double StackPopAsDouble();
        // --- システム変数へのアクセス
        INakoVariable GetVariable(string varname);
        void SetVariable(string varname, INakoVariable value);
        Object GetVariableValue(string varname);
        void SetVariableValue(string varname, Object value);
        // --- ユーティリティ
        // ログに値を書き込む
        void WriteLog(string s);
        // --- 値を作成する
        INakoVarArray CreateArray();
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
    /// なでしこインタプリタを表わすインターフェイス
    /// </summary>
    public interface INakoInterpreter
    {
        int InterpreterId { get; }
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
    
    /// <summary>
    /// プラグイン内の関数の引数指定が問題のエラー
    /// </summary>
    public class NakoPluginArgmentException : ArgumentException
    {
        public NakoPluginArgmentException(string message)
            : base(message)
        {
        }
    }
}

