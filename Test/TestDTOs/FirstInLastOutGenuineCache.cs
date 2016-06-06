using System.Collections.Generic;
using CacheFactory.Cachers;

namespace CacheFactoryTest.TestDTOs
{
    internal class FirstInLastOutGenuineCache : FirstInLastOutCache<GenuineKey, GenuineCacheItem>
    {
        public FirstInLastOutGenuineCache() : base(null, 50)
        {

        }
    }
}
