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
using Libnako.JPNCompiler;
using NakoPlugin;
using System.Web;

namespace NakoPluginHttp
{
    /// <summary>
    /// Description of MyClass.
    /// </summary>
    public class NakoPluginHttp : INakoPlugin
    {
        string _description = "Http処理を行うプラグイン";
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
            bank.AddFunc("URLデコード", "Sを", NakoVarType.String, _urlDecode, "SをURLデコードして返す", "ゆーあーるえるでこーど");
        }
        
        // プラグインの初期化処理
        public void PluginInit(INakoInterpreter runner)
        {
        }
        // プラグインの終了処理
        public void PluginFin(INakoInterpreter runner)
        {
        }
        
        public Object _urlDecode(INakoFuncCallInfo info)
        {
        	String s = info.StackPopAsString();
        	return Uri.UnescapeDataString(s);
        }
        
    }
}