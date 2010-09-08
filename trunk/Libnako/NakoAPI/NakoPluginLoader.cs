using System;
using System.Collections.Generic;
using System.Linq;
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
        
        public NakoPluginInfo[] FindPlugins()
        {
            // プラグインインターフェイスのフルパスを調べる
            string INakoPluginsPath = typeof(INakoPlugin).FullName;

            // プラグインパスを調べる
            string appdir = System.IO.Path.GetDirectoryName(
                System.Reflection.Assembly.GetExecutingAssembly().Location);
            string plugdir = appdir + "\\plug-ins";
            if (!System.IO.Directory.Exists(plugdir))
            {
                return null;
            }

            // プラグインフォルダのDLLを列挙
            string[] dlls = System.IO.Directory.GetFiles(plugdir, "*.dll");
            foreach (string dll in dlls)
            {
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
                }

            }
            return plugins.ToArray();
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
