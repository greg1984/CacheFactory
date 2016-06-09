namespace CacheFactory.Cachers
{
    using Base;
    using CacheEventArgs;
    using System.Collections.Generic;

    /// <summary>
    /// FIFO Cache.
    /// </summary>
    /// <typeparam name="TCacheItemKey"></typeparam>
    /// <typeparam name="TCacheItem">A Generic type that enables the abstraction of a Cache Item.</typeparam>
    public abstract class FirstInFirstOutCache<TCacheItemKey, TCacheItem> : ACacheWithEvents<TCacheItemKey, TCacheItem>
        where TCacheItemKey : ICacheItemKey
        where TCacheItem : ICacheItem<TCacheItemKey>, new()
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cache">Initial value of the cache.</param>
        /// <param name="capacity">The maximum amount of records that can be held in the cache.</param>
        protected FirstInFirstOutCache(Dictionary<TCacheItemKey, TCacheItem> cache = null, int capacity = 50)
            : base(cache, capacity)
        {
            OnCacheOverflow += FirstInFirstOutCache_OnCacheOverflow;
        }

        /// <summary>
        /// An event which is fired when the cache is overflown.
        /// </summary>
        /// <param name="e">The event arguments which occured when the cache overflows.</param>
        private void FirstInFirstOutCache_OnCacheOverflow(CacheItemEventArgs<TCacheItemKey, TCacheItem> e)
        {
            RemoveCacheItem(CacheEvictor<TCacheItemKey, TCacheItem>.GetOldestInsertedItem(GetCache()).GetKey());
        }
    }
}