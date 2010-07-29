using System;
using System.Collections.Generic;
using System.Text;
using Libnako.Parser;
using Libnako.Interpreter;

using System.Reflection;
using System.Reflection.Emit;

namespace cnako
{
    class Program
    {
        static void Main(string[] args)
        {
            NakoNamespace ns = new NakoNamespace();
            NakoInterpreter runner = new NakoInterpreter();

            /*
            String src =
                "A=1\n" +
                "B=2\n" +
                "もし、A=Bならば\n" +
                "  PRINT`真`\n" +
                "違えば\n" +
                "  PRINT`偽`\n";
             */

            String src = "A=1;B=1;もし、A=Bならば、PRINT`OK`違えば,PRINT`NG`";

            ns.source = src;
            ns.Tokenize();
            _w(ns.Tokens.toTypeString());

            ns.Publish();

            // DESCRIPT
            _w("token:" + ns.Tokens.toTypeString());
            _w("nodes:"+ns.TopNode.Children.toTypeString());
            _w("IL:\n"+ns.Codes.ToAddressString());

            //
            runner.Run(ns.Codes);
            _w(runner.PrintLog);

            //
            Console.WriteLine("End.");
            Console.ReadLine();
        }

        static void _w(String s)
        {
            Console.WriteLine(s);
        }

        static void test()
        {
            int a = 30;
            a++;
            Console.WriteLine(a);
        }
    }

}
