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
            NakoILWriter w = new NakoILWriter();
            NakoInterpreter runner = new NakoInterpreter();
            Object o;
            Boolean r;

            ns.source = "A=5;B=3;C=A+B;PRINT C";
            ns.Tokenize();
            _w("token:" + ns.Tokens.toTypeString());
            ns.Parse();
            _w("nodes:"+ns.TopNode.Children.toTypeString());
            w.Write(ns.TopNode);
            _w("IL:"+w.Result.ToTypeString());
            runner.Run(w.Result);
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
