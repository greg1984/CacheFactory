namespace CacheFactoryTest.TestDTOs
{
    using System.Collections.Generic;
    using CacheFactory.Cachers.Base;

    /// <summary>
    /// FIFO Cache with no events attached.
    /// </summary>
    /// <typeparam name="TCacheItemKey"></typeparam>
    /// <typeparam name="TCacheItem">A Generic type that enables the abstraction of a Cache Item.</typeparam>
    public abstract class FirstInFirstOutCacheNoEvents<TCacheItemKey, TCacheItem> : ACache<TCacheItemKey, TCacheItem>
        where TCacheItemKey : ICacheItemKey
        where TCacheItem : ICacheItem<TCacheItemKey>, new()
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cache">Initial value of the cache.</param>
        /// <param name="capacity">The maximum amount of records that can be held in the cache.</param>
        protected FirstInFirstOutCacheNoEvents(Dictionary<TCacheItemKey, TCacheItem> cache = null, int capacity = 50)
            : base(cache, capacity)
        {
        }
    }
}