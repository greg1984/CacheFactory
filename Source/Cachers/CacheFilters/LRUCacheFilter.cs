using System.Linq;

namespace CacheFactory.Cachers.CacheFilters
{
    using Base;
    using System.Collections.Generic;

    public class LRUCacheFilter<TCacheItem, TCacheItemKey> : ICacheFilter<TCacheItem, TCacheItemKey>
        where TCacheItem : CacheItem<TCacheItemKey>
        where TCacheItemKey : CacheItemKey
    {
        public TCacheItem GetFilteredItem(IDictionary<TCacheItemKey, TCacheItem> cache)
        {
            return cache.Aggregate((p1, p2) => (p1.Value.LastAccessed < p2.Value.LastAccessed) ? p1 : p2).Value;
        }
    }
}
