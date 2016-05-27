namespace CacheFactory.Cachers.Base
{
    using System;

    public class CacheItemKey : IEquatable<CacheItemKey>
    {
        /// <summary>
        /// A function to compare the hash value of the key to another item key.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(CacheItemKey other)
        {
            return this.GetHashCode() == other.GetHashCode();
        }
    }
}
