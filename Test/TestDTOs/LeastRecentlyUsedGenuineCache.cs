namespace CacheFactoryTest.TestDTOs
{
    using CacheFactory.Cachers;

    /// <summary>
    /// A Cache object to verify the functionality of the LRU Cache.
    /// </summary>
    internal class LeastRecentlyUsedGenuineCache : LeastRecentlyUsedCache<GenuineKey, GenuineCacheItem>
    {
        public LeastRecentlyUsedGenuineCache() : base(null, 50) { }
    }
}
