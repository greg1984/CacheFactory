using CacheFactory.Cachers.Base;
using System.Collections.Generic;
using System.Linq;

namespace CacheFactory.Cachers
{
    public static class CacheEvictor<TCacheItem, TCacheItemKey> 
        where TCacheItemKey : CacheItemKey
        where TCacheItem : CacheItem<TCacheItemKey>
    {
        /// <summary>
        /// Get the Least Recently Used Cache item.
        /// </summary>
        /// <param name="cache">The cache to retrieve the LRU from.</param>
        /// <returns>The Least Recently Used Cache item.</returns>
        public static TCacheItem GetLRU(Dictionary<TCacheItemKey, TCacheItem> cache)
        {
            return cache.Aggregate((p1, p2) => (p1.Value.LastAccessed < p2.Value.LastAccessed) ? p1 : p2).Value;
        }

        /// <summary>
        /// Get the latest inserted item in the cache.
        /// </summary>
        /// <returns>The most recently inserted cache item.</returns>
        public static TCacheItem GetLatestInsertedItem(Dictionary<TCacheItemKey, TCacheItem> cache)
        {
            return cache.Aggregate((p1, p2) => (p1.Value.Inserted < p2.Value.Inserted) ? p1 : p2).Value;
        }

        /// <summary>
        /// Get the oldest inserted item.
        /// </summary>
        /// <returns>The oldest inserted item.</returns>
        public static TCacheItem GetOldestInsertedItem(Dictionary<TCacheItemKey, TCacheItem> cache)
        {
            return cache.Aggregate((p1, p2) => (p1.Value.Inserted < p2.Value.Inserted) ? p1 : p2).Value;
        }
    }
}
