using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Libnako.JCompiler;
using Libnako.JCompiler.Tokenizer;
using NakoPlugin;

namespace Libnako.NakoAPI
{
    /// <summary>
    /// なでしこのシステム関数を登録する専用の基底クラス
    /// 実際には、このクラスを継承した、NakoSysCallBaseSystem などで実際のシステム関数の登録を行っている
    /// システム関数の登録を補助するヘルパークラス
    /// </summary>
    public class NakoAPIRegister
    {
        // 継承先は Singleton にしなくてはならない
        protected NakoAPIRegister()
        {
            DefineFunction();
        }

        /// <summary>
        /// 関数をシステムに追加する
        /// </summary>
        /// <param name="name">関数の名前</param>
        /// <param name="argdef">引数の定義</param>
        /// <param name="resultType">関数の戻り値</param>
        /// <param name="f">実際に処理を行うC#のdelegate</param>
        /// <param name="desc">関数の説明</param>
        /// <param name="kana">命令のよみがな</param>
        protected void addFunc(String name, String argdef, NakoVarType resultType, SysCallDelegate f, String desc, String kana)
        {
            name = NakoToken.TrimOkurigana(name);
            NakoAPIFunc s = new NakoAPIFunc(name, argdef, resultType, f);
            NakoAPIFuncBank.Instance.AddFunc(s);
        }
        protected void addVar(String name, Object value, String desc, String kane)
        {
            name = NakoToken.TrimOkurigana(name);
            NakoAPIFuncBank.Instance.AddVar(name, value);
        }
        /// <summary>
        /// このメソッドを override してここでシステム関数の登録を行う
        /// </summary>
        protected virtual void DefineFunction()
        {
        }
    }

    /// <summary>
    /// APIの実行時エラーのための例外クラス
    /// </summary>
    public class NakoAPIError : Exception
    {
        public NakoAPIError(string msg) : base (msg) {}
    }
}
