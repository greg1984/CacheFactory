namespace CacheFactoryTest.TestDTOs
{
    using CacheFactory.Cachers;

    internal class FirstInLastOutInvalidCache : FirstInLastOutCache<InvalidCacheItemKey, InvalidCacheItem>
    {
        public FirstInLastOutInvalidCache() : base(null, 50)
        {

        }
    }
}
