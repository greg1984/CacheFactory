namespace CacheFactory.Cachers.Base
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CacheEventArgs;

    /// <summary>
    /// Abstraction of basic Cache functionality. Can be extended and resulting classes consumed by the factory.
    /// </summary>
    /// <typeparam name="TCacheItem">A Generic type that enables the abstraction of a Cache Item.</typeparam>
    /// <typeparam name="TCacheItemKey">A Generic type that enables the abstraction of a Key to look up the Cached item.</typeparam>
    public abstract class ACache<TCacheItem, TCacheItemKey>
        where TCacheItemKey : CacheItemKey
        where TCacheItem : CacheItem<TCacheItemKey>
    {
        /// <summary>
        /// Base Cache instantiation.
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="capacity"></param>
        protected ACache(IEnumerable<KeyValuePair<TCacheItemKey, TCacheItem>> cache = null, int capacity = 0)
        {
            if (cache == null) cache = new Dictionary<TCacheItemKey, TCacheItem>();
            SetCache(cache);
            Capacity = capacity;
            Utilization = 0;
            _cacheName = string.Empty;
        }

        /// <summary>
        /// Generic Cache Hash
        /// </summary>
        protected Dictionary<TCacheItemKey, TCacheItem> Cache;

        /// <summary>
        /// Capacity of the Hash
        /// </summary>
        protected int Capacity;

        /// <summary>
        /// Amount of Cached Items.
        /// </summary>
        protected int Utilization;

        /// <summary>
        /// 
        /// </summary>
        private string _cacheName;

        /// <summary>
        /// Generic Cache held by the Abstracted Cache.
        /// </summary>
        public Dictionary<TCacheItemKey, TCacheItem> GetCache
        {
            get { return Cache; }
        }

        /// <summary>
        /// Manually set the cache values.
        /// </summary>
        /// <param name="cache">The Generic Cache which is to be loaded into the Abstracted Cache</param>
        public void SetCache(IEnumerable<KeyValuePair<TCacheItemKey, TCacheItem>> cache)
        {
            Cache = cache
                .GroupBy(tp => tp.Key, tp => tp.Value)
                .ToDictionary(gr => gr.Key, gr => cache.First(item => item.Key == gr.Key).Value);

            Utilization = Cache.Count();
        }

        /// <summary>
        /// Remove a Cache Item from the Generic Cache by key
        /// </summary>
        /// <param name="key">The key which the Cache Item should be removed from the Hash by.</param>
        public void RemoveCacheItem(TCacheItemKey key)
        {
            if (Cache.ContainsKey(key))
            {
                Cache.Remove(key);
                Utilization = Utilization - 1;
            }
        }

        /// <summary>
        /// The name of the Cache which is created at runtime.
        /// </summary>
        /// <returns>The string holding the Cache Name.</returns>
        public string GetCacheName()
        {
            return _cacheName;
        }

        /// <summary>
        /// Return a Cached item from the Generic Cache by Key if it exists.
        /// </summary>
        /// <param name="key">The Generic Key of the Cached Item that is to be removed.</param>
        /// <returns></returns>
        public TCacheItem GetCachedItem(TCacheItemKey key)
        {
            Cache[key].SetLastAccessed(DateTime.Now);
            return Cache.First(x => x.Key == key).Value;
        }

        /// <summary>
        /// Enables the ability to change the Cache Name while still protecting the Generic Global Caches
        /// </summary>
        /// <param name="cacheName">A string containing the new Cache name.</param>
        public void SetCacheName(string cacheName)
        {
            if (_cacheName == "global_cache")
            {
                throw new ArgumentException("Modifying this cache name will dissolve the globality.");
            }

            if (cacheName == "global_cache")
            {
                throw new ArgumentException("Setting a cache to global can cause multiple instances of a global cache to be created.");
            }

            _cacheName = cacheName;
        }

        /// <summary>
        /// The ability to set the amount of records that can be loaded into the Cache.
        /// </summary>
        /// <param name="capacity">The modified amount of records which can be loaded into the cache.</param>
        /// <returns></returns>
        public bool SetCapacity(int capacity)
        {
            if (Utilization < Capacity) return false;

            Capacity = capacity;
            return true;
        }

        /// <summary>
        /// Insert a given item into the Cache. This will overwrite existing entries if the item already exists.
        /// </summary>
        /// <param name="item"></param>
        public void InsertCacheItem(TCacheItem item)
        {
            Cache.Add(item.Key, item);

            if (Capacity <= 0 || Utilization >= Capacity)
            {
                OverflowEventArgs<TCacheItem, TCacheItemKey> args = new OverflowEventArgs<TCacheItem, TCacheItemKey>(item);
                OnCacheOverflow(args);
            }
            else if (!Cache.ContainsKey(item.Key))
            {
                Utilization = Utilization + 1;
            }
        }

        /// <summary>
        /// Clear all items out of the Cache.
        /// </summary>
        public void Clear()
        {
            Cache = new Dictionary<TCacheItemKey, TCacheItem>();
            Utilization = 0;
        }

        protected abstract void OnCacheOverflow(OverflowEventArgs<TCacheItem, TCacheItemKey> e);
    }
}
