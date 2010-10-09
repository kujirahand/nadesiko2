using System;
using System.Collections.Generic;
using System.Text;

using NakoPlugin;

namespace Libnako.NakoAPI
{
    /// <summary>
    /// プラグインをロードするクラス
    /// </summary>
    public class NakoPluginLoader
    {
        List<NakoPluginInfo> plugins = new List<NakoPluginInfo>();

        // プラグインインターフェイスのフルパスを調べる
        string INakoPluginsPath = typeof(INakoPlugin).FullName;

        protected NakoPluginInfo[] FindPlugins()
        {
            // プラグインのあるパスを調べる
            // アプリケーションディレクトリ
            string basefile = 
                //System.Reflection.Assembly.GetCallingAssembly().Location;
                System.Reflection.Assembly.GetExecutingAssembly().Location;
            string appdir = System.IO.Path.GetDirectoryName(basefile);

            if (System.IO.Directory.Exists(appdir))
            {
                CheckDllFiles(appdir);
            }
            // プラグイン専用ディレクトリ
            string plugdir = appdir + "\\plug-ins";
            if (System.IO.Directory.Exists(plugdir))
            {
                CheckDllFiles(plugdir);
            }

            return plugins.ToArray();
        }

        void CheckDllFiles(string plugdir)
        {
            // プラグインフォルダのDLLを列挙
            string[] dlls = System.IO.Directory.GetFiles(plugdir, "*.dll");
            foreach (string dll in dlls)
            {
                // 例外をチェック
                string name = System.IO.Path.GetFileName(dll);
                name = name.ToLower();
                if (name == "libnako.dll") continue;
                // アセンブリを読み込む
                try
                {
                    System.Reflection.Assembly asm =
                        System.Reflection.Assembly.LoadFrom(dll);
                    foreach (Type t in asm.GetTypes())
                    {
                        if (t.IsClass && t.IsPublic && !t.IsAbstract &&
                            t.GetInterface(INakoPluginsPath) != null)
                        {
                            NakoPluginInfo info = new NakoPluginInfo();
                            info.Location = dll;
                            info.ClassName = t.FullName;
                            plugins.Add(info);
                        }
                    }
                }
                catch
                {
                    throw new Exception("プラグイン読込みエラー。");
                }

            }
        }
        
        /// <summary>
        /// プラグインの取り込みを行う
        /// </summary>
        public void LoadPlugins()
        {
            NakoAPIFuncBank bank = NakoAPIFuncBank.Instance;
            NakoPluginInfo[] plugs = FindPlugins();
            foreach (NakoPluginInfo info in plugs)
            {
                if (bank.PluginList.IndexOf(info.ClassName) < 0)
                {
                    INakoPlugin p = info.CreateInstance();
                    bank.SetPluginInstance(p);
                    p.DefineFunction(bank);
                    bank.PluginList.Add(info.ClassName);
                }
            }
        }
    }
    /// <summary>
    /// プラグイン情報を表わすクラス
    /// </summary>
    public class NakoPluginInfo
    {
        public String Location { get; set; }
        public String ClassName { get; set; }
        public INakoPlugin CreateInstance()
        {
            if (Location == null || ClassName == null) return null;
            System.Reflection.Assembly asm =
                System.Reflection.Assembly.LoadFrom(this.Location);
            return (NakoPlugin.INakoPlugin)
                asm.CreateInstance(this.ClassName);
        }
    }
}
