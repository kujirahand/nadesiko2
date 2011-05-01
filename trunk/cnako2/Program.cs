using System;
using System.Collections.Generic;
using System.Text;

namespace cnako2
{
    class Program
    {
        static CNako2Executor exec = new CNako2Executor();

        [STAThread]
        static void Main(string[] args)
        {
            // 実行時オプションをセット
            bool r = exec.setOptions(args);
            if (r == false)
            {
                exec.ShowHelp();
                return;
            }
            // cnakoを実行
            exec.Run();
        }
    }
}
