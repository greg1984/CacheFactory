namespace CacheFactoryTest.TestDTOs
{
    using CacheFactory.Cachers.Base;
    using System;

    /// <summary>
    /// A Cache to cause exceptions within the test cases.
    /// </summary>
    public class InvalidCacheItem : ACacheItem<InvalidCacheItemKey>
    {
        public InvalidCacheItem(DateTime accessTime, InvalidCacheItemKey key) : base(accessTime, key)
        {
        }

        public InvalidCacheItem() : base(DateTime.Now, new InvalidCacheItemKey())
        {
        }
    }
}
