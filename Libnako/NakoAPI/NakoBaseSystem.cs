using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

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
            //TODO:本当はprivateにしたい
            //-Iterator 実装
            bank.AddFunc("GetEnumerator", "Oの", NakoVarType.Object, __getEnumerator, "反復子を返す(内部用)", "GetEnumerator");
            bank.AddFunc("MoveNext", "Eを", NakoVarType.Int, __moveNext, "反復子の移動結果を返す(内部用)", "MoveNext");
            bank.AddFunc("Current", "Eの", NakoVarType.Object, __current, "反復子の現在値を返す(内部用)", "Current");
            bank.AddFunc("Dispose", "Oの", NakoVarType.Void, __dispose, "オブジェクトを廃棄する(内部用)", "Dispose");
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
            bank.AddFunc("余り", "AをBで", NakoVarType.Object, _mod, "値Aを値Bで割ったあまり返す", "あまり");
            bank.AddFunc("割った余り", "AをBで", NakoVarType.Object, _mod, "変数Aを値Bで割った余りを返す", "わったあまり");
            //-計算関数
            bank.AddFunc("乗", "AのB", NakoVarType.Object, _power, "Aを底としてBの累乗を返す", "じょう");
            bank.AddFunc("乱数", "Nの", NakoVarType.Int, _random, "0から(N-1)までの範囲の乱数を返す", "らんすう");
            bank.AddFunc("絶対値", "Vの", NakoVarType.Int, _abs, "値Vの絶対値を返す", "ぜったいち");
            bank.AddFunc("ABS", "V", NakoVarType.Int, _abs, "値Vの絶対値を返す", "ABS");
            //+サウンド
            bank.AddFunc("BEEP", "", NakoVarType.Void, _beep, "BEEP音を鳴らす", "BEEP");

            bank.AddFunc("整数部分", "Vの", NakoVarType.Int, _intVal, "値Vの整数部分を返す", "せいすうぶぶん");
            bank.AddFunc("小数部分", "Vの", NakoVarType.Double, _floatVal, "値Vの小数部分を返す", "しょうすうぶぶん");
            bank.AddFunc("四捨五入", "AをBで", NakoVarType.Object, _round, "AをBの精度で四捨五入する", "ししゃごにゅう");
            bank.AddFunc("切り下げ", "AをBで", NakoVarType.Object, _ceil, "AをBの精度で切り下げする", "きりさげ");
            bank.AddFunc("代入", "Aを{参照渡し}Bに", NakoVarType.Void, _substitute, "AをBに代入する", "だいにゅう");
			//終わり
			bank.AddFunc("終わり", "", NakoVarType.Void, _quit, "プログラムを終了する", "おわり");
//後回し　整数部分
//後回し　小数点四捨五入→Aを「0.001」に四捨五入　と変更したいなー。指定がなければ一桁目
//後回し　小数点切り下げ→Aを「0.001」に四捨五入　と変更したいなー。指定がなければ一桁目
//後回し　代入
//後回し　小数部分
        }

        /*
        public object _say(NakoFuncCallInfo info)
        {
            object s = info.StackPop();
            if (s == null) s = "";
            string msg = s.ToString();

            MessageBox.Show(msg);
            return null;
        }
         */

//private use
        private object __getEnumerator(INakoFuncCallInfo info)
        {
            var a = info.StackPop();
            if(a is NakoVariable){
                a = ((NakoVariable)a).Body;
            }
            Type t = a.GetType();
            MethodInfo mi = t.GetMethod("GetEnumerator",new Type[0]);
            return mi.Invoke(a,null);
        }

        private object __moveNext(INakoFuncCallInfo info)
        {
            object e = info.StackPop();
            if(e is NakoVariable){
                e = ((NakoVariable)e).Body;
            }
            Type t = e.GetType();
            MethodInfo mi = t.GetMethod("MoveNext",new Type[0]);
            return mi.Invoke(e,null);
        }

        private object __current(INakoFuncCallInfo info)
        {
            object e = info.StackPop();
            if(e is NakoVariable){
                e = ((NakoVariable)e).Body;
            }
            Type t = e.GetType();
            if(e is IEnumerator){
                IEnumerator ie = (IEnumerator)e;
                return ie.Current;
            }
            PropertyInfo pi = t.GetProperty("Current");
            return pi.GetValue(e,null);
        }

        private object __dispose(INakoFuncCallInfo info)
        {
            object e = info.StackPop();
            if(e is NakoVariable){
                e = ((NakoVariable)e).Body;
            }
            if(e is IDisposable){
                ((IDisposable)e).Dispose();
            }
            return null;
        }

