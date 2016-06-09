namespace CacheFactory.Cachers.Base
{
    using System;

    /// <summary>
    /// A Generic Cache Item definition.
    /// </summary>
    /// <typeparam name="TCacheItemKey">A Generic type that enables the abstraction of a Key to look up the Cached item.</typeparam>
    public abstract class ACacheItem<TCacheItemKey> : ICacheItem<TCacheItemKey>
        where TCacheItemKey : ICacheItemKey
    {
        /// <summary>
        /// The Default constructor to make a cache item.
        /// </summary>
        /// <param name="accessTime"></param>
        /// <param name="key"></param>
        public ACacheItem(DateTime accessTime, TCacheItemKey key)
        {
            SetKey(key);
            SetLastAccessedTime(accessTime);
            SetCreatedTime(DateTime.Now);
        }

        /// <summary>
        /// The Key which will identify the Cached item in the Cache.
        /// </summary>
        private TCacheItemKey _key;

        /// <summary>
        /// THe Last time the cache was accessed.
        /// </summary>
        private DateTime _lastAccessedTime;

        /// <summary>
        /// The time the cache was created and / or cleared.
        /// </summary>
        private DateTime _createdTime;

        /// <summary>
        /// The DateTime value of the last time the Cache item was accessed.
        /// </summary>
        public DateTime GetLastAccessedTime()
        {
            return _lastAccessedTime;
        }

        /// <summary>
        /// The Inserted DateTime value of when the Cache item was created.
        /// </summary>
        public DateTime GetCreatedTime()
        {
            return _createdTime;
        }

        /// <summary>
        /// Enables the ability to set the Key within the item to look it up within the cache.
        /// </summary>
        public TCacheItemKey GetKey()
        {
            return _key;
        }

        /// <summary>
        /// The Key to find the item in the Hash.
        /// </summary>
        public void SetKey(TCacheItemKey key)
        {
            _key = key;
        }

        /// <summary>
        /// Enables the ability to set the inserted time of the item within the Cache.
        /// </summary>
        /// <param name="createdTime">The time the entry was created.</param>
        public void SetCreatedTime(DateTime createdTime)
        {
            _createdTime = createdTime;
        }

        /// <summary>
        /// Enables the ability to set the time that the item has last been accessed.
        /// </summary>
        public void SetLastAccessedTime(DateTime accessTime)
        {
            _lastAccessedTime = accessTime;
        }
    }
}