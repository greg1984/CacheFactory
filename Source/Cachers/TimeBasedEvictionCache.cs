
namespace CacheFactory.Cachers
{
    using Base;

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CacheEventArgs;

    /// <summary>
    /// A Time Based Eviction Cache.
    /// </summary>
    /// <typeparam name="TCacheItem">A Generic type that enables the abstraction of a Cache Item.</typeparam>
    /// <typeparam name="TCacheItemKey">A Generic type that enables the abstraction of a Key to look up the Cached item.</typeparam>
    class TimeBasedEvictionCache<TCacheItem, TCacheItemKey> : ACache<TCacheItem, TCacheItemKey>
        where TCacheItemKey : CacheItemKey
        where TCacheItem : CacheItem<TCacheItemKey>
    {
        /// <summary>
        /// TIme Based Eviction Cache Constructor
        /// </summary>
        /// <param name="timeSpan">The amount of time that data is valid.</param>
        /// <param name="cache">The initial values loaded into the cache.</param>
        /// <param name="capacity">The capacity of the cache.</param>
        /// <param name="strategy">The Eviction Strategy utilized against the Time Based Cache.</param>
        public TimeBasedEvictionCache(TimeSpan timeSpan, IEnumerable<KeyValuePair<TCacheItemKey, TCacheItem>> cache = null, int capacity = 50, TimeBasedEvictionStrategies strategy = TimeBasedEvictionStrategies.FirstInFirstOut)
            : base(cache, capacity)
        {
            _timeSpan = timeSpan;
            _evictionStrategy = strategy;
        }

        /// <summary>
        /// The Eviction strategy utilized in the Time Based Eviction Cache.
        /// </summary>
        private TimeBasedEvictionStrategies _evictionStrategy;

        /// <summary>
        /// The Timespan that the Cache items are retained for.
        /// </summary>
        private TimeSpan _timeSpan;

        /// <summary>
        /// Set the amount of time that data is valid.
        /// </summary>
        /// <param name="timeSpan">The amount of time that data is valid.</param>
        public void SetTimespan(TimeSpan timeSpan)
        {
            _timeSpan = timeSpan;
        }

        /// <summary>
        /// Get the amount of time that data is valid.
        /// </summary>
        /// <returns>The amount of time that data is valid.</returns>
        public TimeSpan GetTimespan()
        {
            return _timeSpan;
        }

        /// <summary>
        /// Get the items which have expired in the cache.
        /// </summary>
        /// <returns>The items which have expired in the cache.</returns>
        public IEnumerable<TCacheItem> GetExpiredItems()
        {
            return Cache.Where(x => DateTime.Now - x.Value.Inserted >= _timeSpan).Select(x => x.Value);
        }

        /// <summary>
        /// Remove an Item from the cache by Key value.
        /// </summary>
        /// <param name="key">The key of the Hash that is to be removed.</param>
        public new void RemoveCacheItem(TCacheItemKey key)
        {
            base.RemoveCacheItem(key);
        }

        /// <summary>
        /// An event which is fired when the cache overflows.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnCacheOverflow(OverflowEventArgs<TCacheItem, TCacheItemKey> e)
        {
            RemoveExpiredCacheItems();
            if (Utilization >= Capacity)
            {
                TCacheItemKey key;
                switch (_evictionStrategy)
                {
                    case TimeBasedEvictionStrategies.LeastRecentlyUsed:
                        key = CacheEvictor<TCacheItem, TCacheItemKey>.GetLRU(Cache).Key;
                        break;
                    case TimeBasedEvictionStrategies.FirstInFirstOut:
                        key = CacheEvictor<TCacheItem, TCacheItemKey>.GetOldestInsertedItem(Cache).Key;
                        break;
                    case TimeBasedEvictionStrategies.FirstInLastOut:
                        key = CacheEvictor<TCacheItem, TCacheItemKey>.GetLatestInsertedItem(Cache).Key;
                        break;
                    default:
                        throw new ArgumentException("Eviction strategy not set for the Time Based Eviction Cache " + GetCacheName());
                }

                RemoveCacheItem(key);
            }
        }

        private void RemoveExpiredCacheItems()
        {
            var expiredItems = GetExpiredItems();
            foreach (var item in expiredItems) RemoveCacheItem(item.Key);
        }
    }
}
