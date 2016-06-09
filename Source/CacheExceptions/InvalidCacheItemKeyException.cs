namespace CacheFactory.CacheExceptions
{
    using Cachers.Base;
    using System;

    public class InvalidCacheItemKeyException<TCacheItemKey> : Exception
        where TCacheItemKey : ICacheItemKey
    {
        public InvalidCacheItemKeyException(TCacheItemKey cacheItemKey) : base("Cache item with key already exists " + cacheItemKey) { }
    }
}
