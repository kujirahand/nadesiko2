using System;
using System.Collections.Generic;
using System.Text;

namespace Libnako.JPNCompiler.Tokenizer
{
    /// <summary>
    /// トークンの解析中にエラーが発生した場合にスローされる例外。
    /// </summary>
    public class NakoTokenizerException : ApplicationException
    {
        /// <summary>
        /// 現在の例外の原因であるトークンを取得します。
        /// </summary>
        public NakoToken Token {get; private set;}
        /// <summary>
        /// NakoTokenizerException クラスの新しいインスタンスを初期化します。
        /// </summary>
        public NakoTokenizerException()
        {

        }
        /// <summary>
        /// 指定したエラー メッセージを使用して、NakoTokenizerException クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="message">例外の原因を説明するエラー メッセージ。</param>
        public NakoTokenizerException(string message)
            : base(message)
        {
            
        }
        /// <summary>
        /// 指定したエラー メッセージ、この例外の原因であるトークンを使用して、NakoTokenizerException クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="message">例外の原因を説明するエラー メッセージ。</param>
        /// <param name="token">例外の原因であるトークン。</param>
        public NakoTokenizerException(string message, NakoToken token)
            : base(message)
        {
            Token = token;
        }
        /// <summary>
        /// 指定したエラー メッセージ、この例外の原因であるトークン、この例外の原因である内部例外への参照を使用して、NakoTokenizerException クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="message">例外の原因を説明するエラー メッセージ。</param>
        /// <param name="token">例外の原因であるトークン。</param>
        /// <param name="innerException">現在の例外の原因である例外。innerException パラメータが null 参照でない場合は、内部例外を処理する catch ブロックで現在の例外が発生します。</param>
        public NakoTokenizerException(string message, NakoToken token, Exception innerException)
            : base(message, innerException)
        {
            Token = token;
        }
    }
}
