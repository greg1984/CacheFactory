namespace CacheFactory.Cachers.Base
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CacheEventArgs;

    public abstract class ACache<TCacheItem, TCacheItemKey>
        where TCacheItemKey : CacheItemKey
        where TCacheItem : CacheItem<TCacheItemKey>
    {
        protected ACache(IEnumerable<KeyValuePair<TCacheItemKey, TCacheItem>> cache = null, int capacity = 0)
        {
            if (cache == null) cache = new Dictionary<TCacheItemKey, TCacheItem>();
            SetCache(cache);
            Capacity = capacity;
            Utilization = 0;
            _cacheName = string.Empty;
        }

        protected Dictionary<TCacheItemKey, TCacheItem> Cache;

        protected int Capacity;

        protected int Utilization;

        private string _cacheName;

        public Dictionary<TCacheItemKey, TCacheItem> GetCache
        {
            get { return Cache; }
        }

        public void SetCache(IEnumerable<KeyValuePair<TCacheItemKey, TCacheItem>> cache)
        {
            Cache = cache
                .GroupBy(tp => tp.Key, tp => tp.Value)
                .ToDictionary(gr => gr.Key, gr => cache.First(item => item.Key == gr.Key).Value);

            Utilization = Cache.Count();
        }

        public void RemoveCacheItem(TCacheItemKey key)
        {
            if (Cache.ContainsKey(key))
            {
                Cache.Remove(key);
                Utilization = Utilization - 1;
            }
        }

        public string GetCacheName()
        {
            return _cacheName;
        }

        public TCacheItem GetCachedItem(TCacheItemKey key)
        {
            Cache[key].SetLastAccessed(DateTime.Now);
            return Cache.First(x => x.Key == key).Value;
        }

        protected void SetCacheName(string cacheName)
        {
            _cacheName = cacheName;
        }

        public bool SetCapacity(int capacity)
        {
            if (Utilization >= Capacity)
            {
                Capacity = capacity;
                return true;
            }
            return false;
        }

        public void InsertCacheItem(TCacheItem item)
        {
            Cache.Add(item.Key, item);

            if (Capacity <= 0 || Utilization >= Capacity)
            {
                OverflowEventArgs<TCacheItem, TCacheItemKey> args = new OverflowEventArgs<TCacheItem, TCacheItemKey>(item);
                OnCacheOverflow(args);
            }
            else
            {
                Utilization = Utilization + 1;
            }
        }

        public void Clear()
        {
            Cache = new Dictionary<TCacheItemKey, TCacheItem>();
            Utilization = 0;
        }

        protected abstract void OnCacheOverflow(OverflowEventArgs<TCacheItem, TCacheItemKey> e);
    }
}
