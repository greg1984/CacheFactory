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
    /// <typeparam name="TCacheItem">A Generic type that enables the abstraction of a Cache Item.</typeparam>
    /// <typeparam name="TCacheItemKey">A Generic type that enables the abstraction of a Key to look up the Cached item.</typeparam>
    /// <typeparam name="TCache">The Cache Interface.</typeparam>
    public class CacheFactory<TCacheItem, TCacheItemKey, TCache>
        where TCacheItemKey : CacheItemKey
        where TCacheItem : CacheItem<TCacheItemKey>
        where TCache : ICache<TCacheItem, TCacheItemKey>, new()
    {
        /// <summary>
        /// Create a cache given a specific cache type.
        /// </summary>
        /// <param name="isGlobal">If the function creates a global cache or an instance of a cache.</param>
        /// <returns></returns>
        public TCache CreateCache(bool isGlobal = false)
        {
            if (isGlobal) return GetGlobalCache();

            return new TCache();
        }

        /// <summary>
        /// A method to return the global cache.
        /// </summary>
        /// <returns>The implemented Cache.</returns>
        private static TCache GetGlobalCache()
        {
            var genericType = typeof(TCache).GetGenericTypeDefinition();
            var instances = GetInstances(genericType);

            foreach (var instance in instances.Where(instance => instance.GetCacheName() == "global_cache"))
            {
                return instance;
            }

            return new TCache();
        }

        /// <summary>
        /// A private function which enables us to load classes from memory.
        /// </summary>
        /// <returns>Return the instances of a specific cache type.</returns>
        private static IEnumerable<TCache> GetInstances(Type type)
        {
            return (from t in Assembly.GetExecutingAssembly().GetTypes()
                    where t.BaseType == type && t.GetConstructor(Type.EmptyTypes) != null
                    select (TCache)Activator.GetObject(t, "")).ToList();
        }
    }
}
