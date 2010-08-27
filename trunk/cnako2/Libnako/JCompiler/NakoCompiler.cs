using System;
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
        public String name;
        public String fullpath;
        public String source;
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

        public void ParseOnlyValue()
        {
            NakoParser paser = new NakoParser(tokens);
            paser.ParseOnlyValue();
            this.topNode = paser.topNode;
        }

        protected void RegisterSysCall()
        {
            // APIをBankに登録
            NakoBaseSystem.Instance.registerToSystem();
            // Bankをシステムに登録
            NakoAPIFuncBank.Instance.RegisterToSystem();
        }
    }
}
