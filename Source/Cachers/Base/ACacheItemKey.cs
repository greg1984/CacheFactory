namespace CacheFactory.Cachers.Base
{
    public abstract class ACacheItemKey : ICacheItemKey
    {
        /// <summary>
        /// A function to compare the hash value of the key to another item key.
        /// </summary>
        /// <param name="other">The other Cache Item key to compare with.</param>
        /// <returns>Boolean representing if the keys match.</returns>
        public bool Equals(ICacheItemKey other)
        {
            return GetHashCode() == other.GetHashCode();
        }
    }
}
