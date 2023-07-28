using System.Collections.Generic;

namespace Utils.Project.Scripts.Modules.Utils.Extensions
{
    public class EnumComparator<T> : IEqualityComparer<T>
    {
        public bool Equals(T x, T y)
        {
            if (x == null || y == null)
            {
                return false;
            }
            
            if (x.GetType() != y.GetType())
            {
                return false;
            }

            return x.Equals(y);
        }

        public int GetHashCode(T obj)
        {
            return obj.GetHashCode();
        }
    }
}