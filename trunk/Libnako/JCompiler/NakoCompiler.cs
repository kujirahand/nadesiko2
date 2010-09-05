﻿using System;
using System.Collections.Generic;
using System.Text;
using Libnako.JCompiler.Node;
using Libnako.JCompiler.Tokenizer;
using Libnako.JCompiler.ILWriter;
using Libnako.JCompiler.Parser;
using Libnako.NakoAPI;

namespace Libnako.JCompiler
{
    public class NakoCompiler
    {
		public String name { get; set; }
		public String fullpath { get; set; }
		public String source { get; set; }
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

        public NakoCompiler(String source = null)
        {
            this.source = source;
            // システムの初期化
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
        }

        /// <summary>
        /// 字句解析と構文解析を一気に行う
        /// </summary>
        /// <param name="source">必要なら新たにソースを指定</param>
        public NakoILCodeList Publish(String source = null)
        {
            if (source != null)
            {
                this.source = source;
            }
            Tokenize();
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
            // トークンに予約語句を追加
            NakoReservedWord.Init(TokenDic);
            // APIをBankに登録
            NakoBaseSystem baseSystem = NakoBaseSystem.Instance;
            // Bankをシステムに登録
            NakoAPIFuncBank.Instance.RegisterToSystem(TokenDic, GlobalVar);
        }
    }
}
