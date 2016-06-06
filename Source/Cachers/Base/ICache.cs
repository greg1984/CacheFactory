using System;

namespace CacheFactory.Cachers.Base
{
    using System.Collections.Generic;

    /// <summary>
    /// Abstraction of basic Cache functionality. Can be extended and resulting classes consumed by the factory.
    /// </summary>
    /// <typeparam name="TCacheItem">A Generic type that enables the abstraction of a Cache Item.</typeparam>
    /// <typeparam name="TCacheItemKey"></typeparam>
    public interface ICache<TCacheItemKey, TCacheItem>
        where TCacheItem : ICacheItem<TCacheItemKey>
        where TCacheItemKey : ICacheItemKey
    {
        /// <summary>
        /// Generic Cache held by the Cache Interface.
        /// </summary>
        Dictionary<TCacheItemKey, TCacheItem> GetCache();

        /// <summary>
        /// Manually set the cache values.
        /// </summary>
        /// <param name="cache">The Generic Cache which is to be loaded into the Abstracted Cache</param>
        /// <param name="resize"></param>
        void SetCache(IEnumerable<KeyValuePair<TCacheItemKey, TCacheItem>> cache, bool resize = false);

        /// <summary>
        /// Remove a Cache Item from the Generic Cache by key
        /// </summary>
        /// <param name="key">The key which the Cache Item should be removed from the Hash by.</param>
        void RemoveCacheItem(TCacheItemKey key);

        /// <summary>
        /// The name of the Cache which is created at runtime.
        /// </summary>
        /// <returns>The string holding the Cache Name.</returns>
        string GetCacheName();

        /// <summary>
        /// The name of the Cache which is created at runtime.
        /// </summary>
        /// <returns>The string holding the Cache Name.</returns>
        int GetCapacity();

        /// <summary>
        /// The name of the Cache which is created at runtime.
        /// </summary>
        /// <returns>The string holding the Cache Name.</returns>
        int GetUtilization();

        /// <summary>
        /// The Amount of time the cache is valid for.
        /// </summary>
        /// <returns>The timespan the cache item is valid for (Default is unlimited with a Zero value).</returns>
        TimeSpan GetCreatedTimeToLive();

        /// <summary>
        /// The Amount of time the cache is valid for.
        /// </summary>
        /// <returns>The timespan the cache item is valid for (Default is unlimited with a Zero value).</returns>
        TimeSpan GetAccessedTimeToLive();

        /// <summary>
        /// The Amount of time the cache item is valid for.
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
        /// Enables the ability to change the Cache Name while verifying if a cache of that type with that name already exists.
        /// </summary>
        /// <param name="cacheName">A string containing the new Cache name.</param>
        /// <param name="existingCaches">Existing caches to check if the cache names already exists.</param>
        void SetCacheName<TCache>(string cacheName, IEnumerable<TCache> existingCaches) where TCache : ICache<TCacheItemKey, TCacheItem>;

        /// <summary>
        /// The ability to set the amount of records that can be loaded into the Cache.
        /// </summary>
        /// <param name="capacity">The modified amount of records which can be loaded into the cache.</param>
        /// <returns></returns>
        bool SetCapacity(int capacity);

        /// <summary>
        /// The Amount of time the cache is valid for after created.
        /// </summary>
        /// <param name="timeToLive">The timespan the cache is valid for (Default is unlimited with a Zero value).</param>
        void SetCreatedTimeToLive(TimeSpan timeToLive);

        /// <summary>
        /// The Amount of time the cache is valid for after access.
        /// </summary>
        /// <param name="timeToLive">The timespan the cache is valid for (Default is unlimited with a Zero value).</param>
        void SetAccessedTimeToLive(TimeSpan timeToLive);

        /// <summary>
        /// The Amount of time the cache item is valid for after created.
        /// </summary>
        /// <param name="timeToLive">The timespan the cache is valid for (Default is unlimited with a Zero value).</param>
        void SetItemCreatedTimeToLive(TimeSpan timeToLive);

        /// <summary>
        /// The Amount of time the cache item is valid for after access.
        /// </summary>
        /// <param name="timeToLive">The timespan the cache is valid for (Default is unlimited with a Zero value).</param>
        void SetItemAccessedTimeToLive(TimeSpan timeToLive);

        /// <summary>
        /// Insert a given item into the Cache. This will overwrite existing entries if the item already exists.
        /// </summary>
        /// <param name="item"></param>
        void InsertCacheItem(TCacheItem item);

        /// <summary>
        /// Clear all items out of the Cache.
        /// </summary>
        void Clear();
    }
}
