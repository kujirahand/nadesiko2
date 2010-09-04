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
            NakoILCodeList codes;
            codes = compiler.Publish(
                "a=4;PRINT a;"
             );
            // Run
            NakoInterpreter runner = new NakoInterpreter(codes);
            runner.Run();
            Console.WriteLine(runner.PrintLog);

            // Wait
            Console.ReadLine();
        }
    }
}
