using System;
using System.Collections.Generic;
using System.Text;
using System.IO;


namespace Libnako.Parser
{
    public class NakoLoader
    {
        protected List<string> files;
        protected List<NakoNamespace> namespaceList;
        protected string basePath;
        protected NakoNamespace cur;

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
            namespaceList = new List<NakoNamespace>();
        }

        /// <summary>
        /// ソースコードをトークンに分ける
        /// </summary>
        /// <param name="src"></param>
        public void Parse(string src, string filename)
        {
            cur = new NakoNamespace(src);
            cur.fullpath = filename;
            cur.name = this.GetNamespaceFromPath(filename);
            cur.Tokenize();
            cur.Parse();
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
            String src;
            files.Add(filename2);
            using (StreamReader sr = new StreamReader(filename, Encoding.UTF8))
            {
                src = sr.ReadToEnd();
            }
            this.Parse(src, filename);
        }

        /// <summary>
        /// ベースパスを取り除いたファイル名部分だけを取り出す
        /// </summary>
        /// <param name="path">ファイルパス</param>
        /// <returns></returns>
        public string RemoveBasePath(string path)
        {
            if (basePath == "") {
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
