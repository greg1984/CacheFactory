namespace CacheFactory.Cachers
{
    using Base;
    using CacheEventArgs;
    using System.Collections.Generic;

    /// <summary>
    /// Generic FILO Cache.
    /// </summary>
    /// <typeparam name="TCacheItemKey"></typeparam>
    /// <typeparam name="TCacheItem">A Generic type that enables the abstraction of a Cache Item.</typeparam>
    public abstract class FirstInLastOutCache<TCacheItemKey, TCacheItem> : ACacheWithEvents<TCacheItemKey, TCacheItem>
        where TCacheItemKey : ICacheItemKey
        where TCacheItem : ICacheItem<TCacheItemKey>, new()
    {
        /// <summary>
        /// Generic FILO Cache class Constructor
        /// </summary>
        /// <param name="cache">The initial value of the cache.</param>
        /// <param name="capacity">The amount of records which can fit in the cache.</param>
        protected FirstInLastOutCache(Dictionary<TCacheItemKey, TCacheItem> cache = null, int capacity = 50) : base(cache, capacity)
        {
            OnCacheOverflow += FirstInLastOutCache_OnCacheOverflow;
        }

        private void FirstInLastOutCache_OnCacheOverflow(CacheItemEventArgs<TCacheItemKey, TCacheItem> e)
        {
            RemoveCacheItem(CacheEvictor<TCacheItemKey, TCacheItem>.GetLatestInsertedItem(GetCache()).GetKey());
        }
    }
}