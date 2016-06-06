namespace CacheFactory.Cachers.CacheFilters
{
    using Base;
    using System.Collections.Generic;

    public interface ICacheFilter<TCacheItem, TCacheItemKey> where TCacheItem : CacheItem<TCacheItemKey> where TCacheItemKey : CacheItemKey
    {
        TCacheItem GetFilteredItem(IDictionary<TCacheItemKey, TCacheItem> cache);
    }
}
