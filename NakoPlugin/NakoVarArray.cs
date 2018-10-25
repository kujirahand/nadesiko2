using System;
using System.IO;
using System.Collections;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;

namespace NakoPlugin
{
    /// <summary>
    /// なでしこの配列型(配列とハッシュを扱える)
    /// </summary>
    [Serializable]
    public class NakoVarArray : NakoVariable, ICloneable, IEnumerable
    {
        /// <summary>
        /// 配列の要素を保持するためのリスト
        /// </summary>
        protected System.Collections.Generic.List<NakoVariable> list = new System.Collections.Generic.List<NakoVariable>();

        /// <summary>
        /// Get Enumerator
        /// </summary>
        /// <returns></returns>
        public IEnumerator GetEnumerator(){
            for(int i=0;i<this.Count;i++)
                yield return this.GetVar(i);
        }

        /// <summary>
        /// 配列変数を作成する(コンストラクタ)
        /// </summary>
        public NakoVarArray()
        {
            this._type = NakoVarType.Array;
        }

        public object Clone()
        {
            object clone = null;
            using (MemoryStream stream = new MemoryStream())
            {
                //対象オブジェクトをシリアライズ
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, this);
                stream.Position = 0;

                //シリアライズデータをデシリアライズ
                clone = formatter.Deserialize(stream);
            }
            return (NakoVarArray)clone;
        }

        /// <summary>
        /// 配列の要素数を得る
        /// </summary>
        public int Count
        {
            get
            {
                return list.Count;
            }
        }

        /// <summary>
        /// ハッシュのキーを得る
        /// </summary>
        /// <returns></returns>
        public string[] GetKeys()
        {
            //string[] r = new string[list.Count];
			System.Collections.Generic.List<string> r = new System.Collections.Generic.List<string>();
            for (int i = 0; i < list.Count; i++)
            {
                NakoVariable v = list[i];
				if (v != null) {
					//r [i] = v.key;
					r.Add((v.key==null)? i.ToString() : v.key);
				}
            }
			return r.ToArray();
        }

        /// <summary>
        /// 配列を初期化する
        /// </summary>
        public void Clear()
        {
            list = new System.Collections.Generic.List<NakoPlugin.NakoVariable>();
        }

        /// <summary>
        /// 配列要素を得る
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public NakoVariable GetVar(int index)
        {
            if (list.Count < 0) return null;
            if (list.Count <= index) return null;
            return list[index];
        }
        /// <summary>
        /// 配列要素を得る
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public object GetValue(int index)
        {
            NakoVariable v = GetVar(index);
			if (v == null) return null; 
            return v.Body;
        }
        /// <summary>
        /// ハッシュ名からハッシュ番号を得る
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public int GetIndexFromKey(string key)
        {
            // TODO: ここにキャッシュ検索を入れると早くなる!!
            for (int i = 0; i < list.Count; i++)
            {
                NakoVariable v = list[i];
                if (v == null) continue;
                if (key == v.key) return i;
            }
            return -1;
        }

        /// <summary>
        /// 値を検索してキー番号を返す
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public int FindValue(object v)
        {
            for (int i = 0; i < list.Count; i++)
            {
                NakoVariable item = list[i];
                if (v == null) continue;
                if (v.Equals(item.Body)) return i;
            }
            return -1;
        }
        /// <summary>
        /// 配列要素を得る
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public NakoVariable GetVarFromKey(string key)
        {
            int index = GetIndexFromKey(key);
            if (index < 0) return null;
            return list[index];
        }

        /// <summary>
        /// 配列要素を得る
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public NakoVariable GetVarFromObj(object key)
        {
            if (key is string)
            {
                return GetVarFromKey((string)key);
            }
            else if(key is NakoVariable){
                NakoVariable v = (NakoVariable)key;
                if(v.key!=null){
                    return GetVarFromObj(v.key);
                }else{
                    return GetVarFromObj(v.Body);
                }
            }
            else
            {
                int index = Convert.ToInt32(key);
                return GetVar(index);
            }
        }

