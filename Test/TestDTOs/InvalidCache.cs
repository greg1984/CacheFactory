namespace CacheFactoryTest.TestDTOs
{
    using CacheFactory.Cachers.Base;
    using CacheFactory.CacheEventArgs;

    /// <summary>
    /// A Cache to cause exceptions within the test cases.
    /// </summary>
    internal class InvalidCache : ACache<InvalidCacheItemKey, InvalidCacheItem>
    {
        public InvalidCache() : base(null, 0)
        {
        }
    }
}
