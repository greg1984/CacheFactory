namespace CacheFactoryTest.TestDTOs
{
    using System.Collections.Generic;
    using CacheFactory.Cachers;

    internal class LeastRecentlyUsedGenuineCache : LeastRecentlyUsedCache<GenuineKey, GenuineCacheItem>
    {
        public LeastRecentlyUsedGenuineCache() : base(null, 50)
        {

        }
    }
}
