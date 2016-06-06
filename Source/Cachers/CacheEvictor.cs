namespace CacheFactory.Cachers
{
    using Base;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Cache eviction methods.
    /// </summary>
    /// <typeparam name="TCacheItemKey"></typeparam>
    /// <typeparam name="TCacheItem">A Generic type that enables the abstraction of a Cache Item.</typeparam>
    public static class CacheEvictor<TCacheItemKey, TCacheItem> 
        where TCacheItemKey : ICacheItemKey
        where TCacheItem : ICacheItem<TCacheItemKey>, new()
    {
        /// <summary>
        /// Get the Least Recently Used Cache item.
        /// </summary>
        /// <param name="cache">The cache to retrieve the LRU from.</param>
        /// <returns>The Least Recently Used Cache item.</returns>
        public static TCacheItem GetLRU(Dictionary<TCacheItemKey, TCacheItem> cache)
        {
            return cache.Aggregate((p1, p2) => (p1.Value.GetLastAccessedTime() < p2.Value.GetLastAccessedTime()) ? p1 : p2).Value;
        }

        /// <summary>
        /// Get the latest inserted item in the cache.
        /// </summary>
        /// <returns>The most recently inserted cache item.</returns>
        public static TCacheItem GetLatestInsertedItem(Dictionary<TCacheItemKey, TCacheItem> cache)
        {
            return cache.Aggregate((p1, p2) => (p1.Value.GetCreatedTime() > p2.Value.GetCreatedTime()) ? p1 : p2).Value;
        }

        /// <summary>
        /// Get the oldest inserted item.
        /// </summary>
        /// <returns>The oldest inserted item.</returns>
        public static TCacheItem GetOldestInsertedItem(Dictionary<TCacheItemKey, TCacheItem> cache)
        {
            return cache.Aggregate((p1, p2) => (p1.Value.GetCreatedTime() < p2.Value.GetCreatedTime()) ? p1 : p2).Value;
        }
    }
}
