using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Libnako.JCompiler.Function;
using Libnako.JCompiler;
using Libnako.Interpreter;
using Libnako.NakoAPI.WrapLib;

namespace Libnako.NakoAPI
{
    /// <summary>
    /// なでしこにシステム関数を登録するクラス(実際の関数の挙動もここで定義)
    /// </summary>
    public class NakoBaseSystem : NakoAPIRegister
    {
        // C# Singleton
        private static readonly NakoBaseSystem _Instance = new NakoBaseSystem();
		public static NakoBaseSystem Instance { get { return _Instance; } }

        /// <summary>
        /// システムに関数を登録する
        /// </summary>
        protected override void DefineFunction()
        {
            //+システム
            //-バージョン情報
            addFunc("ナデシコバージョン", "", NakoVariableType.Double, _nakoVersion, "なでしこのバージョン番号を返す", "なでしこばーじょん");
            addFunc("OSバージョン", "", NakoVariableType.String, _osVersion, "OSのバージョン番号を返す", "OSばーじょん");
            addFunc("OS", "", NakoVariableType.String, _os, "OSの種類を返す", "OS");
            
            
            //+コンソールデバッグ用
            addFunc("表示", "Sと|Sを", NakoVariableType.Void, _show, "メッセージSを表示する", "ひょうじ");
            //+計算
            addFunc("足す", "AにBを|Aと", NakoVariableType.Object, _add, "値Aと値Bを足して返す", "たす");
            addFunc("足す!", "{参照渡し}AにBを|Aと", NakoVariableType.Object, _addEx, "変数Aと値Bを足して返す(変数A自身を書き換える)", "たす!");
            addFunc("引く", "AからBを", NakoVariableType.Object, _sub, "値Aから値Bを引いて返す", "ひく");
            addFunc("引く!", "{参照渡し}AからBを", NakoVariableType.Object, _subEx, "変数Aから値Bを引いて返す(変数A自身を書き換える)", "ひく!");
            //+文字列操作
            addFunc("何文字目", "SでSSが|Sの", NakoVariableType.String, _strpos, "文字列Sで文字列SSが何文字目にあるか調べて返す", "なんもじめ");
        }

        /*
        public Object _say(NakoFuncCallInfo info)
        {
            Object s = info.StackPop();
            if (s == null) s = "";
            String msg = s.ToString();

            MessageBox.Show(msg);
            return null;
        }
         */

        public Object _nakoVersion(NakoFuncCallInfo info)
        {
            return NakoInfo.NakoVersion;
        }

        public Object _osVersion(NakoFuncCallInfo info)
        {
            return System.Environment.OSVersion.Version;
        }

        public Object _os(NakoFuncCallInfo info)
        {
            return NWEnviroment.osVersionStr();
        }

        public Object _show(NakoFuncCallInfo info)
        {
            Object s = info.StackPop();
            String msg = "";
            if (s != null) { msg = s.ToString(); }
            info.Runner.PrintLog += msg;
            return null;
        }

        public Object _add(NakoFuncCallInfo info)
        {
            Object a = info.StackPop();
            Object b = info.StackPop();
            if (a is Int64 && b is Int64)
            {
                return ((Int64)a + (Int64)b);
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
            Object a = ((NakoVariable)ar).body;
            Object c;
            if (a is Int64 && b is Int64)
            {
                c = (Int64)a + (Int64)b;
            }
            else
            {
                Double da = NakoValueConveter.ToDouble(a);
                Double db = NakoValueConveter.ToDouble(b);
                c = da + db;
            }
            // 結果をセット
            ((NakoVariable)ar).body = c;
            return (c);
        }

        public Object _sub(NakoFuncCallInfo info)
        {
            Object a = info.StackPop();
            Object b = info.StackPop();
            if (a is Int64 && b is Int64)
            {
                return ((Int64)a - (Int64)b);
            }
            else
            {
                Double da = NakoValueConveter.ToDouble(a);
                Double db = NakoValueConveter.ToDouble(b);
                return (da - db);
            }
        }

        public Object _subEx(NakoFuncCallInfo info)
        {
            Object ar = info.StackPop();
            Object b = info.StackPop();
            if (!(ar is NakoVariable))
            {
                throw new NakoAPIError("『引く!』の引数が変数ではありません");
            }
            Object a = ((NakoVariable)ar).body;
            Object c;
            if (a is Int64 && b is Int64)
            {
                c = (Int64)a - (Int64)b;
            }
            else
            {
                Double da = NakoValueConveter.ToDouble(a);
                Double db = NakoValueConveter.ToDouble(b);
                c = da - db;
            }
            // 結果をセット
            ((NakoVariable)ar).body = c;
            return (c);
        }

        public Object _strpos(NakoFuncCallInfo info)
        {
            String s = info.StackPopAsString();
            String ss = info.StackPopAsString();
            int i = s.IndexOf(ss);
            return (i + 1); // 1からはじまるので
        }
    }
}
