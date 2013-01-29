using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NakoPlugin
{
    /// <summary>
    /// なでしこプラグインのひな形（これを継承してプラグインを作ると楽）
    /// </summary>
    public class NakoPluginTemplate : INakoPlugin
    {
        /// <summary>
        /// プラグインのバージョン番号
        /// </summary>
        protected Version _pluginVersion = new Version(0, 0);
        /// <summary>
        /// プラグインの説明
        /// </summary>
        protected string _pluginDescript = "TODO";
        /// <summary>
        /// 対応プラグインのバージョン番号 (現在は必ず 2.0 を返すようにする)
        /// </summary>
        public Version TargetNakoVersion { get { return new Version(2, 0); } }
        /// <summary>
        /// プラグインの名前 ( this.GetType().FullName とクラスのフルパスを指定する )
        /// </summary>
        public string Name { get { return this.GetType().FullName; } }
        /// <summary>
        /// プラグインのバージョン（任意の値を返すことができる）
        /// </summary>
        public Version PluginVersion { get { return _pluginVersion; } }
        /// <summary>
        /// プラグインの説明を返す
        /// </summary>
        public string Description { get { return _pluginDescript; } }
        /// <summary>
        /// プラグインが利用されたかどうかを判別する (自動的に設定されるので定義するだけでOK)
        /// </summary>
        public bool Used { get; set; }
        /// <summary>
        /// プラグインでなでしこの関数を定義する
        /// Libnako.NakoAPI.NakoBaseSystem.DefineFunction　に登録例あり
        /// </summary>
        /// <param name="bank">このオブジェクトに命令を登録する</param>
        public void DefineFunction(INakoPluginBank bank) { }
        /// <summary>
        /// アプリ開始時に実行する処理
        /// </summary>
        public void PluginInit(INakoInterpreter runner) { }
        /// <summary>
        /// アプリ終了時に実行する処理(メモリの解放など)
        /// </summary>
        public void PluginFin(INakoInterpreter runner) { }
    }
}
