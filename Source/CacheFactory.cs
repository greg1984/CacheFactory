using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CacheFactory
{
    using Cachers.Base;
    using Cachers;

    /// <summary>
    /// A factory to build Cache instances.
    /// </summary>
    /// <typeparam name="TCacheItem">A Generic type that enables the abstraction of a Cache Item.</typeparam>
    /// <typeparam name="TCacheItemKey">A Generic type that enables the abstraction of a Key to look up the Cached item.</typeparam>
    public static class CacheFactory<TCacheItem, TCacheItemKey>
        where TCacheItemKey : CacheItemKey
        where TCacheItem : CacheItem<TCacheItemKey>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type">The type of cache returned.</param>
        /// <param name="isGlobal">If the function creates a global cache or an instance of a cache.</param>
        /// <returns></returns>
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

        /// <summary>
        /// A method to return the global cache.
        /// </summary>
        /// <param name="type">The type of cache returned.</param>
        /// <returns></returns>
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
                    throw new ArgumentException("No Global Cache has been created for the given cache type.");
            }

            foreach (var instance in instances.Where(instance => instance.GetCacheName() == "global_cache"))
            {
                return instance;
            }

            throw new ArgumentException("No Global Cache has been created for the given cache type.");
        }

        /// <summary>
        /// A private function which enables us to load classes from memory.
        /// </summary>
        /// <typeparam name="T">A "class" type class to enable global caching mechanisms.</typeparam>
        /// <returns></returns>
        private static IList<T> GetInstances<T>() where T : class 
        {
            return (from t in Assembly.GetExecutingAssembly().GetTypes()
                    where t.BaseType == (typeof(T)) && t.GetConstructor(Type.EmptyTypes) != null
                    select (T)Activator.CreateInstance(t)).ToList();
        }

    }
}
