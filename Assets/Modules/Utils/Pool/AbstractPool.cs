using System.Collections.Generic;

namespace Utils.Project.Scripts.Modules.Utils.Pool
{
    public abstract class AbstractPool<T> : IPool<T> where T : IPoolItem
    {
        private readonly Queue<T> _pool = new Queue<T>();

        public void Prepare(int count)
        {
            for (int i = 0; i < count; i++)
            {
                Prepare();
            }
        }

        public virtual T Get()
        {
            var item = GetObject();
            item.Retain();
            return item;
        }

        private T GetObject()
        {
            foreach (var item in _pool)
            {
                if (item.IsFree)
                {
                    return item;
                }
            }

            return Prepare();
        }

        protected virtual T Prepare()
        {
            var item = CreateItem();
            item.Load();
            _pool.Enqueue(item);
            return item;
        }

        protected abstract T CreateItem();
    }
}