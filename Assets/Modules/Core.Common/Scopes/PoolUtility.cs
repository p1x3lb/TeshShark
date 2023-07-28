using System.Collections.Generic;

namespace Common.Scopes
{
    public static class PoolUtility<T>
        where T : class, new()
    {
        private static readonly Stack<T> _values = new Stack<T>();

#if ENABLE_PROFILING
        public static int FreeCount => _values.Count;

        public static int SummaryCount
        {
            get;
            private set;
        }
#endif

        public static void Push(T value)
        {
            //Assert.IsFalse(_values.Contains(value));

            _values.Push(value);
        }

        public static T Pull()
        {
            // ReSharper disable once ConvertIfStatementToReturnStatement
            if (_values.Count > 0)
            {
                return _values.Pop();
            }
#if ENABLE_PROFILING
            SummaryCount++;
#endif
            return new T();
        }
    }
}