namespace CacheFactory.Cachers
{
    using Base;
    using System.Collections.Generic;
    using CacheEventArgs;

    /// <summary>
    /// Generic Least Recently Used Cache
    /// </summary>
    /// <typeparam name="TCacheItem">A Generic type that enables the abstraction of a Cache Item.</typeparam>
    /// <typeparam name="TCacheItemKey">A Generic type that enables the abstraction of a Key to look up the Cached item.</typeparam>
    public class LeastRecentlyUsedCache<TCacheItem, TCacheItemKey> : ACache<TCacheItem, TCacheItemKey>
        where TCacheItemKey : CacheItemKey
        where TCacheItem : CacheItem<TCacheItemKey>
    {
        /// <summary>
        /// Constructor to initialize an LRU Cache.
        /// </summary>
        /// <param name="cache">The initial values of the Cache.</param>
        /// <param name="capacity">The capacity of the Cache.</param>
        public LeastRecentlyUsedCache(IEnumerable<KeyValuePair<TCacheItemKey, TCacheItem>> cache = null, int capacity = 50)
            : base(cache, capacity)
        {
        }

        /// <summary>
        /// An event which is fired when the cache overflows.
        /// </summary>
        /// <param name="e">The event arguments which have caused the cache to overflow.</param>
        protected override void OnCacheOverflow(OverflowEventArgs<TCacheItem, TCacheItemKey> e)
        {
            RemoveCacheItem(CacheEvictor<TCacheItem, TCacheItemKey>.GetLRU(Cache).Key);
        }
    }
}