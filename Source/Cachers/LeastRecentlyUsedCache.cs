namespace CacheFactory.Cachers
{
    using Base;
    using System.Collections.Generic;
    using CacheEventArgs;

    /// <summary>
    /// Generic Least Recently Used Cache
    /// </summary>
    /// <typeparam name="TCacheItemKey"></typeparam>
    /// <typeparam name="TCacheItem">A Generic type that enables the abstraction of a Cache Item.</typeparam>
    public abstract class LeastRecentlyUsedCache<TCacheItemKey, TCacheItem> : ACacheWithEvents<TCacheItemKey, TCacheItem>
        where TCacheItemKey : ICacheItemKey
        where TCacheItem : ICacheItem<TCacheItemKey>, new()
    {
        /// <summary>
        /// Constructor to initialize an LRU Cache.
        /// </summary>
        /// <param name="cache">The initial values of the Cache.</param>
        /// <param name="capacity">The capacity of the Cache.</param>
        protected LeastRecentlyUsedCache(Dictionary<TCacheItemKey, TCacheItem> cache = null, int capacity = 50)
            : base(cache, capacity)
        {
            OnCacheOverflow += LeastRecentlyUsedCache_OnCacheOverflow;
        }

        /// <summary>
        /// An event which is fired when the cache overflows.
        /// </summary>
        /// <param name="e">The event arguments which have caused the cache to overflow.</param>
        private void LeastRecentlyUsedCache_OnCacheOverflow(CacheItemEventArgs<TCacheItemKey, TCacheItem> e)
        {
            RemoveCacheItem(CacheEvictor<TCacheItemKey, TCacheItem>.GetLRU(GetCache()).GetKey());
        }
    }
}