using System;

namespace CacheFactory
{
    using Cachers.Base;
    using Cachers;

    class CacheFactory<TCacheItem, TCacheItemKey>
        where TCacheItemKey : CacheItemKey
        where TCacheItem : CacheItem<TCacheItemKey>
    {
        public static ACache<TCacheItem, TCacheItemKey> CreateCache(CacheTypes types)
        {
            switch (types)
            {
                case CacheTypes.FirstInFirstOut:
                    return new FirstInFirstOutCache<TCacheItem, TCacheItemKey>();
                case CacheTypes.FirstInLastOut:
                    return new FirstInLastOutCache<TCacheItem, TCacheItemKey>();
                case CacheTypes.LeastRecentlyUsed:
                    return new LeastRecentlyUsedCache<TCacheItem, TCacheItemKey>();
                case CacheTypes.TimeBasedEviction:
                    return new TimeBasedEvictionCache<TCacheItem, TCacheItemKey>(TimeSpan.FromMilliseconds(1500));
                default:
                    throw new ArgumentException("Invalid Cache type argument passed into the Cache Factory.");
            }
        }
    }
}
