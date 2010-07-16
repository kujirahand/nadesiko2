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

            ns.source = "1+2*3";
            ns.Tokenize();
            ns.ParseOnlyValue();
            w.Write(ns.TopNode);
            runner.Run(w.Result);
            o = runner.StackTop;
            Console.WriteLine(o);


            //
            Console.WriteLine("End.");
            Console.ReadLine();
        }

        static void test()
        {
            int a = 30;
            a++;
            Console.WriteLine(a);
        }
    }

}
