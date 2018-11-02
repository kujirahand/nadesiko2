using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace NakoPlugin
{
    /// <summary>
    /// 文字コードの判定など、文字列処理に便利なクラス
    /// </summary>
    public class StrUnit
    {
        /// <summary>
        /// 文字コードを判別する
        /// this method use SimpleHelpers.FileEncoding in NuGet https://www.nuget.org/packages/SimpleHelpers.FileEncoding
        /// </summary>
        /// <remarks>
        /// license is shown at https://raw.githubusercontent.com/khalidsalomao/SimpleHelpers.Net/master/SimpleHelpers/LICENSE.txt
        /// </remarks>
        /// <param name="bytes">文字コードを調べるデータ</param>
        /// <returns>適当と思われるEncodingオブジェクト。
        /// 判断できなかった時はnull。</returns>
        public static System.Text.Encoding GetCode(byte[] bytes)
        {
            return SimpleHelpers.FileEncoding.DetectFileEncoding (bytes, 0, bytes.Length);
        }

        /// <summary>
        /// ファイルからテキストを読み込む(文字コードの自動判別付き)
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static string LoadFromFileAutoEnc(string filename)
        {
            return SimpleHelpers.FileEncoding.TryLoadFile(filename);
        }

        /// <summary>
        /// バイト列から文字列に変換(文字コードの自動判別付き)
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string ToStringAutoEnc(byte[] data)
        {
            string src;

            // 文字コード判別
            System.Text.Encoding enc = GetCode(data);
            string enc_str = enc.ToString();

            // UTF-8
            if (enc == Encoding.UTF8) {
                src = Encoding.UTF8.GetString (data);
            }
            // UNICODE
            else if (enc == Encoding.Unicode) {
                src = Encoding.Unicode.GetString (data);
            }
            // Shift_JIS
            else if (enc == System.Text.Encoding.GetEncoding (932)) {
                src = System.Text.Encoding.GetEncoding (932).GetString (data);
            }
            // JIS
            else if (enc == Encoding.GetEncoding (50220)) {
                src = System.Text.Encoding.GetEncoding (50220).GetString (data);
            }
            // EUC-JP
            else if (enc == Encoding.GetEncoding (51932)) {
                src = System.Text.Encoding.GetEncoding (51932).GetString (data);
            } else if (enc == Encoding.ASCII) {
                src = Encoding.ASCII.GetString (data);
            }
            else
            {
                src = enc.GetString (data);
            }

            return src;
        }
    }
}
