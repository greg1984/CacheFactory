namespace CacheFactory.Cachers
{
    using Base;

    using System.Collections.Generic;
    using System.Linq;
    using CacheEventArgs;

    public class FirstInFirstOutCache<TCacheItem, TCacheItemKey> : ACache<TCacheItem, TCacheItemKey>
        where TCacheItemKey : CacheItemKey
        where TCacheItem : CacheItem<TCacheItemKey>
    {
        public FirstInFirstOutCache(IEnumerable<KeyValuePair<TCacheItemKey, TCacheItem>> cache = null, int capacity = 50)
            : base(cache, capacity)
        {
        }

        public TCacheItem GetOldestInsertedItem()
        {
            return Cache.Aggregate((p1, p2) => (p1.Value.Inserted < p2.Value.Inserted) ? p1 : p2).Value;
        }

        protected override void OnCacheOverflow(OverflowEventArgs<TCacheItem, TCacheItemKey> e)
        {
            RemoveCacheItem(GetOldestInsertedItem().Key);
        }
    }
}