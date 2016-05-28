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
    public abstract class ACache<TCacheItem, TCacheItemKey> : ICache<TCacheItem, TCacheItemKey>
        where TCacheItemKey : CacheItemKey
        where TCacheItem : CacheItem<TCacheItemKey>
    {
        /// <summary>
        /// Constructor for a Generic Abstract Cache.
        /// </summary>
        protected ACache()
        {
            SetCache(new Dictionary<TCacheItemKey, TCacheItem>());
            _capacity = 50;
            _utilization = 0;
            _cacheName = string.Empty;
        } 

        /// <summary>
        /// Constructor for a Generic Abstract Cache.
        /// </summary>
        /// <param name="cache">Initial value of the cache.</param>
        /// <param name="capacity">Capacity the cache can hold (0 for infinite)</param>
        protected ACache(IEnumerable<KeyValuePair<TCacheItemKey, TCacheItem>> cache = null, int capacity = 0)
        {
            if (cache == null) cache = new Dictionary<TCacheItemKey, TCacheItem>();
            SetCache(cache);
            _capacity = capacity;
            _utilization = cache.Count();
            _cacheName = string.Empty;
        }

        /// <summary>
        /// Generic Cache Hash
        /// </summary>
        private Dictionary<TCacheItemKey, TCacheItem> _cache;

        /// <summary>
        /// Capacity of the Hash
        /// </summary>
        private int _capacity;

        /// <summary>
        /// Amount of Cached Items.
        /// </summary>
        private int _utilization;

        /// <summary>
        /// 
        /// </summary>
        private string _cacheName;

        /// <summary>
        /// Generic Cache held by the Abstracted Cache.
        /// </summary>
        public Dictionary<TCacheItemKey, TCacheItem> GetCache()
        {
            return _cache;
        }

        /// <summary>
        /// Manually set the cache values.
        /// </summary>
        /// <param name="cache">The Generic Cache which is to be loaded into the Abstracted Cache</param>
        /// <param name="resize"></param>
        public void SetCache(IEnumerable<KeyValuePair<TCacheItemKey, TCacheItem>> cache, bool resize = false)
        {
            if (resize && cache.Count() > _capacity) SetCapacity(cache.Count());

            _cache = cache
                .GroupBy(tp => tp.Key, tp => tp.Value)
                .ToDictionary(gr => gr.Key, gr => cache.First(item => item.Key == gr.Key).Value);

            _utilization = _cache.Count();
        }

        /// <summary>
        /// Remove a Cache Item from the Generic Cache by key
        /// </summary>
        /// <param name="key">The key which the Cache Item should be removed from the Hash by.</param>
        public void RemoveCacheItem(TCacheItemKey key)
        {
            if (_cache.ContainsKey(key))
            {
                _cache.Remove(key);
                _utilization = _utilization - 1;
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
        /// The name of the Cache which is created at runtime.
        /// </summary>
        /// <returns>The string holding the Cache Name.</returns>
        public int GetCapacity()
        {
            return _capacity;
        }

        /// <summary>
        /// The name of the Cache which is created at runtime.
        /// </summary>
        /// <returns>The string holding the Cache Name.</returns>
        public int GetUtilization()
        {
            return _utilization;
        }

        /// <summary>
        /// Return a Cached item from the Generic Cache by Key if it exists.
        /// </summary>
        /// <param name="key">The Generic Key of the Cached Item that is to be removed.</param>
        /// <returns>The cache item by key if it exists, null if it does not.</returns>
        public TCacheItem GetCachedItem(TCacheItemKey key)
        {
            if (_cache.ContainsKey(key))
            {
                _cache[key].SetLastAccessed(DateTime.Now);
                return _cache.First(x => x.Key == key).Value;
            }

            return null;
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
            if (_utilization < _capacity) return false;

            _capacity = capacity;
            return true;
        }

        /// <summary>
        /// Insert a given item into the Cache. This will overwrite existing entries if the item already exists.
        /// </summary>
        /// <param name="item"></param>
        public void InsertCacheItem(TCacheItem item)
        {
            if (!_cache.ContainsKey(item.Key))
            {
                _utilization = _utilization + 1;
            }
            _cache.Add(item.Key, item);

            if (_capacity <= 0 || _utilization > _capacity)
            {
                OnCacheOverflow(new OverflowEventArgs<TCacheItem, TCacheItemKey>(item));
            }
        }

        /// <summary>
        /// Clear all items out of the Cache.
        /// </summary>
        public void Clear()
        {
            _cache = new Dictionary<TCacheItemKey, TCacheItem>();
            _utilization = 0;
        }

        protected abstract void OnCacheOverflow(OverflowEventArgs<TCacheItem, TCacheItemKey> e);
    }
}
