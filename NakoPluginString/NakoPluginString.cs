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
using System.Threading;

using NakoPlugin;

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
            //+日付時間処理
            //-日付時間
            bank.AddFunc("文字数", "Sの", NakoVarType.Int, _length, "文字列の文字数を返す", "もじすう");
            bank.AddFunc("文字列検索", "SのNEEDLEを", NakoVarType.Int, _search, "文字列を検索して最初に現れた場所を返す。無ければ-1", "もじれつけんさく");
            bank.AddFunc("置換", "SのSEARCHをREPLACEに", NakoVarType.String, _replace, "文字列を全て置換して返す", "ちかん");
            bank.AddFunc("単置換", "SのSEARCHをREPLACEに", NakoVarType.String, _replace_a, "文字列を一つだけ置換して返す", "たんちかん");
            bank.AddFunc("文字左部分", "SのNUM", NakoVarType.String, _left, "文字列の指定文字数左部分を抜き出す", "もじひだりぶぶん");
            bank.AddFunc("トリム", "Sを", NakoVarType.String, _trim, "文字列をトリムする", "とりむ");
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
       
    }
}