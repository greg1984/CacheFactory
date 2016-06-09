namespace CacheFactory.Cachers.Base
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Linq;
    using CacheEventArgs;
    using CacheExceptions;

    public abstract class ACacheWithEvents<TCacheItemKey, TCacheItem> : ACache<TCacheItemKey, TCacheItem>
        where TCacheItem : ICacheItem<TCacheItemKey>, new()
        where TCacheItemKey : ICacheItemKey
    {
        protected ACacheWithEvents(Dictionary<TCacheItemKey, TCacheItem> cache = null, int capacity = 0)
            : base(cache, capacity)
        {
            OnItemInsert += ACacheWithEvents_OnItemInsert;
            SetCreatedTimeToLive(TimeSpan.Zero);
            SetAccessedTimeToLive(TimeSpan.Zero);
            SetItemCreatedTimeToLive(TimeSpan.Zero);
            SetItemAccessedTimeToLive(TimeSpan.Zero);
        }

        private void ACacheWithEvents_OnItemInsert(CacheItemEventArgs<TCacheItemKey, TCacheItem> e)
        {
            if (GetItemAccessedTimeToLive() == TimeSpan.Zero || (DateTime.Now - e.GetCacheItem().GetLastAccessedTime()).Duration() < GetItemAccessedTimeToLive())
            {
                if (!Cache.ContainsKey(e.GetCacheItem().GetKey()))
                {
                    Utilization = Utilization + 1;
                }

                Cache.Add(e.GetCacheItem().GetKey(), e.GetCacheItem());

                if (Capacity > 0 && Utilization > GetCapacity())
                {
                    var cachedItem = e.GetCacheItem();
                    OnCacheOverflow(new CacheItemEventArgs<TCacheItemKey, TCacheItem>(ref cachedItem, ref Cache));
                }
            }
        }

        #region Events, Delegates and Handlers.

        public delegate void OnCacheClearEventHandler(CacheEventArgs<TCacheItemKey, TCacheItem> e);

        public event OnCacheClearEventHandler OnCacheCleared = delegate { };

        public delegate void OnCacheOverflowEventHandler(CacheItemEventArgs<TCacheItemKey, TCacheItem> e);

        public event OnCacheOverflowEventHandler OnCacheOverflow = delegate { };

        public delegate void OnItemNotFoundEventHandler(CacheItemKeyEventArgs<TCacheItemKey, TCacheItem> e);

        public event OnItemNotFoundEventHandler OnItemNotFound = delegate { };

        public delegate void OnBeforeItemInsertEventHandler(CacheItemEventArgs<TCacheItemKey, TCacheItem> e);

        public event OnBeforeItemInsertEventHandler OnBeforeItemInserted = delegate { };

        public delegate void OnItemInsertEventHandler(CacheItemEventArgs<TCacheItemKey, TCacheItem> e);

        public event OnItemInsertEventHandler OnItemInsert = delegate { };

        public delegate void OnAfterItemInsertEventHandler(CacheItemEventArgs<TCacheItemKey, TCacheItem> e);

        public event OnAfterItemInsertEventHandler OnAfterItemInserted = delegate { };

        public delegate void OnItemAccessedTimeToLiveEventHandler(CacheEventArgs<TCacheItemKey, TCacheItem> e);

        public event OnItemAccessedTimeToLiveEventHandler OnItemAccessedTimeToLive = delegate { };

        public delegate void OnItemCreatedTimeToLiveHandler(CacheEventArgs<TCacheItemKey, TCacheItem> e);

        public event OnItemCreatedTimeToLiveHandler OnItemCreatedTimeToLive = delegate { };

        public delegate void OnAccessedTimeToLiveEventHandler(CacheEventArgs<TCacheItemKey, TCacheItem> e);

        public event OnAccessedTimeToLiveEventHandler OnAccessedTimeToLive = delegate { };

        public delegate void OnCreatedTimeToLiveHandler(CacheEventArgs<TCacheItemKey, TCacheItem> e);

        public event OnCreatedTimeToLiveHandler OnCreatedTimeToLive = delegate { };
        
        public void ACache_OnItemCreatedTimeToLive(CacheEventArgs<TCacheItemKey, TCacheItem> e)
        {
            ClearCacheIfExpired();
            RemoveExpiredItemsByAccessedTime();
            RemoveExpiredItemsByCreatedTime();
            RemoveExpiredItemsByCreatedTime();
        }

        public void ACache_OnAccessedTimeToLive(CacheEventArgs<TCacheItemKey, TCacheItem> e)
        {
            ClearCacheIfExpired();
            RemoveExpiredItemsByAccessedTime();
            RemoveExpiredItemsByCreatedTime();
            AccessedTime = DateTime.Now;
        }

        public void ACache_OnItemAccessedTimeToLive(CacheEventArgs<TCacheItemKey, TCacheItem> e)
        {
            ClearCacheIfExpired();
            RemoveExpiredItemsByAccessedTime();
            RemoveExpiredItemsByCreatedTime();
            AccessedTime = DateTime.Now;
        }

        public void ACache_OnCreatedTimeToLive(CacheEventArgs<TCacheItemKey, TCacheItem> e)
        {
            ClearCacheIfExpired();
            RemoveExpiredItemsByAccessedTime();
            RemoveExpiredItemsByCreatedTime();
            AccessedTime = DateTime.Now;
        }

        #endregion

        /// <summary>
        /// Insert a given item into the Cache. This will overwrite existing entries if the item already exists.
        /// </summary>
        /// <param name="item"></param>
        public new void InsertCacheItem(TCacheItem item)
        {
            OnItemCreatedTimeToLive(new CacheEventArgs<TCacheItemKey, TCacheItem>(ref Cache, CacheName));
            OnItemAccessedTimeToLive(new CacheEventArgs<TCacheItemKey, TCacheItem>(ref Cache, CacheName));
            OnCreatedTimeToLive(new CacheEventArgs<TCacheItemKey, TCacheItem>(ref Cache, CacheName));
            OnAccessedTimeToLive(new CacheEventArgs<TCacheItemKey, TCacheItem>(ref Cache, CacheName));
            OnBeforeItemInserted(new CacheItemEventArgs<TCacheItemKey, TCacheItem>(ref item, ref Cache));
            OnItemInsert(new CacheItemEventArgs<TCacheItemKey, TCacheItem>(ref item, ref Cache));
            OnAfterItemInserted(new CacheItemEventArgs<TCacheItemKey, TCacheItem>(ref item, ref Cache));
        }

        /// <summary>
        /// The name of the Cache which is created at runtime.
        /// </summary>
        /// <returns>The string holding the Cache Name.</returns>
        private async Task<int> GetUtilizationAsync()
        {
            await OnAccessedTimeToLiveAsync(new CacheEventArgs<TCacheItemKey, TCacheItem>(ref Cache, CacheName));
            return Utilization;
        }

        public new int GetUtilization()
        {
            return GetUtilizationAsync().Result;
        }

        public async Task OnAccessedTimeToLiveAsync(CacheEventArgs<TCacheItemKey, TCacheItem> e)
        {
            ACache_OnAccessedTimeToLive(e);
        }

        /// <summary>
        /// The ability to set the amount of records that can be loaded into the Cache.
        /// </summary>
        /// <param name="capacity">The modified amount of records which can be loaded into the cache.</param>
        /// <returns></returns>
        public new bool SetCapacity(int capacity)
        {
            AccessedTime = DateTime.Now;

            OnItemCreatedTimeToLive(new CacheEventArgs<TCacheItemKey, TCacheItem>(ref Cache, CacheName));
            OnItemAccessedTimeToLive(new CacheEventArgs<TCacheItemKey, TCacheItem>(ref Cache, CacheName));
            OnCreatedTimeToLive(new CacheEventArgs<TCacheItemKey, TCacheItem>(ref Cache, CacheName));

            if (GetUtilization() > capacity) throw new CacheOverflowException(GetCacheName(), "Unable to set the utilization to a lower value than the capacity, please plan accordingly.");

            Capacity = capacity;
            return true;
        }

        /// <summary>
        /// The Amount of time the cache item is valid for after it has been created.
        /// </summary>
        /// <param name="timeToLive">The timespan the cache item is valid for (Default is unlimited with a Zero value).</param>
        public new void SetItemCreatedTimeToLive(TimeSpan timeToLive)
        {
            AccessedTime = DateTime.Now;

            if (timeToLive.Equals(TimeSpan.Zero))
            {
                OnItemCreatedTimeToLive -= ACache_OnItemCreatedTimeToLive;
            }
            else
            {
                OnItemCreatedTimeToLive += ACache_OnItemCreatedTimeToLive;
            }
            ItemCreatedTimeToLive = timeToLive;
        }

        /// <summary>
        /// The Amount of time the cache item is valid for after it has been accessed.
        /// </summary>
        /// <param name="timeToLive">The timespan the cache item is valid for (Default is unlimited with a Zero value).</param>
        public new void SetItemAccessedTimeToLive(TimeSpan timeToLive)
        {
            AccessedTime = DateTime.Now;

            if (timeToLive.Equals(TimeSpan.Zero))
            {
                OnItemAccessedTimeToLive -= ACache_OnItemAccessedTimeToLive;
            }
            else
            {
                OnItemAccessedTimeToLive += ACache_OnItemAccessedTimeToLive;
            }

            ItemAccessedTimeToLive = timeToLive;
        }

        /// <summary>
        /// Return a Cached item from the Generic Cache by Key if it exists.
        /// </summary>
        /// <param name="key">The Generic Key of the Cached Item that is to be removed.</param>
        /// <returns>The cache item by key if it exists, null if it does not.</returns>
        public new TCacheItem GetCachedItem(TCacheItemKey key)
        {
            AccessedTime = DateTime.Now;

            OnItemCreatedTimeToLive(new CacheEventArgs<TCacheItemKey, TCacheItem>(ref Cache, CacheName));
            OnItemAccessedTimeToLive(new CacheEventArgs<TCacheItemKey, TCacheItem>(ref Cache, CacheName));
            OnCreatedTimeToLive(new CacheEventArgs<TCacheItemKey, TCacheItem>(ref Cache, CacheName));
            OnAccessedTimeToLive(new CacheEventArgs<TCacheItemKey, TCacheItem>(ref Cache, CacheName));

            if (Cache.ContainsKey(key))
            {
                Cache[key].SetLastAccessedTime(DateTime.Now);
                return Cache.First(x => x.Key.Equals(key)).Value;
            }

            OnItemNotFound(new CacheItemKeyEventArgs<TCacheItemKey, TCacheItem>(key, ref Cache));
            return new TCacheItem();
        }

        /// <summary>
        /// The Amount of time the cache is valid for after it has been created.
        /// </summary>
        /// <param name="timeToLive">The timespan the cache is valid for (Default is unlimited with a Zero value).</param>
        public new void SetCreatedTimeToLive(TimeSpan timeToLive)
        {
            AccessedTime = DateTime.Now;

            if (timeToLive.Equals(TimeSpan.Zero))
            {
                OnCreatedTimeToLive -= ACache_OnCreatedTimeToLive;
            }
            else
            {
                OnCreatedTimeToLive += ACache_OnCreatedTimeToLive;
            }

            CreatedTimeToLive = timeToLive;
        }

        /// <summary>
        /// The Amount of time the cache is valid for after it has been accessed.
        /// </summary>
        /// <param name="timeToLive">The timespan the cache is valid for (Default is unlimited with a Zero value).</param>
        public new void SetAccessedTimeToLive(TimeSpan timeToLive)
        {
            AccessedTime = DateTime.Now;

            if (timeToLive.Equals(TimeSpan.Zero))
            {
                OnAccessedTimeToLive -= ACache_OnAccessedTimeToLive;
            }
            else
            {
                OnAccessedTimeToLive += ACache_OnAccessedTimeToLive;
            }

            AccessedTimeToLive = timeToLive;
        }

        /// <summary>
        /// Clear all items out of the Cache.
        /// </summary>
        public new void Clear()
        {
            AccessedTime = DateTime.Now;
            CreatedTime = DateTime.Now;

            OnCacheCleared(new CacheEventArgs<TCacheItemKey, TCacheItem>(ref Cache, CacheName));
            Cache = new Dictionary<TCacheItemKey, TCacheItem>();
            Utilization = 0;
        }
    }
}
