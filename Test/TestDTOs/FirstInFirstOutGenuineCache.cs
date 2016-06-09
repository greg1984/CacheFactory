namespace CacheFactoryTest.TestDTOs
{
    using CacheFactory.Cachers;

    /// <summary>
    /// Enables the FIFO Cache to be tested.
    /// </summary>
    internal class FirstInFirstOutGenuineCache : FirstInFirstOutCache<GenuineKey, GenuineCacheItem>
    {
        public FirstInFirstOutGenuineCache() : base(null, 50) { }
    }
}
