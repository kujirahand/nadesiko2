using System;
using System.Collections.Generic;
using System.Text;

using Libnako.JPNCompiler;
using Libnako.Interpreter;

namespace DemoCNako2
{
    class Program
    {
        //static NakoCompiler nc2 = new NakoCompiler();
        //static NakoInterpreter ni2 = new NakoInterpreter();
        
        [STAThread]
        public static void Main(string[] args)
        {
            // --------------------------------------------------
            // Compile
            NakoCompiler compiler = new NakoCompiler();
			compiler.DirectSource = 
@"
A[`a`]=`aaa`
A[`b`]=`bbb`
A[`c`]=`ccc`
(Aの配列ハッシュキー列挙)で反復
  「***:{対象}」を継続表示

";
//                "デスクトップ。\nそれのファイル列挙。\nそれを「/」で配列結合。\nそれを表示。" +
//                "";
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
//
//            // Wait
//            cout = "ok.";
//            Console.ReadLine();
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
