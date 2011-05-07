using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using NakoPlugin;


namespace Libnako.JPNCompiler
{
    /// <summary>
    /// なでしこのソースコードを読み込むローダークラス(Singleton)
    /// </summary>
    public class NakoLoader
    {
        /// <summary>
        /// ファイル一覧
        /// </summary>
        protected List<string> files;
        /// <summary>
        /// ネームスペースの一覧
        /// </summary>
        protected List<NakoCompiler> namespaceList;
        /// <summary>
        /// 基本パス
        /// </summary>
        protected string basePath;
        /// <summary>
        /// 最後に利用したコンパイラ
        /// </summary>
        public NakoCompiler cur;
        /// <summary>
        /// 読み込み情報
        /// </summary>
        public NakoCompilerLoaderInfo LoaderInfo = null;
        /// <summary>
        /// デバッグするか
        /// </summary>
        public bool DebugMode = false;
        private static NakoLoader instance = null;
        /// <summary>
        /// ローダー唯一のインスタンス(Singleton)
        /// </summary>
        public static NakoLoader Instance
        {
            get {
                if (instance == null)
                {
                    instance = new NakoLoader();
                }
                return instance;
            }
        }

        private NakoLoader()
        {
            files = new List<string>();
            namespaceList = new List<NakoCompiler>();
        }

        /// <summary>
        /// ソースコードをトークンに分ける
        /// </summary>
        /// <param name="src"></param>
        /// <param name="filename"></param>
        public void ParseEx(string src, string filename)
        {
            cur = new NakoCompiler(LoaderInfo);
            cur.DebugMode = this.DebugMode;
            cur.source = src;
            cur.fullpath = filename;
            cur.name = this.GetNamespaceFromPath(filename);
            cur.Tokenize();
            cur.Parse();
            cur.WriteIL();
            namespaceList.Add(cur);
        }

        /// <summary>
        /// ファイルからソースコードを読みこんでパースする
        /// </summary>
        /// <param name="filename"></param>
        public void LoadFromFile(string filename)
        {
            string filename2 = this.RemoveBasePath(filename);
            int i = files.IndexOf(filename2);
            if (i >= 0) {
                // 二重で取り込みはしない
                return;
            }
            files.Add(filename2);
            String src = StrUnit.LoadFromFileAutoEnc(filename);
                        
            // Parse
            this.ParseEx(src, filename);
        }

        /// <summary>
        /// ベースパスを取り除いたファイル名部分だけを取り出す
        /// </summary>
        /// <param name="path">ファイルパス</param>
        /// <returns></returns>
        public string RemoveBasePath(string path)
        {
            if (basePath == "" || basePath == null) {
                basePath = Path.GetDirectoryName(path);
            }
            if (path.Substring(0, basePath.Length) == path) {
                path = path.Substring(basePath.Length);
            }
            return path;
        }

        /// <summary>
        /// パスからネームスペースを得る
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public string GetNamespaceFromPath(string path)
        {
            string name = Path.GetFileName(path);
            int i = name.IndexOf('.');
            if (i >= 0) {
                name = name.Substring(0, i);
            }
            return name;
        }


    }
}
