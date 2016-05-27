namespace CacheFactory.Cachers.Base
{
    using System;

    public class CacheItem<TCacheItemKey> where TCacheItemKey : CacheItemKey
    {
        public CacheItem(DateTime accessTime, TCacheItemKey key)
        {
            _key = key;
            _lastAccessed = accessTime;
            _insertTime = DateTime.Now;
        }

        private TCacheItemKey _key;

        private DateTime _lastAccessed;
        private DateTime _insertTime;


        public DateTime LastAccessed
        {
            get { return _lastAccessed; }
        }

        public DateTime Inserted
        {
            get { return _insertTime; }
        }

        public TCacheItemKey Key
        {
            get { return _key; }
        }

        public void SetInsertTime(DateTime insertTime)
        {
            _insertTime = insertTime;
        }

        public TCacheItemKey SetKey
        {
            get { return _key; }
        }

        public void SetLastAccessed(DateTime accessTime)
        {
            _lastAccessed = accessTime;
        }
    }
}