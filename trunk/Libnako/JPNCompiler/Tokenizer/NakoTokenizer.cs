using System;
using System.Collections.Generic;
using System.Text;

namespace Libnako.JPNCompiler.Tokenizer
{
    /// <summary>
    /// トークン解析器クラス
    /// </summary>
    public class NakoTokenizer
    {
        /// <summary>
        /// 現在解析中のトークン位置を表す
        /// </summary>
        private int position;
        /// <summary>
        /// 現在のインデントレベル
        /// </summary>
        private int indentLevel;
        /// <summary>
        /// インデントレベルで使うインデントの数
        /// </summary>
        private int indentCount;
        /// <summary>
        /// 現在の行番号
        /// </summary>
        private int lineNumber;
        /// <summary>
        /// トークン辞書
        /// </summary>
        public NakoTokenDic TokenDic { get; set; }
        /// <summary>
        /// 前回のトークンタイプ
        /// </summary>
        private NakoTokenType lastTokenType;
        /// <summary>
        /// 生のソースコード
        /// </summary>
        internal string source;
        /// <summary>
        /// トークン一覧
        /// </summary>
        private NakoTokenList tokens;
        /// <summary>
        /// トークン解析器のコンストラクタ
        /// </summary>
        public NakoTokenizer()
        {
            TokenDic = new NakoTokenDic();
            Initialization();
        }
        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialization()
        {
            position = 0;
            indentLevel = 0;
            lineNumber = 0;
            indentCount = 0;
            lastTokenType = NakoTokenType.UNKNOWN;
            tokens = new NakoTokenList();
        }
        /// <summary>
        /// ソース終端か
        /// </summary>
        /// <returns></returns>
        public bool IsEOF()
        {
            return (position >= source.Length);
        }
        /// <summary>
        /// 現在の文字(大文字小文字整形済み)
        /// </summary>
        public char CurrentChar
        {
            get
            {
                return NakoHalfFlag.ConvertChar(CurrentCharRaw);
            }
        }
        /// <summary>
        /// 現在の文字(整形なし)
        /// </summary>
        public char CurrentCharRaw
        {
            get
            {
                if (IsEOF()) return '\0';
                return source[position];
            }
        }
        /// <summary>
        /// 次の文字
        /// </summary>
        public char NextChar
        {
            get
            {
                if ((position + 1) >= source.Length)
                {
                    return '\0';
                }
                return NakoHalfFlag.ConvertChar(source[position + 1]);
            }
        }
        /// <summary>
        /// 文字列の比較
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        internal bool CompareStr(string str)
        {
            if (source.Length < (position + str.Length)) { return false; }
            return (source.Substring(position, str.Length) == str);
        }
        /// <summary>
        /// 指定した文字まで取得する(区切り文字を含めない)
        /// </summary>
        /// <param name="splitter"></param>
        /// <returns></returns>
        public string GetToSplitter(string splitter)
        {
        	return GetToSplitter(splitter, false);
        }
        /// <summary>
        /// 指定した文字列まで取得する
        /// </summary>
        /// <param name="splitter">区切り文字列</param>
        /// <param name="need_splitter">区切り文字を含めるかどうか</param>
        /// <returns></returns>
        public string GetToSplitter(string splitter, bool need_splitter)
        {
            string r = "";
            while (!IsEOF())
            {
                if (CompareStr(splitter))
                {
                    if (need_splitter)
                    {
                        r += splitter;
                    }
                    position += splitter.Length;
                    break;
                }
                // 全角半角変換しない
                r += source[position];
                position++;
            }
            return r;
        }
        /// <summary>
        /// 指定した文字まで取得
        /// </summary>
        /// <param name="splitter"></param>
        /// <returns></returns>
        public string GetToSplitter(char splitter)
        {
        	return GetToSplitter(splitter, false);
        }
        /// <summary>
        /// 指定したも文字まで取得
        /// </summary>
        /// <param name="splitter"></param>
        /// <param name="need_splitter"></param>
        /// <returns></returns>
        public string GetToSplitter(char splitter, bool need_splitter)
        {
            string r = "";
            while (!IsEOF())
            {
                char c = source[position];
                if (c == splitter)
                {
                    if (need_splitter)
                    {
                        r += splitter;
                    }
                    position++;
                    break;
                }
                r += c;
                position++;
            }
            return r;
        }
        /// <summary>
        /// トークン解析を行う
        /// </summary>
        public NakoTokenList Tokenize(string source)
        {
            Initialization();
            this.source = source;
            // 1回目の解析 --- トークンをひたすら区切る
            TokenizeFirst();
            // 2回目の解析 --- 関数宣言など辞書登録を行う
            TokenizeAnalize();
            // 3回目の解析 --- T_WORDを置き換える
            TokenizeCheckWord();
            return tokens;
        }
        /// <summary>
        /// トークンをひたすらぶった切るだけ、文法を一切考慮しない
        /// </summary>
        public NakoTokenList SplitWord(string source)
        {
            Initialization();
            this.source = source;
            // 繰り返しトークンを取得する
            while (!IsEOF())
            {
                NakoToken token = GetToken();
                if (token == null) continue;
                tokens.Add(token);
            }
            return tokens;
        }
        /// <summary>
        /// 関数宣言など辞書登録を行う
        /// </summary>
        private void TokenizeAnalize()
        {
            tokens.MoveTop();
            while (!tokens.IsEOF())
            {
                if (tokens.CurrentTokenType == NakoTokenType.DEF_FUNCTION)
                {
                    TokenizeAnalize_DefFunction();
                    continue;
                }
                tokens.MoveNext();
            }
        }
        /// <summary>
        /// 予約後のチェックや代入文への変換作業などを行う
        /// </summary>
        private void TokenizeCheckWord()
        {
            // 予約語句のチェックなど
            tokens.MoveTop();
            while (!tokens.IsEOF())
            {
                // 予約語句の置き換え
                if (tokens.CurrentTokenType == NakoTokenType.WORD)
                {
                    NakoToken token = tokens.CurrentToken;
                    string key = token.getValueAsName();
                    if (TokenDic.ContainsKey(key))
                    {
                        token.Type = TokenDic[key];
                    }
                }
                // 助詞が「は」ならば、代入文に変える
                if (tokens.CurrentToken.Josi == "は")
                {
                    tokens.CurrentToken.Josi = "";
                    tokens.InsertAfterCurrentToken(new NakoToken(NakoTokenType.EQ));
                }
                // コメントならばトークンから取り除く
                if (tokens.CurrentTokenType == NakoTokenType.COMMENT)
                {
                    tokens.RemoveCurrentToken();
                    continue;
                }
                tokens.MoveNext();
            }
        }
        /// <summary>
        /// 関数の定義
        /// </summary>
        private void TokenizeAnalize_DefFunction()
        {
            NakoToken firstToken = tokens.CurrentToken;
            tokens.MoveNext(); // skip '*' (DEF_FUNCTION)
            
            // 関数宣言を軽く舐めて、関数名を特定する
            NakoToken fnameToken = null;
            bool argMode = false;
            while (!tokens.IsEOF())
            {
                // 引数宣言に ( ... ) がある場合を考慮
                if (argMode)
                {
                    if (tokens.Accept(NakoTokenType.PARENTHESES_R))
                    {
                        tokens.MoveNext();
                        argMode = false;
                        continue;
                    }
                    tokens.MoveNext();
                    continue;
                }
                if (tokens.Accept(NakoTokenType.PARENTHESES_L))
                {
                    argMode = true;
                    tokens.MoveNext();
                    continue;
                }
                if (tokens.Accept(NakoTokenType.SCOPE_BEGIN))
                {
                    // 改行なら関数宣言の終了
                    if (fnameToken == null)
                    {
                        throw new NakoTokenizerException("関数宣言で関数名がありません。", firstToken);
                    }
                    // 関数名を辞書に登録する
                    TokenDic[fnameToken.getValueAsName()] = NakoTokenType.FUNCTION_NAME;
                    fnameToken.Type = NakoTokenType.FUNCTION_NAME;
                    break;
                }
                // 関数名の可能性
                if (tokens.Accept(NakoTokenType.WORD))
                {
                    fnameToken = tokens.CurrentToken;
                }
                tokens.MoveNext();
            }
        }
        /// <summary>
        /// トークンをひたすらぶった切る！
        /// </summary>
        private void TokenizeFirst()
        {
            // [注意]
            // このメソッドでは Init() を呼んではいけない @see: NakoTokenizer.Tokenize_ExtractSTring()
            // 文字列展開中に lineno がセットされる場合がある

            // はじめにインデントを数える
            indentCount = this.CountIndent();
            indentLevel = 0;
            
            // 繰り返しトークンを取得する
            while (!IsEOF())
            {
                NakoToken token = GetToken();
                if (token == null) continue;
                lastTokenType = token.Type;

                // 文字列の展開があればここで処理してしまう
                if (token.Type == NakoTokenType.STRING_EX)
                {
                    Tokenize_ExtractSTring(token);
                    continue;
                }
                tokens.Add(token);
            }

            // レベルが合うまで T_SCOPE_END を差し込む
            CheckScope();
        }
        private Stack<int> indentStack = null;
        private void CheckScope()
        {
            if (indentStack == null) {
                indentStack = new Stack<int>();
                indentStack.Push(indentCount); // set default indentCount == 0
            }

            int newIndent = CountIndent();
            if (newIndent == indentCount) return;
            if (newIndent > indentCount)
            {
                indentLevel++;
                NakoToken token = new NakoToken(NakoTokenType.SCOPE_BEGIN, lineNumber, indentLevel);
                tokens.Add(token);
                indentStack.Push(newIndent);
            }
            else
            {
                NakoToken t = new NakoToken(NakoTokenType.SCOPE_END, lineNumber, indentLevel);
                // 連続で POP する可能性がある
                while (true)
                {
                    int chk = indentStack.Peek();
                    if (chk == newIndent) break;
                    if (chk < newIndent)
                    {
                        throw new NakoTokenizerException("インデントレベルが間違っています。", t);
                    }
                    indentLevel--;
                    indentStack.Pop();
                    NakoToken token = new NakoToken(NakoTokenType.SCOPE_END, lineNumber, indentLevel);
                    tokens.Add(token);
                }
            }
            indentCount = newIndent;
        }
        /// <summary>
        /// トークンを１つ取得する
        /// </summary>
        /// <returns></returns>
        private NakoToken GetToken()
        {
            // カーソルが最後まで行ったか?
            if (IsEOF()) return null;

            // トークン１文字を取得
            NakoToken token = new NakoToken(NakoTokenType.UNKNOWN, lineNumber, indentLevel);
            char c = CurrentChar;
            char nc;

            // SWITCH
            switch (c) {
                // Check BOM
                case (char)0xFEFF:
                    position++;
                    return null;
                // Check EOL
                case '\r':
                    position++;
                    return null;
                case '\n':
                    token.Type = NakoTokenType.EOL;
                    position++;
                    lineNumber++;
                    tokens.Add(token);
                    CheckScope();
                    return null;
                // Check Indent
                case ' ':
                case '\t':
                    position++; // skip
                    return null;
                // 句読点
                case ';':
                    token.Type = NakoTokenType.EOL; // 明確な区切り
                    position++;
                    return token;
                case ',':
                    position++;
                    return null;
                // Check Flag
                case '=':
                    nc = NextChar;
                    if (nc == '=') {
                        position += 2;
                        token.Type = NakoTokenType.EQ_EQ;
                    }
                    else
                    {
                        position++;
                        token.Type = NakoTokenType.EQ;
                    }
                    return token;
                // Check Flag
                case '&':
                    nc = NextChar;
                    if (nc == '&')
                    {
                        position += 2;
                        token.Type = NakoTokenType.AND_AND;
                    }
                    else
                    {
                        position++;
                        token.Type = NakoTokenType.AND;
                    }
                    return token;
                case '|':
                    nc = NextChar;
                    if (nc == '|')
                    {
                        position += 2;
                        token.Type = NakoTokenType.OR_OR;
                    }
                    else
                    {
                        position++;
                        token.Type = NakoTokenType.OR;
                    }
                    return token;
                case '<':
                    nc = NextChar;
                    if (nc == '=')
                    {
                        position += 2;
                        token.Type = NakoTokenType.LT_EQ;
                    }
                    else if (nc == '>')
                    {
                        position += 2;
                        token.Type = NakoTokenType.NOT_EQ;
                    }
                    else
                    {
                        position++;
                        token.Type = NakoTokenType.LT;
                    }
                    return token;
                case '>':
                    nc = NextChar;
                    if (nc == '=')
                    {
                        position += 2;
                        token.Type = NakoTokenType.GT_EQ;
                    }
                    else if (nc == '<')
                    {
                        position += 2;
                        token.Type = NakoTokenType.NOT_EQ;
                    }
                    else
                    {
                        position++;
                        token.Type = NakoTokenType.GT;
                    }
                    return token;
                case '!':
                    nc = NextChar;
                    if (nc == '=')
                    {
                        position += 2;
                        token.Type = NakoTokenType.NOT_EQ;
                    }
                    else
                    {
                        position++;
                        token.Type = NakoTokenType.NOT;
                    }
                    return token;
                case '「':
                case '『':
                case '"':
                case '`':
                    return GetStringToken();
                case '+':
                    token.Type = NakoTokenType.PLUS;
                    position++;
                    return token;
                case '-':
                    token.Type = NakoTokenType.MINUS;
                    position++;
                    return token;
                case '*':
                    if (lastTokenType == NakoTokenType.EOL ||
                        lastTokenType == NakoTokenType.UNKNOWN)
                    {
                        token.Type = NakoTokenType.DEF_FUNCTION;
                    }
                    else
                    {
                        token.Type = NakoTokenType.MUL;
                    }
                    position++;
                    return token;
                case '/':
                    // コメントかチェック
                    nc = NextChar;
                    if (nc == '*') return GetRangeComment(token);
                    if (nc == '/') return GetLineComment(token);
                    // 割り算かな？
                    token.Type = NakoTokenType.DIV;
                    position++;
                    return token;
                case '%':
                    token.Type = NakoTokenType.MOD;
                    position++;
                    return token;
                case '^':
                    token.Type = NakoTokenType.POWER;
                    position++;
                    return token;
                case '(':
                    token.Type = NakoTokenType.PARENTHESES_L;
                    position++;
                    return token;
                case ')':
                    token.Type = NakoTokenType.PARENTHESES_R;
                    position++;
                    CheckJosi(token);
                    return token;
                case '{':
                    token.Type = NakoTokenType.BRACES_L;
                    position++;
                    return token;
                case '}':
                    token.Type = NakoTokenType.BRACES_R;
                    position++;
                    CheckJosi(token);
                    return token;
                case '[':
                    token.Type = NakoTokenType.BLACKETS_L;
                    position++;
                    return token;
                case ']':
                    token.Type = NakoTokenType.BLACKETS_R;
                    position++;
                    CheckJosi(token);
                    return token;
                case '\\':
                    token.Type = NakoTokenType.YEN;
                    position++;
                    return token;
                case '#':
                    return GetLineComment(token);
                default:
                    NakoToken tt = GetToken_NotFlag();
                    if (tt == null)
                    {
                        string msg = "未定義の文字列:";
                        char ch = CurrentChar;
                        if (ch < 33)
                        {
                            msg += string.Format("0x{0,0:X2}", (int)ch);
                        }
                        else
                        {
                            msg += "`" + ch + "`";
                        }
                        throw new NakoTokenizerException(msg, token);
                    }
                    return tt;
            }
        }
        /// <summary>
        /// 一行コメントを得る
        /// </summary>
        /// <param name="tok"></param>
        /// <returns></returns>
        private NakoToken GetLineComment(NakoToken tok)
        {
            tok.Type = NakoTokenType.COMMENT;
            
            char first_char = CurrentChar;
            position++;

            // 行末までスキップ
            string comment = "";
            while (!IsEOF())
            {
                char ch = CurrentCharRaw;
                if (ch == '\r' || ch == '\n') break;
                comment += ch;
                position++;
            }
            tok.Value = comment; // コメントの文字列をセット
            return tok;
        }
        /// <summary>
        /// 範囲コメントを得る
        /// </summary>
        /// <param name="tok"></param>
        /// <returns></returns>
        private NakoToken GetRangeComment(NakoToken tok)
        {
            tok.Type = NakoTokenType.COMMENT;

            char ch1 = CurrentChar; // = '/'
            char ch2 = NextChar;    // = '*'
            position += 2;
            
            // コメントの最後までを取得
            string comment = "";
            while (!IsEOF())
            {
                char ch = CurrentChar;
                if (ch == '\n')
                {
                    lineNumber++; // 行番号がずれてしまうので重要
                }
                if (ch == ch2)
                {
                    if (NextChar == ch1)
                    {
                        position += 2;
                        break;
                    }
                }
                comment += ch;
                position++;
            }
            tok.Value = comment; // コメントの文字列をセット
            return tok;
        }
        /// <summary>
        /// 文字列の取得
        /// </summary>
        /// <returns></returns>
        internal NakoToken GetStringToken()
        {
            if (IsEOF()) return null;
            var token = new NakoToken(NakoTokenType.STRING, lineNumber, indentLevel);
            char start = CurrentChar;
            char nc = NextChar;
            string stringEnd;

            // 終端文字列の判別
            // S = 「...」
            switch (start)
            {
                case '「': stringEnd = "」"; token.Type = NakoTokenType.STRING_EX; break;
                case '『': stringEnd = "』"; token.Type = NakoTokenType.STRING; break;
                case '"': stringEnd = "\""; token.Type = NakoTokenType.STRING_EX; break;
                case '`': stringEnd = "`"; token.Type = NakoTokenType.STRING; break;
                default: throw new NakoTokenizerException("", null);
            }
            position++;
            // ヒアドキュメント文字列
            // S = 「「 ... 」」
            // S = 「「「 ... 」」」
            if (start == '「' || start == '『')
            {
                position--;
                stringEnd = "";
                while (!IsEOF())
                {
                    if (CurrentChar == '「')
                    {
                        token.Type = NakoTokenType.STRING_EX;
                        stringEnd += '」';
                        position++;
                    }
                    else if (CurrentChar == '『')
                    {
                        token.Type = NakoTokenType.STRING;
                        stringEnd += '』';
                        position++;
                    }
                    else break;
                }
            }
            // 文字列の終端までスキャンする
            char c;
            var builder = new StringBuilder();
			bool isSkipBlank = false;
            while (!IsEOF())
            {
                if (CompareStr(stringEnd))
                {
                    position += stringEnd.Length;
                    break;
                }
                c = CurrentCharRaw;
                position++;

                if (isSkipBlank)
				{
					if (c == ' ' || c == '　' || c == '\t')
					{
						continue;
					}
					else
					{
                        isSkipBlank = false;
					}
				}

                builder.Append(c);
                if (c == '\n')
                {
                    lineNumber++;
                    isSkipBlank = true;
                }
            }
            token.Value = builder.ToString();
            CheckJosi(token);
            return token;
        }
        /// <summary>
        /// アルファベットか
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        private static bool IsAlpha(char c)
        {
            return ('a' <= c && c <= 'z') || ('A' <= c && c <= 'Z');
        }
        /// <summary>
        /// 数字か
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        private static bool IsNumber(char c)
        {
            return ('0' <= c && c <= '9');
        }
        /// <summary>
        /// ひらがなか
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool IsHira(char c)
        {
            return ('ぁ' <= c && c <= 'ん');
        }
        /// <summary>
        /// 記号以外か
        /// </summary>
        /// <returns></returns>
        private NakoToken GetToken_NotFlag()
        {
            char c = CurrentChar;
            // Number
            if (IsNumber(c))
            {
                return GetToken_Number();
            }
            // Word
            if (c > 0xFF || IsAlpha(c) || c == '_')
            {
                return GetToken_Word();
            }
            return null;
        }
        /// <summary>
        /// WORDトークンの取得
        /// </summary>
        /// <returns></returns>
        internal NakoToken GetToken_Word()
        {
            var token = new NakoToken(NakoTokenType.WORD, lineNumber, indentLevel);
            StringBuilder s = new StringBuilder();
            while (!IsEOF())
            {
                char c = CurrentChar;
                if (IsAlpha(c) || IsNumber(c) || c == '_' || c == '!' || c == '?')
                {
                    s.Append(c);
                    position++;
                    continue;
                }
                // 助詞なら区切る
                if (IsHira(c))
                {
                    if (CheckJosi(token)) break;
                }
                // 全角文字なら読む
                if (c >= 0xFF)
                {
                    s.Append(c);
                    position++;
					// 特別な予約語なら区切る
					if (s.ToString() == "もし" || s.ToString() == "ならば")
					{
						break;
					}
                    continue;
                }
                break;
            }
            token.Value = s.ToString();
            return token;
        }
        /// <summary>
        /// 助詞があるか調べる
        /// </summary>
        /// <param name="base_t">助詞をセットするトークン</param>
        /// <returns>助詞があったなら true を返す</returns>
        private bool CheckJosi(NakoToken base_t)
        {
            if (IsEOF()) return false;
            
            // 助詞はひらがなである
            char c = CurrentChar;
            if (!IsHira(c)) return false;

            // 助詞を１つずつ調べる
            NakoJosi JosiList = NakoJosi.Instance;
            foreach (string josi in JosiList)
            {
                if (this.CompareStr(josi))
                {
                    base_t.Josi = josi;
                    position += josi.Length;
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 数字の取得
        /// </summary>
        /// <returns></returns>
        internal NakoToken GetToken_Number()
        {
            NakoToken token = new NakoToken(NakoTokenType.INT, lineNumber, indentLevel);
            string s = "";
            while (!IsEOF())
            {
                char c = CurrentChar;
                if (!IsNumber(c)) break;
                s += c;
                position++;
            }
            if (CurrentChar == '.' && IsNumber(NextChar))
            {
                s += CurrentChar;
                token.Type = NakoTokenType.NUMBER;
                position++;
                while (!IsEOF())
                {
                    char c = CurrentChar;
                    if (!IsNumber(c)) break;
                    s += c;
                    position++;
                }
            }
            token.Value = s;
            CheckJosi(token);
            return token;
        }
        /// <summary>
        /// インデントを数える
        /// </summary>
        /// <returns></returns>
        internal int CountIndent()
        {
            int indent = 0;
            while (!IsEOF())
            {
                char c = this.source[position];
                if (c == ' ')
                {
                    indent++;
                    position++;
                    continue;
                }
                if (c == '\t')
                {
                    indent += 4;
                    position++;
                    continue;
                }
                if (c == '　')
                {
                    indent += 2;
                    position++;
                    continue;
                }
                break;
            }
            return indent;
        }
        /// <summary>
        /// 展開あり文字列を再帰的に展開する
        /// </summary>
        /// <param name="t"></param>
        public void Tokenize_ExtractSTring(NakoToken t)
        {
            string tmp = "";
            string s = t.Value;
            int i = 0;
            bool is_first = true;
            while (i < s.Length)
            {
                char c = s[i];
                if (c == '{' || c == '｛')
                {
                    if (is_first)
                    {
                        is_first = false;
                    }
                    else
                    {
                        tokens.Add(new NakoToken(NakoTokenType.AND, t.LineNumber, t.IndentLevel));
                    }
                    char eoc = (c == '{') ? '}' : '｝';
                    i++;
                    string str_ex = "";
                    while (i < s.Length)
                    {
                        if (s[i] == eoc)
                        {
                            i++;
                            break;
                        }
                        str_ex += s[i];
                        i++;
                    }
                    // Yen Mark method 文字列展開だけの特殊メソッド
                    if (str_ex.Length > 0 && str_ex[0] == '\\')
                    {
                        if (str_ex == "\\t")
                        {
                            tmp += '\t';
                            str_ex = "";
                        }
                        else if (str_ex == "\\r")
                        {
                            tmp += '\r';
                            str_ex = "";
                        }
                        else if (str_ex == "\\n")
                        {
                            tmp += '\n';
                            str_ex = "";
                        }
                        else if (IsNumber(str_ex[1])) // \0
                        {
                            str_ex = str_ex.Substring(1);
                            int i_ex = int.Parse(str_ex);
                            tmp += (char)i_ex;
                            str_ex = "";
                        }
                        else if (str_ex[1] == '$')
                        {
                            str_ex = "0x" + str_ex.Substring(2);
                            int i_ex = int.Parse(str_ex);
                            tmp += (char)i_ex;
                            str_ex = "";
                        }
                        else {
                        	new NakoTokenizerException("展開あり文字列内の利用できない`\\'メソッド:" + str_ex, t);
                        }
                    }
                    // 文字列展開だけの特殊メソッド
                    if (str_ex.Length == 1 && str_ex[0] == '~')
                    {
                    	tmp += "\r\n";
                        str_ex = "";
                    }
                    // string
                    NakoToken tt = new NakoToken(NakoTokenType.STRING, t.LineNumber, t.IndentLevel);
                    tt.Value = tmp;
                    tokens.Add(tt);
                    tmp = "";
                    if (str_ex != "")
                    {
	                    // &
                        tokens.Add(new NakoToken(NakoTokenType.AND, t.LineNumber, t.IndentLevel));
	                    // "("
                        tokens.Add(new NakoToken(NakoTokenType.PARENTHESES_L, t.LineNumber, t.LineNumber));
	                    // 再帰的にトークン解析を行う
	                    NakoTokenizer tok = new NakoTokenizer();
                        tok.Initialization();
                        tok.source = str_ex;
                        tok.lineNumber = t.LineNumber;
                        tok.indentLevel = t.IndentLevel;
	                    tok.TokenizeFirst(); // とりあえず区切るだけ
	                    foreach (NakoToken st in tok.tokens)
	                    {
	                        tokens.Add(st);
	                    }
	                    // ")"
                        tokens.Add(new NakoToken(NakoTokenType.PARENTHESES_R, t.LineNumber, t.LineNumber));
                    }
                    continue;
                }
                tmp += c;
                i++;
            }
            // 文字列が空でなければ
            if (tmp != "")
            {
                if (!is_first)
                {
                    // &
                    tokens.Add(new NakoToken(NakoTokenType.AND, t.LineNumber, t.IndentLevel));
                }
                // string
                NakoToken t3 = new NakoToken(NakoTokenType.STRING, t.LineNumber, t.IndentLevel);
                t3.Value = tmp;
                tokens.Add(t3);
            }
            else {
                // 必要なら空文字列を追加
                if (is_first)
                {
                    NakoToken t4 = new NakoToken(NakoTokenType.STRING, t.LineNumber, t.IndentLevel);
                    t4.Value = "";
                    tokens.Add(t4);
                }
            }
        }
    }
}
