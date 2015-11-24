/*
 * Created by SharpDevelop.
 * User: shigepon
 * Date: 2011/03/31
 * Time: 9:31
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using NakoPlugin;
using Microsoft.VisualBasic;

namespace Libnako.NakoAPI
{
    /// <summary>
    /// Description of MyClass.
    /// </summary>
    public class NakoPluginString : NakoPluginTemplate, INakoPlugin
    {
        /// <summary>
        /// プラグインのバージョン番号
        /// </summary>
        new protected double _pluginVersion = 1.0;
        /// <summary>
        /// プラグインの説明
        /// </summary>
        new protected string _pluginDescript = "文字列処理を行うプラグイン";

        //--- 関数の定義 ---
        /// <summary>
        /// 関数の定義
        /// </summary>
        /// <param name="bank"></param>
        new public void DefineFunction(INakoPluginBank bank)
        {
            //+文字列処理
            //-文字列処理
            bank.AddFunc("文字数", "Sの", NakoVarType.Int, _length, "文字列の文字数を返す", "もじすう");
            bank.AddFunc("文字列検索", "SのNEEDLEを", NakoVarType.Int, _search, "文字列を検索して最初に現れた場所を返す。無ければ-1", "もじれつけんさく");
            bank.AddFunc("置換", "SのSEARCHをREPLACEに", NakoVarType.String, _replace, "文字列を全て置換して返す", "ちかん");
            bank.AddFunc("単置換", "SのSEARCHをREPLACEに", NakoVarType.String, _replace_a, "文字列を一つだけ置換して返す", "たんちかん");
            bank.AddFunc("文字左部分", "SのNUM", NakoVarType.String, _left, "文字列の指定文字数左部分を抜き出す", "もじひだりぶぶん");
            bank.AddFunc("トリム", "Sを", NakoVarType.String, _trim, "文字列をトリムする", "とりむ");

            bank.AddFunc("文字右部分", "SのNUM", NakoVarType.String, _right, "文字列の指定文字数右部分を抜き出す", "もじみぎぶぶん");
            bank.AddFunc("切り取る", "{参照渡し}SからAまで|SのAまでを|SでAを", NakoVarType.String, _cut, "文字列Sから区切り文字Aまでを切り取って返す。Sに変数を指定した場合はSの内容が切り取られる。", "きりとる");
            bank.AddFunc("文字抜き出す", "SのAからCNT", NakoVarType.String, _extract, "文字列SでAからCNT文字分を抜き出して返す", "もじぬきだす");
            bank.AddFunc("全角か判定", "Sが|Sの|Sを", NakoVarType.Int, _Em, "文字列Sの一文字目が全角かどうか判定して返す。", "ぜんかくかはんてい");
            bank.AddFunc("文字削除", "{参照渡し}SのAからB|Sで", NakoVarType.Void, _remove, "文字列SのA文字目からB文字だけ削除する。Sに変数を指定した場合はSの内容も変更する", "もじさくじょ");
            bank.AddFunc("文字右端削除", "{参照渡し}SのB|Sで", NakoVarType.Void, _removeright, "文字列SからB文字だけ右側を削除する。Sに変数を指定した場合はSの内容も変更する", "もじみぎはしさくじょ");//未実装
            bank.AddFunc("文字挿入", "SのCNTにAを", NakoVarType.String, _insert, "文字列SのCNT文字目に文字列Aを挿入して返す。", "もじそうにゅう");
            bank.AddFunc("文字列分解", "Sを|Sの|Sで", NakoVarType.Array, _degrade, "文字列Sを1文字ずつ配列変数に分解する。", "もじれつぶんかい");
            bank.AddFunc("区切る", "SをAで", NakoVarType.Array, _explode, "文字列Sを区切り文字Aで区切って配列として返す。", "くぎる");
            bank.AddFunc("数字か判定", "Sが|Sの|Sを", NakoVarType.Int, _num, "文字列Sの一文字目が数字か判定して返す", "すうじかはんてい");
            bank.AddFunc("追加", "{参照渡し}AにBを|Aへ", NakoVarType.Void, _append, "変数AにBの内容を追加する", "ついか");

            bank.AddFunc("英数半角変換", "Sを", NakoVarType.String, _alnumToEn, "文字列Sを英数文字だけを半角に変換して返す", "えいすうはんかくへんかん");
            bank.AddFunc("ゼロ埋め", "SをAで", NakoVarType.String, _zeroFill, "データSをA桁のゼロで埋めて出力する", "ぜろうめ");
            bank.AddFunc("半角変換", "Sを", NakoVarType.String, _toEn, "文字列Sを半角に変換して返す", "はんかくへんかん");
            bank.AddFunc("小文字変換", "Sを", NakoVarType.String, _lowercase, "文字列Sを小文字変換して返す", "こもじへんかん");
            bank.AddFunc("大文字変換", "Sを", NakoVarType.String, _uppercase, "文字列Sを大文字変換して返す", "へんかん");

            bank.AddFunc("何文字目", "SでSSが|Sの", NakoVarType.String, _strpos, "文字列Sで文字列SSが何文字目にあるか調べて返す", "なんもじめ");
            bank.AddFunc("出現回数", "SでAの", NakoVarType.Int, _occurrence, "文字列SでAの出てくる回数を返す。", "しゅつげんかいすう");
            bank.AddFunc("カナローマ字変換", "Sを|Sから", NakoVarType.String, _convert_kana_to_roman, "文字列Sにあるカタカナをローマ字に変換する。", "かなろーまじへんかん");//未実装
            bank.AddFunc("かな変換", "Sを", NakoVarType.String, _toKana, "文字列Sをひらがなに変換して返す。", "かなへんかん");//未実装
            bank.AddFunc("範囲切り取る", "{参照渡し}SのAからBを|Bまでを", NakoVarType.String, _cutRange, "文字列Sの区切り文字Aから区切り文字Bまでを切り取って返す。Sに変数を指定した場合はSの内容が切り取られる。SにBが存在しないとき、Sの最後まで切り取る。Aが存在しないときは切り取らない。", "はんいきりとる");
        }
        private object _toKana(INakoFuncCallInfo info){
            string s = info.StackPopAsString();
         if(NWEnviroment.isWindows()){
				return Strings.StrConv(s, VbStrConv.Hiragana, 0);
         }else{
             return LinuxCommand.execute("echo '"+s+"' | nkf --hiragana").Replace("\n","");
         }
        }
        private object _cutRange(INakoFuncCallInfo info){
            object sr = info.StackPop();
            string a = info.StackPopAsString();
            string b = info.StackPopAsString();
            string[] predelim = {a};
            string[] postdelim = {b};
            if(!(sr is NakoVariable)){
                throw new ApplicationException("『範囲切り取る』に変数が設定されていません");
            }
            object s = ((NakoVariable)sr).Body;
            string ret;
            if(s is string){
                string[] split_s = ((string)s).Split(predelim,2,StringSplitOptions.None);
                if(split_s.Length==2){
                   ret = split_s[1];
                   string[] post_split_s = ret.Split(postdelim,2,StringSplitOptions.None);
                   if(post_split_s.Length==2){
                   ((NakoVariable)sr).SetBodyAutoType(split_s[0] + post_split_s[1]);
                    return post_split_s[0];
                    }
                }
            }
            return "";
        }

        // Define Method
        /// <summary>
        /// 文字列の長さを調べる
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        private object _length(INakoFuncCallInfo info)
        {
        	string s = info.StackPopAsString();
            return s.Length;
        }
        /// <summary>
        /// 文字列検索
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        private object _search(INakoFuncCallInfo info)
        {
        	string s = info.StackPopAsString();
        	string needle = info.StackPopAsString();
        	return s.IndexOf(needle) + 1;
        }
        /// <summary>
        /// 置換
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        private object _replace(INakoFuncCallInfo info)
        {
        	string s = info.StackPopAsString();
        	string search = info.StackPopAsString();
        	string replace = info.StackPopAsString();
        	return s.Replace(search,replace);
        }
        /// <summary>
        /// 単置換
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        private object _replace_a(INakoFuncCallInfo info)
        {
        	string s = info.StackPopAsString();
        	string search = info.StackPopAsString();
        	string replace = info.StackPopAsString();
        	int index = s.IndexOf(search);
            if (index >= 0) {
                string pre = s.Substring (0, index);
                string post = s.Substring (index + search.Length);
                return pre + replace + post;
            }
            return s;
        }
        /// <summary>
        /// 左からN文字の部分文字列
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        private object _left(INakoFuncCallInfo info)
        {
        	string s = info.StackPopAsString();
        	int len = (int)info.StackPopAsInt();
            return (s.Length > len)? s.Substring(0,len) : s;
        }
        /// <summary>
        /// トリム
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        private object _trim(INakoFuncCallInfo info)
        {
        	string s = info.StackPopAsString();
        	return s.Trim();
        }
        /// <summary>
        /// 右から部分文字列を返す
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        private object _right(INakoFuncCallInfo info){
        	string s = info.StackPopAsString();
        	int len = (int)info.StackPopAsInt();
            return (s.Length > len)? s.Substring(s.Length-len) : s;
        }
        /// <summary>
        /// 切り取る
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        private object _cut(INakoFuncCallInfo info){
            object sr = info.StackPop();
            string a = info.StackPopAsString();
            string[] delim = {a};
            if(!(sr is NakoVariable)){
                throw new ApplicationException("『切り取る』に変数が設定されていません");
            }
            object s = ((NakoVariable)sr).Body;
            object ret;
            if(s is string){
                string[] split_s = ((string)s).Split(delim,2,StringSplitOptions.None);
                if(split_s.Length==2){
                   ret = split_s[1];
                   ((NakoVariable)sr).SetBodyAutoType(ret);
                    return split_s[0];
                }
            }
            return "";
        }
        /// <summary>
        /// 文字抜き出す
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        private object _extract(INakoFuncCallInfo info){
            string s = info.StackPopAsString();
            int a = NadesikoPositionToCSPosition((int)info.StackPopAsInt());
            int cnt = (int)info.StackPopAsInt();
            return (cnt <= s.Length)? s.Substring(a,cnt) : s;
        }
        private object _Em(INakoFuncCallInfo info){
            string s = info.StackPopAsString();
            char c1 = s[0];
            int num = Encoding.GetEncoding("Shift_JIS").GetByteCount(c1.ToString());
            if(num==2){
                return 1;
            }else{
                return 0;
            }
        }
        private object _remove(INakoFuncCallInfo info){
            object sr = info.StackPop();
            int a = NadesikoPositionToCSPosition((int)info.StackPopAsInt());
            int b = (int)info.StackPopAsInt();
            object s = ((NakoVariable)sr).Body;
            object ret;
            if(s is string){
                string str = (string)s;
                if (a < str.Length) {
                    if (a + b > str.Length) {
                        b = str.Length - a;
                    }
                    ret = str.Remove (a, b);
                    ((NakoVariable)sr).SetBodyAutoType (ret);
                }
            }
            return "";
        }
        private object _removeright(INakoFuncCallInfo info){
            object sr = info.StackPop();
            int a = (int)info.StackPopAsInt();
            object s = ((NakoVariable)sr).Body;
            object ret;
            if(s is string){
                string _tmp = (string)s;
                if (a > _tmp.Length) {
                    a = _tmp.Length;
                }
                ret = _tmp.Remove(_tmp.Length - a);
                ((NakoVariable)sr).SetBodyAutoType(ret);
            }
            return "";
        }
        private object _insert(INakoFuncCallInfo info){
            StringBuilder s = new StringBuilder(info.StackPopAsString());
            int cnt = NadesikoPositionToCSPosition((int)info.StackPopAsInt());
            string a = info.StackPopAsString();
            return s.Insert(cnt,a).ToString();
        }
        private object _degrade(INakoFuncCallInfo info){
            string s = info.StackPopAsString();
            char[] splitted = s.ToCharArray();
             NakoVarArray arr = info.CreateArray();
            for(int i=0;i<splitted.Length;i++){
                arr.SetValue(i,splitted[i]);
            }
            return arr;
       }
        private object _explode(INakoFuncCallInfo info){
            string s = info.StackPopAsString();
            string a = info.StackPopAsString();
            string[] splitted = s.Split(new string[]{a},StringSplitOptions.None);
            NakoVarArray arr = info.CreateArray();
            for(int i=0;i<splitted.Length;i++){
                arr.SetValue(i,splitted[i]);
            }
            return arr;
        }
        private object _num(INakoFuncCallInfo info){
            string s = info.StackPopAsString();
            char c1 = s[0];
            int num;
            if(int.TryParse(c1.ToString(), out num)){
                return 1;
            }else{
                return 0;
            }
        }
        /// <summary>
        /// 文字列に追加する
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        private object _append(INakoFuncCallInfo info){
            object sr = info.StackPop();
            object s = ((NakoVariable)sr).Body;
            string a = info.StackPopAsString();
            string ret = (string)s + a;
             ((NakoVariable)sr).SetBodyAutoType(ret);
            return null;
       }
        private object _alnumToEn(INakoFuncCallInfo info)
        {
        	string s = info.StackPopAsString();
        	return Regex.Replace(s,@"[０-９Ａ-Ｚａ-ｚ：－　]+",delegate(Match m){ 
			if(NWEnviroment.isWindows()){
					return Strings.StrConv(m.Value, VbStrConv.Narrow, 0); 
			}else{
				return LinuxCommand.execute("echo '"+m.Value+"' | nkf -Z3").Replace("\n","");
			}
			});
        }
        /// <summary>
        /// ゼロ埋め
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        private object _zeroFill(INakoFuncCallInfo info){
        	long s = info.StackPopAsInt();
        	string l = info.StackPopAsInt().ToString();
        	return String.Format(@"{0:D"+l+"}",s);
        }
        
        private object _toEn(INakoFuncCallInfo info){
        	string s = info.StackPopAsString();
			if(NWEnviroment.isWindows()){
				return Strings.StrConv(s, VbStrConv.Narrow, 0);
			}else{
				return LinuxCommand.execute("echo '"+s+"' | nkf -Z4").Replace("\n","");
			}
        }
        private int NadesikoPositionToCSPosition(int nadesiko_pos){
            return nadesiko_pos - 1;
        }
        
        private object _lowercase(INakoFuncCallInfo info)
        {
        	string s = info.StackPopAsString();
        	return s.ToLower();
        }
        private object _uppercase(INakoFuncCallInfo info)
        {
        	string s = info.StackPopAsString();
        	return s.ToUpper();
        }
        private object _strpos(INakoFuncCallInfo info)
        {
            string s = info.StackPopAsString();
            string ss = info.StackPopAsString();
            int i = s.IndexOf(ss);
            return (i + 1); // 1からはじまるので
        }
		
		private object _occurrence(INakoFuncCallInfo info){
            string s = info.StackPopAsString();
            string search = info.StackPopAsString();
			return (int)((s.Length - s.Replace(search,"").Length)/search.Length);
		}
		private object _convert_kana_to_roman(INakoFuncCallInfo info){
            string s = info.StackPopAsString();
			//TODO: 手動実装（めんどー）キャキュキョから始まって最後に1文字カナを変換
			return s;
		}
    }
}
