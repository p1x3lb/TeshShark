using System;
using System.Text;

namespace Common.Scopes
{
    public readonly struct StringBuilderScope : IDisposable
    {
        private readonly StringBuilder _stringBuilder;

        private StringBuilderScope(StringBuilder stringBuilder)
        {
            _stringBuilder = stringBuilder;
        }

        public static StringBuilderScope Create(out StringBuilder stringBuilder)
        {
            stringBuilder = PoolUtility<StringBuilder>.Pull();

            return new StringBuilderScope(stringBuilder);
        }

        void IDisposable.Dispose()
        {
            _stringBuilder.Clear();

            PoolUtility<StringBuilder>.Push(_stringBuilder);
        }
    }
}