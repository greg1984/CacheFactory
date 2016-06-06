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

        private TCacheItemKey _key;
        private DateTime _lastAccessedTime;
        private DateTime _createdTime;

        /// <summary>
        /// The DateTime value of the last time the Cache Item was accessed.
        /// </summary>
        public DateTime GetLastAccessedTime()
        {
            return _lastAccessedTime;
        }

        /// <summary>
        /// The Inserted DateTime value of when the item was injected into the Cache.
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