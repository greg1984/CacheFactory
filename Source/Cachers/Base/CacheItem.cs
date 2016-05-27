namespace CacheFactory.Cachers.Base
{
    using System;

    /// <summary>
    /// A Generic Cache Item definition.
    /// </summary>
    /// <typeparam name="TCacheItemKey">A Generic type that enables the abstraction of a Key to look up the Cached item.</typeparam>
    public class CacheItem<TCacheItemKey> where TCacheItemKey : CacheItemKey
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessTime"></param>
        /// <param name="key"></param>
        public CacheItem(DateTime accessTime, TCacheItemKey key)
        {
            _key = key;
            _lastAccessed = accessTime;
            _insertTime = DateTime.Now;
        }

        private TCacheItemKey _key;
        private DateTime _lastAccessed;
        private DateTime _insertTime;

        /// <summary>
        /// The DateTime value of the last time the Cache Item was accessed.
        /// </summary>
        public DateTime LastAccessed
        {
            get { return _lastAccessed; }
        }


        /// <summary>
        /// The Inserted DateTime value of when the item was injected into the Cache.
        /// </summary>
        public DateTime Inserted
        {
            get { return _insertTime; }
        }


        /// <summary>
        /// The Key to find the item in the Hash.
        /// </summary>
        public TCacheItemKey Key
        {
            get { return _key; }
        }


        /// <summary>
        /// Enables the ability to set the inserted time of the item within the Cache.
        /// </summary>
        public void SetInsertTime(DateTime insertTime)
        {
            _insertTime = insertTime;
        }


        /// <summary>
        /// Enables the ability to set the Key within the item to look it up within the cache.
        /// </summary>
        public TCacheItemKey SetKey
        {
            get { return _key; }
        }


        /// <summary>
        /// Enables the ability to set the time that the item has last been accessed.
        /// </summary>
        public void SetLastAccessed(DateTime accessTime)
        {
            _lastAccessed = accessTime;
        }
    }
}