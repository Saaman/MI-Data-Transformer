using System.Collections.Generic;

namespace MIProgram.Core.Extensions
{
    public class UpperInvariantComparer : IEqualityComparer<string>
    {
        public bool Equals(string x, string y)
        {
            if (string.IsNullOrEmpty(x) && string.IsNullOrEmpty(y))
            {
                return true;
            }

            if (string.IsNullOrEmpty(x) || string.IsNullOrEmpty(y))
            {
                return false;
            }

            return x.ToUpperInvariant() == y.ToUpperInvariant();
        }

        public int GetHashCode(string obj)
        {
            if (string.IsNullOrEmpty(obj))
                return 0;
            return obj.ToUpperInvariant().GetHashCode();
        }
    }
}