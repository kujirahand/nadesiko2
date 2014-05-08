using System;
using System.Text;

using Libnako.JPNCompiler;
using Libnako.Interpreter;

using NakoPlugin;

namespace NakoPluginEval
{
	public class NakoPluginEval : INakoPlugin
	{
		//--- プラグインの宣言 ---
		string _description = "Evalプラグイン";
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
			//+ テキストファイルの読み書き
			bank.AddFunc("Nadesiko", "Sを|Sで", NakoVarType.String, _eval, "Eval。", "Nadesiko");
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
		public Object _eval(INakoFuncCallInfo info)
		{
			string s = info.StackPopAsString();
			NakoCompiler compiler = new NakoCompiler();
			compiler.DirectSource = s;
			NakoInterpreter runner = new NakoInterpreter(compiler.Codes);
			runner.Run();
			Console.WriteLine("EVALLOG=" + runner.PrintLog);
			return runner.globalVar.GetValue(0);
		}
	}
}
