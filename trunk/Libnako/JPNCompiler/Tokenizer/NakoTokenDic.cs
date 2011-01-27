using System;
using System.Collections.Generic;
using System.Text;

namespace Libnako.JCompiler.Tokenizer
{
    /// <summary>
    /// [単語:トークンタイプ]を覚えておくための辞書
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

		public void Add(string key, NakoTokenType value)
		{
			_dictionary.Add(key, value);
		}

		public bool ContainsKey(string key)
		{
			return _dictionary.ContainsKey(key);
		}

		public ICollection<string> Keys
		{
			get { return _dictionary.Keys; }
		}

		public bool Remove(string key)
		{
			return _dictionary.Remove(key);
		}

		public bool TryGetValue(string key, out NakoTokenType value)
		{
			return _dictionary.TryGetValue(key, out value);
		}

		public ICollection<NakoTokenType> Values
		{
			get { return _dictionary.Values; }
		}

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

		public void Add(KeyValuePair<string, NakoTokenType> item)
		{
			((ICollection<KeyValuePair<string, NakoTokenType>>)_dictionary).Add(item);
		}

		public void Clear()
		{
			_dictionary.Clear();
		}

		public bool Contains(KeyValuePair<string, NakoTokenType> item)
		{
			return ((ICollection<KeyValuePair<string, NakoTokenType>>)_dictionary).Contains(item);
		}

		public void CopyTo(KeyValuePair<string, NakoTokenType>[] array, int arrayIndex)
		{
			((ICollection<KeyValuePair<string, NakoTokenType>>)_dictionary).CopyTo(array, arrayIndex);
		}

		public int Count
		{
			get { return _dictionary.Count; }
		}

		public bool IsReadOnly
		{
			get { return ((ICollection<KeyValuePair<string, NakoTokenType>>)_dictionary).IsReadOnly; }
		}

		public bool Remove(KeyValuePair<string, NakoTokenType> item)
		{
			return ((ICollection<KeyValuePair<string, NakoTokenType>>)_dictionary).Remove(item);
		}

		#endregion

		#region IEnumerable<KeyValuePair<string,NakoTokenType>> メンバー

		public IEnumerator<KeyValuePair<string, NakoTokenType>> GetEnumerator()
		{
			return _dictionary.GetEnumerator();
		}

		#endregion

		#region IEnumerable メンバー

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return _dictionary.GetEnumerator();
		}

		#endregion
	}
}
