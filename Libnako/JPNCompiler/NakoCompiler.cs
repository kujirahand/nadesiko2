using System;
using System.Collections.Generic;
using System.Text;
using Libnako.JPNCompiler.Node;
using Libnako.JPNCompiler.Tokenizer;
using Libnako.JPNCompiler.ILWriter;
using Libnako.JPNCompiler.Parser;
using Libnako.NakoAPI;
using NakoPlugin;

namespace Libnako.JPNCompiler
{
    /// <summary>
    /// なでしこコンパイラ・クラス
    /// </summary>
    public class NakoCompiler
    {
        /// <summary>
        /// 読み込み情報
        /// </summary>
        public NakoCompilerLoaderInfo LoaderInfo { get; set; }
        /// <summary>
        /// 名前
        /// </summary>
        public String name { get; set; }
        /// <summary>
        /// フルパス
        /// </summary>
        public String fullpath { get; set; }
        /// <summary>
        /// ソースコード文字列
        /// </summary>
        public String source { get; set; }
        /// <summary>
        /// デバッグモード
        /// </summary>
        public bool DebugMode { get; set; }
        /// <summary>
        /// トークンの一覧(内部で使用)
        /// </summary>
        protected NakoTokenList tokens = null;
        /// <summary>
        /// トークンの一覧
        /// </summary>
        public NakoTokenList Tokens
        {
            get { return tokens; }
        }
        /// <summary>
        /// ノード(内部で使用)
        /// </summary>
        protected NakoNode topNode = null;
        /// <summary>
        /// ノード
        /// </summary>
        public NakoNode TopNode
        {
            get { return topNode; }
        }
        /// <summary>
        /// 生成された中間コード(内部で使用)
        /// </summary>
        protected NakoILCodeList codes = null;
        /// <summary>
        /// 生成された中間コード
        /// </summary>
        public NakoILCodeList Codes
        {
            get { return codes; }
        }
        /// <summary>
        /// グローバル変数
        /// </summary>
        public readonly NakoVariableManager GlobalVar = new NakoVariableManager(NakoVariableScope.Global);
        /// <summary>
        /// なでしこ単語辞書
        /// </summary>
        public readonly NakoTokenDic TokenDic = new NakoTokenDic();

        /// <summary>
        /// constructor
        /// </summary>
        public NakoCompiler()
        {
            // システムの初期化
            LoaderInfo = new NakoCompilerLoaderInfo();
            LoaderInfo.Init();
            RegisterSysCall();
        }
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="info"></param>
        public NakoCompiler(NakoCompilerLoaderInfo info)
        {
            if (info == null)
            {
            	LoaderInfo = new NakoCompilerLoaderInfo();
            	LoaderInfo.Init();
            }
            
            this.LoaderInfo = info;
            if (info.source != null) {
                this.source = info.source;
            }
            RegisterSysCall();
        }

        /// <summary>
        /// 字句解析(トークンの分割)を行う
        /// 結果は Tokens で得られる
        /// </summary>
        public void Tokenize()
        {
            NakoTokenizer tok = new NakoTokenizer(source);
            tok.TokenDic = this.TokenDic;
            tok.Tokenize();
            tokens = tok.Tokens;
        }

        /// <summary>
        /// トークンの分割が行われた Tokens に対して、構文解析を行う
        /// 結果は TopNode に得られる
        /// </summary>
        public void Parse()
        {
            NakoParser parser = new NakoParser(tokens);
            parser.DebugMode = this.DebugMode;
            parser.globalVar = this.GlobalVar;
            parser.Parse();
            this.topNode = parser.topNode;
        }

        /// <summary>
        /// 構文解析の行われた TopNode に対して、ILコードの発行を行う
        /// 結果は、Codes に得られる
        /// </summary>
        public void WriteIL()
        {
            NakoILWriter w = new NakoILWriter();
            w.Write(this.topNode);
            codes = w.Result;
            codes.globalVar = this.GlobalVar;
        }

