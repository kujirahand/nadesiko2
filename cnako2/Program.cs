using System;
using System.Collections.Generic;
using System.Text;
using Libnako.Parser;

namespace cnako
{
    class Program
    {
        static void Main(string[] args)
        {
            NakoTokenizer tok = new NakoTokenizer(null);
            Boolean r;
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
    }
}
