namespace CacheFactoryTest.TestDTOs
{
    using CacheFactory.Cachers;

    internal class FirstInFirstOutInvalidCache : FirstInFirstOutCache<InvalidCacheItemKey, InvalidCacheItem>
    {
        public FirstInFirstOutInvalidCache() : base(null, 50)
        {

        }
    }
}
