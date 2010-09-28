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
        static void Main(string[] args)
        {
            // --------------------------------------------------
            // Compile
            NakoCompiler compiler = new NakoCompiler();
            compiler.DirectSource =
                "OS\n" +
                "それを表示\n" +
                "";
            cout = "----------";
            cout = "* TOKENS:";
            cout = compiler.Tokens.toTypeString();
            cout = "----------";
            cout = "* NODES:";
            cout = compiler.TopNode.Children.toTypeString();
            cout = "----------";
            cout = "* CODES:";
            cout = compiler.Codes.ToAddressString();

            // --------------------------------------------------
            // Run
            cout = "----------";
            cout = "* RUN";
            NakoInterpreter runner = new NakoInterpreter(compiler.Codes);
            runner.debugMode = true;
            runner.Run();
            Console.WriteLine("LOG="+runner.PrintLog);
            cout = "----------";

            // Wait
            cout = "ok.";
            Console.ReadLine();
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
}
