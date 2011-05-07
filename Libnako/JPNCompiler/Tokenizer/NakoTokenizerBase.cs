using System;
using System.Collections.Generic;
using System.Text;

namespace Libnako.JPNCompiler.Tokenizer
{
    /// <summary>
    /// トークン解析機の基本メソッドを提供する
    /// </summary>
    public class NakoTokenizerBase
    {
        /// <summary>
        /// 現在解析中のトークン位置を表すカーソル
        /// </summary>
        protected int cur;
        /// <summary>
        /// 現在のインデントレベル
        /// </summary>
        protected int level;
        /// <summary>
        /// インデントレベルで使うインデントの数
        /// </summary>
        protected int indentCount;
        /// <summary>
        /// 現在の行番号
        /// </summary>
        protected int lineno;
        private NakoTokenDic _TokenDic = null;
        /// <summary>
        /// トークン辞書
        /// </summary>
        public NakoTokenDic TokenDic {
            get {
                if (_TokenDic == null) { _TokenDic = new NakoTokenDic(); }
                return _TokenDic;
            }
            set { _TokenDic = value; }
        }
        /// <summary>
        /// 前回のトークンタイプ
        /// </summary>
        protected NakoTokenType last_token_type;

        /// <summary>
        /// 生のソースコード(内部で利用)
        /// </summary>
        protected string source;
        /// <summary>
        /// 生のソースコード
        /// </summary>
        public string Source
        {
            set { Init(); this.source = value; }
            get { return this.source; }
        }

        /// <summary>
        /// トークン一覧(内部)
        /// </summary>
        protected NakoTokenList tokens;
        /// <summary>
        /// トークン一覧
        /// </summary>
        public NakoTokenList Tokens
        {
            get { return this.tokens; }
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public void Init()
        {
            cur = 0;
            level = 0;
            lineno = 0;
            indentCount = 0;
            last_token_type = NakoTokenType.UNKNOWN;
            tokens = new NakoTokenList();
        }

        /// <summary>
        /// トークンタイプが合致しているか調べる
        /// </summary>
        /// <param name="checker"></param>
        /// <returns></returns>
        public Boolean CheckTokenType(NakoTokenType[] checker)
        {
            return Tokens.CheckTokenType(checker);
        }

        /// <summary>
        /// ソース終端か
        /// </summary>
        /// <returns></returns>
        public Boolean IsEOF()
        {
            return (cur >= source.Length);
        }
        /// <summary>
        /// 現在の文字(大文字小文字整形済み)
        /// </summary>
        public Char CurrentChar
        {
            get
            {
                if (IsEOF()) return '\0';
                return NakoHalfFlag.ConvertChar(source[cur]);
            }
        }
        /// <summary>
        /// 現在の文字(整形なし)
        /// </summary>
        public Char CurrentCharRaw
        {
            get
            {
                if (IsEOF()) return '\0';
                return source[cur];
            }
        }
        
        /// <summary>
        /// 次の文字
        /// </summary>
        public Char NextChar
        {
            get
            {
                if ((cur + 1) >= source.Length)
                {
                    return '\0';
                }
                return NakoHalfFlag.ConvertChar(source[cur + 1]);
            }
        }

        /// <summary>
        /// 文字列の比較
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public Boolean CompareStr(String str)
        {
            if (source.Length < (cur + str.Length)) { return false;  }
            return (source.Substring(cur, str.Length) == str);
        }

        /// <summary>
        /// 指定した文字まで取得する(区切り文字を含めない)
        /// </summary>
        /// <param name="splitter"></param>
        /// <returns></returns>
        public String GetToSplitter(String splitter)
        {
        	return GetToSplitter(splitter, false);
        }
        /// <summary>
        /// 指定した文字列まで取得する
        /// </summary>
        /// <param name="splitter">区切り文字列</param>
        /// <param name="need_splitter">区切り文字を含めるかどうか</param>
        /// <returns></returns>
        public String GetToSplitter(String splitter, Boolean need_splitter)
        {
            String r = "";
            while (!IsEOF())
            {
                if (CompareStr(splitter))
                {
                    if (need_splitter)
                    {
                        r += splitter;
                    }
                    cur += splitter.Length;
                    break;
                }
                // 全角半角変換しない
                r += source[cur];
                cur++;
            }
            return r;
        }

        /// <summary>
        /// 指定した文字まで取得
        /// </summary>
        /// <param name="splitter"></param>
        /// <returns></returns>
        public String GetToSplitter(Char splitter)
        {
        	return GetToSplitter(splitter, false);
        }
        /// <summary>
        /// 指定したもじまで取得
        /// </summary>
        /// <param name="splitter"></param>
        /// <param name="need_splitter"></param>
        /// <returns></returns>
        public String GetToSplitter(Char splitter, Boolean need_splitter)
        {
            String r = "";
            while (!IsEOF())
            {
                Char c = source[cur];
                if (c == splitter)
                {
                    if (need_splitter)
                    {
                        r += splitter;
                    }
                    cur++;
                    break;
                }
                r += c;
                cur++;
            }
            return r;
        }
    }
}
