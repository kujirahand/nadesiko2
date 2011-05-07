using System;
using System.Collections.Generic;
using System.Text;

using NakoPlugin;

namespace Libnako.NakoAPI
{
    /// <summary>
    /// なでしこにシステム関数を登録するクラス(実際の関数の挙動もここで定義)
    /// </summary>
    public class NakoBaseSystem : NakoPluginTemplate, INakoPlugin
    {
        /// <summary>
        /// プラグインのバージョン番号
        /// </summary>
        new protected double _pluginVersion = 1.0;
        /// <summary>
        /// プラグインの説明
        /// </summary>
        new protected string _pluginDescript = "システム関数を定義したプラグイン";

        /// <summary>
        /// 関数の定義
        /// </summary>
        /// <param name="bank"></param>
        new public void DefineFunction(INakoPluginBank bank)
        {
            //+システム
            //-バージョン情報
            bank.AddFunc("ナデシコバージョン", "", NakoVarType.Double, _nakoVersion, "なでしこのバージョン番号を返す", "なでしこばーじょん");
            bank.AddFunc("OSバージョン", "", NakoVarType.String, _osVersion, "OSのバージョン番号を返す", "OSばーじょん");
            bank.AddFunc("OS", "", NakoVarType.String, _os, "OSの種類を返す", "OS");
            bank.AddFunc("利用中プラグイン列挙", "", NakoVarType.Array, _getPlugins, "現在読み込まれているプラグイン一覧を返す", "りようちゅうぷらぐいんれっきょ");
            
            //-基本定数
            bank.AddVar("はい", 1, "1", "はい");
            bank.AddVar("いいえ", 0, "0", "いいえ");
            bank.AddVar("OK", 1, "1", "OK");
            bank.AddVar("NG", 0, "0", "NG");
            bank.AddVar("オン", 1, "1", "おん");
            bank.AddVar("オフ", 0, "0", "おふ");
            bank.AddVar("真", 1, "1", "しん");
            bank.AddVar("偽", 0, "0", "ぎ");
            bank.AddVar("改行", "\r\n", "改行", "かいぎょう");
            bank.AddVar("タブ", "\t", "タブ文字", "たぶ");

            //+コンソールデバッグ用
            bank.AddFunc("表示", "Sと|Sを", NakoVarType.Void, _show, "メッセージSを表示する", "ひょうじ");
            bank.AddFunc("継続表示", "Sと|Sを", NakoVarType.Void, _show, "メッセージSを表示する", "けいぞくひょうじ");
            bank.AddVar("コマンドライン", null, "起動時の引数を保持する", "こまんどらいん");

            //+計算
            //-四則演算
            bank.AddFunc("足す", "AにBを|Aと", NakoVarType.Object, _add, "値Aと値Bを足して返す", "たす");
            bank.AddFunc("足す!", "{参照渡し}AにBを|Aと", NakoVarType.Object, _addEx, "変数Aと値Bを足して返す(変数A自身を書き換える)", "たす!");
            bank.AddFunc("引く", "AからBを", NakoVarType.Object, _sub, "値Aから値Bを引いて返す", "ひく");
            bank.AddFunc("引く!", "{参照渡し}AからBを", NakoVarType.Object, _subEx, "変数Aから値Bを引いて返す(変数A自身を書き換える)", "ひく!");
            bank.AddFunc("掛ける", "AにBを", NakoVarType.Object, _mul, "値Aと値Bを掛けて返す", "かける");
            bank.AddFunc("掛ける!", "{参照渡し}AにBを", NakoVarType.Object, _mulEx, "変数Aと値Bを掛けて返す(変数A自身を書き換える)", "かける!");
            bank.AddFunc("割る", "AをBで", NakoVarType.Object, _div, "値Aを値Bで割って返す", "わる");
            bank.AddFunc("割る!", "{参照渡し}AをBで", NakoVarType.Object, _divEx, "変数Aを値Bで割って返す(変数A自身を書き換える)", "わる");
            //-計算関数
            bank.AddFunc("乱数", "Nの", NakoVarType.Int, _random, "0から(N-1)までの範囲の乱数を返す", "らんすう");
            bank.AddFunc("絶対値", "Vの", NakoVarType.Int, _abs, "値Vの絶対値を返す", "ぜったいち");
            bank.AddFunc("ABS", "V", NakoVarType.Int, _abs, "値Vの絶対値を返す", "ABS");
            //+サウンド
            bank.AddFunc("BEEP", "", NakoVarType.Void, _beep, "BEEP音を鳴らす", "BEEP");
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

        private Object _nakoVersion(INakoFuncCallInfo info)
        {
            return NakoInfo.NakoVersion;
        }

        private Object _osVersion(INakoFuncCallInfo info)
        {
            return System.Environment.OSVersion.Version;
        }

        private Object _os(INakoFuncCallInfo info)
        {
            return NWEnviroment.osVersionStr();
        }

        private Object _getPlugins(INakoFuncCallInfo info)
        {
            NakoVarArray a = info.CreateArray();
            foreach (KeyValuePair<string, INakoPlugin> pair in NakoAPIFuncBank.Instance.PluginList)
            {
                NakoVariable v = new NakoVariable();
                v.SetBodyAutoType(pair.Key);
                a.Add(v);
            }
            return a;
        }

        private Object _beep(INakoFuncCallInfo info)
        {
        	// BEEP
        	System.Media.SystemSounds.Beep.Play();
            return null;
        }

        private Object _show(INakoFuncCallInfo info)
        {
            String msg = info.StackPopAsString();
            info.WriteLog(msg);
            return null;
        }

        private Object _add(INakoFuncCallInfo info)
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

        private Object _addEx(INakoFuncCallInfo info)
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
            ((NakoVariable)ar).SetBodyAutoType(c);
            return (c);
        }

