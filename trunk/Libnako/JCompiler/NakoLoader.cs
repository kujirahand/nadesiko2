using System;
using System.Collections.Generic;
using System.Text;
using System.IO;


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
            String src;
            files.Add(filename2);
            
            // 文字コード判別
            byte[] data = File.ReadAllBytes(filename);
            System.Text.Encoding enc = GetCode(data);
            
            if (enc == Encoding.UTF8) {
                src = Encoding.UTF8.GetString(data);
            }
            else if (enc == System.Text.Encoding.GetEncoding(932)) {
                src = System.Text.Encoding.GetEncoding(932).GetString(data);
            }
            else if (enc == Encoding.Unicode) {
                src = Encoding.Unicode.GetString(data);
            }
            else {
                throw new Exception("[Source Code Encoding Error]: " + filename);
            }
            
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


        /// <summary>
        /// 文字コードを判別する
        /// http://dobon.net/vb/dotnet/string/detectcode.html より引用
        /// </summary>
        /// <remarks>
        /// Jcode.pmのgetcodeメソッドを移植したものです。
        /// Jcode.pm(http://openlab.ring.gr.jp/Jcode/index-j.html)
        /// Jcode.pmのCopyright: Copyright 1999-2005 Dan Kogai
        /// </remarks>
        /// <param name="byts">文字コードを調べるデータ</param>
        /// <returns>適当と思われるEncodingオブジェクト。
        /// 判断できなかった時はnull。</returns>
        public static System.Text.Encoding GetCode(byte[] bytes)
        {
            const byte bEscape = 0x1B;
            const byte bAt = 0x40;
            const byte bDollar = 0x24;
            const byte bAnd = 0x26;
            const byte bOpen = 0x28;    //'('
            const byte bB = 0x42;
            const byte bD = 0x44;
            const byte bJ = 0x4A;
            const byte bI = 0x49;

            int len = bytes.Length;
            byte b1, b2, b3, b4;

            //Encode::is_utf8 は無視

            bool isBinary = false;
            for (int i = 0; i < len; i++)
            {
                b1 = bytes[i];
                if (b1 <= 0x06 || b1 == 0x7F || b1 == 0xFF)
                {
                    //'binary'
                    isBinary = true;
                    if (b1 == 0x00 && i < len - 1 && bytes[i + 1] <= 0x7F)
                    {
                        //smells like raw unicode
                        return System.Text.Encoding.Unicode;
                    }
                }
            }
            if (isBinary)
            {
                return null;
            }

            //not Japanese
            bool notJapanese = true;
            for (int i = 0; i < len; i++)
            {
                b1 = bytes[i];
                if (b1 == bEscape || 0x80 <= b1)
                {
                    notJapanese = false;
                    break;
                }
            }
            if (notJapanese)
            {
                return System.Text.Encoding.ASCII;
            }

            for (int i = 0; i < len - 2; i++)
            {
                b1 = bytes[i];
                b2 = bytes[i + 1];
                b3 = bytes[i + 2];

                if (b1 == bEscape)
                {
                    if (b2 == bDollar && b3 == bAt)
                    {
                        //JIS_0208 1978
                        //JIS
                        return System.Text.Encoding.GetEncoding(50220);
                    }
                    else if (b2 == bDollar && b3 == bB)
                    {
                        //JIS_0208 1983
                        //JIS
                        return System.Text.Encoding.GetEncoding(50220);
                    }
                    else if (b2 == bOpen && (b3 == bB || b3 == bJ))
                    {
                        //JIS_ASC
                        //JIS
                        return System.Text.Encoding.GetEncoding(50220);
                    }
                    else if (b2 == bOpen && b3 == bI)
                    {
                        //JIS_KANA
                        //JIS
                        return System.Text.Encoding.GetEncoding(50220);
                    }
                    if (i < len - 3)
                    {
                        b4 = bytes[i + 3];
                        if (b2 == bDollar && b3 == bOpen && b4 == bD)
                        {
                            //JIS_0212
                            //JIS
                            return System.Text.Encoding.GetEncoding(50220);
                        }
                        if (i < len - 5 &&
                            b2 == bAnd && b3 == bAt && b4 == bEscape &&
                            bytes[i + 4] == bDollar && bytes[i + 5] == bB)
                        {
                            //JIS_0208 1990
                            //JIS
                            return System.Text.Encoding.GetEncoding(50220);
                        }
                    }
                }
            }

            //should be euc|sjis|utf8
            //use of (?:) by Hiroki Ohzaki <ohzaki@iod.ricoh.co.jp>
            int sjis = 0;
            int euc = 0;
            int utf8 = 0;
            for (int i = 0; i < len - 1; i++)
            {
                b1 = bytes[i];
                b2 = bytes[i + 1];
                if (((0x81 <= b1 && b1 <= 0x9F) || (0xE0 <= b1 && b1 <= 0xFC)) &&
                    ((0x40 <= b2 && b2 <= 0x7E) || (0x80 <= b2 && b2 <= 0xFC)))
                {
                    //SJIS_C
                    sjis += 2;
                    i++;
                }
            }
            for (int i = 0; i < len - 1; i++)
            {
                b1 = bytes[i];
                b2 = bytes[i + 1];
                if (((0xA1 <= b1 && b1 <= 0xFE) && (0xA1 <= b2 && b2 <= 0xFE)) ||
                    (b1 == 0x8E && (0xA1 <= b2 && b2 <= 0xDF)))
                {
                    //EUC_C
                    //EUC_KANA
                    euc += 2;
                    i++;
                }
                else if (i < len - 2)
                {
                    b3 = bytes[i + 2];
                    if (b1 == 0x8F && (0xA1 <= b2 && b2 <= 0xFE) &&
                        (0xA1 <= b3 && b3 <= 0xFE))
                    {
                        //EUC_0212
                        euc += 3;
                        i += 2;
                    }
                }
            }
            for (int i = 0; i < len - 1; i++)
            {
                b1 = bytes[i];
                b2 = bytes[i + 1];
                if ((0xC0 <= b1 && b1 <= 0xDF) && (0x80 <= b2 && b2 <= 0xBF))
                {
                    //UTF8
                    utf8 += 2;
                    i++;
                }
                else if (i < len - 2)
                {
                    b3 = bytes[i + 2];
                    if ((0xE0 <= b1 && b1 <= 0xEF) && (0x80 <= b2 && b2 <= 0xBF) &&
                        (0x80 <= b3 && b3 <= 0xBF))
                    {
                        //UTF8
                        utf8 += 3;
                        i += 2;
                    }
                }
            }
            //M. Takahashi's suggestion
            //utf8 += utf8 / 2;

            System.Diagnostics.Debug.WriteLine(
                string.Format("sjis = {0}, euc = {1}, utf8 = {2}", sjis, euc, utf8));
            if (euc > sjis && euc > utf8)
            {
                //EUC
                return System.Text.Encoding.GetEncoding(51932);
            }
            else if (sjis > euc && sjis > utf8)
            {
                //SJIS
                return System.Text.Encoding.GetEncoding(932);
            }
            else if (utf8 > euc && utf8 > sjis)
            {
                //UTF8
                return System.Text.Encoding.UTF8;
            }

            return null;
        }
    }
}
