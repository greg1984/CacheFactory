namespace CacheFactoryTest.TestDTOs
{
    using CacheFactory.Cachers;

    /// <summary>
    /// Enables the FILO Cache to be tested.
    /// </summary>
    internal class FirstInLastOutGenuineCache : FirstInLastOutCache<GenuineKey, GenuineCacheItem>
    {
        public FirstInLastOutGenuineCache() : base(null, 50) { }
    }
}
