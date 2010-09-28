using System;
using System.Collections.Generic;
using System.Text;

using Libnako.JCompiler;
using Libnako.Interpreter;
using Libnako.JCompiler.ILWriter;
using Libnako.JCompiler.Tokenizer;

namespace cnako2
{
    class Program
    {
        static bool DescriptMode = false;
        static bool DebugMode = false;
        static bool WaitMode = false;

        static void ShowHelp()
        {
            _w("# [CNAKO2]");
            _w("# USAGE:");
            _w("# >cnako2 [-desc][-debug][-wait][-e (code)]|[(sourcefile)]");
            _w("# Example:");
            _w("# >cnako2 (sourcefile)");
            _w("# >cnako2 -e (one liner code)");
        }
        
        static void Main(string[] args)
        {
            
            // --------------------------------------------------
            // Read Source Code
            if (args.Length == 0)
            {
                ShowHelp();
                return;
            }
            // --------------------------------------------------
            NakoConsoleMode mode = NakoConsoleMode.RunFile;
            string source = null;

            int i = 0;
            while (i < args.Length)
            {
                string arg = args[i];
                // eval
                if (arg == "-e" || arg == "-eval")
                {
                    mode = NakoConsoleMode.OneLiner;
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

            switch (mode)
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

        static void OneLinerMode(string code)
        {
            NakoCompiler compiler = new NakoCompiler();
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
                Console.WriteLine(runner.PrintLog);
            }
        }

        static void RunFileMode(string sourcefile)
        {
            NakoLoader loader = NakoLoader.Instance;
            loader.LoadFromFile(sourcefile);
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
                Console.WriteLine(runner.PrintLog);
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

    enum NakoConsoleMode
    {
        OneLiner,
        RunFile
    }
}
