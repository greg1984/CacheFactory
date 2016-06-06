using System.Linq;

namespace CacheFactory.Cachers.CacheFilters
{
    using Base;
    using System.Collections.Generic;

    public class FILOCacheFilter<TCacheItem, TCacheItemKey> : ICacheFilter<TCacheItem, TCacheItemKey>
        where TCacheItem : CacheItem<TCacheItemKey>
        where TCacheItemKey : CacheItemKey
    {
        public TCacheItem GetFilteredItem(IDictionary<TCacheItemKey, TCacheItem> cache)
        {
            return cache.Aggregate((p1, p2) => (p1.Value.Inserted < p2.Value.Inserted) ? p1 : p2).Value;
        }
    }
}
