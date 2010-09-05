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
            // Compile
            NakoCompiler compiler = new NakoCompiler();
            compiler.DirectSource =
                "A[3]=566\n"+
                "A[3]を表示\n"+
                "A[`a`]=566\n" +
                "A[`a`]を表示\n" +
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

            // Run
            cout = "----------";
            cout = "* RUN";
            NakoInterpreter runner = new NakoInterpreter(compiler.Codes);
            runner.Run();
            Console.WriteLine(runner.PrintLog);

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
