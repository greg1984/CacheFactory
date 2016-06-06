using System.Collections.Generic;

namespace CacheFactory.CacheEventArgs
{
    using Cachers.Base;
    using System;

    /// <summary>
    /// An event which is related to a Cached Item.
    /// </summary>
    public class CacheItemKeyEventArgs<TCacheItemKey, TCacheItem> : EventArgs
        where TCacheItemKey : ICacheItemKey
        where TCacheItem : ICacheItem<TCacheItemKey>
    {
        private Dictionary<TCacheItemKey, TCacheItem> _cache;
        private TCacheItemKey _cachedItemKey;

        /// <summary>
        /// Get the Cached Item value.
        /// </summary>
        /// <returns>The Cached Item Keys value.</returns>
        public TCacheItemKey GetCacheItemKey()
        {
            return _cachedItemKey;
        }

        public Dictionary<TCacheItemKey, TCacheItem> GetCache()
        {
            return _cache;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="itemKey"></param>
        public void SetCacheItemKey(TCacheItemKey itemKey)
        {
            _cachedItemKey = itemKey;
        }

        public void SetCache(ref Dictionary<TCacheItemKey, TCacheItem> cache)
        {
            _cache = cache;
        }

        /// <summary>
        /// An event which is related to a Cached Item.
        /// </summary>
        /// <param name="itemKey">The item causing the event.</param>
        /// <param name="cache">The cache attached to the event.</param>
        public CacheItemKeyEventArgs(TCacheItemKey itemKey, ref Dictionary<TCacheItemKey, TCacheItem> cache)
        {
            SetCacheItemKey(itemKey);
            SetCache(ref cache);
        }
    }
}
