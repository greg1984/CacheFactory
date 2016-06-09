namespace CacheFactoryTest.TestDTOs
{
    /// <summary>
    /// Enables the ACache object to be tested when utilizing the ACache with no events.
    /// </summary>
    internal class FirstInFirstOutGenuineCacheNoEvents : FirstInFirstOutCacheNoEvents<GenuineKey, GenuineCacheItem>
    {
        public FirstInFirstOutGenuineCacheNoEvents() : base(null, 50) { }
    }
}
