using System;
using System.Collections.Generic;

namespace Common.Scopes
{
    public readonly struct SortedDictionaryScope<TKey, TValue> : IDisposable
    {
        private readonly SortedDictionary<TKey, TValue> _dictionary;

        private SortedDictionaryScope(SortedDictionary<TKey, TValue> dictionary)
        {
            _dictionary = dictionary;
        }

        public static SortedDictionaryScope<TKey, TValue> Create(out SortedDictionary<TKey, TValue> dictionary)
        {
            dictionary = PoolUtility<SortedDictionary<TKey, TValue>>.Pull();

            return new SortedDictionaryScope<TKey, TValue>(dictionary);
        }

        void IDisposable.Dispose()
        {
            _dictionary.Clear();

            PoolUtility<SortedDictionary<TKey, TValue>>.Push(_dictionary);
        }
    }
}