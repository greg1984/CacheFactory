namespace CacheFactoryTest.TestDTOs
{
    using System.Collections.Generic;
    using CacheFactory.Cachers.Base;

    internal class GenuineCacheContract : ACache<GenuineKey, GenuineCacheItem>
    {
        public GenuineCacheContract() : base(null, 0)
        {
        }

        protected GenuineCacheContract(IEnumerable<KeyValuePair<GenuineKey, GenuineCacheItem>> cache = null, int capacity = 0) : base(cache, capacity)
        {
        }
    }
}
