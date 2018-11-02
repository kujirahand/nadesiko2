/*
 * Created by SharpDevelop.
 * User: shigepon
 * Date: 2011/03/31
 * Time: 9:33
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using NakoPlugin;
using System.IO;

using HtmlAgilityPack;

namespace NakoPluginTag
{
    /// <summary>
    /// Description of NakoPluginTag.
    /// </summary>
    public class NakoPluginTag : INakoPlugin
    {
        string _description = "XML,HTMLタグ処理を行うプラグイン";
        Version _version = new Version(1, 0);
        //--- プラグイン共通の部分 ---
        public Version TargetNakoVersion { get { return new Version(2, 0); } }
        public bool Used { get; set; }
        public string Name { get { return this.GetType().FullName; } }
        public Version PluginVersion { get { return _version; } }
        public string Description { get { return _description; } }
        //--- 関数の定義 ---
        public void DefineFunction(INakoPluginBank bank)
        {
            //+日付時間処理
            //-日付時間
            bank.AddFunc("タグで区切る", "SをA", NakoVarType.Array, _split, "SからAのタグを切り取る。タグは残す", "たぐでくぎる");
            bank.AddFunc("タグ切り出し", "SからAの|SでAを", NakoVarType.Array, _extract, "SからAのタグを切り取る。タグは残さない", "たぐきりだし");
            bank.AddFunc("階層タグ切り出し", "SからAの|SでAを", NakoVarType.Array, _hirextract, "Sから特定階層下のタグAを切り取る。たとえば『head/title』『item/link』など。", "かいそうたぐきりだし");
            bank.AddFunc("タグ属性取得", "SのAからBを|Sで", NakoVarType.Array, _attr, "SからタグAの属性Bを取り出す。", "たぐぞくせいしゅとく");
            bank.AddFunc("タグで削除", "Sから|Sの", NakoVarType.String, _remove, "Sのタグを削除。", "たぐさくじょ");
            bank.AddFunc("タグ属性一覧取得", "SからAの|Sで", NakoVarType.Array, _attrlist, "SからタグAにある属性をハッシュ形式で取得する。", "たぐぞくせいいちらんしゅとく");
            bank.AddFunc("HTMLリンク抽出", "Sから|Sの", NakoVarType.Array, _link, "SからHTMLのリンク（A,IMGタグ）を抽出して返す。", "えいちてぃーえむえるりんくちゅうしゅつ");
        }
        
        // プラグインの初期化処理
        public void PluginInit(INakoInterpreter runner)
        {
        }
        // プラグインの終了処理
        public void PluginFin(INakoInterpreter runner)
        {
        }
        public Object __extract(INakoFuncCallInfo info, string s, string a, bool inner)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.OptionFixNestedTags = true;
            doc.OptionOutputAsXml = true;
            doc.LoadHtml(s);
            string html = doc.DocumentNode.OuterHtml;
            HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes(String.Format(@"//{0}",a));
            NakoVarArray res = info.CreateArray();
            if(nodes!=null){
                for(int i=0;i<nodes.Count;i++){
                    HtmlNode node = nodes[i];
                    if(html.Contains(node.OuterHtml)){
                        if (inner) {
                            res.SetValue(i,node.InnerHtml);
                        }else{
                            res.SetValue(i,node.OuterHtml);
                        }
                        html = html.Remove(html.IndexOf(node.OuterHtml), node.OuterHtml.Length);
                    }
                }
            }
            return res;
        }
        public Object _split(INakoFuncCallInfo info)
        {
            string s = info.StackPopAsString();
            string a = info.StackPopAsString();
            return __extract (info, s, a, false);
        }
        public Object _extract(INakoFuncCallInfo info)
        {
        	string s = info.StackPopAsString();
        	string a = info.StackPopAsString();
            return __extract (info, s, a, true);
        }
         public Object _hirextract(INakoFuncCallInfo info)
        {
        	string s = info.StackPopAsString();
        	string a = info.StackPopAsString();
        	HtmlDocument doc = new HtmlDocument();
        	doc.LoadHtml(s);
        	HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes(String.Format(@"//{0}",a));
        	NakoVarArray res = info.CreateArray();
        	if(nodes!=null){
            	for(int i=0;i<nodes.Count;i++){
            	    HtmlNode node = nodes[i];
            	    res.SetValue(i,node.InnerHtml);
            	}
        	}
            return res;
        }
       public Object _attr(INakoFuncCallInfo info)
        {
        	string s = info.StackPopAsString();
        	string a = info.StackPopAsString();
        	string b = info.StackPopAsString();
        	HtmlDocument doc = new HtmlDocument();
        	doc.LoadHtml(s);
        	HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes(String.Format(@"//{0}",a));
        	NakoVarArray res = info.CreateArray();
        	if(nodes!=null){
            	for(int i=0;i<nodes.Count;i++){
            	    HtmlNode node = nodes[i];
            	    res.SetValue(i,node.GetAttributeValue(b,""));
            	}
        	}
            return res;
        }
       public Object _attrlist(INakoFuncCallInfo info)
        {
        	string s = info.StackPopAsString();
        	string a = info.StackPopAsString();
        	HtmlDocument doc = new HtmlDocument();
        	doc.LoadHtml(s);
        	HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes(String.Format(@"//{0}",a));
        	NakoVarArray res = info.CreateArray();
        	if(nodes!=null){
            	foreach(HtmlNode node in nodes){
        	        foreach(HtmlAttribute attr in node.Attributes){
            	       res.SetValueFromKey(attr.Name,attr.Value);
        	        }
            	}
        	}
            return res;
        }
        public Object _link(INakoFuncCallInfo info)
        {
        	string s = info.StackPopAsString();
        	HtmlDocument doc = new HtmlDocument();
        	doc.LoadHtml(s);
        	HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes(@"//a");
        	NakoVarArray res = info.CreateArray();
        	if(nodes!=null){
            	for(int i=0;i<nodes.Count;i++){
            	    HtmlNode node = nodes[i];
            	    res.SetValue(i,node.GetAttributeValue("href",""));
            	}
        	}
            return res;
        }
        public Object _remove(INakoFuncCallInfo info)
        {
        	string s = info.StackPopAsString();
        	HtmlDocument doc = new HtmlDocument();
         	doc.LoadHtml(s);
            StringWriter sw = new StringWriter();
            ConvertTo(doc.DocumentNode, sw);
            sw.Flush();
            return sw.ToString();
        }
        
        private void ConvertTo(HtmlNode node, TextWriter outText)
        {
            string html;
            switch (node.NodeType)
            {
                case HtmlNodeType.Comment:
                    // don't output comments
                    break;

                case HtmlNodeType.Document:
                    ConvertContentTo(node, outText);
                    break;

                case HtmlNodeType.Text:
                    // script and style must not be output
                    string parentName = node.ParentNode.Name;
                    if ((parentName == "script") || (parentName == "style"))
                        break;

                    // get text
                    html = ((HtmlTextNode) node).Text;

                    // is it in fact a special closing node output as text?
                    if (HtmlNode.IsOverlappedClosingElement(html))
                        break;

                    // check the text is meaningful and not a bunch of whitespaces
                    if (html.Trim().Length > 0)
                    {
                        outText.Write(HtmlEntity.DeEntitize(html));
                    }
                    break;

                case HtmlNodeType.Element:
                    switch (node.Name)
                    {
                        case "p":
                            // treat paragraphs as crlf
                            outText.Write("\r\n");
                            break;
                    }

                    if (node.HasChildNodes)
                    {
                        ConvertContentTo(node, outText);
                    }
                    break;
            }
        }
        
         private void ConvertContentTo(HtmlNode node, TextWriter outText)
        {
            foreach (HtmlNode subnode in node.ChildNodes)
            {
                ConvertTo(subnode, outText);
            }
        }
       
    }
}
