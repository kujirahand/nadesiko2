using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Libnako.JPNCompiler;
using Libnako.Interpreter;
using Libnako.JPNCompiler.ILWriter;
using Libnako.JPNCompiler.Tokenizer;
using Libnako.JPNCompiler.Parser;
using Libnako.NakoAPI;
using NakoPlugin;

namespace cnako2
{
    /// <summary>
    /// cnakoでコマンドラインを解析して実行まで行うクラス
    /// </summary>
    public class CNako2Executor
    {
        /// <summary>
        /// （デバッグ用）中間コード生成までの解説を表示するか
        /// </summary>
        public bool DescriptMode = false;
        /// <summary>
        /// （デバッグ用）実行時に経過を表示するか
        /// </summary>
        public bool DebugMode = false;
        /// <summary>
        /// プログラム終了後、Enterが押されるのを待つかどうか
        /// </summary>
        public bool WaitMode = false;
        /// <summary>
        /// cnakoの実行モード
        /// </summary>
        public NakoConsoleMode runMode = NakoConsoleMode.RunFile;
        /// <summary>
        /// 実行するソースコード
        /// </summary>
        public string source = null;
        /// <summary>
        /// ログ出力を使うかどうか
        /// </summary>
        public bool UseLog = false;
        /// <summary>
        /// プログラムの実行後に出力されたログが設定される
        /// </summary>
        public string PrintLog = null;
        /// <summary>
        /// コマンドライン引数を覚えておく
        /// </summary>
        private string[] args = null;

        /// <summary>
        /// cnakoのヘルプを表示する
        /// </summary>
        public void ShowHelp()
        {
            Console.WriteLine("# [CNAKO2]");
            Console.WriteLine("# USAGE:");
            Console.WriteLine("# >cnako2 [-desc][-debug][-wait][-e (code)]|[(sourcefile)]");
            Console.WriteLine("# Example:");
            Console.WriteLine("# >cnako2 (sourcefile)");
            Console.WriteLine("# >cnako2 -e (one liner code)");
        }

        /// <summary>
        /// コマンドライン引数を解析し、オプションを設定する
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public bool SetOptions(string[] args)
        {
            // オプションなしなら失敗
            if (args.Length == 0) return false;

            this.args = args;
            source = null;
            runMode = NakoConsoleMode.RunFile;
            
            foreach (var arg in args)
            {
                switch (arg)
                {
                    case "-e":
                    case "-eval":
                        runMode = NakoConsoleMode.OneLiner;
                        break;
                    case "-desc":
                    case "-descript":
                        DescriptMode = true;
                        UseLog = true;
                        break;
                    case "-debug":
                        DebugMode = true;
                        break;
                    case "-wait":
                        WaitMode = true;
                        break;
                    default:
                        source = arg;
                        break;
                }
            }

            // ソースファイルが指定されてなければエラー
            if (runMode == NakoConsoleMode.RunFile && source == null) return false;
            return true;
        }

