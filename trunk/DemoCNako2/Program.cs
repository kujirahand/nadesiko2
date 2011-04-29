using System;
using System.Collections.Generic;
using System.Text;

using Libnako.JPNCompiler;
using Libnako.Interpreter;
using Libnako.JPNCompiler.ILWriter;
using Libnako.JPNCompiler.Tokenizer;

namespace DemoCNako2
{
    class Program
    {
        static NakoCompiler nc2 = new NakoCompiler();
        static NakoInterpreter ni2 = new NakoInterpreter();
        
        [STAThread]
        static void Main(string[] args)
        {
            // --------------------------------------------------
            // Compile
            NakoCompiler compiler = new NakoCompiler();
            compiler.DirectSource =
                "A=「」;A[`a`]=30;A[`b`]=31;Aの配列ハッシュキー列挙。それを「,」で配列結合して表示。" +
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
            Console.WriteLine("LOG=" + runner.PrintLog);
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
