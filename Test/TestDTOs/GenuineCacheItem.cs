namespace CacheFactoryTest.TestDTOs
{
    using System;
    using CacheFactory.Cachers.Base;

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

        public new void SetKey(GenuineKey key)
        {
            base.SetKey(key);
        }
    }
}
