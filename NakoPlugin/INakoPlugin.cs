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
        Version TargetNakoVersion { get; }
        /// <summary>
        /// プラグインの名前 ( this.GetType().FullName とクラスのフルパスを指定する )
        /// </summary>
        string Name { get; }
        /// <summary>
        /// プラグインのバージョン（任意の値を返すことができる）
        /// </summary>
        Version PluginVersion { get; }
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
        /// Libnako.NakoAPI.NakoBaseSystem.DefineFunction　に登録例あり
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
        /// <summary>
        /// 型無し/引数無し
        /// </summary>
        Void,
        /// <summary>
        /// 任意のオブジェクト
        /// </summary>
        Object,     // = 任意のオブジェクト
        /// <summary>
        /// 整数(=long)
        /// </summary>
        Int,        // = long
        /// <summary>
        /// 浮動小数点数(=double)
        /// </summary>
        Double,     // = double
        /// <summary>
        /// 文字列
        /// </summary>
        String,     // = string
        /// <summary>
        /// 配列
        /// </summary>
        Array,      // = NakoVarArray
        /// <summary>
        /// グループ
        /// </summary>
        Group,
        /// <summary>
        /// ユーザー関数
        /// </summary>
        UserFunc,
        /// <summary>
        /// システム関数(プラグイン関数)
        /// </summary>
        SystemFunc
    }

    /// <summary>
    /// プラグイン関数呼び出しに使うインターフェイス
    /// </summary>
    /// Libnako.JPNCompiler.Functio.NakoFuncCallInfo に本体の定義あり
    public interface INakoFuncCallInfo
    {
        // --- 関数の引数を取得するメソッド
        /// <summary>
        /// 引数スタックから値を取り出す
        /// </summary>
        /// <returns></returns>
        object StackPop();
        /// <summary>
        /// 引数スタックから文字列を取り出す
        /// </summary>
        /// <returns></returns>
        string StackPopAsString();
        /// <summary>
        /// 引数スタックから整数を取り出す
        /// </summary>
        /// <returns></returns>
        long StackPopAsInt();
        /// <summary>
        /// 引数スタックから実数を取り出す
        /// </summary>
        /// <returns></returns>
        double StackPopAsDouble();
        // --- システム変数へのアクセス
        /// <summary>
        /// インタプリタの変数を取り出す
        /// </summary>
        /// <param name="varname"></param>
        /// <returns></returns>
        NakoVariable GetVariable(string varname);
        /// <summary>
        /// インタプリタに変数を与える
        /// </summary>
        /// <param name="varname"></param>
        /// <param name="value"></param>
        void SetVariable(string varname, NakoVariable value);
        /// <summary>
        /// インタプリタから変数の値を取り出す
        /// </summary>
        /// <param name="varname"></param>
        /// <returns></returns>
        object GetVariableValue(string varname);
        /// <summary>
        /// インタプリタの変数に値を設定する
        /// </summary>
        /// <param name="varname"></param>
        /// <param name="value"></param>
        void SetVariableValue(string varname, object value);
        // --- ユーティリティ
        /// <summary>
        /// ログに値を書き込む
        /// </summary>
        /// <param name="s"></param>
        void WriteLog(string s);
        // --- 値を作成する
        /// <summary>
        /// 配列変数を生成する
        /// </summary>
        /// <returns></returns>
        NakoVarArray CreateArray();
    }

    /// <summary>
    /// システム関数の型
    /// </summary>
    /// <param name="info">インタプリタの情報を受け取る</param>
    /// <returns></returns>
    public delegate object SysCallDelegate(INakoFuncCallInfo info);

    /// <summary>
    /// プラグインにシステム関数を追加する
    /// </summary>
    /// Libnako.NakoAPI.NakoAPIFuncBank に実際の定義がある
    public interface INakoPluginBank
    {
        /// <summary>
        /// 関数をシステムに追加する
        /// </summary>
        void SetPluginInstance(INakoPlugin plugin);
        /// <summary>
        /// 関数を追加する
        /// </summary>
        /// <param name="name">関数の名前(日本語)</param>
        /// <param name="argdef">引数の定義</param>
        /// <param name="resultType">戻り値の型</param>
        /// <param name="f">関数本体</param>
        /// <param name="desc">説明</param>
        /// <param name="kana">よみがな(アルファベットはそのままで)</param>
        void AddFunc(string name, string argdef, NakoVarType resultType, SysCallDelegate f, string desc, string kana);
        /// <summary>
        /// 変数を追加する
        /// </summary>
        /// <param name="name">変数の名前</param>
        /// <param name="value">変数の初期値</param>
        /// <param name="desc">変数の説明</param>
        /// <param name="kane">よみがな(アルファベットはそのままで)</param>
        void AddVar(string name, object value, string desc, string kane);
    }
    
    /// <summary>
    /// なでしこインタプリタを表わすインターフェイス
    /// </summary>
    public interface INakoInterpreter
    {
        /// <summary>
        /// インタプリタのID番号
        /// </summary>
        int InterpreterId { get; }
    }
    
    /// <summary>
    /// プラグイン内の関数によるランタイムエラー
    /// </summary>
    public class NakoPluginRuntimeException : ApplicationException
    {
        /// <summary>
        /// エラーを生成するコンストラクタ
        /// </summary>
        /// <param name="message"></param>
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
        /// <summary>
        /// エラーを生成するコンストラクタ
        /// </summary>
        /// <param name="message"></param>
        public NakoPluginArgmentException(string message)
            : base(message)
        {
        }
    }
}