        private Object _sub(INakoFuncCallInfo info)
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

        private Object _subEx(INakoFuncCallInfo info)
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
            ((NakoVariable)ar).SetBodyAutoType(c);
            return (c);
        }

        private Object _mul(INakoFuncCallInfo info)
        {
            Object a = info.StackPop();
            Object b = info.StackPop();
            if (a is Int64 && b is Int64)
            {
                return ((Int64)a * (Int64)b);
            }
            else
            {
                Double da = NakoValueConveter.ToDouble(a);
                Double db = NakoValueConveter.ToDouble(b);
                return (da * db);
            }
        }

        private Object _mulEx(INakoFuncCallInfo info)
        {
            Object ar = info.StackPop();
            Object b = info.StackPop();
            if (!(ar is NakoVariable))
            {
                throw new ApplicationException("『掛ける!』の引数が変数ではありません");
            }
            Object a = ((NakoVariable)ar).Body;
            Object c;
            if (a is Int64 && b is Int64)
            {
                c = (Int64)a * (Int64)b;
            }
            else
            {
                Double da = NakoValueConveter.ToDouble(a);
                Double db = NakoValueConveter.ToDouble(b);
                c = da * db;
            }
            // 結果をセット
            ((NakoVariable)ar).SetBodyAutoType(c);
            return (c);
        }

        private Object _div(INakoFuncCallInfo info)
        {
            Object a = info.StackPop();
            Object b = info.StackPop();
            if (a is Int64 && b is Int64)
            {
                return ((Int64)a / (Int64)b);
            }
            else
            {
                Double da = NakoValueConveter.ToDouble(a);
                Double db = NakoValueConveter.ToDouble(b);
                return (da / db);
            }
        }

        private Object _divEx(INakoFuncCallInfo info)
        {
            Object ar = info.StackPop();
            Object b = info.StackPop();
            if (!(ar is NakoVariable))
            {
                throw new ApplicationException("『掛ける!』の引数が変数ではありません");
            }
            Object a = ((NakoVariable)ar).Body;
            Object c;
            if (a is Int64 && b is Int64)
            {
                c = (Int64)a / (Int64)b;
            }
            else
            {
                Double da = NakoValueConveter.ToDouble(a);
                Double db = NakoValueConveter.ToDouble(b);
                c = da / db;
            }
            // 結果をセット
            ((NakoVariable)ar).SetBodyAutoType(c);
            return (c);
        }

        private Random _randObj = null;
        private Object _random(INakoFuncCallInfo info)
        {
            Int64 range = info.StackPopAsInt();
            if (_randObj == null) {
                _randObj = new Random();
            }
            int v = _randObj.Next((int)range);
            return (Int64)v;
        }

        private Object _abs(INakoFuncCallInfo info)
        {
            double v = info.StackPopAsDouble();
            v = Math.Abs(v);
            return v;
        }

    }
}
