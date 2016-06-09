namespace CacheFactory.CacheEventArgs
{
    using Cachers.Base;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// An event which is related to a Cached Item.
    /// </summary>
    public class CacheItemEventArgs<TCacheItemKey, TCacheItem> : EventArgs
        where TCacheItemKey : ICacheItemKey
        where TCacheItem : ICacheItem<TCacheItemKey>
    {
        private Dictionary<TCacheItemKey, TCacheItem> _cache;
        private TCacheItem _cachedItem;

        /// <summary>
        /// Get the Cached Item value.
        /// </summary>
        /// <returns>The Cached Item value.</returns>
        public TCacheItem GetCacheItem()
        {
            return _cachedItem;
        }

        public Dictionary<TCacheItemKey, TCacheItem> GetCache()
        {
            return _cache;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public void SetCacheItem(ref TCacheItem item)
        {
            _cachedItem = item;
        }

        public void SetCache(ref Dictionary<TCacheItemKey, TCacheItem> cache)
        {
            _cache = cache;
        }

        /// <summary>
        /// An event which is related to a Cached Item.
        /// </summary>
        /// <param name="item">The item causing the event.</param>
        /// <param name="cache">The cache attached to the event.</param>
        public CacheItemEventArgs(ref TCacheItem item, ref Dictionary<TCacheItemKey, TCacheItem> cache)
        {
            SetCacheItem(ref item);
            SetCache(ref cache);
        }
    }
}
