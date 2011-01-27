using System;
using System.Collections.Generic;
using System.Text;

namespace Libnako.JPNCompiler.Tokenizer
{
    /// <summary>
    /// トークン解析機
    /// </summary>
    public class NakoTokenizer : NakoTokenizerBase
    {

        public NakoTokenizer(string source)
        {
            Init();
            this.source = source; 
        }

        public void Tokenize()
        {
            // 1回目の解析 --- トークンをひたすら区切る
            TokenizeFirst();
            // 2回目の解析 --- 関数宣言など辞書登録を行う
            TokenizeAnalize();
            // 3回目の解析 --- T_WORDを置き換える
            TokenizeCheckWord();
        }

        /// <summary>
        /// トークンをひたすらぶった切るだけ、文法を一切考慮しない
        /// </summary>
        public void splitWord()
        {
            // 繰り返しトークンを取得する
            while (!IsEOF())
            {
                NakoToken token = GetToken();
                if (token == null) continue;
                tokens.Add(token);
            }
        }


        /// <summary>
        /// 関数宣言など辞書登録を行う
        /// </summary>
        protected void TokenizeAnalize()
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

        protected void TokenizeCheckWord()
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
                        token.type = TokenDic[key];
                    }
                }
                // 助詞が「は」ならば、代入文に変える
                if (tokens.CurrentToken.josi == "は")
                {
                    tokens.CurrentToken.josi = "";
                    tokens.InsertAfterCurrentToken(new NakoToken(NakoTokenType.EQ));
                }
                tokens.MoveNext();
            }
        }
        
        protected void TokenizeAnalize_DefFunction()
        {
            NakoToken firstToken = tokens.CurrentToken;
            tokens.MoveNext(); // skip '*' (DEF_FUNCTION)
            
            // 関数宣言を軽く舐めて、関数名を特定する
            NakoToken fnameToken = null;
            Boolean argMode = false;
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
                    fnameToken.type = NakoTokenType.FUNCTION_NAME;
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
        protected void TokenizeFirst()
        {
            // [注意]
            // このメソッドでは Init() を呼んではいけない @see: NakoTokenizer.Tokenize_ExtractSTring()
            // 文字列展開中に lineno がセットされる場合がある

            // はじめにインデントを数える
            this.indentCount = this.CountIndent();
            this.level = 0;
            
            // 繰り返しトークンを取得する
            while (!IsEOF())
            {
                NakoToken token = GetToken();
                if (token == null) continue;
                last_token_type = token.type;

                // 文字列の展開があればここで処理してしまう
                if (token.type == NakoTokenType.STRING_EX)
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
                level++;
                NakoToken token = new NakoToken(NakoTokenType.SCOPE_BEGIN, lineno, level);
                tokens.Add(token);
                indentStack.Push(newIndent);
            }
            else
            {
                NakoToken t = new NakoToken(NakoTokenType.SCOPE_END, lineno, level);
                // 連続で POP する可能性がある
                while (true)
                {
                    int chk = indentStack.Peek();
                    if (chk == newIndent) break;
                    if (chk < newIndent)
                    {
                        throw new NakoTokenizerException("インデントレベルが間違っています。", t);
                    }
                    level--;
                    indentStack.Pop();
                    NakoToken token = new NakoToken(NakoTokenType.SCOPE_END, lineno, level);
                    tokens.Add(token);
                }
            }
            indentCount = newIndent;
        }

        public NakoToken GetToken()
        {
            // カーソルが最後まで行ったか?
            if (IsEOF()) return null;

            // トークン１文字を取得
            NakoToken token = new NakoToken(NakoTokenType.UNKNOWN, lineno, level);
            Char c = CurrentChar;
            Char nc;

            // SWITCH
            switch (c) {
                // Check EOL
                case '\r':
                    cur++;
                    return null;
                case '\n':
                    token.type = NakoTokenType.EOL;
                    cur++;
                    lineno++;
                    tokens.Add(token);
                    CheckScope();
                    return null;
                // Check Indent
                case ' ':
                case '\t':
                    cur++; // skip
                    return null;
                // 句読点
                case ';':
                    token.type = NakoTokenType.EOL; // 明確な区切り
                    cur++;
                    return token;
                case ',':
                    cur++;
                    return null;
                // Check Flag
                case '=':
                    nc = NextChar;
                    if (nc == '=') {
                        cur += 2;
                        token.type = NakoTokenType.EQ_EQ;
                    }
                    else
                    {
                        cur++;
                        token.type = NakoTokenType.EQ;
                    }
                    return token;
                // Check Flag
                case '&':
                    nc = NextChar;
                    if (nc == '&')
                    {
                        cur += 2;
                        token.type = NakoTokenType.AND_AND;
                    }
                    else
                    {
                        cur++;
                        token.type = NakoTokenType.AND;
                    }
                    return token;
                case '|':
                    nc = NextChar;
                    if (nc == '|')
                    {
                        cur += 2;
                        token.type = NakoTokenType.OR_OR;
                    }
                    else
                    {
                        cur++;
                        token.type = NakoTokenType.OR;
                    }
                    return token;
                case '<':
                    nc = NextChar;
                    if (nc == '=')
                    {
                        cur += 2;
                        token.type = NakoTokenType.LT_EQ;
                    }
                    else if (nc == '>')
                    {
                        cur += 2;
                        token.type = NakoTokenType.NOT_EQ;
                    }
                    else
                    {
                        cur++;
                        token.type = NakoTokenType.LT;
                    }
                    return token;
                case '>':
                    nc = NextChar;
                    if (nc == '=')
                    {
                        cur += 2;
                        token.type = NakoTokenType.GT_EQ;
                    }
                    else if (nc == '<')
                    {
                        cur += 2;
                        token.type = NakoTokenType.NOT_EQ;
                    }
                    else
                    {
                        cur++;
                        token.type = NakoTokenType.GT;
                    }
                    return token;
                case '!':
                    nc = NextChar;
                    if (nc == '=')
                    {
                        cur += 2;
                        token.type = NakoTokenType.NOT_EQ;
                    }
                    else
                    {
                        cur++;
                        token.type = NakoTokenType.NOT;
                    }
                    return token;
                case '「':
                case '『':
                case '"':
                case '`':
                    return GetToken_String();
                case '+':
                    token.type = NakoTokenType.PLUS;
                    cur++;
                    return token;
                case '-':
                    token.type = NakoTokenType.MINUS;
                    cur++;
                    return token;
                case '*':
                    if (last_token_type == NakoTokenType.EOL ||
                        last_token_type == NakoTokenType.UNKNOWN)
                    {
                        token.type = NakoTokenType.DEF_FUNCTION;
                    }
                    else
                    {
                        token.type = NakoTokenType.MUL;
                    }
                    cur++;
                    return token;
                case '/':
                    token.type = NakoTokenType.DIV;
                    cur++;
                    return token;
                case '%':
                    token.type = NakoTokenType.MOD;
                    cur++;
                    return token;
                case '^':
                    token.type = NakoTokenType.POWER;
                    cur++;
                    return token;
                case '(':
                    token.type = NakoTokenType.PARENTHESES_L;
                    cur++;
                    return token;
                case ')':
                    token.type = NakoTokenType.PARENTHESES_R;
                    cur++;
                    CheckJosi(token);
                    return token;
                case '{':
                    token.type = NakoTokenType.BRACES_L;
                    cur++;
                    return token;
                case '}':
                    token.type = NakoTokenType.BRACES_R;
                    cur++;
                    CheckJosi(token);
                    return token;
                case '[':
                    token.type = NakoTokenType.BLACKETS_L;
                    cur++;
                    return token;
                case ']':
                    token.type = NakoTokenType.BLACKETS_R;
                    cur++;
                    CheckJosi(token);
                    return token;
                case '\\':
                    token.type = NakoTokenType.YEN;
                    cur++;
                    return token;
                default:
                    NakoToken tt = GetToken_NotFlag();
                    if (tt == null)
                    {
                        String msg = "未定義の文字列:";
                        Char ch = CurrentChar;
                        if (ch < 33)
                        {
                            msg += String.Format("0x{0,0:X2}", (int)ch);
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

        public NakoToken GetToken_String()
        {
            if (IsEOF()) return null;
            NakoToken token = new NakoToken(NakoTokenType.STRING, lineno, level);
            Char c = CurrentChar;
            Char nc = NextChar;
            String eos = "」";
            Boolean is_extract = true;

            // 文字列
            // S = 「...」
            if (c != nc)
            {
                switch (c)
                {
                    case '「': eos = "」"; is_extract = true; break;
                    case '『': eos = "』"; is_extract = false; break;
                    case '"': eos = "\""; is_extract = true; break;
                    case '`': eos = "`"; is_extract = false; break;
                }
                cur++;
            }
            // ヒアドキュメント文字列
            // S = 「「 ... 」」
            // S = 「「「 ... 」」」
            if (c == nc && (c == '「' || c == '『'))
            {
                eos = "";
                while (!IsEOF())
                {
                    if (c == '「' && CurrentChar == '「')
                    {
                        is_extract = true;
                        eos += '」';
                        cur++;
                        continue;
                    }
                    if (c == '『' && CurrentChar == '『')
                    {
                        is_extract = false;
                        eos += '』';
                        cur++;
                        continue;
                    }
                    break;
                }
            }
            // 文字列の終端までスキャンする
            String str = "";
            while (!IsEOF())
            {
                if (CompareStr(eos))
                {
                    cur += eos.Length;
                    break;
                }
                c = CurrentCharRaw;
                str += c;
                cur++;
                if (c == '\n')
                {
                    lineno++;
                }
            }
            // Extract ?
            if (is_extract)
            {
                token.type = NakoTokenType.STRING_EX;
            }
            token.value = str;
            CheckJosi(token);
            return token;
        }

        public static Boolean IsAlpha(Char c)
        {
            return ('a' <= c && c <= 'z') || ('A' <= c && c <= 'Z');
        }

        public static Boolean IsNumber(Char c)
        {
            return ('0' <= c && c <= '9');
        }

        public static Boolean IsHira(Char c)
        {
            return ('ぁ' <= c && c <= 'ん');
        }

        private NakoToken GetToken_NotFlag()
        {
            Char c = CurrentChar;
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

        public NakoToken GetToken_Word()
        {
            NakoToken token = new NakoToken(NakoTokenType.WORD, lineno, level);
            String s = "";
            while (!IsEOF())
            {
                Char c = CurrentChar;
                if (IsAlpha(c) || IsNumber(c) || c == '_' || c == '!' || c == '?')
                {
                    s += c;
                    cur++;
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
                    s += c;
                    cur++;
                    continue;
                }
                break;
            }
            token.value = s;
            return token;
        }

        /// <summary>
        /// 助詞があるか調べる
        /// </summary>
        /// <param name="base_t">助詞をセットするトークン</param>
        /// <returns>助詞があったなら true を返す</returns>
        public Boolean CheckJosi(NakoToken base_t)
        {
            if (IsEOF()) return false;
            
            // 助詞はひらがなである
            Char c = CurrentChar;
            if (!IsHira(c)) return false;

            // 助詞を１つずつ調べる
            NakoJosi JosiList = NakoJosi.Instance;
            foreach (String josi in JosiList)
            {
                if (this.CompareStr(josi))
                {
                    base_t.josi = josi;
                    cur += josi.Length;
                    return true;
                }
            }
            return false;
        }

        public NakoToken GetToken_Number()
        {
            NakoToken token = new NakoToken(NakoTokenType.INT, lineno, level);
            String s = "";
            while (!IsEOF())
            {
                Char c = CurrentChar;
                if (!IsNumber(c)) break;
                s += c;
                cur++;
            }
            if (CurrentChar == '.' && IsNumber(NextChar))
            {
                s += CurrentChar;
                token.type = NakoTokenType.NUMBER;
                cur++;
                while (!IsEOF())
                {
                    Char c = CurrentChar;
                    if (!IsNumber(c)) break;
                    s += c;
                    cur++;
                }
            }
            token.value = s;
            CheckJosi(token);
            return token;
        }

        public int CountIndent()
        {
            int indent = 0;
            while (!IsEOF())
            {
                char c = this.source[cur];
                if (c == ' ')
                {
                    indent++;
                    cur++;
                    continue;
                }
                if (c == '\t')
                {
                    indent += 4;
                    cur++;
                    continue;
                }
                if (c == '　')
                {
                    indent += 2;
                    cur++;
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
            String tmp = "";
            String s = t.value;
            int i = 0;
            Boolean is_first = true;
            while (i < s.Length)
            {
                Char c = s[i];
                if (c == '{' || c == '｛')
                {
                    if (is_first)
                    {
                        is_first = false;
                    }
                    else
                    {
                        tokens.Add(new NakoToken(NakoTokenType.AND, t.lineno, t.level));
                    }
                    Char eoc = (c == '{') ? '}' : '｝';
                    i++;
                    String str_ex = "";
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
                            tmp += (Char)i_ex;
                            str_ex = "";
                        }
                        else if (str_ex[1] == '$')
                        {
                            str_ex = "0x" + str_ex.Substring(2);
                            int i_ex = int.Parse(str_ex);
                            tmp += (Char)i_ex;
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
                    NakoToken tt = new NakoToken(NakoTokenType.STRING, t.lineno, t.level);
                    tt.value = tmp;
                    tokens.Add(tt);
                    tmp = "";
                    if (str_ex != "")
                    {
	                    // &
	                    tokens.Add(new NakoToken(NakoTokenType.AND, t.lineno, t.level));
	                    // "("
	                    tokens.Add(new NakoToken(NakoTokenType.PARENTHESES_L, t.lineno, t.lineno));
	                    // 再帰的にトークン解析を行う
	                    NakoTokenizer tok = new NakoTokenizer(str_ex);
	                    tok.lineno = t.lineno;
	                    tok.level = t.level;
	                    tok.TokenizeFirst(); // とりあえず区切るだけ
	                    foreach (NakoToken st in tok.tokens)
	                    {
	                        tokens.Add(st);
	                    }
	                    // ")"
	                    tokens.Add(new NakoToken(NakoTokenType.PARENTHESES_R, t.lineno, t.lineno));
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
                    tokens.Add(new NakoToken(NakoTokenType.AND, t.lineno, t.level));
                }
                // string
                NakoToken t3 = new NakoToken(NakoTokenType.STRING, t.lineno, t.level);
                t3.value = tmp;
                tokens.Add(t3);
            }
            else {
                // 必要なら空文字列を追加
                if (is_first)
                {
                    NakoToken t4 = new NakoToken(NakoTokenType.STRING, t.lineno, t.level);
                    t4.value = "";
                    tokens.Add(t4);
                }
            }
        }
    }
}
