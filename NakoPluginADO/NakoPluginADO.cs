/*
 * Created by SharpDevelop.
 * User: shigepon
 * Date: 2011/04/26
 * Time: 10:42
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Data;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using NakoPlugin;

namespace NakoPluginADO
{
    /// <summary>
    /// Description of MyClass.
    /// </summary>
    public class NakoPluginADO : INakoPlugin
    {
        //TODO:グループみたいに使いたい。ネイティブドライバの時でも同じメソッドを使える感じで
		//TODO:今のままだと2回SELECTすると先に登録した結果セットが上書きされる。そこらへんを修正したい。
        string _description = "ODBCドライバによるＤＢ処理を行うプラグイン";
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
            //TODO:説明を変更する
            bank.AddFunc("ADO開く", "Sで", NakoVarType.Object, _open, "URLエンコードされた文字列をURLデコードして返す", "えーでぃーおーひらく");
            bank.AddFunc("DB閉じる", "HANDLEを", NakoVarType.Void, _close, "URLエンコードされた文字列をURLデコードして返す", "えーでぃーおーとじる");
            bank.AddFunc("SQL実行", "HANDLEにSを｜HANDLEへSの｜Sで", NakoVarType.Int, _execute, "URLエンコードされた文字列をURLデコードして返す", "えすきゅーえるじっこう");
            bank.AddFunc("DBフィールド取得", "HANDLEに｜HANDLEへSの", NakoVarType.String, _getField, "URLエンコードされた文字列をURLデコードして返す", "えーでぃーおーとじる");
            bank.AddFunc("DB次移動", "HANDLEの｜HANDLEに｜HANDLEへ", NakoVarType.Void, _next, "URLエンコードされた文字列をURLデコードして返す", "えーでぃーおーとじる");
            bank.AddFunc("DB最後判定", "HANDLEの｜HANDLEに｜HANDLEへ", NakoVarType.Int, _isEnd, "URLエンコードされた文字列をURLデコードして返す", "えーでぃーおーとじる");
            bank.AddFunc("DBデータ有り", "HANDLEの｜HANDLEに｜HANDLEへ", NakoVarType.Int, _isExist, "URLエンコードされた文字列をURLデコードして返す", "えーでぃーおーとじる");
        }
        
        // プラグインの初期化処理
        public void PluginInit(INakoInterpreter runner)
        {
        }
        // プラグインの終了処理
        public void PluginFin(INakoInterpreter runner)
        {
        }
        
        public object _open(INakoFuncCallInfo info)
        {
        	string s = info.StackPopAsString();
			SqlConnection connection = new SqlConnection(s);
			connection.Open();
			return connection;
        	//ADODB.Connection con = new ADODB.Connection();
        	//con.Open(s,null,null,0);
        	//return con;
        }
        public object _close(INakoFuncCallInfo info)
        {
            object c = info.StackPop();
            if(!(c is SqlConnection)){
                throw new NakoPluginArgmentException("connection not found");
            }
            SqlConnection con = (SqlConnection)c;
            con.Close();
            return null;
/*            if(!(c is ADODB.Connection)){
                throw new NakoPluginArgmentException("connection not found");
            }
            ADODB.Connection con = (ADODB.Connection)c;
            con.Close();
            return null;*/
        }
		SqlDataReader _rs = null;//TODO:Listにして、コネクションに応じて返却するデータを変えればいけるか？
        /*ADODB.Recordset _rs = new ADODB.Recordset();*/
        public object _execute(INakoFuncCallInfo info)
        {
            object c = info.StackPop();
            if(!(c is SqlConnection)){
                throw new NakoPluginArgmentException("connection not found");
            }
            string q = info.StackPopAsString();
            SqlConnection con = (SqlConnection)c;
			SqlCommand cmd = new SqlCommand(q,con);
            _rs = cmd.ExecuteReader();
			cmd.Dispose();
            return _rs.RecordsAffected;
            /*object c = info.StackPop();
            if(!(c is ADODB.Connection)){
                throw new NakoPluginArgmentException("connection not found");
            }
            string q = info.StackPopAsString();
            ADODB.Connection con = (ADODB.Connection)c;
            object affectedrows;
            _rs = con.Execute(q,out affectedrows,(int)ADODB.CommandTypeEnum.adCmdText);
            return affectedrows;*/
        }
        public object _getField(INakoFuncCallInfo info)
        {
            object c = info.StackPop();
            string s = info.StackPopAsString();
            return _rs[s].ToString();
            
/*            object c = info.StackPop();
            string s = info.StackPopAsString();
            return _rs.Fields[s].Value;*/
        }
        public object _next(INakoFuncCallInfo info)//TODO:booleanにする？
        {
            is_end = _rs.Read();
			if(is_end) _rs.Close();
            return null;
            
        }
		protected bool is_end = false;
        public object _isEnd(INakoFuncCallInfo info)
        {
            //return _rs.EOF;
			return is_end;
        }
        public object _isExist(INakoFuncCallInfo info)
        {
			return !is_end;
            //return !_rs.EOF;
        }

        
    }
}