        /// <summary>
        /// 配列要素を得る
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object GetValueFromObj(object key)
        {
            NakoVariable v = GetVarFromObj(key);
            if (v != null)
            {
                return v.Body;
            }
            return null;
        }

        /// <summary>
        /// 配列に値を代入する
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object GetValueFromKey(string key)
        {
            NakoVariable v = GetVarFromKey(key);
            if (v != null) return v.Body;
            return null;
        }

        /// <summary>
        /// 配列に値を代入する
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        public void SetVar(int index, NakoVariable value)
        {
            while (index >= list.Count) { list.Add(null); }
            list[index] = value;
        }

        /// <summary>
        /// 配列(インデックス/ハッシュ)に変数を代入する
        /// </summary>
        /// <param name="key">整数 or 文字列でキーを指定</param>
        /// <param name="value"></param>
        public void SetVarFromObj(object key, NakoVariable value)
        {
            if (key is string)
            {
                SetVarFromKey((string)key, value);
            }
            else
            {
                int index = Convert.ToInt32(key);
                SetVar(index, value);
            }
        }

        /// <summary>
        /// ハッシュに変数を代入する
        /// </summary>
        /// <param name="key">文字列でキーを指定</param>
        /// <param name="var"></param>
        public void SetVarFromKey(string key, NakoVariable var)
        {
            int i = GetIndexFromKey(key);
            if (i >= 0)
            {
                list.RemoveAt(i);
            }
            var.key = key;
            list.Add(var);
        }

        /// <summary>
        /// 配列に変数を代入する
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        public void SetValue(int index, object value)
        {
            NakoVariable v = new NakoVariable();
            v.SetBodyAutoType(value);
            this.SetVar(index, v);
        }

        /// <summary>
        /// 配列変数(ハッシュ)の要素を設定する
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetValueFromKey(string key, object value)
        {
            NakoVariable v = new NakoVariable();
            v.key = key;
            v.SetBodyAutoType(value);
            this.SetVarFromKey(key, v);
        }

        /// <summary>
        /// 文字列を改行で区切って配列変数に変換する
        /// </summary>
        /// <param name="str"></param>
        public void SetValuesFromString(string str)
        {
            Clear();
            string[] splitter = new string[] { "\r\n" };
            string[] a = str.Split(splitter, StringSplitOptions.None);
            int i = 0;
            foreach (string n in a)
            {
                SetValue(i++, n);
            }
        }
        /// <summary>
        /// string[]を配列変数に変換して代入する
        /// </summary>
        /// <param name="strings"></param>
        public void SetValuesFromStrings(string[] strings)
        {
            Clear();
            int i = 0;
            foreach (string n in strings)
            {
                SetValue(i++, n);
            }
        }

        /// <summary>
        /// 要素を削除
        /// </summary>
        /// <param name="index"></param>
        public void RemoveAt(int index)
        {
            list.RemoveAt(index);
        }

        /// <summary>
        /// 配列要素を追加する
        /// </summary>
        /// <param name="item"></param>
        public void Add(NakoVariable item)
        {
            list.Add(item);
        }

        /// <summary>
        /// 末尾の要素を切り取って返す
        /// </summary>
        /// <returns></returns>
        public NakoVariable Pop()
        {
            if (list.Count == 0) return null;
            int last = list.Count - 1;
            NakoVariable last_o = list[last];
            list.RemoveAt(last);
            return last_o;
        }

        /// <summary>
        /// 配列に要素を挿入する
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        public void Insert(int index, NakoVariable item)
        {
            list.Insert(index, item);
        }

        /// <summary>
        /// 配列の要素を反転する
        /// </summary>
        public void Reverse()
        {
            list.Reverse();
        }

        /// <summary>
        /// 配列の値を文字列に変換して出力する
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string r = "";
            NakoVariable v;
            for (int i = 0; i < list.Count; i++)
            {
                if (i >= 1) r += "\r\n";
                v = list[i];
                if (v != null)
                {
                    r += v.ToString();
                }
            }
            r += "";
            return r;
        }
    }
}
