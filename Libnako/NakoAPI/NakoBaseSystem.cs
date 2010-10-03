﻿using System;
using System.Collections.Generic;

using System.Text;

using Libnako.JCompiler.Function;
using Libnako.JCompiler;
using Libnako.Interpreter;
using Libnako.NakoAPI.WrapLib;
using NakoPlugin;

namespace Libnako.NakoAPI
{
    /// <summary>
    /// なでしこにシステム関数を登録するクラス(実際の関数の挙動もここで定義)
    /// </summary>
    public class NakoBaseSystem : INakoPlugin
    {
        public string Name
        {
            get { return this.GetType().FullName; }
        }

        public double PluginVersion
        {
            get { return 1.0; }
        }

        public string Description
        {
            get { return "システム関数を定義したプラグイン"; }
        }

        /// <summary>
        /// システムに関数を登録する
        /// </summary>
        public void DefineFunction(INakoPluginBank bank)
        {
            //+システム
            //-バージョン情報
            bank.AddFunc("ナデシコバージョン", "", NakoVarType.Double, _nakoVersion, "なでしこのバージョン番号を返す", "なでしこばーじょん");
            bank.AddFunc("OSバージョン", "", NakoVarType.String, _osVersion, "OSのバージョン番号を返す", "OSばーじょん");
            bank.AddFunc("OS", "", NakoVarType.String, _os, "OSの種類を返す", "OS");
            
            //-基本定数
            bank.AddVar("はい", 1, "1", "はい");
            bank.AddVar("いいえ", 0, "0", "いいえ");
            
            //+コンソールデバッグ用
            bank.AddFunc("表示", "Sと|Sを", NakoVarType.Void, _show, "メッセージSを表示する", "ひょうじ");
            //+計算
            bank.AddFunc("足す", "AにBを|Aと", NakoVarType.Object, _add, "値Aと値Bを足して返す", "たす");
            bank.AddFunc("足す!", "{参照渡し}AにBを|Aと", NakoVarType.Object, _addEx, "変数Aと値Bを足して返す(変数A自身を書き換える)", "たす!");
            bank.AddFunc("引く", "AからBを", NakoVarType.Object, _sub, "値Aから値Bを引いて返す", "ひく");
            bank.AddFunc("引く!", "{参照渡し}AからBを", NakoVarType.Object, _subEx, "変数Aから値Bを引いて返す(変数A自身を書き換える)", "ひく!");
            //+文字列操作
            bank.AddFunc("何文字目", "SでSSが|Sの", NakoVarType.String, _strpos, "文字列Sで文字列SSが何文字目にあるか調べて返す", "なんもじめ");
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

        public Object _nakoVersion(INakoFuncCallInfo info)
        {
            return NakoInfo.NakoVersion;
        }

        public Object _osVersion(INakoFuncCallInfo info)
        {
            return System.Environment.OSVersion.Version;
        }

        public Object _os(INakoFuncCallInfo info)
        {
            return NWEnviroment.osVersionStr();
        }

        public Object _show(INakoFuncCallInfo info)
        {
            Object s = info.StackPop();
            String msg = "";
            if (s != null) { msg = s.ToString(); }
            info.WriteLog(msg);
            return null;
        }

        public Object _add(INakoFuncCallInfo info)
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

        public Object _addEx(INakoFuncCallInfo info)
        {
            Object ar = info.StackPop();
            Object b = info.StackPop();
            if (!(ar is NakoVariable))
            {
                throw new ApplicationException("『足す!』の引数が変数ではありません");
            }
            Object a = ((NakoVariable)ar).Body;
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
            ((NakoVariable)ar).Body = c;
            return (c);
        }

        public Object _sub(INakoFuncCallInfo info)
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

        public Object _subEx(INakoFuncCallInfo info)
        {
            Object ar = info.StackPop();
            Object b = info.StackPop();
            if (!(ar is NakoVariable))
            {
                throw new ApplicationException("『引く!』の引数が変数ではありません");
            }
            Object a = ((NakoVariable)ar).Body;
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
            ((NakoVariable)ar).Body = c;
            return (c);
        }

        public Object _strpos(INakoFuncCallInfo info)
        {
            String s = info.StackPopAsString();
            String ss = info.StackPopAsString();
            int i = s.IndexOf(ss);
            return (i + 1); // 1からはじまるので
        }

    }
}