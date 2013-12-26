using System;
using NakoPlugin;

namespace NakoPluginSystem
{
    public class NakoPluginCast : INakoPlugin
    {
        
        string _description = "変数変換プラグイン";
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
            bank.AddFunc("整数変換", "Sを", NakoVarType.Int, _to_int,"変数Sを整数に変換して返す", "せいすうへんかん");
            bank.AddFunc("文字列変換", "Sを", NakoVarType.String, _to_string,"変数Sを文字列に変換して", "もじれつへんかん");
            bank.AddFunc("実数変換", "Sを", NakoVarType.Double, _toDouble,"変数Sを実数に変換して返す", "じっすうへんかん");
        }
        // プラグインの初期化処理
        public void PluginInit(INakoInterpreter runner)
        {
        }
        // プラグインの終了処理
        public void PluginFin(INakoInterpreter runner)
        {
        }
        public object _to_int(INakoFuncCallInfo info){
            return info.StackPopAsInt();
        }
        
        public Object _to_string(INakoFuncCallInfo info){
            return info.StackPopAsString();
        }

        public object _toDouble(INakoFuncCallInfo info){
            return info.StackPopAsDouble();
        }
    }
}

