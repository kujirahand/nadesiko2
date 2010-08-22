using System;
using System.Collections.Generic;
using System.Text;

namespace Libnako.Parser
{
    public class NakoHalfFlag : Dictionary<Char, Char>
    {
        // Singleton class
        private static NakoHalfFlag instance = null;
        public static NakoHalfFlag GetInstance()
        {
            if (NakoHalfFlag.instance == null)
            {
                NakoHalfFlag.instance = new NakoHalfFlag();
            }
            return NakoHalfFlag.instance;
        }
        public static Char ConvertChar(Char c)
        {
            // 半角なら変換不要
            if (c <= 0xFF)
            {
                return c;
            }
            // 数字?
            if ('０' <= c && c <= '９') {
                return (Char)(c - '０');
            }
            // アルファベット?
            if ('Ａ' <= c && c <= 'Ｚ') {
                return (Char)('A' + c - 'Ａ');
            }
            if ('ａ' <= c && c <= 'ｚ')
            {
                return (Char)('a' + c - 'ａ');
            }

            // 変換の可能性
            NakoHalfFlag f = GetInstance();
            if (f.ContainsKey(c)) {
                return f[c];
            }
            return c;
        }

        private NakoHalfFlag()
        {
            Init();
        }

        public void Init()
        {

            this.Add('●','*');
            this.Add('＊','*');
            this.Add('！','!');
            this.Add('＃','#');
            this.Add('／','/');
            this.Add('％','%');
            this.Add('＆','&');
            this.Add('’','\'');
            this.Add('（','(');
            this.Add('）',')');
            this.Add('＝','=');
            this.Add('－','-');
            this.Add('～','~');
            this.Add('＾','^');
            this.Add('｜','|');
            this.Add('￥','\\');
            this.Add('｛','{');
            this.Add('｝','}');
            this.Add('【','[');
            this.Add('】',']');
            this.Add('［','[');
            this.Add('］',']');
            this.Add('：',':');
            this.Add('；',';');
            this.Add('＋','+');
            this.Add('＜','<');
            this.Add('＞','>');
            this.Add('？','?');
            this.Add('。',';');
            this.Add('、',',');
            this.Add('，',',');
            this.Add('．', '.');
            this.Add('＿', '_');
            this.Add('※', '#');
        }
    }
}
