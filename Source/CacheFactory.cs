using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CacheFactory
{
    using Cachers.Base;
    using Cachers;

    public static class CacheFactory<TCacheItem, TCacheItemKey>
        where TCacheItemKey : CacheItemKey
        where TCacheItem : CacheItem<TCacheItemKey>
    {
        public static ACache<TCacheItem, TCacheItemKey> CreateCache(CacheTypes type, bool isGlobal = false)
        {
            if (isGlobal) return GetGlobalCache(type);

            switch (type)
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

        private static ACache<TCacheItem, TCacheItemKey> GetGlobalCache(CacheTypes type)
        {
            IEnumerable<ACache<TCacheItem, TCacheItemKey>> instances;

            switch (type)
            {
                case CacheTypes.FirstInFirstOut:
                    instances = GetInstances<FirstInFirstOutCache<TCacheItem, TCacheItemKey>>();
                    break;
                case CacheTypes.FirstInLastOut:
                    instances = GetInstances<FirstInLastOutCache<TCacheItem, TCacheItemKey>>();
                    break;
                case CacheTypes.LeastRecentlyUsed:
                    instances = GetInstances<LeastRecentlyUsedCache<TCacheItem, TCacheItemKey>>();
                    break;
                case CacheTypes.TimeBasedEviction:
                    instances = GetInstances<TimeBasedEvictionCache<TCacheItem, TCacheItemKey>>();
                    break;
                default:
                    throw new ArgumentException("Invalid Cache type argument passed into the Cache Factory.");
            }

            foreach (var instance in instances.Where(instance => instance.GetCacheName() == "global_cache"))
            {
                return instance;
            }

            throw new ArgumentException("Invalid Cache type argument passed into the Cache Factory.");
        }

        private static IList<T> GetInstances<T>()
        {
            return (from t in Assembly.GetExecutingAssembly().GetTypes()
                    where t.BaseType == (typeof(T)) && t.GetConstructor(Type.EmptyTypes) != null
                    select (T)Activator.CreateInstance(t)).ToList();
        }

    }
}
