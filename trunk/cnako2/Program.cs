using System;
using System.Collections.Generic;
using System.Text;

namespace cnako2
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var exec = new CNako2Executor();
            // 実行時オプションを設定
            if (!exec.SetOptions(args))
            {
                exec.ShowHelp();
                return;
            }
            // cnakoを実行
            exec.Run();
        }
    }
}
