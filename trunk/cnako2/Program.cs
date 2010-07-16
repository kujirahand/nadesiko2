using System;
using System.Collections.Generic;
using System.Text;
using Libnako.Parser;

using System.Reflection;
using System.Reflection.Emit;

namespace cnako
{
    class Program
    {
        static void Main(string[] args)
        {
            Boolean r;

            // 1
            NakoNamespace ns = new NakoNamespace(null);
            NakoILWriter w = new NakoILWriter();
            ns.source = "1+2*3";
            ns.Tokenize();
            ns.ParseOnlyValue();
            Console.WriteLine(ns.TopNode.hasChildren());
            r = ns.TopNode.Children.checkNodeType(new int[] {
                NodeType.N_CALC
            });
            Console.WriteLine(r);
            w.Write(ns.TopNode);
            Console.WriteLine(w.Result.ToTypeString());
            //


            test();
            NakoTokenizer tok = new NakoTokenizer(null);
            tok.Source = "1+2*3";
            tok.Tokenize();
            r = tok.CheckTokenType(new int[] { 
                TokenType.T_INT,
                TokenType.T_PLUS,
                TokenType.T_INT,
                TokenType.T_MUL,
                TokenType.T_INT
            });
            Console.WriteLine(r);

            Console.WriteLine(
                tok.Tokens.toTypeString()
                );
            NakoParser parser = new NakoParser(tok.Tokens);
            
            parser.ParseOnlyValue();
            if (parser.topNode.hasChildren())
            {
                Console.WriteLine(
                    parser.topNode.Children.toNodeTypeString()
                );
            }
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
