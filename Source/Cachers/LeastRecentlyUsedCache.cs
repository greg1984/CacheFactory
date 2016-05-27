namespace CacheFactory.Cachers
{
    using Base;
    using System.Collections.Generic;
    using System.Linq;
    using CacheEventArgs;

    public class LeastRecentlyUsedCache<TCacheItem, TCacheItemKey> : ACache<TCacheItem, TCacheItemKey>
        where TCacheItemKey : CacheItemKey
        where TCacheItem : CacheItem<TCacheItemKey>
    {
        public LeastRecentlyUsedCache(IEnumerable<KeyValuePair<TCacheItemKey, TCacheItem>> cache = null, int capacity = 50)
            : base(cache, capacity)
        {
        }

        public TCacheItem GetLRU()
        {
            return Cache.Aggregate((p1, p2) => (p1.Value.LastAccessed < p2.Value.LastAccessed) ? p1 : p2).Value;
        }

        protected override void OnCacheOverflow(OverflowEventArgs<TCacheItem, TCacheItemKey> e)
        {
            RemoveCacheItem(GetLRU().Key);
        }
    }
}