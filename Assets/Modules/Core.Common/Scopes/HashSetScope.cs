﻿using System;
using System.Collections.Generic;

namespace Common.Scopes
{
    public readonly struct HashSetScope<T> : IDisposable
    {
        private readonly HashSet<T> _hashSet;

        private HashSetScope(HashSet<T> hashSet)
        {
            _hashSet = hashSet;
        }

        public static HashSetScope<T> Create(out HashSet<T> hashSet)
        {
            hashSet = PoolUtility<HashSet<T>>.Pull();

            return new HashSetScope<T>(hashSet);
        }

        void IDisposable.Dispose()
        {
            _hashSet.Clear();

            PoolUtility<HashSet<T>>.Push(_hashSet);
        }
    }
}