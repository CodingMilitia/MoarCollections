using System;
using System.Collections;
using System.Collections.Generic;

namespace CodingMilitia.MoarCollections.Dictionaries
{
    /// <summary>
    /// <para>
    /// Acts as a wrapper over a <see cref="Dictionary{TKey, TValue}"/> with the ability of returning a default value (static or using a <see cref="Func{TKey, TValue}"/>) 
    /// when the requested key is not present.
    /// </para>
    /// <para>
    /// Most of the times, using <see cref="IDictionary{TKey, TValue}.TryGetValue(TKey, out TValue)"/> on a regular Dictionary will be good enough,
    /// this should only be useful when you just want to index and forget.
    /// </para>
    /// <para>The <see cref="Func{TKey, TValue}"/> can be also useful in cases we want to throw a specific exception when the key is not found.</para>
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class DictionaryWithDefaultValue<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private readonly Dictionary<TKey, TValue> _backingDictionary;
        private readonly TValue _defaultValue;
        private readonly Func<TKey, TValue> _defaultValueFunc;

        public DictionaryWithDefaultValue(TValue defaultValue)
        {
            _backingDictionary = new Dictionary<TKey, TValue>();
            _defaultValue = defaultValue;
            _defaultValueFunc = null;
        }

        public DictionaryWithDefaultValue(IDictionary<TKey, TValue> sourceDictionary, TValue defaultValue)
        {
            _backingDictionary = sourceDictionary != null ? new Dictionary<TKey, TValue>(sourceDictionary) : throw new ArgumentNullException(nameof(sourceDictionary));
            _defaultValue = defaultValue;
            _defaultValueFunc = null;
        }

        public DictionaryWithDefaultValue(Func<TKey, TValue> defaultValueFunc)
        {
            _backingDictionary = new Dictionary<TKey, TValue>();
            _defaultValue = default(TValue);
            _defaultValueFunc = defaultValueFunc ?? throw new ArgumentNullException(nameof(defaultValueFunc));
        }

        public DictionaryWithDefaultValue(IDictionary<TKey, TValue> sourceDictionary, Func<TKey, TValue> defaultValueFunc)
        {
            _backingDictionary = sourceDictionary != null ? new Dictionary<TKey, TValue>(sourceDictionary) : throw new ArgumentNullException(nameof(sourceDictionary));
            _defaultValue = default(TValue);
            _defaultValueFunc = defaultValueFunc ?? throw new ArgumentNullException(nameof(defaultValueFunc));
        }

        public TValue this[TKey key]
        {
            get => _backingDictionary.TryGetValue(key, out var value) ? value : _defaultValueFunc != null ? _defaultValueFunc(key) : _defaultValue;
            set => _backingDictionary[key] = value;
        }

        public ICollection<TKey> Keys => _backingDictionary.Keys;

        public ICollection<TValue> Values => _backingDictionary.Values;

        public int Count => _backingDictionary.Count;

        public bool IsReadOnly => ((IDictionary<TKey, TValue>)_backingDictionary).IsReadOnly;

        public void Add(TKey key, TValue value) => _backingDictionary.Add(key, value);

        public void Add(KeyValuePair<TKey, TValue> item) => ((IDictionary<TKey, TValue>)_backingDictionary).Add(item);

        public void Clear() => _backingDictionary.Clear();

        public bool Contains(KeyValuePair<TKey, TValue> item) => ((IDictionary<TKey, TValue>)_backingDictionary).Contains(item);

        public bool ContainsKey(TKey key) => _backingDictionary.ContainsKey(key);

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) => ((IDictionary<TKey, TValue>)_backingDictionary).CopyTo(array, arrayIndex);

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => _backingDictionary.GetEnumerator();

        public bool Remove(TKey key) => _backingDictionary.Remove(key);

        public bool Remove(KeyValuePair<TKey, TValue> item) => ((IDictionary<TKey, TValue>)_backingDictionary).Remove(item);

        public bool TryGetValue(TKey key, out TValue value) => _backingDictionary.TryGetValue(key, out value);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}