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
    public class NakoCompiler
    {
        public NakoCompilerLoaderInfo LoaderInfo { get; set; }
        public String name { get; set; }
        public String fullpath { get; set; }
        public String source { get; set; }
        public bool DebugMode { get; set; }
        // token
        protected NakoTokenList tokens = null;
        public NakoTokenList Tokens
        {
            get { return tokens; }
        }
        // node
        protected NakoNode topNode = null;
        public NakoNode TopNode
        {
            get { return topNode; }
        }
        // ilcode
        protected NakoILCodeList codes = null;
        public NakoILCodeList Codes
        {
            get { return codes; }
        }
        // global variables
        public readonly NakoVariableManager GlobalVar = new NakoVariableManager(NakoVariableScope.Global);
        // NakoTokenDic
        public readonly NakoTokenDic TokenDic = new NakoTokenDic();

        public NakoCompiler()
        {
            // システムの初期化
            LoaderInfo = new NakoCompilerLoaderInfo();
            LoaderInfo.Init();
            RegisterSysCall();
        }
        
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

        public void ParseOnlyValue()
        {
            NakoParser paser = new NakoParser(tokens);
            paser.ParseOnlyValue();
            this.topNode = paser.topNode;
        }

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
        
        private static bool FlagPluginLoaded = false;
        protected void LoadPlugins()
        {
        	// Was the plug-in loaded?
        	if (FlagPluginLoaded) return;
        	FlagPluginLoaded = true;
        	
            NakoPluginLoader loader = new NakoPluginLoader();
            loader.LoadPlugins();
        }
    }
    
    public class NakoCompilerLoaderInfo
    {
        public string source = "";
        public INakoPlugin[] PreloadModules = null;
        public INakoPlugin[] ImportantModules = null;
        public bool UsePlugins { get; set; }
        
        public NakoCompilerLoaderInfo()
        {
        }
        
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
    
    public class NakoCompilerException : ApplicationException
    {
        public NakoCompilerException(string msg) : base(msg)
        {
        }
    }
}
