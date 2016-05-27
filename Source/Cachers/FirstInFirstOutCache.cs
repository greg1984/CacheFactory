namespace CacheFactory.Cachers
{
    using Base;

    using System.Collections.Generic;
    using CacheEventArgs;

    /// <summary>
    /// FIFO Cache.
    /// </summary>
    /// <typeparam name="TCacheItem">A Generic type that enables the abstraction of a Cache Item.</typeparam>
    /// <typeparam name="TCacheItemKey">A Generic type that enables the abstraction of a Key to look up the Cached item.</typeparam>
    public class FirstInFirstOutCache<TCacheItem, TCacheItemKey> : ACache<TCacheItem, TCacheItemKey>
        where TCacheItemKey : CacheItemKey
        where TCacheItem : CacheItem<TCacheItemKey>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cache">Initial value of the cache.</param>
        /// <param name="capacity">The maximum amount of records that can be held in the cache.</param>
        public FirstInFirstOutCache(IEnumerable<KeyValuePair<TCacheItemKey, TCacheItem>> cache = null, int capacity = 50)
            : base(cache, capacity)
        {
        }

        /// <summary>
        /// An event which is fired when the cache is overflown.
        /// </summary>
        /// <param name="e">The event arguments which occured when the cache overflows.</param>
        protected override void OnCacheOverflow(OverflowEventArgs<TCacheItem, TCacheItemKey> e)
        {
            RemoveCacheItem(CacheEvictor<TCacheItem, TCacheItemKey>.GetOldestInsertedItem(Cache).Key);
        }
    }
}