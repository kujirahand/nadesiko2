using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Libnako.JCompiler;
using Libnako.Interpreter;
using Libnako.JCompiler.ILWriter;

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
                "A[`a`][3]=566\n" +
                "A[`a`][3]を表示\n" +
                "A[3][1]=566\n" +
                "A[3][1]を表示\n" +
                ""
                ;
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

            // --------------------------------------------------
            // Compile2
            NakoCompiler compiler2 = new NakoCompiler();
            compiler2.DirectSource =
                "A[3]=566\n" +
                "A[3]を表示\n" +
                "A[`a`]=566\n" +
                "A[`a`]を表示\n" +
                ""
                ;
            cout = "----------";
            cout = "* TOKENS2:";
            cout = compiler2.Tokens.toTypeString();
            cout = "----------";
            cout = "* NODES2:";
            cout = compiler2.TopNode.Children.toTypeString();
            cout = "----------";
            cout = "* CODES2:";
            cout = compiler2.Codes.ToAddressString();

            // --------------------------------------------------
            // Run2
            cout = "----------";
            cout = "* RUN2";
            NakoInterpreter runner2 = new NakoInterpreter(compiler2.Codes);
            runner2.debugMode = true;
            runner2.Run();
            Console.WriteLine("LOG2=" + runner2.PrintLog);

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