        /// <summary>
        /// 字句解析と構文解析を一気に行う
        /// </summary>
        /// <param name="source">必要なら新たにソースを指定</param>
        public NakoILCodeList Publish(String source)
        {
            if (source != null)
            {
                this.source = source;
            }
            Tokenize();
            // Console.WriteLine(this.Tokens.toTypeString());
            Parse();
            WriteIL();
            return Codes;
        }

        /// <summary>
        /// Publish メソッドを変数風に呼ぶセッター
        /// </summary>
        public string DirectSource
        {
            set { this.Publish(value); }
        }

        /// <summary>
        /// パース
        /// </summary>
        public void ParseOnlyValue()
        {
            NakoParser paser = new NakoParser(tokens);
            paser.ParseOnlyValue();
            this.topNode = paser.topNode;
        }

        /// <summary>
        /// システム関数を登録する
        /// </summary>
        protected void RegisterSysCall()
        {
            NakoAPIFuncBank bank = NakoAPIFuncBank.Instance;
            
            // (1) LoaderInfo で指定のプラグインのみプリロード
            if (LoaderInfo.PreloadModules != null)
            {
                foreach (INakoPlugin plugin in LoaderInfo.PreloadModules)
                {
                    string fullname = plugin.GetType().FullName;
                    if (!bank.PluginList.ContainsKey(fullname))
                    {
                        bank.PluginList[fullname] = plugin;
                        bank.SetPluginInstance(plugin);
                        plugin.DefineFunction(bank);
                    }
                }
            }
            
            // (2) プラグインを登録
            LoadPlugins();
            
            // (3) 重要プラグインをロード
            if (LoaderInfo.ImportantModules != null)
            {
                foreach (INakoPlugin plugin in LoaderInfo.ImportantModules)
                {
                    string fullname = plugin.GetType().FullName;
                    if (!bank.PluginList.ContainsKey(fullname)) {
                        bank.PluginList[fullname] = plugin;
                        bank.SetPluginInstance(plugin);
                        plugin.DefineFunction(bank);
                    }
                }
            }
            
            // --- 各種登録作業 ---
            // トークンに予約語句を追加
            NakoReservedWord.Init(TokenDic);
            // 使用識別フラグをリセット
            bank.ResetUsedFlag();
            // Global変数とシステム辞書に単語を登録
            NakoAPIFuncBank.Instance.RegisterToSystem(TokenDic, GlobalVar);
        }
        
        /// <summary>
        /// プラグインを読み込む
        /// </summary>
        protected void LoadPlugins()
        {
        	// Was the plug-in loaded?
        	if (FlagPluginLoaded) return;
        	FlagPluginLoaded = true;
        	
            NakoPluginLoader loader = new NakoPluginLoader();
            loader.LoadPlugins();
        }
        /// <summary>
        /// プラグインを読み込んだどうかのフラグ
        /// </summary>
        private static bool FlagPluginLoaded = false;
    }
    
    /// <summary>
    /// コンパイラに与えるのローダーオプション情報
    /// </summary>
    public class NakoCompilerLoaderInfo
    {
        /// <summary>
        /// ソースコード
        /// </summary>
        public string source = "";
        /// <summary>
        /// 読み込んでいるモジュール一覧
        /// </summary>
        public INakoPlugin[] PreloadModules = null;
        /// <summary>
        /// 重要なモジュール
        /// </summary>
        public INakoPlugin[] ImportantModules = null;
        /// <summary>
        /// プラグインを利用するかどうか
        /// </summary>
        public bool UsePlugins { get; set; }
        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public NakoCompilerLoaderInfo()
        {
        }
        
        /// <summary>
        /// 初期化
        /// </summary>
        public void Init()
        {
            source = "";
            PreloadModules = new INakoPlugin[] {
                new NakoBaseSystem(),
                new NakoPluginArray(),
                new NakoPluginString()
            };
        }
    }
    
    /// <summary>
    /// コンパイラの例外クラス
    /// </summary>
    public class NakoCompilerException : ApplicationException
    {
        /// <summary>
        /// コンパイラ例外を出す
        /// </summary>
        /// <param name="msg"></param>
        public NakoCompilerException(string msg) : base(msg)
        {
        }
    }
}
