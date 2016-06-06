namespace CacheFactoryTest.TestDTOs
{
    using CacheFactory.Cachers.Base;
    using System;

    /// <summary>
    /// A Cache Item Key to cause exceptions within the test cases.
    /// </summary>
    public class InvalidCacheItemKey : ACacheItemKey
    {
        public InvalidCacheItemKey()
        {
            throw new ArgumentException("Invalid Cache Item Key.");
        }
    }
}
