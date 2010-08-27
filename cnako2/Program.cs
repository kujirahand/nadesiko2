using System;
using System.Collections.Generic;
using System.Text;
using Libnako.JCompiler;
using Libnako.Interpreter;

using System.Reflection;
using System.Reflection.Emit;
using Libnako.JCompiler.ILWriter;

namespace cnako
{
    class Program
    {
        static void Main(string[] args)
        {
            NakoCompiler ns = new NakoCompiler();
            NakoILWriter writer = new NakoILWriter(null);
            NakoInterpreter runner = new NakoInterpreter();
            runner.debugMode = true;

            string src =
                "A=3;B=4;AにBを足す!;PRINT A;" +
                "";
            _w(src);

            // TokenizerTest
            ns.source = src;
            ns.Tokenize();
            _w(ns.Tokens.toTypeString());
            Console.WriteLine("End.");
            //Console.ReadLine();

            ns.Publish(src);

            // DESCRIPT
            _w("token:" + ns.Tokens.toTypeString());
            _w("nodes:"+ns.TopNode.Children.toTypeString());
            _w("IL:\n"+ns.Codes.ToAddressString());
            
            runner.Run(ns.Codes);
            _w(runner.PrintLog);

            //

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
