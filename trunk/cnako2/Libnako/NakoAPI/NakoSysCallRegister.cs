using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Libnako.JCompiler;

namespace Libnako.NakoAPI
{
    /// <summary>
    /// なでしこのシステム関数を登録する専用の基底クラス
    /// 実際には、このクラスを継承した、NakoSysCallBaseSystem で実際のシステム関数の登録を行っている
    /// システム関数の登録を補助するヘルパークラス
    /// </summary>
    public class NakoSysCallRegister
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
        protected void addFunc(String name, String argdef, NakoVariableType resultType, SysCallDelegate f, String desc, String kana)
        {
            NakoAPIFunc s = new NakoAPIFunc(name, argdef, resultType, f);
            NakoAPIFuncBank.Instance.AddFunc(s);
        }
        /// <summary>
        /// このメソッドを override してここでシステム関数の登録を行う
        /// </summary>
        public virtual void registerToSystem()
        {
        }
    }
}
