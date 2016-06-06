namespace CacheFactoryTest.TestDTOs
{
    using CacheFactory.Cachers.Base;
    using System;

    internal class GenuineKey : ACacheItemKey
    {
        public readonly Guid Key = Guid.NewGuid();
    }
}
