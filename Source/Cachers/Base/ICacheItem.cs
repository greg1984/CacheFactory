namespace CacheFactory.Cachers.Base
{
    using System;

    /// <summary>
    /// The Type of item you want to cache.
    /// </summary>
    /// <typeparam name="TCacheItemKey"></typeparam>
    public interface ICacheItem<TCacheItemKey> where TCacheItemKey : ICacheItemKey
    {
        /// <summary>
        /// The DateTime value of the last time the Cache Item was accessed.
        /// </summary>
        DateTime GetLastAccessedTime();

        /// <summary>
        /// The Inserted DateTime value of when the item was injected into the Cache.
        /// </summary>
        DateTime GetCreatedTime();

        /// <summary>
        /// Enables the ability to set the Key within the item to look it up within the cache.
        /// </summary>
        TCacheItemKey GetKey();

        /// <summary>
        /// The Key to find the item in the Hash.
        /// </summary>
        void SetKey(TCacheItemKey key);

        /// <summary>
        /// Enables the ability to set the inserted time of the item within the Cache.
        /// </summary>
        /// <param name="createdTime">The time the entry was created.</param>
        void SetCreatedTime(DateTime createdTime);

        /// <summary>
        /// Enables the ability to set the time that the item has last been accessed.
        /// </summary>
        void SetLastAccessedTime(DateTime accessTime);
    }
}