//public use
        private object _substitute(INakoFuncCallInfo info)
        {
            object a = info.StackPop();
            object br = info.StackPop();
            if (!(br is NakoVariable))
            {
                throw new ApplicationException("『代入』の引数が変数ではありません");
            }
            ((NakoVariable)br).SetBodyAutoType(a);
            return null;
        }

        private object _intVal(INakoFuncCallInfo info)
        {
            return info.StackPopAsInt();
        }

        private object _floatVal(INakoFuncCallInfo info)
        {
            string s = info.StackPopAsString();
            int index = s.IndexOf('.');
            if(index==-1){
                return 0;
            }
            string ds = "0"+s.Substring(index);
            return double.Parse(ds);
        }

        private object _round(INakoFuncCallInfo info)
        {
            object a = info.StackPop();
            object b = info.StackPop();
            int ib=0;
            if(b is string){
                string sb = (string)b;
                if(sb.Contains(".")){
                    ib = GetDigitOfDecimal(sb);
                }else{
                    ib = int.Parse(sb);
                }
            }else{
                ib = NakoValueConveter.ToInt(b);
            }
            double da = NakoValueConveter.ToDouble(a);
            return __round(da,ib);
        }

        private object _ceil(INakoFuncCallInfo info)
        {
            object a = info.StackPop();
            object b = info.StackPop();
            int ib = 0;
            if(b is string){
                string sb = (string)b;
                if(sb.Contains(".")){
                    ib = GetDigitOfDecimal(sb);
                }else{
                    ib = int.Parse(sb);
                }
            }else{
                ib = NakoValueConveter.ToInt(b);
            }
            double da = NakoValueConveter.ToDouble(a);
            return __ceil(da,ib);
        }

		/// <summary>
		/// Gets the digit of decimal.
		/// </summary>
		/// <returns>The digit of decimal.</returns>
		/// <param name="s">String of number</param>
        public static int GetDigitOfDecimal(string s){
            int index = s.IndexOf('.');
            if(index==-1){
                return 0;
            }
            return -1*s.Substring(index+1).Length;
        }

        private static double __round(double variable, int digit){
            double base_value = Math.Pow(10,-digit);
            return variable>0 ? Math.Floor((variable*base_value)+0.5)/base_value :
                Math.Ceiling((variable*base_value)-0.5)/base_value;
        }
        private static double __ceil(double variable, int digit){
            double base_value = Math.Pow(10,-digit);
            return variable>0 ? Math.Floor(variable*base_value)/base_value :
                Math.Ceiling(variable*base_value)/base_value;
        }

        private object _nakoVersion(INakoFuncCallInfo info)
        {
            return NakoInfo.NakoVersion;
        }

        private object _osVersion(INakoFuncCallInfo info)
        {
            return System.Environment.OSVersion.Version;
        }

        private object _os(INakoFuncCallInfo info)
        {
            return NWEnviroment.osVersionStr();
        }

        private object _getPlugins(INakoFuncCallInfo info)
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

        private object _beep(INakoFuncCallInfo info)
        {
        	// BEEP
        	System.Media.SystemSounds.Beep.Play();
            return null;
        }

        private object _show(INakoFuncCallInfo info)
        {
            string msg = info.StackPopAsString();
            info.WriteLog(msg);
            return null;
        }

        private object _add(INakoFuncCallInfo info)
        {
            object a = info.StackPop();
            object b = info.StackPop();
            if (a is long && b is long)
            {
                return ((long)a + (long)b);
            }
            else
            {
                double da = NakoValueConveter.ToDouble(a);
                double db = NakoValueConveter.ToDouble(b);
                return (da + db);
            }
        }

        private object _addEx(INakoFuncCallInfo info)
        {
            object ar = info.StackPop();
            object b = info.StackPop();
            if (!(ar is NakoVariable))
            {
                throw new ApplicationException("『足す!』の引数が変数ではありません");
            }
            object a = ((NakoVariable)ar).Body;
            object c;
            if (a is long && b is long)
            {
                c = (long)a + (long)b;
            }
            else
            {
                double da = NakoValueConveter.ToDouble(a);
                double db = NakoValueConveter.ToDouble(b);
                c = da + db;
            }
            // 結果をセット
            ((NakoVariable)ar).SetBodyAutoType(c);
            return (c);
        }

        private object _sub(INakoFuncCallInfo info)
        {
            object a = info.StackPop();
            object b = info.StackPop();
            if (a is long && b is long)
            {
                return ((long)a - (long)b);
            }
            else
            {
                double da = NakoValueConveter.ToDouble(a);
                double db = NakoValueConveter.ToDouble(b);
                return (da - db);
            }
        }

        private object _subEx(INakoFuncCallInfo info)
        {
            object ar = info.StackPop();
            object b = info.StackPop();
            if (!(ar is NakoVariable))
            {
                throw new ApplicationException("『引く!』の引数が変数ではありません");
            }
            object a = ((NakoVariable)ar).Body;
            object c;
            if (a is long && b is long)
            {
                c = (long)a - (long)b;
            }
            else
            {
                double da = NakoValueConveter.ToDouble(a);
                double db = NakoValueConveter.ToDouble(b);
                c = da - db;
            }
            // 結果をセット
            ((NakoVariable)ar).SetBodyAutoType(c);
            return (c);
        }

        private object _mul(INakoFuncCallInfo info)
        {
            object a = info.StackPop();
            object b = info.StackPop();
            if (a is long && b is long)
            {
                return ((long)a * (long)b);
            }
            else
            {
                double da = NakoValueConveter.ToDouble(a);
                double db = NakoValueConveter.ToDouble(b);
                return (da * db);
            }
        }

        private object _mulEx(INakoFuncCallInfo info)
        {
            object ar = info.StackPop();
            object b = info.StackPop();
            if (!(ar is NakoVariable))
            {
                throw new ApplicationException("『掛ける!』の引数が変数ではありません");
            }
            object a = ((NakoVariable)ar).Body;
            object c;
            if (a is long && b is long)
            {
                c = (long)a * (long)b;
            }
            else
            {
                double da = NakoValueConveter.ToDouble(a);
                double db = NakoValueConveter.ToDouble(b);
                c = da * db;
            }
            // 結果をセット
            ((NakoVariable)ar).SetBodyAutoType(c);
            return (c);
        }

        private object _div(INakoFuncCallInfo info)
        {
            object a = info.StackPop();
            object b = info.StackPop();
            if (a is long && b is long)
            {
                return ((long)a / (long)b);
            }
            else
            {
                double da = NakoValueConveter.ToDouble(a);
                double db = NakoValueConveter.ToDouble(b);
                return (da / db);
            }
        }

        private object _divEx(INakoFuncCallInfo info)
        {
            object ar = info.StackPop();
            object b = info.StackPop();
            if (!(ar is NakoVariable))
            {
                throw new ApplicationException("『掛ける!』の引数が変数ではありません");
            }
            object a = ((NakoVariable)ar).Body;
            object c;
            if (a is long && b is long)
            {
                c = (long)a / (long)b;
            }
            else
            {
                double da = NakoValueConveter.ToDouble(a);
                double db = NakoValueConveter.ToDouble(b);
                c = da / db;
            }
            // 結果をセット
            ((NakoVariable)ar).SetBodyAutoType(c);
            return (c);
        }

        private Random _randObj = null;
        private object _random(INakoFuncCallInfo info)
        {
            long range = info.StackPopAsInt();
            if (_randObj == null) {
                _randObj = new Random();
            }
            int v = _randObj.Next((int)range);
            return (long)v;
        }

        private object _abs(INakoFuncCallInfo info)
        {
            double v = info.StackPopAsDouble();
            v = Math.Abs(v);
            return v;
        }

        private object _mod(INakoFuncCallInfo info)
        {
            object a = info.StackPop();
            object b = info.StackPop();
            if (a is long && b is long)
            {
                return ((long)a % (long)b);
            }
            else
            {
                double da = NakoValueConveter.ToDouble(a);
                double db = NakoValueConveter.ToDouble(b);
                return (da % db);
            }
        }

        private object _power(INakoFuncCallInfo info)
        {
            double a = info.StackPopAsDouble();
            double b = info.StackPopAsDouble();
            a = Math.Pow(a,b);
            return a;
        }

		private object _quit(INakoFuncCallInfo info)
		{
			Environment.Exit (0);
			return null;
		}

    }
}
