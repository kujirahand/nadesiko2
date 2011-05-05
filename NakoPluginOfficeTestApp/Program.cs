using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Libnako.JPNCompiler;
using Libnako.Interpreter;
using Libnako.JPNCompiler.ILWriter;
using Libnako.JPNCompiler.Tokenizer;


namespace NakoPluginOfficeTestApp
{
    class Program
    {
        static NakoCompiler nc2 = new NakoCompiler();
        static NakoInterpreter ni2 = new NakoInterpreter();

        [STAThread]
        static void Main(string[] args)
        {
            // --------------------------------------------------
            // Compile
            NakoCompiler compiler = new NakoCompiler();
            try
            {
                compiler.DirectSource =
                "TMP=テンポラリフォルダ。TMP=TMP&「test.xlsx」。\n" +
                "0でエクセル起動。「A1」に「ABC」をエクセルセル設定。/*TMPへエクセル保存。*/エクセル終了。\n" +
                "0でエクセル起動。TMPのエクセル開く。「A1」のエクセルセル取得。それを表示。\n"+
                    //"TMP=テンポラリフォルダ。TMP=TMP&「test.xlsx」。TMPを表示。" +
                    //"1でエクセル起動。S1=(デスクトップ)＆「test.xlsx」。S2=(デスクトップ)＆「abc.xlsx」。S1をエクセル開く。S2へエクセル保存。3秒待つ。" +
                    //"1でエクセル起動。「C2」に「いろは」をエクセルセル設定。「C2」のエクセルセル取得。それを表示。３秒待つ。" +
                    //"1でエクセル起動。エクセルブック追加。「わんわん」にエクセルタイトル変更。3秒待つ。" +
                    //"1でエクセル起動。エクセルブック追加。1秒待つ。" +
                    //"1でエクセル起動。エクセルバージョンを表示。2秒待つ。エクセル終了。" +
                    //"A=「」;A[`a`]=30;A[`b`]=31;Aの配列ハッシュキー列挙。それを「,」で配列結合して表示。" +
                    "";
                cout = "----------";
                cout = "* TOKENS:";
                cout = compiler.Tokens.toTypeString();
                cout = "----------";
                cout = "* NODES:";
                cout = compiler.TopNode.Children.toTypeString();
                cout = "----------";
                cout = "* CODES:";
                cout = compiler.Codes.ToAddressString();
            }
            catch(Exception e){
                cout = e.Message;
                Console.ReadLine();
                return;
            }

            // --------------------------------------------------
            // Run
            cout = "----------";
            cout = "* RUN";
            NakoInterpreter runner = new NakoInterpreter(compiler.Codes);
            runner.debugMode = true;
            runner.Run();
            Console.WriteLine("LOG=" + runner.PrintLog);
            cout = "----------";

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
