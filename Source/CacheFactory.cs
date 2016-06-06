namespace CacheFactory
{
    using Cachers.Base;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// A factory to build Cache instances.
    /// </summary>
    /// <typeparam name="TCacheItemKey"></typeparam>
    /// <typeparam name="TCacheItem">A Generic type that enables the abstraction of a Cache Item.</typeparam>
    /// <typeparam name="TCache">The Cache Interface.</typeparam>
    public class CacheFactory<TCacheItemKey, TCacheItem, TCache>
        where TCacheItemKey : ICacheItemKey
        where TCacheItem : ICacheItem<TCacheItemKey>, new()
        where TCache : ICache<TCacheItemKey, TCacheItem>, new()
    {
        /// <summary>
        /// Create a cache given a specific cache type.
        /// </summary>
        /// <param name="isGlobal">If the function creates a global cache or an instance of a cache.</param>
        /// <returns></returns>
        public TCache CreateCache(bool isGlobal = false)
        {
            return isGlobal ? GetGlobalCache() : new TCache();
        }

        /// <summary>
        /// A method to return the global cache.
        /// </summary>
        /// <returns>The implemented Cache.</returns>
        private static TCache GetGlobalCache()
        {
            var instances = GetInstances();

            foreach (var instance in instances.Where(instance => instance.GetCacheName() == "global_cache"))
            {
                return instance;
            }

            var newCache = new TCache();
            newCache.SetCacheName("global_cache", instances);

            return newCache;
        }

        /// <summary>
        /// A private function which enables us to load classes from memory.
        /// </summary>
        /// <returns>Return the instances of a specific cache type.</returns>
        private static IEnumerable<TCache> GetInstances()
        {
            return (from t in Assembly.GetExecutingAssembly().GetTypes().Union(Assembly.GetCallingAssembly().GetTypes()).Distinct()
                    where t == typeof(TCache)
                    select (TCache)Activator.GetObject(t, ""));
        }
    }
}
