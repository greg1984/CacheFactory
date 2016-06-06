using System.Collections.ObjectModel;

namespace CacheFactory.Cachers.Base
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CacheEventArgs;

    /// <summary>
    /// Abstraction of basic Cache functionality. Can be extended and resulting classes consumed by the factory.
    /// </summary>
    /// <typeparam name="TCacheItemKey"></typeparam>
    /// <typeparam name="TCacheItem">A Generic type that enables the abstraction of a Cache Item.</typeparam>
    public abstract class ACache<TCacheItemKey, TCacheItem> : ICache<TCacheItemKey, TCacheItem>
        where TCacheItem : ICacheItem<TCacheItemKey>, new()
        where TCacheItemKey : ICacheItemKey
    {
        /// <summary>
        /// Constructor for a Generic Abstract Cache.
        /// </summary>
        public ACache()
        {
            SetCache(new Dictionary<TCacheItemKey, TCacheItem>());
            _capacity = 50;
            _utilization = 0;
            _cacheName = string.Empty;
            _createdTime = DateTime.Now;
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
            SetCreatedTimeToLive(TimeSpan.Zero);
            SetAccessedTimeToLive(TimeSpan.Zero);
            SetItemCreatedTimeToLive(TimeSpan.Zero);
            SetItemAccessedTimeToLive(TimeSpan.Zero);
        }


        // A delegate type for hooking up change notifications.
        public delegate void OnCacheClearEventHandler(CacheEventArgs<TCacheItemKey, TCacheItem> e);

        public event OnCacheClearEventHandler OnCacheCleared = delegate{  };
        
        public delegate void OnCacheOverflowEventHandler(CacheItemEventArgs<TCacheItemKey, TCacheItem> e);

        public event OnCacheOverflowEventHandler OnCacheOverflowed = delegate {  };

        public delegate void OnItemNotFoundEventHandler(CacheItemKeyEventArgs<TCacheItemKey, TCacheItem> e);

        public event OnItemNotFoundEventHandler OnItemNotFound = delegate {  };

        public delegate void OnBeforeItemInsertEventHandler(CacheItemEventArgs<TCacheItemKey, TCacheItem> e);

        public event OnBeforeItemInsertEventHandler OnBeforeItemInserted = delegate {  };

        public delegate void OnAfterItemInsertEventHandler(CacheItemEventArgs<TCacheItemKey, TCacheItem> e);
        
        public event OnAfterItemInsertEventHandler OnAfterItemInserted = delegate {  };

        public delegate void OnItemAccessedTimeToLiveEventHandler(CacheEventArgs<TCacheItemKey, TCacheItem> e);

        public event OnItemAccessedTimeToLiveEventHandler OnItemAccessedTimeToLive = delegate { };

        public delegate void OnItemCreatedTimeToLiveHandler(CacheEventArgs<TCacheItemKey, TCacheItem> e);

        public event OnItemCreatedTimeToLiveHandler OnItemCreatedTimeToLive = delegate { };

        public delegate void OnAccessedTimeToLiveEventHandler(CacheEventArgs<TCacheItemKey, TCacheItem> e);

        public event OnAccessedTimeToLiveEventHandler OnAccessedTimeToLive = delegate { };

        public delegate void OnCreatedTimeToLiveHandler(CacheEventArgs<TCacheItemKey, TCacheItem> e);

        public event OnCreatedTimeToLiveHandler OnCreatedTimeToLive = delegate { };
        
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
        /// The name of the cache, automatically generated on instantiation.
        /// </summary>
        private string _cacheName;

        /// <summary>
        /// The amount of time the cache is valid for, By default cache items last forever with a Zero value.
        /// </summary>
        private TimeSpan _accessedTimeToLive;
        
        /// <summary>
        /// The amount of time the cache is valid for, By default cache items last forever with a Zero value.
        /// </summary>
        private TimeSpan _createdTimeToLive;

        /// <summary>
        /// The amount of time the cache is valid for, By default cache items last forever with a Zero value.
        /// </summary>
        private TimeSpan _itemCreatedTimeToLive;

        /// <summary>
        /// The amount of time the cache is valid for, By default cache items last forever with a Zero value.
        /// </summary>
        private TimeSpan _itemAccessedTimeToLive;

        private DateTime _accessedTime;
        private DateTime _createdTime;

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
            _accessedTime = DateTime.Now;

            if (resize && cache.Count() > _capacity) SetCapacity(cache.Count());

            _cache = cache
                .GroupBy(tp => tp.Key, tp => tp.Value)
                .ToDictionary(gr => gr.Key, gr => cache.First(item => item.Key.Equals(gr.Key)).Value);

            _utilization = _cache.Count();
        }

        /// <summary>
        /// Remove a Cache Item from the Generic Cache by key
        /// </summary>
        /// <param name="key">The key which the Cache Item should be removed from the Hash by.</param>
        public void RemoveCacheItem(TCacheItemKey key)
        {
            _accessedTime = DateTime.Now;

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
            _accessedTime = DateTime.Now;

            return _cacheName;
        }

        /// <summary>
        /// The name of the Cache which is created at runtime.
        /// </summary>
        /// <returns>The string holding the Cache Name.</returns>
        public int GetCapacity()
        {
            _accessedTime = DateTime.Now;

            return _capacity;
        }

        /// <summary>
        /// The name of the Cache which is created at runtime.
        /// </summary>
        /// <returns>The string holding the Cache Name.</returns>
        public int GetUtilization()
        {
            _accessedTime = DateTime.Now;

            OnCreatedTimeToLive(new CacheEventArgs<TCacheItemKey, TCacheItem>(ref _cache));
            OnAccessedTimeToLive(new CacheEventArgs<TCacheItemKey, TCacheItem>(ref _cache));
            OnItemAccessedTimeToLive(new CacheEventArgs<TCacheItemKey, TCacheItem>(ref _cache));
            OnItemCreatedTimeToLive(new CacheEventArgs<TCacheItemKey, TCacheItem>(ref _cache));
            return _utilization;
        }

        /// <summary>
        /// The Amount of time the cache is valid for after creation.
        /// </summary>
        /// <returns>The timespan the cache item is valid for (Default is unlimited with a Zero value).</returns>
        public TimeSpan GetCreatedTimeToLive()
        {
            return _createdTimeToLive;
        }

        /// <summary>
        /// The Amount of time the cache is valid for after access.
        /// </summary>
        /// <returns>The timespan the cache item is valid for (Default is unlimited with a Zero value).</returns>
        public TimeSpan GetAccessedTimeToLive()
        {
            return _accessedTimeToLive;
        }

        /// <summary>
        /// The Amount of time the cache item is valid for.
        /// </summary>
        /// <returns>The timespan the cache item is valid for (Default is unlimited with a Zero value).</returns>
        public TimeSpan GetItemCreatedTimeToLive()
        {
            _accessedTime = DateTime.Now;

            return _itemCreatedTimeToLive;
        }

        /// <summary>
        /// The Amount of time the cache item is valid for.
        /// </summary>
        /// <returns>The timespan the cache item is valid for (Default is unlimited with a Zero value).</returns>
        public TimeSpan GetItemAccessedTimeToLive()
        {
            _accessedTime = DateTime.Now;

            return _itemAccessedTimeToLive;
        }

        /// <summary>
        /// Return a Cached item from the Generic Cache by Key if it exists.
        /// </summary>
        /// <param name="key">The Generic Key of the Cached Item that is to be removed.</param>
        /// <returns>The cache item by key if it exists, null if it does not.</returns>
        public TCacheItem GetCachedItem(TCacheItemKey key)
        {
            _accessedTime = DateTime.Now;

            OnItemCreatedTimeToLive(new CacheEventArgs<TCacheItemKey, TCacheItem>(ref _cache));
            OnItemAccessedTimeToLive(new CacheEventArgs<TCacheItemKey, TCacheItem>(ref _cache));
            OnCreatedTimeToLive(new CacheEventArgs<TCacheItemKey, TCacheItem>(ref _cache));
            OnAccessedTimeToLive(new CacheEventArgs<TCacheItemKey, TCacheItem>(ref _cache));

            if (_cache.ContainsKey(key))
            {
                _cache[key].SetLastAccessedTime(DateTime.Now);
                return _cache.First(x => x.Key.Equals(key)).Value;
            }

            OnItemNotFound(new CacheItemKeyEventArgs<TCacheItemKey, TCacheItem>(key, ref _cache));
            return new TCacheItem();
        }

        /// <summary>
        /// Enables the ability to change the Cache Name while verifying if a cache of that type with that name already exists.
        /// </summary>
        /// <param name="cacheName">A string containing the new Cache name.</param>
        /// <param name="existingCaches">Existing caches to check if the cache names already exists.</param>
        public void SetCacheName<TCache>(string cacheName, IEnumerable<TCache> existingCaches) where TCache : ICache<TCacheItemKey, TCacheItem>
        {
            _accessedTime = DateTime.Now;

            var cacheExists = existingCaches.Any(m => m.GetCacheName() == cacheName);

            if (cacheExists)
            {
                throw new ArgumentException("Cache with the name \"" + cacheName + "\" already exists.");
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
            _accessedTime = DateTime.Now;

            OnItemCreatedTimeToLive(new CacheEventArgs<TCacheItemKey, TCacheItem>(ref _cache));
            OnItemAccessedTimeToLive(new CacheEventArgs<TCacheItemKey, TCacheItem>(ref _cache));
            OnCreatedTimeToLive(new CacheEventArgs<TCacheItemKey, TCacheItem>(ref _cache));
            OnAccessedTimeToLive(new CacheEventArgs<TCacheItemKey, TCacheItem>(ref _cache));

            if (_utilization > _capacity) return false;

            _capacity = capacity;
            return true;
        }
        
        /// <summary>
        /// The Amount of time the cache item is valid for after it has been created.
        /// </summary>
        /// <param name="timeToLive">The timespan the cache item is valid for (Default is unlimited with a Zero value).</param>
        public void SetItemCreatedTimeToLive(TimeSpan timeToLive)
        {
            _accessedTime = DateTime.Now;

            if (timeToLive.Equals(TimeSpan.Zero))
            {
                OnItemCreatedTimeToLive -= ACache_OnItemCreatedTimeToLive;
            }
            else
            {
                OnItemCreatedTimeToLive += ACache_OnItemCreatedTimeToLive;
            }
            _itemCreatedTimeToLive = timeToLive;
        }

        /// <summary>
        /// The Amount of time the cache item is valid for after it has been accessed.
        /// </summary>
        /// <param name="timeToLive">The timespan the cache item is valid for (Default is unlimited with a Zero value).</param>
        public void SetItemAccessedTimeToLive(TimeSpan timeToLive)
        {
            _accessedTime = DateTime.Now;

            if (timeToLive.Equals(TimeSpan.Zero))
            {
                OnItemAccessedTimeToLive -= ACache_OnItemAccessedTimeToLive;
            }
            else
            {
                OnItemAccessedTimeToLive += ACache_OnItemAccessedTimeToLive;
            }

            _itemAccessedTimeToLive = timeToLive;
        }

        /// <summary>
        /// The Amount of time the cache is valid for after it has been created.
        /// </summary>
        /// <param name="timeToLive">The timespan the cache is valid for (Default is unlimited with a Zero value).</param>
        public void SetCreatedTimeToLive(TimeSpan timeToLive)
        {
            _accessedTime = DateTime.Now;

            if (timeToLive.Equals(TimeSpan.Zero))
            {
                OnCreatedTimeToLive -= ACache_OnCreatedTimeToLive;
            }
            else
            {
                OnCreatedTimeToLive += ACache_OnCreatedTimeToLive;
            }

            _createdTimeToLive = timeToLive;
        }

        private void ACache_OnCreatedTimeToLive(CacheEventArgs<TCacheItemKey, TCacheItem> e)
        {
            _accessedTime = DateTime.Now;

            if (GetAccessedTimeToLive() <= (DateTime.Now - _createdTime).Duration().Duration()) Clear();
        }
        
        /// <summary>
        /// The Amount of time the cache is valid for after it has been accessed.
        /// </summary>
        /// <param name="timeToLive">The timespan the cache is valid for (Default is unlimited with a Zero value).</param>
        public void SetAccessedTimeToLive(TimeSpan timeToLive)
        {
            _accessedTime = DateTime.Now;

            if (timeToLive.Equals(TimeSpan.Zero))
            {
                OnAccessedTimeToLive -= ACache_OnAccessedTimeToLive;
            }
            else
            {
                OnAccessedTimeToLive += ACache_OnAccessedTimeToLive;
            }

            _accessedTimeToLive = timeToLive;
        }

        private void ACache_OnItemCreatedTimeToLive(CacheEventArgs<TCacheItemKey, TCacheItem> e)
        {
            RemoveExpiredItemsByCreatedTime();
        }

        private void ACache_OnAccessedTimeToLive(CacheEventArgs<TCacheItemKey, TCacheItem> e)
        {
            if (GetAccessedTimeToLive() <=
                (DateTime.Now - _accessedTime).Duration().Duration()) Clear();
        }

        private void ACache_OnItemAccessedTimeToLive(CacheEventArgs<TCacheItemKey, TCacheItem> e)
        {
            RemoveExpiredItemsByAccessedTime();
        }

        /// <summary>
        /// Insert a given item into the Cache. This will overwrite existing entries if the item already exists.
        /// </summary>
        /// <param name="item"></param>
        public void InsertCacheItem(TCacheItem item)
        {
            _accessedTime = DateTime.Now;

            OnItemCreatedTimeToLive(new CacheEventArgs<TCacheItemKey, TCacheItem>(ref _cache));
            OnItemAccessedTimeToLive(new CacheEventArgs<TCacheItemKey, TCacheItem>(ref _cache));
            OnCreatedTimeToLive(new CacheEventArgs<TCacheItemKey, TCacheItem>(ref _cache));
            OnAccessedTimeToLive(new CacheEventArgs<TCacheItemKey, TCacheItem>(ref _cache));
            OnBeforeItemInserted(new CacheItemEventArgs<TCacheItemKey, TCacheItem>(ref item, ref _cache));

            if (!_cache.ContainsKey(item.GetKey()))
            {
                _utilization = _utilization + 1;
            }

            _cache.Add(item.GetKey(), item);

            if (_capacity <= 0 || _utilization > _capacity)
            {
                OnCacheOverflowed(new CacheItemEventArgs<TCacheItemKey, TCacheItem>(ref item, ref _cache));
            }

            OnAfterItemInserted(new CacheItemEventArgs<TCacheItemKey, TCacheItem>(ref item, ref _cache));
        }
        
        /// <summary>
        /// Get the items which have expired in the cache.
        /// </summary>
        /// <returns>The items which have expired in the cache.</returns>
        public IEnumerable<TCacheItem> GetExpiredItemsByCreatedTime()
        {
            _accessedTime = DateTime.Now;

            if (_itemCreatedTimeToLive <= TimeSpan.Zero) return new Collection<TCacheItem>();
            return GetCache().Where(x => (DateTime.Now - x.Value.GetCreatedTime()).Duration() >= _itemCreatedTimeToLive).Select(x => x.Value);
        }

        public void RemoveExpiredItemsByCreatedTime()
        {
            _accessedTime = DateTime.Now;

            var expiredItems = GetExpiredItemsByCreatedTime().Select(m => m.GetKey()).ToList();
            foreach (var item in expiredItems) RemoveCacheItem(item);
        }

        /// <summary>
        /// Get the items which have expired in the cache.
        /// </summary>
        /// <returns>The items which have expired in the cache.</returns>
        public IEnumerable<TCacheItem> GetExpiredItemsByAccessedTime()
        {
            _accessedTime = DateTime.Now;

            if (_itemAccessedTimeToLive <= TimeSpan.Zero) return new Collection<TCacheItem>();
            return GetCache().Where(x => (DateTime.Now - x.Value.GetLastAccessedTime()).Duration() >= _itemAccessedTimeToLive).Select(x => x.Value);
        }

        public void RemoveExpiredItemsByAccessedTime()
        {
            _accessedTime = DateTime.Now;

            var expiredItems = GetExpiredItemsByAccessedTime().Select(m => m.GetKey()).ToList();
            foreach (var item in expiredItems) RemoveCacheItem(item);
        }

        /// <summary>
        /// Clear all items out of the Cache.
        /// </summary>
        public void Clear()
        {
            _accessedTime = DateTime.Now;
            _createdTime = DateTime.Now;

            OnCacheCleared(new CacheEventArgs<TCacheItemKey, TCacheItem>(ref _cache));
            _cache = new Dictionary<TCacheItemKey, TCacheItem>();
            _utilization = 0;
        }
    }
}
