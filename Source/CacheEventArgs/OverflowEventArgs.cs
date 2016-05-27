namespace CacheFactory.CacheEventArgs
{
    using Cachers.Base;

    using System;

    public class OverflowEventArgs<TCacheItem, TCacheItemKey> : EventArgs
        where TCacheItemKey : CacheItemKey
        where TCacheItem : CacheItem<TCacheItemKey>
    {
        public OverflowEventArgs(TCacheItem item)
        {
            _cachedItem = item;
        }

        private TCacheItem _cachedItem;

        public void SetCacheItem(TCacheItem item)
        {
            _cachedItem = item;
        }

        public TCacheItem GetCacheItem()
        {
            return _cachedItem;
        }
    }
}
