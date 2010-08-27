using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Libnako.JCompiler.Function;
using Libnako.JCompiler;
using Libnako.Interpreter;

namespace Libnako.NakoAPI
{
    public class NakoBaseSystem : NakoAPIRegister
    {
        // C# Singleton
        public static readonly NakoBaseSystem Instance = new NakoBaseSystem();
        private NakoBaseSystem() { }

        public override void registerToSystem()
        {
            addFunc("言う", "Sと|Sを", NakoVariableType.Void, _say, "メッセージSを画面に表示する", "いう");
            addFunc("足す", "AにBを|Aと", NakoVariableType.Object, _add, "値Aと値Bを足して返す", "たす");
            addFunc("足す!", "{参照渡し}AにBを|Aと", NakoVariableType.Object, _addEx, "変数Aと値Bを足して返す(変数A自身を書き換える)", "たす!");
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

        public void _addEx(NakoFuncCallInfo info)
        {
            Object ar = info.StackPop();
            Object b = info.StackPop();
            if (!(ar is NakoVariable))
            {
                throw new NakoAPIError("『足す!』の引数が変数ではありません");
            }
            Object a = ((NakoVariable)ar).value;
            Object c;
            if (a.GetType() == typeof(Int32) && b.GetType() == typeof(Int32))
            {
                c = (int)a + (int)b;
            }
            else
            {
                Double da = NakoValueConveter.ToDouble(a);
                Double db = NakoValueConveter.ToDouble(b);
                c = da + db;
            }
            info.StackPush(c);
            //
            ((NakoVariable)ar).value = c;
        }
    }
}
