/*
 * Created by SharpDevelop.
 * User: shigepon
 * Date: 2011/04/25
 * Time: 15:58
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using NakoPlugin;

namespace Libnako.NakoAPI
{
    /// <summary>
    /// 配列関数プラグイン
    /// </summary>
    public class NakoPluginArray : INakoPlugin
    {
        string _description = "配列関数プラグイン";
        double _version = 1.0;
        //--- プラグイン共通の部分 ---
        public double TargetNakoVersion { get { return 2.0; } }
        public bool Used { get; set; }
        public string Name { get { return this.GetType().FullName; } }
        public double PluginVersion { get { return _version; } }
        public string Description { get { return _description; } }
        //--- 関数の定義 ---
        public void DefineFunction(INakoPluginBank bank)
        {
            bank.AddFunc("配列追加", "{参照渡し}AにSを", NakoVarType.Void, _append,"配列Aに要素Sを追加する。Aの内容を書き換える。", "はいれつついか");
            bank.AddFunc("配列ポップ", "{参照渡し}Aの", NakoVarType.Object, _pop, "配列Aの末尾を削除して、削除した要素を返り値として返す。Aの内容を書き換える。", "はいれつぽっぷ");
            bank.AddFunc("配列要素数", "Aの", NakoVarType.Int, _count, "配列Aの要素数を返す。", "はいれつようそすう");
            bank.AddFunc("配列削除", "{参照渡し}AのIを", NakoVarType.Void, _removeAt, "配列AのI番目（０起点）の要素を削除する。Aの内容を書き換える。削除した要素を返す。", "はいれつさくじょ");
            bank.AddFunc("配列結合", "AをSで", NakoVarType.String, _concat,"配列Aを文字列Sでつなげて文字列として返す。", "はいれつついか");
            bank.AddFunc("配列逆順", "{参照渡し}Aを", NakoVarType.Void, _reverse,"配列Aの並びを逆順にする。Aの内容を書き換える。", "はいれつぎゃくじゅん");
            bank.AddFunc("配列検索", "Aの{整数=0}IからKEYを|Aで", NakoVarType.Int, _search,"配列Aの要素I番からKEYを検索してそのインデックス番号を返す。見つからなければ-1を返す。", "はいれつけんさく");//TODO:見つからない時はException?
            bank.AddFunc("配列ハッシュキー列挙", "Aの", NakoVarType.Array, _enumKeys, "配列Aのキー一覧を配列で返す。", "はいれつはっしゅきーれっきょ");
        }
        
        // プラグインの初期化処理
        public void PluginInit(INakoInterpreter runner)
        {
        }
        // プラグインの終了処理
        public void PluginFin(INakoInterpreter runner)
        {
        }

        /// <summary>
        /// 配列のキー列挙
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public Object _enumKeys(INakoFuncCallInfo info)
        {
            Object ar = info.StackPop();
            NakoVarArray arv = (NakoVarArray)ar;
            if (arv.Type != NakoVarType.Array)
            {
                throw new NakoPluginArgmentException("『ハッシュキー列挙』の引数が配列ではありません。");
            }
            String[] keys = arv.GetKeys();
            NakoVarArray res = info.CreateArray();
            int i = 0;
            foreach (String key in keys)
            {
                res.SetValue(i++, key);
            }
            return res;
        }

        public Object _append(INakoFuncCallInfo info){

            Object ary = info.StackPop(); // 参照渡しなので変数への参照が得られる
            Object s   = info.StackPop();
            if (!(ary is NakoVariable))
            {
                throw new NakoPluginRuntimeException("『追加』の引数がvariableではありません");
            }
            NakoVariable ary_link = (NakoVariable)ary;
            if (ary_link.Body is NakoVarArray)
            {
                NakoVarArray arr = (NakoVarArray)ary_link.Body;
                NakoVariable new_item = new NakoVariable();
                new_item.SetBodyAutoType(s);
                arr.Add(new_item);
            }
            // 結果をセット
            return null;
        }

        public Object _pop(INakoFuncCallInfo info)
        {

            Object ary = info.StackPop(); // 参照渡しなので変数への参照が得られる
            if (!(ary is NakoVariable))
            {
                throw new NakoPluginRuntimeException("『ポップ』の引数がvariableではありません");
            }
            NakoVariable ary_link = (NakoVariable)ary;
            if (ary_link.Body is NakoVarArray)
            {
                NakoVarArray arr = (NakoVarArray)ary_link.Body;
                return arr.Pop();
            }
            // 結果をセット
            return null;
        }

        public Object _count(INakoFuncCallInfo info)
        {
            Object ar = info.StackPop();
            if (!(ar is NakoVarArray))
            {
                throw new NakoPluginRuntimeException("『要素数』の引数がarrayではありません");
            }
            NakoVarArray arr = (NakoVarArray)ar;
            return arr.Count;
        }

        public Object _removeAt(INakoFuncCallInfo info){

            Object a = info.StackPop();
            long   i = info.StackPopAsInt();
            if (!(a is NakoVariable))
            {
                throw new NakoPluginRuntimeException("『削除』の引数が変数ではありません");
            }
            
            NakoVariable av = (NakoVariable)a;
            NakoVarArray a_body = null;
            // 配列でなければ配列に強制変換する
            if (!(av.Body is NakoVarArray))
            {
                a_body = info.CreateArray();
                a_body.SetValuesFromString(av.Body.ToString());
                av.SetBody(a_body, NakoVarType.Array);
            }
            else
            {
                a_body = (NakoVarArray)av.Body;
            }
            // 要素を削除する
            a_body.RemoveAt((int)i);

            // 結果をセット
            return null;
        }
        public Object _concat(INakoFuncCallInfo info){
            Object ar = info.StackPop();
            String s = info.StackPopAsString();
            if (!(ar is NakoVarArray))
            {
                throw new NakoPluginRuntimeException("『結合』の引数が配列ではありません");
            }
            StringBuilder sb = new StringBuilder();
            NakoVarArray arr = (NakoVarArray)ar;
            int index = 0;
            while(arr.GetValue(index)!=null){
                if(index > 0) sb.Append(s);
                Object var = arr.GetValue(index);
                if (var != null)
                {
                    sb.Append(var.ToString());
                }
                index++;
            }
            return sb.ToString();
        }
        public Object _reverse(INakoFuncCallInfo info){

            Object ar = info.StackPop();
            if (!(ar is NakoVariable))
            {
                throw new NakoPluginRuntimeException("『逆順』の引数が変数ではありません");
            }
            Object a = ((NakoVariable)ar).Body;
            if (a is NakoVarArray)
            {
                ((NakoVarArray)a).Reverse();
            }
            else
            {
                NakoVarArray a2 = info.CreateArray();
                a2.SetValuesFromString(a.ToString());
                a2.Reverse();
                ((NakoVariable)ar).SetBody(a2, NakoVarType.Array);
            }
            // 結果をセット
            return null;
        }
        public Object _search(INakoFuncCallInfo info){
            Object ar = info.StackPop();
            int i = (int)info.StackPopAsInt();
            Object key = info.StackPop();
            if (!(ar is NakoVarArray))
            {
                throw new NakoPluginRuntimeException("『検索』の引数が配列ではありません");
            }
            NakoVarArray arr = (NakoVarArray)ar;
            while(arr.GetValue(i)!=null){
               Object var = arr.GetValue(i);
               if(var.ToString()==key.ToString()){
                   return i;
               }
               i++;
            }
            return -1;
        }
    }
}