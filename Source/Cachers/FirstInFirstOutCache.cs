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
    public class FirstInFirstOutCache<TCacheItemKey, TCacheItem> : ACache<TCacheItemKey, TCacheItem>
        where TCacheItemKey : ICacheItemKey
        where TCacheItem : ICacheItem<TCacheItemKey>, new()
    {
        /// <summary>
        /// Constructor to initialize an FIFO Cache.
        /// </summary>
        public FirstInFirstOutCache() : base(new Dictionary<TCacheItemKey, TCacheItem>(), 50)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cache">Initial value of the cache.</param>
        /// <param name="capacity">The maximum amount of records that can be held in the cache.</param>
        public FirstInFirstOutCache(IEnumerable<KeyValuePair<TCacheItemKey, TCacheItem>> cache = null, int capacity = 50)
            : base(cache, capacity)
        {
            OnCacheOverflowed += FirstInFirstOutCache_OnCacheOverflowed;
        }

        /// <summary>
        /// An event which is fired when the cache is overflown.
        /// </summary>
        /// <param name="e">The event arguments which occured when the cache overflows.</param>
        private void FirstInFirstOutCache_OnCacheOverflowed(CacheItemEventArgs<TCacheItemKey, TCacheItem> e)
        {
            RemoveCacheItem(CacheEvictor<TCacheItemKey, TCacheItem>.GetOldestInsertedItem(GetCache()).GetKey());
        }
    }
}