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
        
        public Object _open(INakoFuncCallInfo info)
        {
        	String s = info.StackPopAsString();
        	ADODB.Connection con = new ADODB.Connection();
        	con.Open(s,null,null,0);
        	
        	return con;
        }
        public Object _close(INakoFuncCallInfo info)
        {
            Object c = info.StackPop();
            if(!(c is ADODB.Connection)){
                throw new NakoPluginArgmentException("connection not found");
            }
            ADODB.Connection con = (ADODB.Connection)c;
            con.Close();
            return null;
        }
        ADODB.Recordset _rs = new ADODB.Recordset();
        public Object _execute(INakoFuncCallInfo info)
        {
            Object c = info.StackPop();
            if(!(c is ADODB.Connection)){
                throw new NakoPluginArgmentException("connection not found");
            }
            String q = info.StackPopAsString();
            ADODB.Connection con = (ADODB.Connection)c;
            object affectedrows;
            _rs = con.Execute(q,out affectedrows,(int)ADODB.CommandTypeEnum.adCmdText);
            return affectedrows;
        }
        public Object _getField(INakoFuncCallInfo info)
        {
            Object c = info.StackPop();
            String s = info.StackPopAsString();
            return _rs.Fields[s].Value;
            
        }
        public Object _next(INakoFuncCallInfo info)
        {
            _rs.MoveNext();
            return null;
            
        }
        public Object _isEnd(INakoFuncCallInfo info)
        {
            return _rs.EOF;
        }
        public Object _isExist(INakoFuncCallInfo info)
        {
            return !_rs.EOF;
        }

        
    }
}