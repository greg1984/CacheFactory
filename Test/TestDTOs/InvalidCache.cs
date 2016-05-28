namespace CacheFactoryTest.TestDTOs
{
    using CacheFactory.Cachers.Base;
    using CacheFactory.CacheEventArgs;

    /// <summary>
    /// A Cache to cause exceptions within the test cases.
    /// </summary>
    /// <typeparam name="TCacheItem">A Generic type that enables the abstraction of a Cache Item.</typeparam>
    /// <typeparam name="TCacheItemKey">A Generic type that enables the abstraction of a Key to look up the Cached item.</typeparam>
    class InvalidCache<TCacheItem, TCacheItemKey> : ACache<TCacheItem, TCacheItemKey>
        where TCacheItemKey : CacheItemKey
        where TCacheItem : CacheItem<TCacheItemKey>
    {
        public InvalidCache()
        {
            throw new System.NotImplementedException();
        }  

        protected override void OnCacheOverflow(OverflowEventArgs<TCacheItem, TCacheItemKey> e)
        {
            throw new System.NotImplementedException();
        }
    }
}
