/*
 * Created by SharpDevelop.
 * User: shigepon
 * Date: 2011/04/26
 * Time: 10:42
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Libnako.JPNCompiler;
using NakoPlugin;

namespace NakoPluginRegex
{
    /// <summary>
    /// Description of MyClass.
    /// </summary>
    public class NakoPluginRegex : INakoPlugin
    {
        string _description = "正規表現処理を行うプラグイン";
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
            bank.AddFunc("正規表現マッチ", "SをPATTERNで", NakoVarType.String, _match, ".Netの正規表現。文字列AをパターンBで最初にマッチした結果を返す。", "せいきひょうげんまっち");
            bank.AddFunc("正規表現全てマッチ", "SをPATTERNで", NakoVarType.String, _matchAll, ".Netの正規表現。文字列AをパターンBでマッチした結果を全て返す。", "せいきひょうげんすべてまっち");
            bank.AddFunc("正規表現置換", "SのPATTERNをREPLACEに", NakoVarType.String, _replace, ".Netの正規表現。文字列SのパターンAをBで置換して結果を返す。", "せいきひょうげんちかん");
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
        public Object _match(INakoFuncCallInfo info)
        {
        	String s = info.StackPopAsString();
        	String pattern = info.StackPopAsString();
        	m = Regex.Match(s,pattern);
        	NakoVarArray groups = new NakoVarArray();
        	NakoVariable ret = new NakoVariable();
        	if(m.Success){
        	    for(int i = 0;i < m.Groups.Count;i++){
        	        groups.SetValue(i,m.Groups[i].Value);
        	    }
        	    ret.SetBodyAutoType(groups);
        	    info.SetVariable("抽出文字列",ret);
        	    return m.Value;
        	}
            return null;
        }
        public Object _matchAll(INakoFuncCallInfo info)
        {
        	String s = info.StackPopAsString();
        	String pattern = info.StackPopAsString();
        	m = Regex.Match(s,pattern);
        	NakoVarArray res = new NakoVarArray();
        	int index = 0;
//        	NakoVarArray groups = new NakoVarArray();
//        	NakoVariable ret = new NakoVariable();
        	while(m.Success){
                res.SetValue(index,m.Value);
                index++;
                m.NextMatch();
//        	    for(int i = 0;i < m.Groups.Count;i++){
//        	        groups.SetValue(i,m.Groups[i].Value);
//        	    }
//        	    ret.Type = NakoVarType.Array;
//        	    ret.Body = groups;
        	}
     	    return res;
        }
        private Match m;
//        public Object _group(INakoFuncCallInfo info){
//        	NakoVarArray groups = new NakoVarArray();
//        	if(m is Match && m.Success){
//        	    for(int i = 0;i < m.Groups.Count;i++){
//        	        groups.SetValue(i,m.Groups[i].Value);
//        	    }
//        	    return groups;
//        	}
//        	return null;
//        }
        
        public Object _replace(INakoFuncCallInfo info)
        {
        	String s = info.StackPopAsString();
        	String pattern = info.StackPopAsString();
        	String replace = info.StackPopAsString();
        	return Regex.Replace(s,pattern,replace);
        }
        
    }
}