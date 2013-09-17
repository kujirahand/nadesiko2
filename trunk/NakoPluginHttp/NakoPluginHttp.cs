/*
 * Created by SharpDevelop.
 * User: shigepon
 * Date: 2011/04/26
 * Time: 10:42
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using NakoPlugin;
using System.Web;
using System.Net;

namespace NakoPluginHttp
{
    /// <summary>
    /// Description of MyClass.
    /// </summary>
    public class NakoPluginHttp : INakoPlugin
    {
        string _description = "Http処理を行うプラグイン";
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
            bank.AddFunc("URLデコード", "Sを", NakoVarType.String, _urlDecode, "SをURLデコードして返す", "URLでこーど");
            bank.AddFunc("URLエンコード", "Sを", NakoVarType.String, _urlEncode, "SをURLエンコードして返す", "URLえんこーど");
            bank.AddFunc("HTTPデータ取得", "URLから|URLの|URLを", NakoVarType.String, _dataGet, "URLからデータをダウンロードして内容を返す。", "HTTPでーたしゅとく");
            bank.AddFunc("HTTPダウンロード", "URLをFILEへ|URLからFILEに", NakoVarType.Void, _downloadGet, "URLをローカルFILEへダウンロードする。", "HTTPだうんろーど");
            bank.AddFunc("HTTPヘッダ取得", "URLから|URLの|URLを", NakoVarType.String, _getHeader, "URLからヘッダを取得して内容を返す。", "HTTPへっだしゅとく");
            bank.AddFunc("HTTPヘッダハッシュ取得", "URLから|URLの|URLを", NakoVarType.Array, _getHeaderHash, "URLからヘッダを取得してハッシュに変換して返す。", "HTTPへっだはっしゅしゅとく");
            bank.AddFunc("HTTPポスト", "URLへVALUESを|URLに", NakoVarType.String, _post, "ポストしたい値（ハッシュ形式）VALUESをURLへポストしその結果を返す。", "HTTPぽすと");
            bank.AddFunc("HTTPゲット", "HEADをURLへ|HEADで", NakoVarType.String, _get, "送信ヘッダHEADを指定してURLへGETコマンドを発行し、その結果を返す。", "HTTPげっと");
        }
        
        // プラグインの初期化処理
        public void PluginInit(INakoInterpreter runner)
        {
        }
        // プラグインの終了処理
        public void PluginFin(INakoInterpreter runner)
        {
        }
        
        public Object _urlDecode(INakoFuncCallInfo info)
        {
        	string s = info.StackPopAsString();
        	return Uri.UnescapeDataString(s);
        }
        public Object _urlEncode(INakoFuncCallInfo info)
        {
        	string s = info.StackPopAsString();
        	return Uri.EscapeDataString(s);
        }
        public Object _dataGet(INakoFuncCallInfo info)
		{
        	string s = info.StackPopAsString();
			WebClient client = new WebClient();
			string ret = client.DownloadString(s);
            client.Dispose();
            return ret;
		}

        public object _downloadGet(INakoFuncCallInfo info){
            string url = info.StackPopAsString();
            string file = info.StackPopAsString();
            WebClient client = new WebClient();
            client.DownloadFile(url,file);
            client.Dispose();
            return null;
        }
        public object _getHeader(INakoFuncCallInfo info){
            string url = info.StackPopAsString();
            try{
            WebRequest req = WebRequest.Create(url);
            req.Method = "HEAD";
            HttpWebResponse response = (HttpWebResponse)req.GetResponse();
//            StringBuilder ret = new StringBuilder();
//            foreach(string key in response.Headers.Keys){
//                ret.AppendFormat("{0}:{1}",key,String.Join(",",response.Headers.GetValues(key)));
//                ret.AppendLine();
//            }
//            return ret.ToString();
            return String.Format(@"HTTP/{0} {1} {2}\r\n{3}",response.ProtocolVersion,response.StatusCode.GetHashCode(),response.StatusDescription,response.Headers.ToString());
            //return response.Headers.ToString();
            }catch(WebException e){
                HttpWebResponse response = (HttpWebResponse)e.Response;
                return String.Format(@"HTTP/{0} {1} {2}\r\n{3}",response.ProtocolVersion,response.StatusCode.GetHashCode(),response.StatusDescription,response.Headers.ToString());
            }
        }
        public object _getHeaderHash(INakoFuncCallInfo info){
            string url = info.StackPopAsString();
            WebRequest req = WebRequest.Create(url);
            req.Method = "HEAD";
            HttpWebResponse response = (HttpWebResponse) req.GetResponse();
            NakoVarArray returnArray = new NakoVarArray();
            foreach(var key in response.Headers.Keys){
                string[] values = response.Headers.GetValues((string)key);
                NakoVarArray arrays = new NakoVarArray();
                arrays.SetValuesFromStrings(values);
                returnArray.SetValueFromKey((string)key,arrays);
            }
            returnArray.SetValueFromKey((string)"HTTP.Response",response.StatusCode.GetHashCode().ToString());
            returnArray.SetValueFromKey((string)"Status",response.StatusCode.GetHashCode().ToString());
            return returnArray;
        }
        public object _post(INakoFuncCallInfo info){
            string url = info.StackPopAsString();
            object val = info.StackPop();//TODO:Array
            string query = "";
            if(val is string){
               query = (string)val;
            }else if(val is NakoVariable){
                List<string> qp = new List<string>();
                NakoVarArray arr = (NakoVarArray)val;
                foreach(NakoVariable kv in arr){
                    qp.Add(String.Format("{0}={1}",HttpUtility.UrlEncode(kv.key),HttpUtility.UrlEncode((string)kv.Body)));
                }
                query = String.Join ("&",qp.ToArray());
            }else{
                throw new Exception("variable is not NakoVariable or string");
            }

            byte[] queryBytes = Encoding.ASCII.GetBytes(query);
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method="POST";
            req.ContentType = "application/x-www-form-urlencoded";
            using(Stream str = req.GetRequestStream())
            {
                str.Write(queryBytes,0,queryBytes.Length);
            }
            WebResponse response = req.GetResponse();
            string ret = "";
            using(StreamReader sr = new StreamReader(response.GetResponseStream())){
                ret = sr.ReadToEnd();
            }
            return ret;
        }
        public object _get(INakoFuncCallInfo info){
            WebClient client = new WebClient();
            string head = info.StackPopAsString();
            string url = info.StackPopAsString();
            using(StringReader rs = new StringReader(head))
            {
                string line;
                while((line = rs.ReadLine())!=null){
                    string[] heads = line.Split(new string[]{":"},2,StringSplitOptions.None);
                    client.Headers.Add(heads[0],heads[1]);
                }
            }
            return client.DownloadString(url);
        }
    }
}