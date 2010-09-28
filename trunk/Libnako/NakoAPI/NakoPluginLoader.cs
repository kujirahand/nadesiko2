﻿using System;
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

        public NakoPluginInfo[] FindPlugins()
        {
            // プラグインのあるパスを調べる
            // アプリケーションディレクトリ
            string appdir = System.IO.Path.GetDirectoryName(
                System.Reflection.Assembly.GetExecutingAssembly().Location);

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
                if (name == "Libnako.dll") continue;
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
