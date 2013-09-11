using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Libnako.JPNCompiler.Tokenizer
{
    /// <summary>
    /// ソースコードからトークンを解析するクラス、NakoTokenizer の利用を支援する静的クラスです。
    /// </summary>
    /// <remarks>
    /// NakoTokenizer インスタンスの使い回しはスレッドセーフではありませんので、トークン解析はこのクラスから行ってください。
    /// </remarks>
    public static class NakoTokenization
    {
        /// <summary>
        /// 指定したソースコードからトークンを解析します。
        /// </summary>
        /// <param name="source">解析するソースコード。</param>
        /// <returns>指定したソースコードから解析した、トークン一覧。</returns>
        public static NakoTokenList Tokenize(string source)
        {
            return Tokenize(source, new NakoTokenDic());
        }
        /// <summary>
        /// 指定したソースコードからトークンを解析します。
        /// </summary>
        /// <param name="source">解析するソースコード。</param>
        /// <param name="tokenDic">解析に使用する、トークン辞書。</param>
        /// <returns>指定したソースコードから解析した、トークン一覧。</returns>
        public static NakoTokenList Tokenize(string source, NakoTokenDic tokenDic)
        {
            return new NakoTokenizer().Tokenize(source, tokenDic);
        }
        /// <summary>
        /// 指定したソースコードからトークンに区切るだけの解析をします。文法を一切考慮しません。
        /// </summary>
        /// <param name="source">解析するソースコード。</param>
        /// <returns>指定したソースコードから解析した、トークン一覧。</returns>
        /// <remarks>
        /// 関数の引数の解析に使用します。
        /// </remarks>
        public static NakoTokenList TokenizeSplitOnly(string source)
        {
            return TokenizeSplitOnly(source, 0, 0);
        }
        /// <summary>
        /// 指定したソースコードからトークンに区切るだけの解析をします。文法を一切考慮しません。
        /// </summary>
        /// <param name="source">解析するソースコード。</param>
        /// <param name="lineNo">開始する行番号。</param>
        /// <param name="indentLevel">開始するインデントレベル。</param>
        /// <returns>指定したソースコードから解析した、トークン一覧。</returns>
        /// <remarks>
        /// 展開あり文字列の解析に使用します。
        /// </remarks>
        public static NakoTokenList TokenizeSplitOnly(string source, int lineNo, int indentLevel)
        {
            return new NakoTokenizer().TokenizeSplitOnly(source, lineNo, indentLevel);
        }
    }
}
