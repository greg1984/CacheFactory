namespace CacheFactory.CacheEventArgs
{
    using Cachers.Base;
    using System;

    /// <summary>
    /// A Generic definition for the Key of a Cache Item.
    /// </summary>
    /// <typeparam name="TCacheItem">A Generic type that enables the abstraction of a Cache Item.</typeparam>
    /// <typeparam name="TCacheItemKey">A Generic type that enables the abstraction of a Key to look up the Cached item.</typeparam>
    public class OverflowEventArgs<TCacheItem, TCacheItemKey> : EventArgs
        where TCacheItemKey : CacheItemKey
        where TCacheItem : CacheItem<TCacheItemKey>
    {
        private TCacheItem _cachedItem;

        /// <summary>
        /// Get the Cached Item value.
        /// </summary>
        /// <returns>The Cached Item value.</returns>
        public TCacheItem GetCacheItem()
        {
            return _cachedItem;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public void SetCacheItem(TCacheItem item)
        {
            _cachedItem = item;
        }

        /// <summary>
        /// The method which occur when an event overflows.
        /// </summary>
        /// <param name="item">The item causing the event to overflow.</param>
        public OverflowEventArgs(TCacheItem item)
        {
            _cachedItem = item;
        }
    }
}
