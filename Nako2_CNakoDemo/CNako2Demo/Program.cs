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
@"#が等しい
A[0]=1
B[0]=1
もしAとBが等しいならば
    「等しい」を表示
違えば
    「違う」を表示
";
/*@"#再帰
●フィボナッチ (Iの)
    もしI<2ならば
        Iで戻る
    違えば
        F1 = (I-2)のフィボナッチ
        F2 = (I-1)のフィボナッチ
        F1 + F2で戻る
6のフィボナッチを表示
";*/
/*@"#マップ機能
●マップ(Sを{関数(1)}Fで)
    R = F(S)
    Rで戻る
●関数A (Sの)
    S+10で戻る
RET = マップ(10, 関数Aの定義)
RETを表示
#ほげ=関数Aの定義
";*/
/*@"#関数の代入
●関数A (Sの)
    S+10で戻る
B = 関数Aの定義
A[`a`]= B
A[`a`](10)を表示
";*/
/*@"#プラグイン関数名に助詞を使えるように。終末行にタブを入れてみた。
A=10
S=Aを文字列変換
Sを表示
    
";*/
/*@"#プラグイン関数名に助詞を使えるように。終末行にタブを入れてみた。
S=「<html><h1>hoge</h1><h2>fuga</h2>」
H1=Sから「h1」タグで区切る
H1を表示
    
";*/
/*@"#〜回の実装
5回
    回数を表示
";*/
/*@"#。で区切る
「hoge」と表示。「fuga」と表示。
";*/
/*@"#〜回の実装
5回
    回数を表示
";*/
/*@"#関数の循環呼び出し
37の関数A
●関数B (Sの)
    S = S / 2
    「{S},」を表示
    もしS==1ならば戻る
    m = Sを2で割った余り
    もしm == 0ならば
        Sの関数B
    違えば
        Sの関数A
●関数A(Sの)
    S=S*3+1
    「{S},」を表示
    m = Sを2で割った余り
	もしm == 0ならば
		Sの関数B
	違えば
		Sの関数A
";*/
/*@"A=1#もし〜がの実装
もしAが1ならば
    「hoge」と表示
";*/
/*@"●keisan(Aと{整数=4}Bで)#初期値の実装
	C=A+B
    Cで戻る
D = 5とkeisan
Dを表示
";*/
/*@"
●配列操作(ARの)
    C = Bを配列コピー
    「C:」&Cを表示
    C[1] = 4
    AR[2] = C
    「INNER:」&AR[2]を表示
    「B:」&B[1]を表示
I[0] = 1
I[1] = 1
Iで反復
    A[1] = 1
    B[「fuga」] = 1
    B[「hoge」] = 2
    A[2] = B
    「OUTER:」&A[2]を表示
    Aの配列操作
";*/
/*●hoge({カウントダウンタイマー}oの)#インスタンスを扱うテスト
    oのNOを継続表示
    「hoge」と表示
B=カウントダウンタイマー
Bについて「hoge」をOnZero設定
Bを3でカウントダウン
Bを7でカウントダウン
D=BのNO
「fuga」を継続表示
Dを継続表示

";*/
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
