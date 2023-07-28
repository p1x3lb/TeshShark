using System;
using System.Collections.Generic;

namespace Common.Scopes
{
    public readonly struct StackScope<T> : IDisposable
    {
        private readonly Stack<T> _stack;

        private StackScope(Stack<T> stack)
        {
            _stack = stack;
        }

        public static StackScope<T> Create(out Stack<T> stack)
        {
            stack = PoolUtility<Stack<T>>.Pull();

            return new StackScope<T>(stack);
        }

        void IDisposable.Dispose()
        {
            _stack.Clear();

            PoolUtility<Stack<T>>.Push(_stack);
        }
    }
}