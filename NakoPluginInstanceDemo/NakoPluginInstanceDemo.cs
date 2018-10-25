using System;
using NakoPlugin;

namespace NakoPluginInstanceDemo
{
    public class CountDown {
        public event EventHandler on_zero;
        public int no = 10;
        public int decrement(int d){
            no = no - d;
            if (no == 0 && on_zero != null) {
                OnZero(new EventArgs());
            }
            return no;
        }
        protected virtual void OnZero(EventArgs args){
            if (on_zero != null) {
                on_zero(this, args);
            }
        }
    }
    public class NakoPluginInstanceDemo : NakoPluginTemplate, INakoPlugin
    {
        string _description = "Plugin for instance demo";
        Version _version = new Version(1, 0);
        //--- プラグイン共通の部分 ---
        public Version TargetNakoVersion { get { return new Version(2, 0); } }
        public string Name { get { return "カウントダウンタイマー"; } }
        public Version PluginVersion { get { return _version; } }
        public string Description { get { return _description; } }
        //--- 関数の定義 ---
        public void DefineFunction(INakoPluginBank bank)
        {
            bank.AddFunc ("カウントダウンタイマー", "", NakoVarType.Instance, _create, "create instance", "カウントダウンタイマー");
            bank.AddInstanceFunc ("カウントダウン", "OをAで", NakoVarType.String, _methodA, "instance method", "methodA");
            bank.AddInstanceFunc ("NO", "Oの", NakoVarType.String, _no, "表示", "print");
            bank.AddInstanceFunc ("OnZero設定", "OについてFを", NakoVarType.Void, _onzero, "onzero callback", "OnZero");
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
        public object _onzero(INakoFuncCallInfo info){
            CountDown o = info.StackPop() as CountDown;
            string func_name = info.StackPopAsString ();
            o.on_zero += info.GetCallback(func_name);
            return null;
        }
    }
}

