namespace CacheFactoryTest.Cachers
{
    using CacheFactory.Cachers;
    using CacheFactory.Cachers.Base;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections.Generic;
    using TestDTOs;

    /// <summary>
    /// Tests for the First In First Out Cache.
    /// </summary>
    [TestClass]
    public class FirstInLastOutCacheTests
    {
        /// <summary>
        /// Test the Constructor for the First In Last Out Cache.
        /// </summary>
        [TestMethod]
        public void FILOConstructorTest()
        {
            try
            {
                new FirstInLastOutCache<CacheItem<CacheItemKey>, CacheItemKey>();
            }
            catch (Exception ex)
            {
                Assert.Fail("Exception thrown when generating the First In Last Out Cache.");
            }
        }

        /// <summary>
        /// Test the Constructor for the First In First Out Cache
        /// </summary>
        [TestMethod]
        public void FILOConstructorTest2()
        {
            var cache = new FirstInLastOutCache<CacheItem<CacheItemKey>, CacheItemKey>();
            if (cache == null) Assert.Fail("FIFO Cache resulted in null when being constructed.");
        }

        /// <summary>
        /// Ensure that the cache will only load to capacity.
        /// </summary>
        [TestMethod]
        public void OnCacheOverflowTest()
        {
            var cache = new FirstInLastOutCache<CacheItem<GenuineKey>, GenuineKey>(new Dictionary<GenuineKey, CacheItem<GenuineKey>>(), 1);
            cache.InsertCacheItem(new CacheItem<GenuineKey>(DateTime.Now.AddMinutes(2), new GenuineKey()));
            cache.InsertCacheItem(new CacheItem<GenuineKey>(DateTime.Now.AddMinutes(1), new GenuineKey()));
            cache.InsertCacheItem(new CacheItem<GenuineKey>(DateTime.Now, new GenuineKey()));
            Assert.AreEqual(cache.GetCapacity(), cache.GetUtilization());
        }

        /// <summary>
        /// Verify the eviction strategy.
        /// </summary>
        [TestMethod]
        public void OnCacheOverflowTest2()
        {
            var item1 = new CacheItem<GenuineKey>(DateTime.Now, new GenuineKey());
            item1.SetInsertTime(DateTime.Now);
            var item2 = new CacheItem<GenuineKey>(DateTime.Now.AddSeconds(1), new GenuineKey());
            item2.SetInsertTime(DateTime.Now.AddSeconds(1));
            var item3 = new CacheItem<GenuineKey>(DateTime.Now.AddSeconds(2), new GenuineKey());
            var cache = new FirstInLastOutCache<CacheItem<GenuineKey>, GenuineKey>(new Dictionary<GenuineKey, CacheItem<GenuineKey>>(), 1);
            item3.SetInsertTime(DateTime.Now.AddSeconds(2));
            cache.InsertCacheItem(item1);
            cache.InsertCacheItem(item2);
            cache.InsertCacheItem(item3);
            Assert.IsTrue(cache.GetCache().ContainsKey(item1.Key));
        }
    }
}
