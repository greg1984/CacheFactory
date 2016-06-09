namespace CacheFactory
{
    using Cachers.Base;
    using System.Linq;

    /// <summary>
    /// A factory to build Cache instances.
    /// </summary>
    /// <typeparam name="TCacheItemKey"></typeparam>
    /// <typeparam name="TCacheItem">A Generic type that enables the abstraction of a Cache Item.</typeparam>
    /// <typeparam name="TCache">The Cache Interface.</typeparam>
    public class CacheFactory<TCacheItemKey, TCacheItem, TCache>
        where TCacheItemKey : ICacheItemKey
        where TCacheItem : ICacheItem<TCacheItemKey>, new()
        where TCache : ACacheWithEvents<TCacheItemKey, TCacheItem>, new()
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
            var instances = CacheManager<TCacheItemKey, TCacheItem, TCache>.GetCacheByName("global_cache");

            if (instances.Any())
            {
                return instances.First();
            }

            var newCache = new TCache();
            newCache.SetCacheName("global_cache");
            CacheManager<TCacheItemKey, TCacheItem, TCache>.AddCache(newCache);
            return newCache;
        }
    }
}
