using System;
using System.Collections.Generic;

namespace Common.Scopes
{
    public readonly struct QueueScope<T> : IDisposable
    {
        private readonly Queue<T> _queue;

        private QueueScope(Queue<T> queue)
        {
            _queue = queue;
        }

        public static QueueScope<T> Create(out Queue<T> queue)
        {
            queue = PoolUtility<Queue<T>>.Pull();

            return new QueueScope<T>(queue);
        }

        void IDisposable.Dispose()
        {
            _queue.Clear();

            PoolUtility<Queue<T>>.Push(_queue);
        }
    }
}