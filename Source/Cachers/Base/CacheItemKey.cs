namespace CacheFactory.Cachers.Base
{
    using System;

    public class CacheItemKey : IEquatable<CacheItemKey>
    {
        public bool Equals(CacheItemKey other)
        {
            return this.GetHashCode() == other.GetHashCode();
        }
    }
}
