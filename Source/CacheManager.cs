namespace CacheFactory
{
    using System.Collections.Generic;
    using System.Linq;
    using CacheExceptions;
    using Cachers.Base;
    using CacheEventArgs;

    public static class CacheManager<TCacheItemKey, TCacheItem, TCache>
        where TCacheItemKey : ICacheItemKey
        where TCacheItem : ICacheItem<TCacheItemKey>, new()
        where TCache : ACacheWithEvents<TCacheItemKey, TCacheItem>, new()
    {
        private static readonly object SyncRoot = new object();
        private static readonly Dictionary<string, TCache> Caches = new Dictionary<string, TCache>();

        public static void AddCache(TCache cache, bool overwriteCache = false)
        {
            lock (SyncRoot)
            {
                if (!overwriteCache && Caches.ContainsKey(cache.GetCacheName())) throw new InvalidCacheNameException(cache.GetCacheName(), "Cache already exists with that name within manager.");
                Caches.Add(cache.GetCacheName(), cache);
                cache.OnCacheNameChanged += ACache_OnCacheNameChanged;
            }
        }
        
        public static void ACache_OnCacheNameChanged(CacheEventArgs<TCacheItemKey, TCacheItem> e)
        {
            lock (SyncRoot)
            {
                if (Caches.Any(m => m.Value.GetCache() != e.GetCache() && m.Value.GetCacheName() == e.GetCacheName())) throw new InvalidCacheNameException(e.GetCacheName(), "Cache with that name already exists within the manager.");
            }
        }

        public static IEnumerable<TCache> GetCacheByName(string name)
        {
            lock (SyncRoot)
            {
                return Caches.Where(m => m.Value.GetCacheName() == name).Select(cache => cache.Value);
            }
        }

        public static void RemoveCache(string cacheName)
        {
            lock (SyncRoot)
            {
                if (!Caches.ContainsKey(cacheName)) throw new InvalidCacheNameException(cacheName, "Unable to retrieve the cache by the provided name.");
                var cache = Caches[cacheName];
                cache.OnCacheNameChanged -= ACache_OnCacheNameChanged;
                Caches.Remove(cacheName);
            }
        }

        public static int GetCacheCount()
        {
            lock (SyncRoot)
            {
                return Caches.Count;
            }
        }
    }
}
