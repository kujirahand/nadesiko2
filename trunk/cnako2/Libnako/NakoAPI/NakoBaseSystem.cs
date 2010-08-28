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
        private static readonly NakoBaseSystem _Instance = new NakoBaseSystem();
		public static NakoBaseSystem Instance { get { return _Instance; } }
        private NakoBaseSystem() { }

        public override void registerToSystem()
        {
            addFunc("言う", "Sと|Sを", NakoVariableType.Void, _say, "メッセージSを画面に表示する", "いう");
            addFunc("表示", "Sと|Sを", NakoVariableType.Void, _show, "メッセージSを表示する", "ひょうじ");
            addFunc("足す", "AにBを|Aと", NakoVariableType.Object, _add, "値Aと値Bを足して返す", "たす");
            addFunc("足す!", "{参照渡し}AにBを|Aと", NakoVariableType.Object, _addEx, "変数Aと値Bを足して返す(変数A自身を書き換える)", "たす!");
        }

        public Object _say(NakoFuncCallInfo info)
        {
            String msg = (String)info.StackPop();
            info.Runner.PrintLog += msg;
            return null;
        }

        public Object _show(NakoFuncCallInfo info)
        {
            String msg = (String)info.StackPop();
            info.Runner.PrintLog += msg;
            return null;
        }

        public Object _add(NakoFuncCallInfo info)
        {
            Object a = info.StackPop();
            Object b = info.StackPop();
            if (a.GetType() == typeof(Int32) && b.GetType() == typeof(Int32))
            {
                return ((int)a + (int)b);
            }
            else
            {
                Double da = NakoValueConveter.ToDouble(a);
                Double db = NakoValueConveter.ToDouble(b);
                return (da + db);
            }
        }

        public Object _addEx(NakoFuncCallInfo info)
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
            // 結果をセット
            ((NakoVariable)ar).value = c;
            return (c);
        }
    }
}
