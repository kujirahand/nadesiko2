using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Libnako.JCompiler.Function;
using Libnako.JCompiler;

namespace Libnako.SysCall
{
    public class NakoBaseSystem : NakoSysCallRegister
    {
        // C# Singleton
        public static readonly NakoBaseSystem Instance = new NakoBaseSystem();
        private NakoBaseSystem() { }

        public override void registerToSystem()
        {
            addFunc("言う", "Sと|Sを", NakoVariableType.Void, _say, "メッセージSを画面に表示する", "いう");
        }

        public void _say(NakoFuncCallInfo info)
        {
            String msg = (String)info.StackPop();
            info.Runner.PrintLog += msg;
        }
    }
}
