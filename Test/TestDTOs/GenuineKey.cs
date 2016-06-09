namespace CacheFactoryTest.TestDTOs
{
    using CacheFactory.Cachers.Base;
    using System;

    /// <summary>
    /// A Genuine Cache Item Key test object
    /// </summary>
    internal class GenuineKey : ACacheItemKey
    {
        public readonly Guid Key = Guid.NewGuid();
    }
}
