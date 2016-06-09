namespace CacheFactoryTest.TestDTOs
{
    using System;
    using CacheFactory.Cachers.Base;

    /// <summary>
    /// Enables the Cache Item object to be tested
    /// </summary>
    internal class GenuineCacheItem : ACacheItem<GenuineKey>, ICacheItem<GenuineKey>
    {
        public GenuineCacheItem() : base(DateTime.Now, new GenuineKey())
        {
        }

        public GenuineCacheItem(DateTime accessTime, GenuineKey key) : base(accessTime, key)
        {
        }

        public new GenuineKey GetKey()
        {
            return base.GetKey();
        }
    }
}
