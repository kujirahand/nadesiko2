using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Libnako.JCompiler.Function;
using Libnako.JCompiler;
using Libnako.Interpreter;

namespace Libnako.NakoAPI
{
    public class NakoBaseSystem : NakoSysCallRegister
    {
        // C# Singleton
        public static readonly NakoBaseSystem Instance = new NakoBaseSystem();
        private NakoBaseSystem() { }

        public override void registerToSystem()
        {
            addFunc("言う", "Sと|Sを", NakoVariableType.Void, _say, "メッセージSを画面に表示する", "いう");
            addFunc("足す", "AにBを|Aと", NakoVariableType.Object, _add, "値Aと値Bを足して返す", "たす");
        }

        public void _say(NakoFuncCallInfo info)
        {
            String msg = (String)info.StackPop();
            info.Runner.PrintLog += msg;
        }

        public void _add(NakoFuncCallInfo info)
        {
            Object a = info.StackPop();
            Object b = info.StackPop();
            if (a.GetType() == typeof(Int32) && b.GetType() == typeof(Int32))
            {
                info.StackPush((int)a + (int)b);
            }
            else
            {
                Double da = NakoValueConveter.ToDouble(a);
                Double db = NakoValueConveter.ToDouble(b);
                info.StackPush(da + db);
            }
        }
    }
}
