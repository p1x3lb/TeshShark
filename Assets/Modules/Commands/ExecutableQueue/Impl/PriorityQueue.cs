using System;
using System.Collections.Generic;
using System.Linq;
using ModestTree;

namespace Commands.Project.Scripts.Modules.Commands.ExecutableQueue.Impl
{
    public class PriorityQueue<TPriority, TValue> : IPriorityQueue<TPriority, TValue>, IDisposable where TValue : IDisposable
    {
        public bool IsEmpty => _queues.Count == 0;
        public int Count => _queues.Sum(queue => queue.Value.Count);
        
        private readonly Dictionary<TPriority, Queue<TValue>> _queues;
        private readonly IComparer<TPriority> _comparer;
        
        public PriorityQueue(IComparer<TPriority> comparer, IEqualityComparer<TPriority> equalityComparer)
        {
            _comparer = comparer;
            _queues = new Dictionary<TPriority, Queue<TValue>>(equalityComparer);
        }
        
        public PriorityQueue(IComparer<TPriority> comparer)
        {
            _comparer = comparer;
            _queues = new Dictionary<TPriority, Queue<TValue>>();
        }

        public void Enqueue(TPriority priority, TValue value)
        {
            if (!_queues.TryGetValue(priority, out Queue<TValue> queue))
            {
                queue = new Queue<TValue>();
                _queues.Add(priority, queue);
            }
            queue.Enqueue(value);
        }

        public TValue Dequeue()
        {
           
            var element = SelectQueue();

            var result = element.Value.Dequeue();
            if (element.Value.IsEmpty())
            {
                _queues.Remove(element.Key);
            }
            
            return result;
        }
        
        public TValue Peek()
        {
            return SelectQueue().Value.Peek();
        }
        
        private KeyValuePair<TPriority, Queue<TValue>> SelectQueue()
        {
            if (_queues.Count < 0)
            {
                throw new NullReferenceException();
            }

            var element = _queues.First();
            foreach (var queue in _queues)
            {
                if (_comparer.Compare(element.Key, queue.Key) > 0)
                {
                    element = queue;
                }
            }

            return element;
        }
        
        public void Clear()
        {
            foreach (var queue in _queues)
            {
                foreach (var disposable in queue.Value)
                {
                    disposable.Dispose();
                }

                queue.Value.Clear();
            }

            _queues.Clear();
        }
        
        public void Dispose()
        {
            Clear();
        }
    }
}