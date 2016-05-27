
namespace CacheFactory.Cachers
{
    using Base;

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CacheEventArgs;

    class TimeBasedEvictionCache<TCacheItem, TCacheItemKey> : ACache<TCacheItem, TCacheItemKey>
        where TCacheItemKey : CacheItemKey
        where TCacheItem : CacheItem<TCacheItemKey>
    {
        public TimeBasedEvictionCache(TimeSpan timeSpan, IEnumerable<KeyValuePair<TCacheItemKey, TCacheItem>> cache = null, int capacity = 50)
            : base(cache, capacity)
        {
            _timeSpan = timeSpan;
        }

        private TimeSpan _timeSpan;

        public void SetTimespan(TimeSpan timeSpan)
        {
            _timeSpan = timeSpan;
        }

        public TimeSpan GetTimespan()
        {
            return _timeSpan;
        }

        public IEnumerable<TCacheItem> GetExpiredItems()
        {
            return Cache.Where(x => DateTime.Now - x.Value.Inserted >= _timeSpan).Select(x => x.Value);
        }

        public new void RemoveCacheItem(TCacheItemKey key)
        {
            base.RemoveCacheItem(key);
        }

        protected override void OnCacheOverflow(OverflowEventArgs<TCacheItem, TCacheItemKey> e)
        {
            var expiredItems = GetExpiredItems();
            foreach (var item in expiredItems) RemoveCacheItem(item.Key);
        }
    }
}
