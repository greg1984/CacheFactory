namespace CacheFactory.Cachers.Base
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Interface of basic Cache functionality.
    /// </summary>
    /// <typeparam name="TCacheItemKey"></typeparam>
    /// <typeparam name="TCacheItem">A Generic type that enables the abstraction of a Cache Item.</typeparam>
    public interface ICache<TCacheItemKey, TCacheItem>
        where TCacheItem : ICacheItem<TCacheItemKey>
        where TCacheItemKey : ICacheItemKey
    {

        /// <summary>
        /// Generic Cache held by the Abstracted Cache.
        /// </summary>
        Dictionary<TCacheItemKey, TCacheItem> GetCache();

        /// <summary>
        /// The name of the Cache which is created at runtime.
        /// </summary>
        /// <returns>The string holding the Cache Name.</returns>
        string GetCacheName();

        /// <summary>
        /// Get the amount of records the cache can hold. Does cache expiry checks and sets the access time.
        /// </summary>
        /// <returns>The string holding the Cache Name.</returns>
        int GetCapacity();

        /// <summary>
        /// Get the utilization of the cache, does expiry checks to ensure that the utilization return value is accurate.
        /// </summary>
        /// <returns>The amount of records that are held within the cache.</returns>
        int GetUtilization();

        /// <summary>
        /// The Amount of time the cache is valid for after creation.
        /// </summary>
        /// <returns>The timespan the cache item is valid for (Default is unlimited with a Zero value).</returns>
        TimeSpan GetCreatedTimeToLive();

        /// <summary>
        /// The Amount of time the cache is valid for after access.
        /// </summary>
        /// <returns>The timespan the cache item is valid for (Default is unlimited with a Zero value).</returns>
        TimeSpan GetAccessedTimeToLive();

        /// <summary>
        /// The Amount of time cache items are valid for.
        /// </summary>
        /// <returns>The timespan the cache item is valid for (Default is unlimited with a Zero value).</returns>
        TimeSpan GetItemCreatedTimeToLive();

        /// <summary>
        /// The Amount of time the cache item is valid for.
        /// </summary>
        /// <returns>The timespan the cache item is valid for (Default is unlimited with a Zero value).</returns>
        TimeSpan GetItemAccessedTimeToLive();

        /// <summary>
        /// Return a Cached item from the Generic Cache by Key if it exists.
        /// </summary>
        /// <param name="key">The Generic Key of the Cached Item that is to be removed.</param>
        /// <returns>The cache item by key if it exists, null if it does not.</returns>
        TCacheItem GetCachedItem(TCacheItemKey key);

        /// <summary>
        /// Remove cache items which have expired against the TTL of the created time if it has not been set to 0.
        /// </summary>
        /// <returns>The items which have expired in the cache.</returns>
        IEnumerable<TCacheItem> GetExpiredItemsByCreatedTime();
        
        /// <summary>
        /// Gets the Identifier of the Cache instance.
        /// </summary>
        /// <returns>A GUID identifying the Cache instance.</returns>
        Guid GetCacheID();
        
        /// <summary>
        /// Get cache items which have expired against the TTL of the accessed time if it has not been set to 0.
        /// </summary>
        /// <returns>The items which have expired in the cache.</returns>
        IEnumerable<TCacheItem> GetExpiredItemsByAccessedTime();

        /// <summary>
        /// Manually set the cache values.
        /// </summary>
        /// <param name="cache">The Generic Cache which is to be loaded into the Abstracted Cache</param>
        /// <param name="resize"></param>
        void SetCache(Dictionary<TCacheItemKey, TCacheItem> cache, bool resize = false);

        /// <summary>
        /// Enables the ability to change the Cache Name while verifying if a cache of that type with that name already exists.
        /// </summary>
        /// <param name="cacheName">A string containing the new Cache name.</param>
        void SetCacheName(string cacheName);

        /// <summary>
        /// The ability to set the amount of records that can be loaded into the Cache.
        /// </summary>
        /// <param name="capacity">The modified amount of records which can be loaded into the cache.</param>
        /// <returns></returns>
        bool SetCapacity(int capacity);

        /// <summary>
        /// The Amount of time the cache item is valid for after it has been created.
        /// </summary>
        /// <param name="timeToLive">The timespan the cache item is valid for (Default is unlimited with a Zero value).</param>
        void SetItemCreatedTimeToLive(TimeSpan timeToLive);

        /// <summary>
        /// The Amount of time the cache item is valid for after it has been accessed.
        /// </summary>
        /// <param name="timeToLive">The timespan the cache item is valid for (Default is unlimited with a Zero value).</param>
        void SetItemAccessedTimeToLive(TimeSpan timeToLive);

        /// <summary>
        /// The Amount of time the cache is valid for after it has been created.
        /// </summary>
        /// <param name="timeToLive">The timespan the cache is valid for (Default is unlimited with a Zero value).</param>
        void SetCreatedTimeToLive(TimeSpan timeToLive);

        /// <summary>
        /// Set the identification Guid for loading / saving as well as identification of the cache within reflection.
        /// </summary>
        /// <param name="cacheID">A Guid identifying the cache.</param>
        void SetCacheID(Guid cacheID);

        /// <summary>
        /// The Amount of time the cache is valid for after it has been accessed.
        /// </summary>
        /// <param name="timeToLive">The timespan the cache is valid for (Default is unlimited with a Zero value).</param>
        void SetAccessedTimeToLive(TimeSpan timeToLive);

        /// <summary>
        /// Insert a given item into the Cache. This will overwrite existing entries if the item already exists.
        /// </summary>
        /// <param name="item"></param>
        void InsertCacheItem(TCacheItem item);

        /// <summary>
        /// Remove cache items which have expired against the TTL of the creation time if it has not been set to 0.
        /// </summary>
        void RemoveExpiredItemsByCreatedTime();

        /// <summary>
        /// Remove cache items which have expired against the TTL of the accessed time if it has not been set to 0.
        /// </summary>
        void RemoveExpiredItemsByAccessedTime();

        /// <summary>
        /// Remove a Cache Item from the Generic Cache by key
        /// </summary>
        /// <param name="key">The key which the Cache Item should be removed from the Hash by.</param>
        void RemoveCacheItem(TCacheItemKey key);

        /// <summary>
        /// Clear the entire cache if the access or creation dates have expired.
        /// </summary>
        void ClearCacheIfExpired();

        /// <summary>
        /// Clear all items out of the Cache.
        /// </summary>
        void Clear();
    }
}
