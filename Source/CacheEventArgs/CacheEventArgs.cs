namespace CacheFactory.CacheEventArgs
{
    using Cachers.Base;
    using System;
    using System.Collections.Generic;


    /// <summary>
    /// A Generic definition for the Key of a Cache Item.
    /// </summary>
    public class CacheEventArgs<TCacheItemKey, TCacheItem> : EventArgs
        where TCacheItem : ICacheItem<TCacheItemKey>
        where TCacheItemKey : ICacheItemKey
    {
        private Dictionary<TCacheItemKey, TCacheItem> _cache;

        /// <summary>
        /// The constructor for the cache event arguments..
        /// </summary>
        /// <param name="cache">The cache attached to the event.</param>
        public CacheEventArgs(ref Dictionary<TCacheItemKey, TCacheItem> cache)
        {
            SetCache(ref cache);
        }

        public Dictionary<TCacheItemKey, TCacheItem> GetCache()
        {
            return _cache;
        }

        public void SetCache(ref Dictionary<TCacheItemKey, TCacheItem> cache)
        {
            _cache = cache;
        }
    }
}
