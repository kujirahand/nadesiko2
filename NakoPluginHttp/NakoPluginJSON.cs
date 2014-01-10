using System;
using NakoPlugin;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Script.Serialization;
using fastJSON;

namespace NakoPluginFormat
{
	public class NakoPluginJSON : INakoPlugin
	{
        string _description = "JSONプラグイン";
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
            bank.AddFunc("JSONエンコード", "Vを|Vの", NakoVarType.String, _encode,"値VをJSON形式に変換する。", "JSONえんこーど");
            bank.AddFunc("JSONデコード", "Sを|Sの", NakoVarType.Array, _decode,"文字列JSONを変数に変換する。", "JSONでこーど");
        }
        
        // プラグインの初期化処理
        public void PluginInit(INakoInterpreter runner)
        {
        }
        // プラグインの終了処理
        public void PluginFin(INakoInterpreter runner)
        {
        }

        //Dictionary<string,object> objects = new Dictionary<string, object>();
        public object _encode(INakoFuncCallInfo info){
            object v = info.StackPop();
            if(v is NakoVarArray){
                NakoVarArray arr = (NakoVarArray)v;
                object objects = NakoVarArrayToArray(arr);
                return fastJSON.JSON.Instance.ToJSON(objects);
//                JavaScriptSerializer serializer = new JavaScriptSerializer();
//                string s = serializer.Serialize(objects);

            }
            return null;
        }

        public object _decode(INakoFuncCallInfo info){
            string s = info.StackPopAsString();
            var json = fastJSON.JSON.Instance.Parse(s);
            NakoVarArray a = ArrayToNakoVarArray(json);
            return a;
//            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
//            object json = serializer.Deserialize<object>(s);
        }
        private static NakoVarArray ArrayToNakoVarArray(object obj){
            NakoVarArray a = new NakoVarArray();
            if(obj is IDictionary<string,object>){
                Dictionary<string,object> dic = (Dictionary<string,object>)obj;
                foreach (KeyValuePair<string, object> item in dic) {
                    if((item.Value is IDictionary<string,object>) || (item.Value is object[]) || (item.Value is IList<object>)){
                        a.SetValueFromKey(item.Key,ArrayToNakoVarArray(item.Value));
                    }else{
                        a.SetValueFromKey(item.Key,item.Value);
                    }
                }
            }else if(obj is object[]){
                object[] li = (object[])obj;
                foreach(object item in li) {
                    if((item is IDictionary<string,object>) || (item is object[]) || (item is IList<object>)){
                        a.SetValue(a.Count,ArrayToNakoVarArray(item));
                    }else{
                        a.SetValue(a.Count,item);
                    }
                }
            }else if(obj is IList<object>){
                List<object> li = (List<object>)obj;
                foreach(object item in li) {
                    if((item is IDictionary<string,object>) || (item is object[]) || (item is IList<object>)){
                        a.SetValue(a.Count,ArrayToNakoVarArray(item));
                    }else{
                        a.SetValue(a.Count,item);
                    }
                }
            }
            return a;
        }

        protected static object NakoVarArrayToArray(NakoVarArray array){
            bool is_dictionary = false;
            if(array.Count>0){
                NakoVariable check = array.GetVar(0);
                if(check.key!=null){
                    is_dictionary = true;
                }
                if(is_dictionary){
                    var result = new Dictionary<string,object>();
                    foreach(NakoVariable variable in array){
                        if(variable.Body is NakoVarArray){
                            result.Add(variable.key,NakoVarArrayToArray((NakoVarArray)variable.Body));
                        }else{
                            result.Add(variable.key,variable.Body);
                        }
                    }
                    return result;
                }else{
                    var result = new List<object>();
                    foreach(NakoVariable variable in array){
                        if(variable.Body is NakoVarArray){
                            result.Add(NakoVarArrayToArray((NakoVarArray)variable.Body));
                        }else{
                            result.Add(variable.Body);
                        }
                    }
                    return result;
                }
            }
            return null;
        }
	}
}

