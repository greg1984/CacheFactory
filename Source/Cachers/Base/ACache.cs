using CacheFactory.CacheEventArgs;

namespace CacheFactory.Cachers.Base
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using CacheExceptions;

    /// <summary>
    /// Abstraction of basic Cache functionality. Can be extended and resulting classes consumed by the factory.
    /// </summary>
    /// <typeparam name="TCacheItemKey"></typeparam>
    /// <typeparam name="TCacheItem">A Generic type that enables the abstraction of a Cache Item.</typeparam>
    public abstract class ACache<TCacheItemKey, TCacheItem> : ICache<TCacheItemKey, TCacheItem>
        where TCacheItem : ICacheItem<TCacheItemKey>
        where TCacheItemKey : ICacheItemKey
    {
        /// <summary>
        /// Constructor for a Generic Abstract Cache.
        /// </summary>
        /// <param name="cache">Initial value of the cache.</param>
        /// <param name="capacity">Capacity the cache can hold (0 for infinite)</param>
        protected ACache(Dictionary<TCacheItemKey, TCacheItem> cache = null, int capacity = 0)
        {
            if (cache == null) cache = new Dictionary<TCacheItemKey, TCacheItem>();
            SetCacheName(Guid.NewGuid().ToString());
            SetCache(cache);
            SetCapacity(capacity);
            Utilization = cache.Count();
            CacheName = Guid.NewGuid().ToString();
            SetCreatedTimeToLive(TimeSpan.Zero);
            SetAccessedTimeToLive(TimeSpan.Zero);
            SetItemCreatedTimeToLive(TimeSpan.Zero);
            SetItemAccessedTimeToLive(TimeSpan.Zero);
            SetCacheID(Guid.NewGuid());
        }

        #region protected variables
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
        /// The name of the cache, automatically generated on instantiation.
        /// </summary>
        protected string CacheName;

        /// <summary>
        /// The amount of time the cache is valid for, By default cache items last forever with a Zero value.
        /// </summary>
        protected TimeSpan AccessedTimeToLive;
        
        /// <summary>
        /// The amount of time the cache is valid for, By default cache items last forever with a Zero value.
        /// </summary>
        protected TimeSpan CreatedTimeToLive;

        /// <summary>
        /// The amount of time the cache items are valid for, By default cache items last forever with a Zero value.
        /// </summary>
        protected TimeSpan ItemCreatedTimeToLive;

        /// <summary>
        /// The amount of time the cache items are valid for, By default cache items last forever with a Zero value.
        /// </summary>
        protected TimeSpan ItemAccessedTimeToLive;

        /// <summary>
        /// The amount of time the cache is valid for after creation, By default cache items last forever with a Zero value.
        /// </summary>
        protected DateTime CreatedTime;

        /// <summary>
        /// The amount of time the cache iis valid for after last accessed, By default cache items last forever with a Zero value.
        /// </summary>
        protected DateTime AccessedTime;

        /// <summary>
        /// The Identification Guid of the cache.
        /// </summary>
        protected Guid CacheID;
        #endregion

        /// <summary>
        /// Generic Cache held by the Abstracted Cache.
        /// </summary>
        public Dictionary<TCacheItemKey, TCacheItem> GetCache()
        {
            ClearCacheIfExpired();
            return Cache;
        }

        /// <summary>
        /// The name of the Cache which is created at runtime.
        /// </summary>
        /// <returns>The string holding the Cache Name.</returns>
        public string GetCacheName()
        {
            return CacheName;
        }

        /// <summary>
        /// Get the amount of records the cache can hold. Does cache expiry checks and sets the access time.
        /// </summary>
        /// <returns>The string holding the Cache Name.</returns>
        public int GetCapacity()
        {
            return Capacity;
        }

        /// <summary>
        /// Get the utilization of the cache, does expiry checks to ensure that the utilization return value is accurate.
        /// </summary>
        /// <returns>The amount of records that are held within the cache.</returns>
        public int GetUtilization()
        {
            ClearCacheIfExpired();
            RemoveExpiredItemsByCreatedTime();
            RemoveExpiredItemsByAccessedTime();
            return Utilization;
        }

        /// <summary>
        /// The Amount of time the cache is valid for after creation.
        /// </summary>
        /// <returns>The timespan the cache item is valid for (Default is unlimited with a Zero value).</returns>
        public TimeSpan GetCreatedTimeToLive()
        {
            return CreatedTimeToLive;
        }

        /// <summary>
        /// The Amount of time the cache is valid for after access.
        /// </summary>
        /// <returns>The timespan the cache item is valid for (Default is unlimited with a Zero value).</returns>
        public TimeSpan GetAccessedTimeToLive()
        {
            return AccessedTimeToLive;
        }

        /// <summary>
        /// The Amount of time cache items are valid for.
        /// </summary>
        /// <returns>The timespan the cache item is valid for (Default is unlimited with a Zero value).</returns>
        public TimeSpan GetItemCreatedTimeToLive()
        {
            return ItemCreatedTimeToLive;
        }

        /// <summary>
        /// The Amount of time the cache item is valid for.
        /// </summary>
        /// <returns>The timespan the cache item is valid for (Default is unlimited with a Zero value).</returns>
        public TimeSpan GetItemAccessedTimeToLive()
        {
            return ItemAccessedTimeToLive;
        }

        /// <summary>
        /// Return a Cached item from the Generic Cache by Key if it exists.
        /// </summary>
        /// <param name="key">The Generic Key of the Cached Item that is to be removed.</param>
        /// <returns>The cache item by key if it exists, null if it does not.</returns>
        public TCacheItem GetCachedItem(TCacheItemKey key)
        {
            ClearCacheIfExpired();

            if (Cache.ContainsKey(key))
            {
                Cache[key].SetLastAccessedTime(DateTime.Now);
                return Cache.First(x => x.Key.Equals(key)).Value;
            }

            throw new ItemNotFoundException(key);
        }

        /// <summary>
        /// Remove cache items which have expired against the TTL of the created time if it has not been set to 0.
        /// </summary>
        /// <returns>The items which have expired in the cache.</returns>
        public IEnumerable<TCacheItem> GetExpiredItemsByCreatedTime()
        {
            if (ItemCreatedTimeToLive <= TimeSpan.Zero) return new Collection<TCacheItem>();
            return GetCache().Where(x => (DateTime.Now - x.Value.GetCreatedTime()).Duration() >= ItemCreatedTimeToLive).Select(x => x.Value);
        }

        /// <summary>
        /// Get cache items which have expired against the TTL of the accessed time if it has not been set to 0.
        /// </summary>
        /// <returns>The items which have expired in the cache.</returns>
        public IEnumerable<TCacheItem> GetExpiredItemsByAccessedTime()
        {
            if (ItemAccessedTimeToLive <= TimeSpan.Zero) return new Collection<TCacheItem>();
            return GetCache().Where(x => (DateTime.Now - x.Value.GetLastAccessedTime()).Duration() >= ItemAccessedTimeToLive).Select(x => x.Value);
        }

        /// <summary>
        /// Gets the Identifier of the Cache instance.
        /// </summary>
        /// <returns>A GUID identifying the Cache instance.</returns>
        public Guid GetCacheID()
        {
            return CacheID;
        }

        /// <summary>
        /// Manually set the cache values.
        /// </summary>
        /// <param name="cache">The Generic Cache which is to be loaded into the Abstracted Cache</param>
        /// <param name="resize"></param>
        public void SetCache(Dictionary<TCacheItemKey, TCacheItem> cache, bool resize = false)
        {
            AccessedTime = DateTime.Now;

            if (GetCapacity() != 0 && cache.Count > GetCapacity())
            {
                if (resize)
                {
                    SetCapacity(cache.Count());
                }
                else
                {
                    throw new CacheOverflowException(GetCacheName(), "Unable to set the capacity to a lower value than the utilization, please plan accordingly.");
                }
            }

            Cache = cache
                .GroupBy(tp => tp.Key, tp => tp.Value)
                .ToDictionary(gr => gr.Key, gr => cache.First(item => item.Key.Equals(gr.Key)).Value);

            Utilization = Cache.Count();
        }

        /// <summary>
        /// Enables the ability to change the Cache Name while verifying if a cache of that type with that name already exists.
        /// </summary>
        /// <param name="cacheName">A string containing the new Cache name.</param>
        public void SetCacheName(string cacheName)
        {
            ClearCacheIfExpired();
            OnCacheNameChanged(new CacheEventArgs<TCacheItemKey, TCacheItem>(ref Cache, cacheName));
            CacheName = cacheName;
        }

        public delegate void OnCacheNameChangedHandler(CacheEventArgs<TCacheItemKey, TCacheItem> e);

        public event OnCacheNameChangedHandler OnCacheNameChanged = delegate { };
        
        /// <summary>
        /// The ability to set the amount of records that can be loaded into the Cache.
        /// </summary>
        /// <param name="capacity">The modified amount of records which can be loaded into the cache.</param>
        /// <returns></returns>
        public bool SetCapacity(int capacity)
        {
            ClearCacheIfExpired();
            RemoveExpiredItemsByAccessedTime();
            RemoveExpiredItemsByCreatedTime();

            if (Utilization > capacity) throw new CacheOverflowException(GetCacheName(), "Unable to set the utilization to a lower value than the capacity, please plan accordingly.");

            Capacity = capacity;
            return true;
        }
        
        /// <summary>
        /// The Amount of time the cache item is valid for after it has been created.
        /// </summary>
        /// <param name="timeToLive">The timespan the cache item is valid for (Default is unlimited with a Zero value).</param>
        public void SetItemCreatedTimeToLive(TimeSpan timeToLive)
        {
            ClearCacheIfExpired();
            ItemCreatedTimeToLive = timeToLive;
        }

        /// <summary>
        /// The Amount of time the cache item is valid for after it has been accessed.
        /// </summary>
        /// <param name="timeToLive">The timespan the cache item is valid for (Default is unlimited with a Zero value).</param>
        public void SetItemAccessedTimeToLive(TimeSpan timeToLive)
        {
            ClearCacheIfExpired();
            ItemAccessedTimeToLive = timeToLive;
        }

        /// <summary>
        /// The Amount of time the cache is valid for after it has been created.
        /// </summary>
        /// <param name="timeToLive">The timespan the cache is valid for (Default is unlimited with a Zero value).</param>
        public void SetCreatedTimeToLive(TimeSpan timeToLive)
        {
            AccessedTime = DateTime.Now;
            CreatedTimeToLive = timeToLive;
        }

        /// <summary>
        /// Set the identification Guid for loading / saving as well as identification of the cache within reflection.
        /// </summary>
        /// <param name="cacheID">A Guid identifying the cache.</param>
        public void SetCacheID(Guid cacheID)
        {
            CacheID = cacheID;
        }
        
        /// <summary>
        /// The Amount of time the cache is valid for after it has been accessed.
        /// </summary>
        /// <param name="timeToLive">The timespan the cache is valid for (Default is unlimited with a Zero value).</param>
        public void SetAccessedTimeToLive(TimeSpan timeToLive)
        {
            ClearCacheIfExpired();
            AccessedTimeToLive = timeToLive;
        }

        /// <summary>
        /// Insert a given item into the Cache. This will overwrite existing entries if the item already exists.
        /// </summary>
        /// <param name="item"></param>
        public void InsertCacheItem(TCacheItem item)
        {
            ClearCacheIfExpired();
            RemoveExpiredItemsByAccessedTime();
            RemoveExpiredItemsByCreatedTime();

            if (GetItemAccessedTimeToLive() == TimeSpan.Zero || (DateTime.Now - item.GetLastAccessedTime()).Duration() < GetItemAccessedTimeToLive())
            {
                if (!Cache.ContainsKey(item.GetKey()))
                {
                    Utilization = Utilization + 1;
                }
                else
                {
                    throw new InvalidCacheItemKeyException<TCacheItemKey>(item.GetKey());
                }

                Cache.Add(item.GetKey(), item);

                if (Capacity > 0 && Utilization > Capacity)
                {
                    throw new CacheOverflowException(GetCacheName(), "Cache overflowed, please attach appropriate evicton techniques.");
                }
            }
        }

        /// <summary>
        /// Remove cache items which have expired against the TTL of the creation time if it has not been set to 0.
        /// </summary>
        public void RemoveExpiredItemsByCreatedTime()
        {
            var expiredItems = GetExpiredItemsByCreatedTime().Select(m => m.GetKey()).ToList();
            foreach (var item in expiredItems) RemoveCacheItem(item);
        }

        /// <summary>
        /// Remove cache items which have expired against the TTL of the accessed time if it has not been set to 0.
        /// </summary>
        public void RemoveExpiredItemsByAccessedTime()
        {
            var expiredItems = GetExpiredItemsByAccessedTime().Select(m => m.GetKey()).ToList();
            foreach (var item in expiredItems) RemoveCacheItem(item);
        }

        /// <summary>
        /// Remove a Cache Item from the Generic Cache by key
        /// </summary>
        /// <param name="key">The key which the Cache Item should be removed from the Hash by.</param>
        public void RemoveCacheItem(TCacheItemKey key)
        {
            AccessedTime = DateTime.Now;

            if (Cache.ContainsKey(key))
            {
                Cache.Remove(key);
                Utilization = Utilization - 1;
            }
            else
            {
                throw new ItemNotFoundException(key);
            }
        }

        /// <summary>
        /// Clear the entire cache if the access or creation dates have expired.
        /// </summary>
        public void ClearCacheIfExpired()
        {
            if (GetAccessedTimeToLive() != TimeSpan.Zero && GetAccessedTimeToLive() <= (DateTime.Now - AccessedTime).Duration().Duration() || GetCreatedTimeToLive() != TimeSpan.Zero && GetCreatedTimeToLive() <= (DateTime.Now - CreatedTime).Duration().Duration())
            {
                Clear();
                CreatedTime = DateTime.Now;
            }

            AccessedTime = DateTime.Now;
        }

        /// <summary>
        /// Clear all items out of the Cache.
        /// </summary>
        public void Clear()
        {
            AccessedTime = DateTime.Now;
            CreatedTime = DateTime.Now;
            Cache = new Dictionary<TCacheItemKey, TCacheItem>();
            Utilization = 0;
        }
    }
}
