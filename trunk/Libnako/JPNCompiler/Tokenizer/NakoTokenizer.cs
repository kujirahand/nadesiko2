using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Libnako.JPNCompiler.Tokenizer
{
    /// <summary>
    /// ソースコードからトークンを解析するクラスです。
    /// </summary>
    public class NakoTokenizer
    {
        /// <summary>
        /// 生のソースコード。
        /// </summary>
        private string source;
        /// <summary>
        /// 現在の位置。
        /// </summary>
        private int position;
        /// <summary>
        /// 現在のインデントレベル。
        /// </summary>
        private int indentLevel;
        /// <summary>
        /// インデントレベルで使用するインデントの数。
        /// </summary>
        private int indentCount;
        /// <summary>
        /// 現在の行番号。
        /// </summary>
        private int lineNo;
        /// <summary>
        /// トークン一覧。
        /// </summary>
        private NakoTokenList tokens;
        /// <summary>
        /// 前回のトークンの種類。
        /// </summary>
        private NakoTokenType lastTokenType;
        /// <summary>
        /// トークン辞書。
        /// </summary>
        public NakoTokenDic tokenDic;
        /// <summary>
        /// インデントのスタック。
        /// </summary>
        private Stack<int> indentStack;
        /// <summary>
        /// NakoTokenizer クラスの新しいインスタンスを初期化します。
        /// </summary>
        public NakoTokenizer()
        {
            tokens = new NakoTokenList();
            indentStack = new Stack<int>();
        }
        /// <summary>
        /// トークン化メソッドを呼び出す前に初期化します。
        /// </summary>
        private void Initialization()
        {
            position = 0;
            indentLevel = 0;
            lineNo = 0;
            indentCount = 0;
            tokens.Clear();
            indentStack.Clear();
            indentStack.Push(indentCount); // デフォルトであるインデント、0 をプッシュします。
        }
        /// <summary>
        /// ソースコードが終端かどうかを示す値を取得します。
        /// </summary>
        /// <returns></returns>
        private bool IsEOF
        {
            get
            {
                return position >= source.Length;
            }
        }
        /// <summary>
        /// 現在の文字を半角整形して取得します。
        /// </summary>
        private char CurrentChar
        {
            get
            {
                return NakoUtility.ToHalfChar(CurrentCharRaw);
            }
        }
        /// <summary>
        /// 現在の文字を整形しないで取得します。
        /// </summary>
        private char CurrentCharRaw
        {
            get
            {
                if (IsEOF)
                {
                    return '\0';
                }
                return source[position];
            }
        }
        /// <summary>
        /// 次の文字を半角整形して取得します。
        /// </summary>
        private char NextChar
        {
            get
            {
                if (position + 1 >= source.Length)
                {
                    return '\0';
                }
                return NakoUtility.ToHalfChar(source[position + 1]);
            }
        }
        /// <summary>
        /// 指定したソースコードからトークンを解析します。
        /// </summary>
        /// <param name="source">解析するソースコード。</param>
        /// <param name="tokenDic">解析に使用する、トークン辞書。</param>
        /// <returns>指定したソースコードから解析した、トークン一覧。</returns>
        public NakoTokenList Tokenize(string source, NakoTokenDic tokenDic)
        {
            Initialization();
            this.source = source;
            this.tokenDic = tokenDic;
            SplitToToken();
            DefineFunction();
            CheckWord();
			Include ();
			CheckWord();//includeした後に関数が追加された可能性があるので、再チェック
            return tokens;
        }
        /// <summary>
        /// 指定したソースコードからトークンに区切るだけの解析をします。文法を一切考慮しません。
        /// </summary>
        /// <param name="source">解析するソースコード。</param>
        /// <param name="lineNo">開始する行番号。</param>
        /// <param name="indentLevel">開始するインデントレベル。</param>
        /// <returns>指定したソースコードから解析した、トークン一覧。</returns>
        public NakoTokenList TokenizeSplitOnly(string source, int lineNo, int indentLevel)
        {
            Initialization();
            this.source = source;
            this.indentLevel = indentLevel;
            this.lineNo = lineNo;
            // 繰り返しトークンを取得する
            while (!IsEOF)
            {
                var token = GetToken();
                if (token == null)
                {
                    continue;
                }
                lastTokenType = token.Type;
                tokens.Add(token);
            }
            return tokens;
        }
        /// <summary>
        /// トークンに区切ります。
        /// </summary>
        private void SplitToToken()
        {
            // 最初にインデントを数える
            indentCount = CountIndent();
            indentLevel = 0;
            // 繰り返しトークンを取得する
            while (!IsEOF)
            {
                var token = GetToken();
                if (token == null)
                {
                    lastTokenType = NakoTokenType.EOL;
                    continue;
                }
                lastTokenType = token.Type;
                // 展開あり文字列をここで展開
                if (token.Type == NakoTokenType.STRING_EX)
                {
                    foreach (var extractToken in StringTokenExtract(token))
                    {
                        tokens.Add(extractToken);
                    }
                }
                else tokens.Add(token);
            }
            // レベルが合うまで SCOPE_END を追加する
            CheckScope();
        }
        /// <summary>
        /// 関数宣言を辞書に登録します。
        /// </summary>
        private void DefineFunction()
        {
            tokens.MoveTop();
            for (; !tokens.IsEOF(); tokens.MoveNext())
            {
                if (tokens.CurrentTokenType == NakoTokenType.DEF_FUNCTION)
                {
                    var firstToken = tokens.CurrentToken;
                    // DEF_FUNCTION をスキップ
                    tokens.MoveNext();
                    // 関数名のトークン
                    NakoToken funcNameToken = null;
                    for (; !tokens.IsEOF(); tokens.MoveNext())
                    {
                        // 関数名
                        if (tokens.Accept(NakoTokenType.WORD))
                        {
                            funcNameToken = tokens.CurrentToken;
                        }
                        // 改行ならば関数宣言の終了
						else if (tokens.Accept(NakoTokenType.SCOPE_BEGIN) || tokens.Accept(NakoTokenType.PARENTHESES_L))
                        {
                            if (funcNameToken == null)
                            {
                                throw new NakoTokenizerException("関数宣言で関数名がありません。", firstToken);
                            }
                            // 関数名を辞書に登録
                            tokenDic[funcNameToken.GetValueAsName()] = NakoTokenType.FUNCTION_NAME;
                            funcNameToken.Type = NakoTokenType.FUNCTION_NAME;
                            break;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 予約後のチェックと代入文への変換作業を行います。
        /// </summary>
        private void CheckWord()
        {
            // 予約語句のチェックなど
            tokens.MoveTop();
            for (; !tokens.IsEOF(); tokens.MoveNext())
            {
                // 予約語句の置き換え
                if (tokens.CurrentTokenType == NakoTokenType.WORD)
                {
                    var token = tokens.CurrentToken;
                    string key = token.GetValueAsName();
                    if (tokenDic.ContainsKey(token.GetValueAsName()))
                    {
                        token.Type = tokenDic[key];
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
            }
        }
		/// <summary>
		/// INCLUDEのチェックとINCLUDEの実行。
		/// </summary>
		private void Include()
		{
			// INCLUDEのチェック
			tokens.MoveTop();
			NakoToken previousToken = tokens.CurrentToken;
			for (; !tokens.IsEOF(); tokens.MoveNext())
			{
				// TOKENを挿入
				if (tokens.CurrentTokenType == NakoTokenType.INCLUDE)
				{
					string filename = previousToken.Value;
					string source = File.ReadAllText (filename);
					NakoToken currentToken = tokens.CurrentToken;
					int index = tokens.IndexOf (currentToken);
					NakoTokenList includedTokens = new NakoTokenizer().Tokenize (source, this.tokenDic);
					int insertIndex = index;
					foreach (NakoToken tok in includedTokens) {
						insertIndex++;
						tokens.Insert (insertIndex, tok);
					}
					tokens.Remove (previousToken);
					tokens.Remove (currentToken);
				}
				previousToken = tokens.CurrentToken;
			}
		}
        /// <summary>
        /// 現在の位置からトークンを 1 つ取得します。
        /// </summary>
        /// <returns>取得したトークン。位置がソースコードの終端に達している場合は null。</returns>
        /// <remarks>
        /// このメソッドは取得したトークンの文字数だけ位置を進めます。
        /// </remarks>
        private NakoToken GetToken()
        {
            if (IsEOF)
            {
                return null;
            }
            var token = new NakoToken(NakoTokenType.UNKNOWN, lineNo, indentLevel);
            char nc;
            switch (CurrentChar)
            {
                // BOM かどうか確認
                case (char)0xFEFF:
                    position++;
                    return null;
                // 行末かどうか確認
                case '\r':
                    position++;
                    return null;
                case '\n':
                    token.Type = NakoTokenType.EOL;
                    position++;
                    lineNo++;
                    tokens.Add(token);
                    CheckScope();
                    return null;
                // インデントかどうか確認
                case ' ':
                case '\t':
                    position++; // skip
                    return null;
                // 句読点かどうか確認
                case ';':
                    token.Type = NakoTokenType.EOL; // 明確な区切り
                    position++;
                    return token;
                case ',':
                    position++;
                    return null;
                // 記号かどうか確認
                case '=':
                    nc = NextChar;
                    if (nc == '=')
                    {
                        position += 2;
                        token.Type = NakoTokenType.EQ_EQ;
                    }
                    else
                    {
                        position++;
                        token.Type = NakoTokenType.EQ;
                    }
                    return token;
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
                    // コメントかどうか確認
                    nc = NextChar;
                    if (nc == '*') return GetRangeCommentToken();
                    if (nc == '/') return GetLineCommentToken();
                    // 割り算かどうか確認
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
                    token.Type = NakoTokenType.BRACKETS_L;
                    position++;
                    return token;
                case ']':
                    token.Type = NakoTokenType.BRACKETS_R;
                    position++;
                    CheckJosi(token);
                    return token;
                case '\\':
                    token.Type = NakoTokenType.YEN;
                    position++;
                    return token;
                case '#':
                    return GetLineCommentToken();
                default:
                    token = GetNotFlagToken();
                    if (token.Type == NakoTokenType.UNKNOWN)
                    {
                        char ch = CurrentChar;
                        string msg = "未定義の文字列: " + (ch < 0x20 ? String.Format("0x{0,0:X2}", (int)ch) : "`" + ch + "`");
                        throw new NakoTokenizerException(msg, token);
                    }
                    return token;
            }
        }
        /// <summary>
        /// 現在の位置から一行コメントのトークンを 1 つ取得します。
        /// </summary>
        /// <returns>取得した一行コメントのトークン。</returns>
        /// <remarks>
        /// このメソッドは取得したトークンの文字数だけ位置を進めます。
        /// また、このメソッドを呼び出す前に位置を "//"、または "#" の前に設定することに注意して下さい。
        /// </remarks>
        private NakoToken GetLineCommentToken()
        {
            var token = new NakoToken(NakoTokenType.COMMENT, lineNo, indentLevel);
            if (CurrentChar == '/')
            {
                // "//" をスキップ
                position += 2;
            }
            else
            {
                // "#" をスキップ
                position++;
            }
            // 行末までスキップ
            string comment = "";
            while (!IsEOF)
            {
                char ch = CurrentCharRaw;
                if (ch == '\r' || ch == '\n') break;
                comment += ch;
                position++;
            }
            // コメントの文字列を設定
            token.Value = comment;
            return token;
        }
        /// <summary>
        /// 現在の位置から範囲コメントのトークンを 1 つ取得します。
        /// </summary>
        /// <returns>取得した範囲コメントのトークン。</returns>
        /// <remarks>
        /// このメソッドは取得したトークンの文字数だけ位置を進めます。
        /// また、このメソッドを呼び出す前に位置を "/*" の前に設定することに注意して下さい。
        /// </remarks>
        private NakoToken GetRangeCommentToken()
        {
            var token = new NakoToken(NakoTokenType.COMMENT, lineNo, indentLevel);
            // "/*" をスキップ
            position += 2;
            // コメントの最後までを取得
            string comment = "";
            while (!IsEOF)
            {
                char ch = CurrentChar;
                if (ch == '\n')
                {
                    lineNo++; // 行番号がずれてしまうので重要
                }
                if (ch == '*' && NextChar == '/')
                {
                    position += 2;
                    break;
                }
                comment += ch;
                position++;
            }
            // コメントの文字列を設定
            token.Value = comment;
            return token;
        }
        /// <summary>
        /// 現在の位置から文字列のトークンを 1 つ取得します。
        /// </summary>
        /// <returns>取得した文字列のトークン。</returns>
        /// <remarks>
        /// このメソッドは取得したトークンの文字数だけ位置を進めます。
        /// また、このメソッドを呼び出す前に位置を '「'、'『'、'"'、または '`' の前に設定することに注意して下さい。
        /// </remarks>
        private NakoToken GetStringToken()
        {
            if (IsEOF) return null;
            var token = new NakoToken(NakoTokenType.STRING, lineNo, indentLevel);
            char start = CurrentChar;
            string stringEnd;
            // 終端文字列の判別
            // S = 「...」
            switch (start)
            {
                case '「': stringEnd = "」"; token.Type = NakoTokenType.STRING_EX; break;
                case '『': stringEnd = "』"; token.Type = NakoTokenType.STRING; break;
                case '"': stringEnd = "\""; token.Type = NakoTokenType.STRING_EX; break;
                case '`': stringEnd = "`"; token.Type = NakoTokenType.STRING; break;
                default: throw new NakoTokenizerException(/*TODO*/);
            }
            position++;
            // ヒアドキュメント文字列
            // S = 「「 ... 」」
            // S = 「「「 ... 」」」
            if (start == '「' || start == '『')
            {
                position--;
                stringEnd = "";
                while (!IsEOF)
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
            // 文字列の終端まで取得
            var builder = new StringBuilder();
            bool isSkipBlank = false; // 空白のスキップを行うかどうか
            while (!IsEOF)
            {
                if (Equals(stringEnd))
                {
                    position += stringEnd.Length;
                    break;
                }
                char c = CurrentCharRaw;
                position++;
                if (isSkipBlank)
                {
                    if (c == ' ' || c == '　' || c == '\t')
                    {
                        continue;
                    }
                    isSkipBlank = false;
                }
                if (c == '\n')
                {
                    lineNo++;
                    isSkipBlank = true;
                }
                builder.Append(c);
            }
            token.Value = builder.ToString();
            CheckJosi(token);
            return token;
        }
        /// <summary>
        /// 現在の位置から記号以外のトークンを 1 つ取得します。
        /// </summary>
        /// <returns>取得した記号以外のトークン。未定義の文字列が検出された場合は不明なトークン。</returns>
        /// <remarks>
        /// このメソッドは取得したトークンの文字数だけ位置を進めます。
        /// </remarks>
        private NakoToken GetNotFlagToken()
        {
            char c = CurrentChar;
            // 数字の場合
            if (NakoUtility.IsNumber(c))
            {
                return GetNumberToken();
            }
            // 単語の場合
            if (c > 0xFF || NakoUtility.IsAlpha(c) || c == '_')
            {
                return GetWordToken();
            }
            return new NakoToken(NakoTokenType.UNKNOWN, lineNo, indentLevel);
        }
        /// <summary>
        /// 現在の位置から数字のトークンを 1 つ取得します。
        /// </summary>
        /// <returns>取得した数字のトークン。</returns>
        /// <remarks>
        /// このメソッドは取得したトークンの文字数だけ位置を進めます。
        /// </remarks>
        private NakoToken GetNumberToken()
        {
            var token = new NakoToken(NakoTokenType.INT, lineNo, indentLevel);
            string str = "";
            for (; !IsEOF; position++)
            {
                char ch = CurrentChar;
                if (!NakoUtility.IsNumber(ch)) break;
                str += ch;
            }
            if (CurrentChar == '.' && NakoUtility.IsNumber(NextChar))
            {
                str += CurrentChar;
                token.Type = NakoTokenType.NUMBER;
                position++;
                for (; !IsEOF; position++)
                {
                    char ch = CurrentChar;
                    if (!NakoUtility.IsNumber(ch)) break;
                    str += ch;

                }
            }
            token.Value = str;
            CheckJosi(token);
            return token;
        }
        /// <summary>
        /// 現在の位置から単語のトークンを 1 つ取得します。
        /// </summary>
        /// <returns>取得した単語のトークン。</returns>
        /// <remarks>
        /// このメソッドは取得したトークンの文字数だけ位置を進めます。
        /// </remarks>
        private NakoToken GetWordToken()
        {
            var token = new NakoToken(NakoTokenType.WORD, lineNo, indentLevel);
            var builder = new StringBuilder();
            while (!IsEOF)
            {
                char c = CurrentChar;
                if (NakoUtility.IsAlpha(c) || NakoUtility.IsNumber(c) || c == '_' || c == '!' || c == '?')
                {
                    builder.Append(c);
                    position++;
                    continue;
                }
                // 助詞なら区切る
                if (NakoUtility.IsHiragana(c))
                {
                    if (CheckJosi(token)) break;
                }
                // 全角文字なら読む
                if (c >= 0xFF)
                {
                    builder.Append(c);
                    position++;
                    // 特別な予約語なら区切る
                    if (builder.ToString() == "もし" || builder.ToString() == "ならば")
                    {
                        break;
                    }
                    continue;
                }
                break;
            }
            token.Value = builder.ToString();
            return token;
        }
        /// <summary>
        /// 展開あり文字列トークンを再帰的に展開します。
        /// </summary>
        /// <param name="token">展開あり文字列トークン。</param>
        /// <returns>再帰的に展開したトークン一覧。</returns>
        private static NakoTokenList StringTokenExtract(NakoToken token)
        {
            var tokens = new NakoTokenList();
            string tmp = "";
            string str = token.Value;
            int i = 0;
            bool isFirst = true;
            while (i < str.Length)
            {
                char ch = str[i];
                if (ch == '{' || ch == '｛')
                {
                    if (isFirst)
                    {
                        isFirst = false;
                    }
                    else
                    {
                        // "&" トークンを追加
                        tokens.Add(new NakoToken(NakoTokenType.AND, token.LineNo, token.IndentLevel));
                    }
                    i++;
                    // 展開する文字列 ("{" と "}" との間) を取得する
                    string exString = "";
                    {
                        char end = (ch == '{') ? '}' : '｝';
                        for (; i < str.Length; i++)
                        {
                            if (str[i] == end)
                            {
                                i++;
                                break;
                            }
                            exString += str[i];
                        }
                    }
                    // 文字列展開だけの特殊メソッド ('\' メソッド)
                    if (exString.Length > 0 && exString[0] == '\\')
                    {
                        if (exString == "\\t")
                        {
                            // \t の場合
                            tmp += '\t';
                            exString = "";
                        }
                        else if (exString == "\\r")
                        {
                            // \r の場合
                            tmp += '\r';
                            exString = "";
                        }
                        else if (exString == "\\n")
                        {
                            // \n の場合
                            tmp += '\n';
                            exString = "";
                        }
                        else if (NakoUtility.IsNumber(exString[1]))
                        {
                            // \0 のような場合
                            exString = exString.Substring(1);
                            tmp += (char)int.Parse(exString);
                            exString = "";
                        }
                        else if (exString[1] == '$')
                        {
                            // \$00 のような場合
                            exString = "0x" + exString.Substring(2);
                            tmp += (char)int.Parse(exString);
                            exString = "";
                        }
                        else
                        {
                            new NakoTokenizerException("展開あり文字列内の利用できない`\\'メソッド:" + exString, token);
                        }
                    }
                    // 文字列展開だけの特殊メソッド ('~' メソッド)
                    else if (exString.Length == 1 && exString[0] == '~')
                    {
                        tmp += "\r\n";
                        exString = "";
                    }
                    // 文字列トークンを追加
                    tokens.Add(new NakoToken(NakoTokenType.STRING, token.LineNo, token.IndentLevel, tmp));
                    tmp = "";
                    if (exString != "")
                    {
                        // "&" トークンを追加
                        tokens.Add(new NakoToken(NakoTokenType.AND, token.LineNo, token.IndentLevel));
                        // "(" トークンを追加
                        tokens.Add(new NakoToken(NakoTokenType.PARENTHESES_L, token.LineNo, token.IndentLevel));
                        // 再帰的にトークンを解析
                        var innerTokens = new NakoTokenizer().TokenizeSplitOnly(exString, token.LineNo, token.IndentLevel); // とりあえず区切るだけ
                        foreach (var innerToken in innerTokens)
                        {
                            tokens.Add(innerToken);
                        }
                        // ")" トークンを追加
                        tokens.Add(new NakoToken(NakoTokenType.PARENTHESES_R, token.LineNo, token.IndentLevel));
                    }
                    continue;
                }
                tmp += ch;
                i++;
            }
            if (tmp != "")
            {
                if (!isFirst)
                {
                    // "&" トークンを追加
                    tokens.Add(new NakoToken(NakoTokenType.AND, token.LineNo, token.IndentLevel));
                }
                // 文字列トークンを追加
                tokens.Add(new NakoToken(NakoTokenType.STRING, token.LineNo, token.IndentLevel, tmp));
            }
            else
            {
                // 必要なら空文字列トークンを追加
                if (isFirst)
                {
                    tokens.Add(new NakoToken(NakoTokenType.STRING, token.LineNo, token.IndentLevel, ""));
                }
            }
            // 助詞をコピー
            tokens[tokens.Count - 1].Josi = token.Josi;
            return tokens;
        }
        /// <summary>
        /// 現在の位置からインデントの個数をカウントします。
        /// </summary>
        /// <returns>インデントの個数。</returns>
        /// <remarks>
        /// このメソッドは、インデントの文字数だけ位置を進めます。
        /// </remarks>
        private int CountIndent()
        {
            int indent = 0;
            for (; !IsEOF; position++)
            {
                switch (CurrentCharRaw)
                {
                    case ' ':
                        indent++;
                        break;
                    case '\t':
                        indent += 4;
                        break;
                    case '　':
                        indent += 2;
                        break;
                    default: return indent;
                }
            }
            return indent;
        }
        /// <summary>
        /// 現在の位置のスコープを確認します。もし、スコープに変更がある場合は、インデントレベルを変更し、スコープのトークンを追加します。
        /// </summary>
        private void CheckScope()
        {
            int newIndentCount = CountIndent();
            if (newIndentCount == indentCount)
            {
                return;
            }
            if (newIndentCount > indentCount)
            {
                indentLevel++;
                tokens.Add(new NakoToken(NakoTokenType.SCOPE_BEGIN, lineNo, indentLevel));
                indentStack.Push(newIndentCount);
            }
            else
            {
                var errorToken = new NakoToken(NakoTokenType.SCOPE_END, lineNo, indentLevel);
                // 連続で POP する可能性がある
                while (true)
                {
                    int check = indentStack.Peek();
                    if (check == newIndentCount)
                    {
                        break;
                    }
                    if (check < newIndentCount)
                    {
                        throw new NakoTokenizerException("インデントレベルが間違っています。", errorToken);
                    }
                    indentLevel--;
                    indentStack.Pop();
                    tokens.Add(new NakoToken(NakoTokenType.SCOPE_END, lineNo, indentLevel));
                }
            }
            indentCount = newIndentCount;
        }
        /// <summary>
        /// 現在の位置から助詞が存在するかどうかを調査します。もし助詞が存在する場合は、指定したトークンに助詞を設定します。
        /// </summary>
        /// <param name="token">助詞を設定するトークン。</param>
        /// <returns>助詞が存在する場合は true。それ以外の場合は false。</returns>
        /// <remarks>
        /// このメソッドは、助詞が存在する場合、助詞の文字数だけ位置を進めます。
        /// </remarks>
        private bool CheckJosi(NakoToken token)
        {
            if (IsEOF)
            {
                return false;
            }
            // 助詞はひらがななので
            if (!NakoUtility.IsHiragana(CurrentChar))
            {
                return false;
            }
            // 助詞を 1 つずつ調べる
            foreach (string josi in NakoJosi.Instance)
            {
                if (this.Equals(josi))
                {
                    token.Josi = josi;
                    position += josi.Length;
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// このインスタンスの現在の位置からのソースと、指定した文字列が同一かどうかを判断します。
        /// </summary>
        /// <param name="value">System.String。</param>
        /// <returns>value パラメータの値が、このインスタンスの現在の位置からのソースと同じ場合は true。それ以外の場合は false。</returns>
        private bool Equals(string value)
        {
            if (source.Length < position + value.Length)
            {
                return false;
            }
            return source.Substring(position, value.Length) == value;
        }
    }
}
