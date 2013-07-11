using System;
using System.Collections.Generic;
using System.Text;
using Libnako.JPNCompiler.Node;
using Libnako.JPNCompiler.Tokenizer;


namespace Libnako.JPNCompiler.Parser
{
    /// <summary>
    /// トークンを読み込んで構文木に変換するクラス NakoParser のための下処理用クラス
    /// NakoParser には、意味解析処理自体を記述し、このクラスで意味解析に必要な下処理を書く
    /// </summary>
    public class NakoParserBase
    {
        /// <summary>
        /// デバッグするかどうか
        /// </summary>
        public bool DebugMode = false;
        /// <summary>
        /// ノードのトップ
        /// </summary>
		public NakoNode topNode { get; set; }
        /// <summary>
        /// 親ノード
        /// </summary>
        protected NakoNode parentNode;
        /// <summary>
        /// 前回のノード
        /// </summary>
        protected NakoNode lastNode;
        /// <summary>
        /// トークン一覧
        /// </summary>
        protected NakoTokenList tok;
        /// <summary>
        /// 計算用スタック
        /// </summary>
        protected NakoNodeList calcStack;
        /// <summary>
        /// 計算用スタックのカウンタ
        /// </summary>
        protected Stack<int> calcStackCounters;
        /// <summary>
        /// 関数呼び出しフレームのスタック
        /// </summary>
		internal Stack<NakoParserFrame> frameStack { get; set; }
        /// <summary>
        /// スタック状態のスタック
        /// </summary>
		internal Stack<NakoParserNodeState> stateStack { get; set; }
        /// <summary>
        /// 値の設定時か
        /// </summary>
        protected bool flag_set_variable = false;
        /// <summary>
        /// ローカル変数
        /// </summary>
        public NakoVariableManager localVar { get; set; }
        /// <summary>
        /// グローバル変数
        /// </summary>
        public NakoVariableManager globalVar
        {
            get
            {
                if (_globalVar == null)
                {
                    throw new ApplicationException("グローバル変数が設定されていません!!");
                }
                return _globalVar;
            }
            set { _globalVar = value; }
        }
        private NakoVariableManager _globalVar = null;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="tokens"></param>
        public NakoParserBase(NakoTokenList tokens)
        {
            this.tok = tokens;
            tokens.MoveTop();
            parentNode = topNode = new NakoNode();
            frameStack = new Stack<NakoParserFrame>();
            stateStack = new Stack<NakoParserNodeState>();
            calcStack = new NakoNodeList();
            calcStackCounters = new Stack<int>();
            lastNode = null;
            localVar = new NakoVariableManager(NakoVariableScope.Local);
        }
        /// <summary>
        /// ログを書き出す
        /// </summary>
        /// <param name="s"></param>
        public void WriteLog(string s)
        {
            if (!DebugMode) return;
            Console.Write(s);
        }

        /// <summary>
        /// 値を１つだけ解析したい場合
        /// </summary>
        /// <returns></returns>
        public bool ParseOnlyValue()
        {
            lastNode = null;
            if (tok.IsEOF()) return false;
            if (!_value()) return false;
            topNode.AddChild(lastNode);
            return true;
        }
        
        /// <summary>
        /// 値の取得
        /// </summary>
        /// <returns></returns>
        protected virtual bool _value()
        {
        	return false;
        }

        /// <summary>
        /// 現在のトークンが指定したタイプと合致しているか
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        protected bool Accept(NakoTokenType type)
        {
            return (tok.CurrentTokenType == type);
        }
        /// <summary>
        /// トークンの位置を記録
        /// </summary>
        protected void TokenTry()
        {
            tok.Save();
            calcStackCounters.Push(calcStack.Count);
        }
        /// <summary>
        /// 記録した位置までトークンを戻す
        /// </summary>
        protected void TokenBack()
        {
            tok.Restore();
            int c = calcStackCounters.Pop();
            while (c < calcStack.Count)
            {
                calcStack.Pop();
            }
        }
        /// <summary>
        /// 記録したトークンをクリアする
        /// </summary>
        protected void TokenFinally()
        {
            tok.RemoveTop();
            calcStackCounters.Pop();
        }

        /// <summary>
        /// フレームを記録
        /// </summary>
        protected void PushFrame()
        {
            NakoParserFrame f = new NakoParserFrame();
            f.lastNode = lastNode;
            f.parentNode = parentNode;
            f.localVar = localVar;
            frameStack.Push(f);
        }

        /// <summary>
        /// フレームの復元
        /// </summary>
        protected void PopFrame()
        {
            NakoParserFrame f = frameStack.Pop();
            lastNode = f.lastNode;
            parentNode = f.parentNode;
            localVar = f.localVar;
        }
        
        /// <summary>
        /// 状態をプッシュ
        /// </summary>
        protected void PushNodeState()
        {
            NakoParserNodeState s = new NakoParserNodeState();
            s.lastNode = this.lastNode;
            s.parentNode = this.parentNode;
            stateStack.Push(s);
        }
        /// <summary>
        /// 状態をポップ
        /// </summary>
        protected void PopNodeState()
        {
            NakoParserNodeState s = stateStack.Pop();
            this.lastNode = s.lastNode;
            this.parentNode = s.parentNode;
        }

    }

    /// <summary>
    /// ノードの状態を表すクラス
    /// </summary>
    internal class NakoParserNodeState
    {
        /// <summary>
        /// 親ノード
        /// </summary>
		public NakoNode parentNode { get; set; }
        /// <summary>
        /// 最後に取り扱ったノード
        /// </summary>
		public NakoNode lastNode { get; set; }
    }
    /// <summary>
    /// フレームの状態を表すクラス
    /// </summary>
    internal class NakoParserFrame
    {
        /// <summary>
        /// ローカル変数
        /// </summary>
		public NakoVariableManager localVar { get; set; }
        /// <summary>
        /// 最後に使ったノード
        /// </summary>
		public NakoNode lastNode { get; set; }
        /// <summary>
        /// 親ノード
        /// </summary>
		public NakoNode parentNode { get; set; }
    }

}