        /// <summary>
        /// プログラムを実行する
        /// </summary>
        public void Run()
        {
            try
            {
                switch (runMode)
                {
                    case NakoConsoleMode.OneLiner:
                        OneLinerMode(source);
                        break;
                    case NakoConsoleMode.RunFile:
                        RunFileMode(source);
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("[SystemError]" + e.Message);
                Console.WriteLine("[TRACE]" + e.StackTrace);
            }
            if (WaitMode)
            {
                Console.ReadLine();
            }
        }

        NakoCompilerLoaderInfo GetLoaderInfo()
        {
            //TODO: CNAKO2 LOADER INFO
            var loaderInfo = new NakoCompilerLoaderInfo();
            
            // 設定によって Console.Write() メソッドを使わないように指示する(テストで使用)
            var oNakoPluginConsole = new NakoPluginConsole.NakoPluginConsole();
            oNakoPluginConsole.UsePrintLog = UseLog;
            
            loaderInfo.Init();
            loaderInfo.ImportantModules = new NakoPlugin.INakoPlugin[] {
                oNakoPluginConsole
        	};
            return loaderInfo;
        }

        void OneLinerMode(string code)
        {
            var compiler = new NakoCompiler(GetLoaderInfo())
            {
                DebugMode = DebugMode, 
            };
            
            var runner = new NakoInterpreter(compiler.WriteIL(code));
            SetCommandLine(runner);
            
            if (DescriptMode)
            {
                Console.WriteLine("----------");
                Console.WriteLine("* TOKENS:");
                Console.WriteLine(compiler.Tokens.toTypeString());
                Console.WriteLine("----------");
                Console.WriteLine("* NODES:");
                Console.WriteLine(compiler.TopNode.Children.toTypeString());
                Console.WriteLine("----------");
                Console.WriteLine("* CODES:");
                Console.WriteLine(compiler.Codes.ToAddressString());
                Console.WriteLine("----------");
                Console.WriteLine("* RUN");
                runner.debugMode = DebugMode;
                runner.Run();
                Console.WriteLine("LOG=" + runner.PrintLog);
                Console.WriteLine("----------");
                Console.WriteLine("ok.");
            }
            else
            {
                runner.debugMode = DebugMode;
                runner.Run();
                PrintLog = runner.PrintLog;
                // Console.WriteLine(runner.PrintLog); // Consoleでは stdout へ直接出力しているはずなので不要のはず
            }
        }

        void RunFileMode(string sourcefile)
        {
            var loader = NakoLoader.Instance;
            loader.LoaderInfo = GetLoaderInfo();
            if (DebugMode)
            {
                Console.WriteLine("----------");
                Console.WriteLine("* TOKENIZE");
            }
            loader.DebugMode = DebugMode;
            try
            {
                loader.LoadFromFile(sourcefile);
            }
            catch (NakoParserException e)
            {
                Console.WriteLine("[ParseError]" + e.Message);
                return;
            }
            catch (Exception e)
            {
                Console.WriteLine("[ERROR] " + e.Message);
                Console.WriteLine("[TRACE] " + e.StackTrace);
                return;
            }
            NakoCompiler compiler = loader.cur;
            
            NakoInterpreter runner = new NakoInterpreter(compiler.Codes);
            SetCommandLine(runner);
            runner.debugMode = DebugMode;
            
            if (DescriptMode)
            {
                Console.WriteLine();
                Console.WriteLine("----------");
                Console.WriteLine("* TOKENS:");
                Console.WriteLine(compiler.Tokens.toTypeString());
                Console.WriteLine("----------");
                Console.WriteLine("* NODES:");
                Console.WriteLine(compiler.TopNode.Children.toTypeString());
                Console.WriteLine("----------");
                Console.WriteLine("* CODES:");
                Console.WriteLine(compiler.Codes.ToAddressString());
                Console.WriteLine("----------");
                Console.WriteLine("* RUN");
                runner.Run();
                Console.WriteLine("LOG=" + runner.PrintLog);
                Console.WriteLine("----------");
                Console.WriteLine("ok.");
            }
            else
            {
                runner.Run();
                PrintLog = runner.PrintLog;
            }
        }

        void SetCommandLine(NakoInterpreter runner)
        {
            NakoVarArray a = new NakoVarArray();
            a.SetValuesFromStrings(this.args);
            int i = runner.globalVar.GetIndex("コマンドライン");
            runner.globalVar.SetValue(i, a);
        }
    }

    /// <summary>
    /// なでしこコンソールの実行モード
    /// </summary>
    public enum NakoConsoleMode
    {
        /// <summary>
        /// コマンドラインからの一行プログラム
        /// </summary>
        OneLiner,
        /// <summary>
        /// ファイルを読み込んで実行する場合
        /// </summary>
        RunFile
    }
}
