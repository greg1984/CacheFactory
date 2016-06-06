namespace CacheFactoryTest.TestDTOs
{
    using CacheFactory.Cachers;

    public class LeastRecentlyUsedInvalidCache : LeastRecentlyUsedCache<InvalidCacheItemKey, InvalidCacheItem>
    {
        public LeastRecentlyUsedInvalidCache() : base(null, 50)
        {

        }
    }
}
