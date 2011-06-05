using System;
using System.Collections.Generic;
using System.Text;

namespace Libnako.JPNCompiler.Tokenizer
{
    /// <summary>
    /// [単語:トークンタイプ]を覚えておくための辞書
    /// NakoReservedWord.cs で実際に単語を定義している
    /// </summary>
    public class NakoTokenDic : IDictionary<string, NakoTokenType>
    {
        private Dictionary<string, NakoTokenType> _dictionary = new Dictionary<string, NakoTokenType>();

        /// <summary>
        /// 単語を辞書に追加する
        /// </summary>
        /// <param name="key">単語</param>
        /// <param name="type">単語の種類</param>
        public void AddWord(string key, NakoTokenType type)
        {
            key = NakoToken.TrimOkurigana(key);
            this._dictionary.Add(key, type);
        }

        #region IDictionary<string,NakoTokenType> メンバー

        /// <summary>
        /// 追加
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
		public void Add(string key, NakoTokenType value)
		{
			_dictionary.Add(key, value);
		}
        /// <summary>
        /// 含むか
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
		public bool ContainsKey(string key)
		{
			return _dictionary.ContainsKey(key);
		}
        /// <summary>
        /// キー一覧を取得
        /// </summary>
		public ICollection<string> Keys
		{
			get { return _dictionary.Keys; }
		}
        /// <summary>
        /// 削除
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
		public bool Remove(string key)
		{
			return _dictionary.Remove(key);
		}
        /// <summary>
        /// 値を得る
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
		public bool TryGetValue(string key, out NakoTokenType value)
		{
			return _dictionary.TryGetValue(key, out value);
		}
        
        /// <summary>
        /// 値一覧を得る
        /// </summary>
		public ICollection<NakoTokenType> Values
		{
			get { return _dictionary.Values; }
		}

        /// <summary>
        /// 値を得る
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
		public NakoTokenType this[string key]
		{
			get
			{
				return _dictionary[key];
			}
			set
			{
				_dictionary[key] = value;
			}
		}

		#endregion

		#region ICollection<KeyValuePair<string,NakoTokenType>> メンバー

        /// <summary>追加</summary>
        /// <param name="item"></param>
		public void Add(KeyValuePair<string, NakoTokenType> item)
		{
			((ICollection<KeyValuePair<string, NakoTokenType>>)_dictionary).Add(item);
		}
        /// <summary>
        /// 削除
        /// </summary>
		public void Clear()
		{
			_dictionary.Clear();
		}
        /// <summary>
        /// 含むか
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
		public bool Contains(KeyValuePair<string, NakoTokenType> item)
		{
			return ((ICollection<KeyValuePair<string, NakoTokenType>>)_dictionary).Contains(item);
		}
        /// <summary>
        /// コピー
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
		public void CopyTo(KeyValuePair<string, NakoTokenType>[] array, int arrayIndex)
		{
			((ICollection<KeyValuePair<string, NakoTokenType>>)_dictionary).CopyTo(array, arrayIndex);
		}
        /// <summary>
        /// 個数
        /// </summary>
		public int Count
		{
			get { return _dictionary.Count; }
		}
        /// <summary>
        /// 読み取り専用か
        /// </summary>
		public bool IsReadOnly
		{
			get { return ((ICollection<KeyValuePair<string, NakoTokenType>>)_dictionary).IsReadOnly; }
		}
        /// <summary>
        /// 削除
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
		public bool Remove(KeyValuePair<string, NakoTokenType> item)
		{
			return ((ICollection<KeyValuePair<string, NakoTokenType>>)_dictionary).Remove(item);
		}

		#endregion

		#region IEnumerable<KeyValuePair<string,NakoTokenType>> メンバー

        /// <summary>
        /// Enumeratorを得る
        /// </summary>
        /// <returns></returns>
		public IEnumerator<KeyValuePair<string, NakoTokenType>> GetEnumerator()
		{
			return _dictionary.GetEnumerator();
		}

		#endregion

		#region IEnumerable メンバー

        /// <summary>
        /// Enumeratorを得る
        /// </summary>
        /// <returns></returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return _dictionary.GetEnumerator();
		}

		#endregion
	}
}
