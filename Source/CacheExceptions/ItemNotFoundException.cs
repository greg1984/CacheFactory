namespace CacheFactory.CacheExceptions
{
    using Cachers.Base;
    using System;

    public class ItemNotFoundException : Exception
    {
        public ItemNotFoundException(ICacheItemKey cacheItemKey) : base("Could not find cache item " + cacheItemKey, new ArgumentException("Item not found in cache.")) { }
    }
}
