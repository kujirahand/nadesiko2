using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Libnako.NakoAPI.WrapLib;


namespace Libnako.JCompiler
{
    public class NakoLoader
    {
        protected List<string> files;
        protected List<NakoCompiler> namespaceList;
        protected string basePath;
        public NakoCompiler cur;

        private static NakoLoader instance = null;
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
        public void ParseEx(string src, string filename)
        {
            cur = new NakoCompiler(src);
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
        /// <param name="?"></param>
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
