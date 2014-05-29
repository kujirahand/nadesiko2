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
    public class NakoPluginArray : NakoPluginTemplate, INakoPlugin
    {
        /// <summary>
        /// プラグインのバージョン番号
        /// </summary>
        new protected double _pluginVersion = 1.0;
        /// <summary>
        /// プラグインの説明
        /// </summary>
        new protected string _pluginDescript = "配列処理を行うプラグイン";
        /// <summary>
        /// 関数の定義
        /// </summary>
        /// <param name="bank"></param>
        new public void DefineFunction(INakoPluginBank bank)
        {
            bank.AddFunc("配列追加", "{参照渡し}AにSを", NakoVarType.Void, _append,"配列Aに要素Sを追加する。Aの内容を書き換える。", "はいれつついか");
            bank.AddFunc("配列ポップ", "{参照渡し}Aの", NakoVarType.Object, _pop, "配列Aの末尾を削除して、削除した要素を返り値として返す。Aの内容を書き換える。", "はいれつぽっぷ");
            bank.AddFunc("配列要素数", "Aの", NakoVarType.Int, _count_array, "配列Aの要素数を返す。", "はいれつようそすう");
            bank.AddFunc("配列削除", "{参照渡し}AのIを", NakoVarType.Void, _removeAt, "配列AのI番目（０起点）の要素を削除する。Aの内容を書き換える。削除した要素を返す。", "はいれつさくじょ");
            bank.AddFunc("配列結合", "AをSで", NakoVarType.String, _concat,"配列Aを文字列Sでつなげて文字列として返す。", "はいれつついか");
            bank.AddFunc("配列逆順", "{参照渡し}Aを", NakoVarType.Void, _reverse,"配列Aの並びを逆順にする。Aの内容を書き換える。", "はいれつぎゃくじゅん");
            bank.AddFunc("配列検索", "Aの{整数=0}IからKEYを|Aで", NakoVarType.Int, _search,"配列Aの要素I番からKEYを検索してそのインデックス番号を返す。見つからなければ-1を返す。", "はいれつけんさく");//TODO:見つからない時はException?
            bank.AddFunc("配列ハッシュキー列挙", "Aの", NakoVarType.Array, _enumKeys, "配列Aのキー一覧を配列で返す。", "はいれつはっしゅきーれっきょ");
            bank.AddFunc("要素数", "Sの", NakoVarType.Int, _count, "ハッシュ・配列の要素数、文字列の行数を返す。", "ようそすう");
            bank.AddFunc("配列一括挿入", "{参照渡し}AのIにSを|Iから", NakoVarType.Void, _insertArray,"配列AのI番目(0起点)に配列Sの内容を一括挿入する。Aの内容を書き換える。", "はいれついっかつそうにゅう");
			bank.AddFunc("配列キー存在?", "AのKEYを|Aが", NakoVarType.Int, _hasKey,"配列AからKEYを検索してKEYがあれば1を返す。見つからなければ0を返す。", "はいれつきーそんざい?");
        }


        private object _insertArray(INakoFuncCallInfo info){
            object obj_base = info.StackPop(); // 参照渡しなので変数への参照が得られる
            int i   = (int)info.StackPopAsInt();
            object obj_insert = info.StackPop(); 
            if (!(obj_base is NakoVariable))
            {
                throw new NakoPluginRuntimeException("『配列一括挿入』の元配列がvariableではありません");
            }
            NakoVarArray array_insert = new NakoVarArray();
            if (obj_insert is NakoVariable)
            {
                NakoVariable var_insert = (NakoVariable)obj_insert;
                if(var_insert.Body is NakoVarArray){
                    array_insert = (NakoVarArray)var_insert.Body;
                }
            }else if(obj_insert is string){
                array_insert.SetValuesFromString((string)obj_insert);
            }else{
                throw new NakoPluginRuntimeException("『配列一括挿入』の挿入配列がvariableではありません");
            }
            NakoVariable var_base = (NakoVariable)obj_base;
            if(var_base.Body is NakoVarArray){
                NakoVarArray array_base = (NakoVarArray)var_base.Body;
                while(array_insert.Count>0){
                    NakoVariable variable = array_insert.Pop();
                    array_base.Insert(i,variable);
                }
            }
            return null;
        }

        /// <summary>
        /// 配列のキー列挙
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        private object _enumKeys(INakoFuncCallInfo info)
        {
            object ar = info.StackPop();
            NakoVarArray arv = (NakoVarArray)ar;
            if (arv.Type != NakoVarType.Array)
            {
                throw new NakoPluginArgmentException("『ハッシュキー列挙』の引数が配列ではありません。");
            }
            string[] keys = arv.GetKeys();
            NakoVarArray res = info.CreateArray();
            int i = 0;
            foreach (string key in keys)
            {
                res.SetValue(i++, key);
            }
            return res;
        }

        private object _append(INakoFuncCallInfo info){

            object ary = info.StackPop(); // 参照渡しなので変数への参照が得られる
            object s   = info.StackPop();
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
			}else if(ary_link.Body is string && (string)ary_link.Body==""){
                NakoVarArray arr = new NakoVarArray();
                NakoVariable new_item = new NakoVariable();
                new_item.SetBodyAutoType(s);
                arr.Add(new_item);
                ary_link.SetBody(arr,NakoVarType.Array);
            }
            // 結果をセット
            return null;
        }

        private object _pop(INakoFuncCallInfo info)
        {

            object ary = info.StackPop(); // 参照渡しなので変数への参照が得られる
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

        private object _count_array(INakoFuncCallInfo info)
        {
            object ar = info.StackPop();
            if (!(ar is NakoVarArray))
            {
                throw new NakoPluginRuntimeException("『要素数』の引数がarrayではありません");
            }
            NakoVarArray arr = (NakoVarArray)ar;
            return arr.Count;
        }

        private object _count(INakoFuncCallInfo info)
        {
            object ar = info.StackPop();
            if (ar is NakoVarArray)
            {
                NakoVarArray arr = (NakoVarArray)ar;
                return arr.Count;
            }else if(ar is string){
                int count = 0;
                System.IO.StringReader sr = new System.IO.StringReader((string)ar);
                while(sr.Peek()>=0){
                    sr.ReadLine();
                    count++;
                }
                sr.Close();
                return count;
            }
            return 0;//TODO:Exception??
        }

        private object _removeAt(INakoFuncCallInfo info){

            object a = info.StackPop();
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
        private object _concat(INakoFuncCallInfo info){
            object ar = info.StackPop();
            string s = info.StackPopAsString();
            if (!(ar is NakoVarArray))
            {
                throw new NakoPluginRuntimeException("『結合』の引数が配列ではありません");
            }
            StringBuilder sb = new StringBuilder();
            NakoVarArray arr = (NakoVarArray)ar;
            int index = 0;
            while(arr.GetValue(index)!=null){
                if(index > 0) sb.Append(s);
                object var = arr.GetValue(index);
                if (var != null)
                {
                    sb.Append(var.ToString());
                }
                index++;
            }
            return sb.ToString();
        }
        private object _reverse(INakoFuncCallInfo info){

            object ar = info.StackPop();
            if (!(ar is NakoVariable))
            {
                throw new NakoPluginRuntimeException("『逆順』の引数が変数ではありません");
            }
            object a = ((NakoVariable)ar).Body;
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
        private object _search(INakoFuncCallInfo info){
            object ar = info.StackPop();
            int i = (int)info.StackPopAsInt();
            object key = info.StackPop();
            if (!(ar is NakoVarArray))
            {
                throw new NakoPluginRuntimeException("『検索』の引数が配列ではありません");
            }
            NakoVarArray arr = (NakoVarArray)ar;
            while(arr.GetValue(i)!=null){
               object var = arr.GetValue(i);
               if(var.ToString()==key.ToString()){
                   return i;
               }
               i++;
            }
            return -1;
        }

		/// <summary>
		/// 配列キー存在?
		/// </summary>
		/// <param name="array"></param>
		/// <param name="key"></param>
		/// <returns></returns>
		private object _hasKey(INakoFuncCallInfo info)
		{
			object ar = info.StackPop();
			NakoVarArray arv = (NakoVarArray)ar;
			string searchKey = info.StackPopAsString ();
			if (arv.Type != NakoVarType.Array)
			{
				throw new NakoPluginArgmentException("『存在』の引数が配列ではありません。");
			}
			string[] keys = arv.GetKeys();
			NakoVarArray res = info.CreateArray();
			foreach (string key in keys)
			{
				if (key == searchKey)
					return 1;
			}
			return 0;
		}
    }
}