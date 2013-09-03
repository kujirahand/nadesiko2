using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using NakoPlugin;

namespace NakoPluginSample
{
    /// <summary>
    /// サンプルプラグイン
    /// </summary>
    public class NakoPluginSample : INakoPlugin
    {
        
        string _description = "サンプルプラグイン";
        Version _version = new Version(1, 0);
        //--- プラグイン共通の部分 ---
        public Version TargetNakoVersion { get { return new Version(2, 0); } }
        public bool Used { get; set; }
        public string Name { get { return this.GetType().FullName; } }
        public Version PluginVersion { get { return _version; } }
        public string Description { get { return _description; } }
        //--- 関数の定義 ---
        public void DefineFunction(INakoPluginBank bank)
        {
            bank.AddFunc("接続!", "{参照渡し}AにBを|Aと", NakoVarType.Object, _sample_addEx,"変数Aと値Bをつなげて返す(変数A自身を書き換える)", "せつぞく!");
            bank.AddFunc("変数HOGE書換", "Sに|Sへ", NakoVarType.Void, _sample_changeHoge,"変数HOGEの内容をSに書き換える", "へんすうHOGEかきかえ");
        }
        
        // プラグインの初期化処理
        public void PluginInit(INakoInterpreter runner)
        {
        }
        // プラグインの終了処理
        public void PluginFin(INakoInterpreter runner)
        {
        }
       public Object _sample_addEx(INakoFuncCallInfo info){

            Object ar = info.StackPop();
            Object b = info.StackPop();
            if (!(ar is NakoVariable))
            {
                throw new ApplicationException("『接続!』の引数が変数ではありません");
            }
            Object a = ((NakoVariable)ar).Body;
            Object c;
            if (a is string && b is string)
            {
                c = (string)a + (string)b;
            }
            else
            {
                c = null;
            }
            // 結果をセット
            ((NakoVariable)ar).SetBodyAutoType(c);
            return (c);
        }
        
        public Object _sample_changeHoge(INakoFuncCallInfo info){
        	Object tmp = info.StackPop();
        	info.SetVariableValue("HOGE", tmp);
        	return null;
        }
    }
}