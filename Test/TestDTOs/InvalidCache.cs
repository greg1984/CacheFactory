namespace CacheFactoryTest.TestDTOs
{
    using CacheFactory.Cachers.Base;

    /// <summary>
    /// A Cache to cause exceptions within the test cases.
    /// </summary>
    internal class InvalidCache : ACacheWithEvents<InvalidCacheItemKey, InvalidCacheItem>
    {
        public InvalidCache() : base(null, 0) { }
    }
}
