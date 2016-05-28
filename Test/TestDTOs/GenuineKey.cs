namespace CacheFactoryTest.TestDTOs
{
    using CacheFactory.Cachers.Base;
    using System;

    public class GenuineKey : CacheItemKey
    {
        public readonly Guid Key = Guid.NewGuid();
    }
}
