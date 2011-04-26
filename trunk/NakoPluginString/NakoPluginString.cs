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
using Libnako.JPNCompiler;
using NakoPlugin;
using Microsoft.VisualBasic;

namespace NakoPluginString
{
    /// <summary>
    /// Description of MyClass.
    /// </summary>
    public class NakoPluginString : INakoPlugin
    {
        string _description = "文字列処理を行うプラグイン";
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
            bank.AddFunc("文字挿入", "SのCNTにAを", NakoVarType.String, _insert, "文字列SのCNT文字目に文字列Aを挿入して返す。", "もじそうにゅう");
            bank.AddFunc("文字列分解", "Sを|Sの|Sで", NakoVarType.Array, _degrade, "文字列Sを1文字ずつ配列変数に分解する。", "もじれつぶんかい");
            bank.AddFunc("区切る", "SをAで", NakoVarType.Array, _explode, "文字列Sを区切り文字Aで区切って配列として返す。", "くぎる");
            bank.AddFunc("数字か判定", "Sが|Sの|Sを", NakoVarType.Int, _num, "文字列Sの一文字目が数字か判定して返す", "すうじかはんてい");
            bank.AddFunc("追加", "AにBを|Aへ", NakoVarType.String, _append, "変数AにBの内容を追加する", "ついか");

            bank.AddFunc("英数半角変換", "Sを", NakoVarType.String, _alnumToEn, "文字列の文字数を返す", "えいすうはんかくへんかん");
            bank.AddFunc("ゼロ埋め", "SをAで", NakoVarType.String, _zeroFill, "文字列の文字数を返す", "ぜろうめ");
            bank.AddFunc("半角変換", "Sを", NakoVarType.String, _toEn, "文字列を全て置換して返す", "はんかくへんかん");
        }
        
        // プラグインの初期化処理
        public void PluginInit(INakoInterpreter runner)
        {
        }
        // プラグインの終了処理
        public void PluginFin(INakoInterpreter runner)
        {
        }
        
        // Define Method
        public Object _length(INakoFuncCallInfo info)
        {
        	String s = info.StackPopAsString();
            return s.Length;
        }
        public Object _search(INakoFuncCallInfo info)
        {
        	String s = info.StackPopAsString();
        	String needle = info.StackPopAsString();
        	return s.IndexOf(needle) + 1;
        }
        public Object _replace(INakoFuncCallInfo info)
        {
        	String s = info.StackPopAsString();
        	String search = info.StackPopAsString();
        	String replace = info.StackPopAsString();
        	return s.Replace(search,replace);
        }
        public Object _replace_a(INakoFuncCallInfo info)
        {
        	String s = info.StackPopAsString();
        	String search = info.StackPopAsString();
        	String replace = info.StackPopAsString();
        	int index = s.IndexOf(search);
        	String pre = s.Substring(0,index);
        	String post = s.Substring(index+search.Length);
        	return pre + replace + post;
        }
        public Object _left(INakoFuncCallInfo info)
        {
        	String s = info.StackPopAsString();
        	int len = (int)info.StackPopAsInt();
        	return s.Substring(0,len);
        }
        public Object _trim(INakoFuncCallInfo info)
        {
        	String s = info.StackPopAsString();
        	return s.Trim();
        }
        public Object _right(INakoFuncCallInfo info){
        	String s = info.StackPopAsString();
        	int len = (int)info.StackPopAsInt();
        	return s.Substring(s.Length-len);
        }
        public Object _cut(INakoFuncCallInfo info){
            Object sr = info.StackPop();
            String a = info.StackPopAsString();
            string[] delim = {a};
            if(!(sr is NakoVariable)){
                throw new ApplicationException("『切り取る』に変数が設定されていません");
            }
            Object s = ((NakoVariable)sr).Body;
            Object ret;
            if(s is String){
                string[] split_s = ((String)s).Split(delim,2,StringSplitOptions.None);
                if(split_s.Length==2){
                   ret = split_s[1];
                   ((NakoVariable)sr).Body = ret;
                    return split_s[0];
                }
            }
            return null;
        }
        public Object _extract(INakoFuncCallInfo info){
            String s = info.StackPopAsString();
            int a = NadesikoPositionToCSPosition((int)info.StackPopAsInt());
            int cnt = (int)info.StackPopAsInt();
            return s.Substring(a,cnt);
        }
        public Object _Em(INakoFuncCallInfo info){
            String s = info.StackPopAsString();
            char c1 = s[0];
            int num = Encoding.GetEncoding("Shift_JIS").GetByteCount(c1.ToString());
            if(num==2){
                return 1;
            }else{
                return 0;
            }
        }
        public Object _remove(INakoFuncCallInfo info){
            Object sr = info.StackPop();
            int a = NadesikoPositionToCSPosition((int)info.StackPopAsInt());
            int b = (int)info.StackPopAsInt();
            Object s = ((NakoVariable)sr).Body;
            Object ret;
            if(s is String){
                ret = ((String)s).Remove(a,b);
            }else{
                ret = null;
            }
            ((NakoVariable)sr).Body = ret;
            return null;
        }
        public Object _insert(INakoFuncCallInfo info){
            StringBuilder s = new StringBuilder(info.StackPopAsString());
            int cnt = NadesikoPositionToCSPosition((int)info.StackPopAsInt());
            String a = info.StackPopAsString();
            return s.Insert(cnt,a).ToString();
        }
        public Object _degrade(INakoFuncCallInfo info){
            String s = info.StackPopAsString();
            char[] splitted = s.ToCharArray();
             NakoVarArray arr = new NakoVarArray();
            for(int i=0;i<splitted.Length;i++){
                arr.SetValue(i,splitted[i]);
            }
            return arr;
       }
        public Object _explode(INakoFuncCallInfo info){
            String s = info.StackPopAsString();
            String a = info.StackPopAsString();
            string[] splitted = s.Split(new string[]{a},StringSplitOptions.None);
            NakoVarArray arr = new NakoVarArray();
            for(int i=0;i<splitted.Length;i++){
                arr.SetValue(i,splitted[i]);
            }
            return arr;
        }
        public Object _num(INakoFuncCallInfo info){
            String s = info.StackPopAsString();
            char c1 = s[0];
            int num;
            if(int.TryParse(c1.ToString(), out num)){
                return 1;
            }else{
                return 0;
            }
        }
        public Object _append(INakoFuncCallInfo info){
            StringBuilder s = new StringBuilder(info.StackPopAsString());
            String a = info.StackPopAsString();
            return s.Append(a).ToString();
        }
        public Object _alnumToEn(INakoFuncCallInfo info)
        {
        	String s = info.StackPopAsString();
        	return Regex.Replace(s,@"[０-９Ａ-Ｚａ-ｚ：－　]+",delegate(Match m){ return Strings.StrConv(m.Value, VbStrConv.Narrow, 0); });
        }
        public Object _zeroFill(INakoFuncCallInfo info){
        	long s = info.StackPopAsInt();
        	String l = info.StackPopAsInt().ToString();
        	return　String.Format(@"{0:D"+l+"}",s);
        }
        
        public Object _toEn(INakoFuncCallInfo info){
        	String s = info.StackPopAsString();
            return Strings.StrConv(s, VbStrConv.Narrow, 0);
        }
        private int NadesikoPositionToCSPosition(int nadesiko_pos){
            return nadesiko_pos - 1;
        }
    }
}