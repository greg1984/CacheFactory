namespace CacheFactory.Cachers
{
    using Base;

    using System.Collections.Generic;
    using System.Linq;
    using CacheEventArgs;

    /// <summary>
    /// Generic FILO Cache.
    /// </summary>
    /// <typeparam name="TCacheItem">A Generic type that enables the abstraction of a Cache Item.</typeparam>
    /// <typeparam name="TCacheItemKey">A Generic type that enables the abstraction of a Key to look up the Cached item.</typeparam>
    public class FirstInLastOutCache<TCacheItem, TCacheItemKey> : ACache<TCacheItem, TCacheItemKey>
        where TCacheItemKey : CacheItemKey
        where TCacheItem : CacheItem<TCacheItemKey>
    {
        /// <summary>
        /// Generic FILO Cache class Constructor
        /// </summary>
        /// <param name="cache">The initial value of the cache.</param>
        /// <param name="capacity">The amount of records which can fit in the cache.</param>
        public FirstInLastOutCache(IEnumerable<KeyValuePair<TCacheItemKey, TCacheItem>> cache = null, int capacity = 50)
            : base(cache, capacity)
        {
        }

        /// <summary>
        /// An event which is fired when the cache overflows.
        /// </summary>
        /// <param name="e">The event arguments which have caused the cache to overflow.</param>
        protected override void OnCacheOverflow(OverflowEventArgs<TCacheItem, TCacheItemKey> e)
        {
            RemoveCacheItem(CacheEvictor<TCacheItem, TCacheItemKey>.GetLatestInsertedItem(Cache).Key);
        }
    }
}