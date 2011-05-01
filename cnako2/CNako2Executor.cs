using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Libnako.JPNCompiler;
using Libnako.Interpreter;
using Libnako.JPNCompiler.ILWriter;
using Libnako.JPNCompiler.Tokenizer;
using Libnako.JPNCompiler.Parser;


namespace cnako2
{
    public class CNako2Executor
    {
        
        public bool DescriptMode = false;
        public bool DebugMode = false;
        public bool WaitMode = false;
        public NakoConsoleMode runMode = NakoConsoleMode.RunFile;
        public string source = null;
        public bool UseLog = false;
        public string PrintLog = null;

        public void ShowHelp()
        {
            _w("# [CNAKO2]");
            _w("# USAGE:");
            _w("# >cnako2 [-desc][-debug][-wait][-e (code)]|[(sourcefile)]");
            _w("# Example:");
            _w("# >cnako2 (sourcefile)");
            _w("# >cnako2 -e (one liner code)");
        }

        public bool setOptions(string[] args)
        {
            // オプションなしなら失敗
            if (args.Length == 0) return false;

            source = null;
            runMode = NakoConsoleMode.RunFile;
            
            int i = 0;
            while (i < args.Length)
            {
                string arg = args[i];
                // eval
                if (arg == "-e" || arg == "-eval")
                {
                    runMode = NakoConsoleMode.OneLiner;
                    i++;
                    continue;
                }
                if (arg == "-desc" || arg == "-descript")
                {
                    DescriptMode = true;
                    i++;
                    continue;
                }
                if (arg == "-debug")
                {
                    DebugMode = true;
                    i++;
                    continue;
                }
                if (arg == "-wait")
                {
                    WaitMode = true;
                    i++;
                    continue;
                }
                // other
                if (source == null)
                {
                    source = args[i];
                    i++;
                    continue;
                }
                i++;
            }
            // ソースファイルが指定されてなければエラー
            if (runMode == NakoConsoleMode.RunFile && source == null) return false;
            
            return true;
        }

        public void Run()
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
            if (WaitMode)
            {
                Console.ReadLine();
            }
        }

        NakoCompilerLoaderInfo GetLoaderInfo()
        {
            //TODO: CNAKO2 LOADER INFO
            NakoCompilerLoaderInfo loaderInfo = new NakoCompilerLoaderInfo();
            
            // 設定によって Console.Write() メソッドを使わないように指示する(テストで使用)
            NakoPluginConsole.NakoPluginConsole oNakoPluginConsole = new NakoPluginConsole.NakoPluginConsole();
            oNakoPluginConsole.UsePrintLog = UseLog;

            loaderInfo.Init();
            loaderInfo.ImportantModules = new NakoPlugin.INakoPlugin[] {
                oNakoPluginConsole
        	};
            return loaderInfo;
        }

        void OneLinerMode(string code)
        {
            NakoCompiler compiler = new NakoCompiler(GetLoaderInfo());
            compiler.DirectSource = code;
            if (DescriptMode)
            {
                cout = "----------";
                cout = "* TOKENS:";
                cout = compiler.Tokens.toTypeString();
                cout = "----------";
                cout = "* NODES:";
                cout = compiler.TopNode.Children.toTypeString();
                cout = "----------";
                cout = "* CODES:";
                cout = compiler.Codes.ToAddressString();
                cout = "----------";
                cout = "* RUN";
                NakoInterpreter runner = new NakoInterpreter(compiler.Codes);
                runner.debugMode = DebugMode;
                runner.Run();
                Console.WriteLine("LOG=" + runner.PrintLog);
                cout = "----------";
                cout = "ok.";
            }
            else
            {
                NakoInterpreter runner = new NakoInterpreter(compiler.Codes);
                runner.debugMode = DebugMode;
                runner.Run();
                PrintLog = runner.PrintLog;
                // Console.WriteLine(runner.PrintLog); // Consoleでは stdout へ直接出力しているはずなので不要のはず
            }
        }

        void RunFileMode(string sourcefile)
        {
            NakoLoader loader = NakoLoader.Instance;
            loader.LoaderInfo = GetLoaderInfo();
            try
            {
                loader.LoadFromFile(sourcefile);
            }
            catch (NakoParserException e)
            {
                cout = "[ParseError]" + e.Message;
                return;
            }
            catch (Exception e)
            {
                cout = "[ERROR] " + e.Message + "";
                return;
            }
            NakoCompiler compiler = loader.cur;
            if (DescriptMode)
            {
                cout = "----------";
                cout = "* TOKENS:";
                cout = compiler.Tokens.toTypeString();
                cout = "----------";
                cout = "* NODES:";
                cout = compiler.TopNode.Children.toTypeString();
                cout = "----------";
                cout = "* CODES:";
                cout = compiler.Codes.ToAddressString();
                cout = "----------";
                cout = "* RUN";
                NakoInterpreter runner = new NakoInterpreter(compiler.Codes);
                runner.debugMode = DebugMode;
                runner.Run();
                Console.WriteLine("LOG=" + runner.PrintLog);
                cout = "----------";
                cout = "ok.";
            }
            else
            {
                NakoInterpreter runner = new NakoInterpreter(compiler.Codes);
                runner.debugMode = DebugMode;
                runner.Run();
                // Console.WriteLine(runner.PrintLog);
                PrintLog = runner.PrintLog;
            }
        }

        static void _w(string s)
        {
            Console.WriteLine(s);
        }

        static string cout
        {
            set { Console.WriteLine(value); }
        }
    }

    public enum NakoConsoleMode
    {
        OneLiner,
        RunFile
    }
}
