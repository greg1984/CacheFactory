namespace CacheFactory.Cachers
{
    using Base;
    using CacheEventArgs;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    /// <summary>
    /// A Time Based Eviction Cache.
    /// </summary>
    /// <typeparam name="TCacheItem">A Generic type that enables the abstraction of a Cache Item.</typeparam>
    /// <typeparam name="TCacheItemKey">A Generic type that enables the abstraction of a Key to look up the Cached item.</typeparam>
    public class TimeBasedEvictionCache<TCacheItem, TCacheItemKey> : ACache<TCacheItem, TCacheItemKey>
        where TCacheItemKey : CacheItemKey
        where TCacheItem : CacheItem<TCacheItemKey>
    {
        /// <summary>
        /// Constructor to initialize an TTL Cache.
        /// </summary>
        public TimeBasedEvictionCache()
        {
            _timeSpan = TimeSpan.FromSeconds(1500);
            _evictionStrategy = TimeBasedEvictionStrategies.FirstInFirstOut;
            SetCache(new Dictionary<TCacheItemKey, TCacheItem>());
            SetCapacity(50);
        }

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

        public new void InsertCacheItem(TCacheItem cacheItem)
        {
            RemoveExpiredCacheItems();
            base.InsertCacheItem(cacheItem);
        }

        /// <summary>
        /// Get the items which have expired in the cache.
        /// </summary>
        /// <returns>The items which have expired in the cache.</returns>
        public IEnumerable<TCacheItem> GetExpiredItems()
        {
            if (_timeSpan <= TimeSpan.Zero) return new Collection<TCacheItem>();
            return GetCache().Where(x => (DateTime.Now - x.Value.Inserted).Duration() >= _timeSpan).Select(x => x.Value);
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
            if (GetUtilization() >= GetCapacity())
            {
                TCacheItemKey key;
                switch (_evictionStrategy)
                {
                    case TimeBasedEvictionStrategies.LeastRecentlyUsed:
                        key = CacheEvictor<TCacheItem, TCacheItemKey>.GetLRU(GetCache()).Key;
                        break;
                    case TimeBasedEvictionStrategies.FirstInFirstOut:
                        key = CacheEvictor<TCacheItem, TCacheItemKey>.GetOldestInsertedItem(GetCache()).Key;
                        break;
                    case TimeBasedEvictionStrategies.FirstInLastOut:
                        key = CacheEvictor<TCacheItem, TCacheItemKey>.GetLatestInsertedItem(GetCache()).Key;
                        break;
                    default:
                        throw new ArgumentException("Eviction strategy not set for the Time Based Eviction Cache " + GetCacheName());
                }

                RemoveCacheItem(key);
            }
        }

        public void RemoveExpiredCacheItems()
        {
            var expiredItems = GetExpiredItems().Select(m => m.Key).ToList();
            foreach (var item in expiredItems) RemoveCacheItem(item);
        }
    }
}
