using CacheFactory.Cachers;

namespace CacheFactoryTest.TestDTOs
{
    internal class FirstInFirstOutGenuineCache : FirstInFirstOutCache<GenuineKey, GenuineCacheItem>
    {
        public FirstInFirstOutGenuineCache() : base(null, 50)
        {
            
        }
    }
}
