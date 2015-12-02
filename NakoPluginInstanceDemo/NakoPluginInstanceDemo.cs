using System;
using NakoPlugin;

namespace NakoPluginInstanceDemo
{
    public class CountDown {
        public int no = 10;
        public int decrement(int d){
            no = no - d;
            return no;
        }
    }
    public class NakoPluginInstanceDemo : INakoPlugin
    {
        string _description = "Plugin for instance demo";
        Version _version = new Version(1, 0);
        //--- プラグイン共通の部分 ---
        public Version TargetNakoVersion { get { return new Version(2, 0); } }
        public bool Used { get; set; }
        public string Name { get { return this.GetType().FullName; } }
        public Version PluginVersion { get { return _version; } }
        public string Description { get { return _description; } }
        // プラグインの初期化処理
        public void PluginInit(INakoInterpreter runner)
        {
        }
        // プラグインの終了処理
        public void PluginFin(INakoInterpreter runner)
        {
        }
        //--- 関数の定義 ---
        public void DefineFunction(INakoPluginBank bank)
        {
            bank.AddFunc ("カウントダウンタイマー", "", NakoVarType.Instance, _create, "create instance", "カウントダウンタイマー");
            bank.AddInstanceFunc ("カウントダウン", "OをAで", NakoVarType.String, _methodA, "instance method", "methodA");
            bank.AddInstanceFunc ("NO", "Oの", NakoVarType.String, _no, "表示", "print");
        }

        public object _create(INakoFuncCallInfo info){
            return new CountDown();
        }
        public object _methodA(INakoFuncCallInfo info){
            CountDown o = info.StackPop() as CountDown;
            int a = (int)info.StackPopAsInt ();
            o.decrement(a);
            return o;
        }
        public object _no(INakoFuncCallInfo info){
            CountDown o = info.StackPop() as CountDown;
            return o.no;
        }
    }
}

