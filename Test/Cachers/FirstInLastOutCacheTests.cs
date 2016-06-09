namespace CacheFactoryTest.Cachers
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using TestDTOs;

    /// <summary>
    /// Tests for the First In First Out Cache.
    /// </summary>
    [TestClass]
    public class FirstInLastOutCacheTests
    {
        /// <summary>
        /// Test the Constructor for the First In First Out Cache
        /// </summary>
        [TestMethod]
        public void FILOConstructorTest()
        {
            var cache = new FirstInLastOutGenuineCache();
            if (cache == null) Assert.Fail("FIFO Cache resulted in null when being constructed.");
        }

        /// <summary>
        /// Ensure that the cache will only load to capacity.
        /// </summary>
        [TestMethod]
        public void OnCacheOverflowCheckUtilizationTest()
        {
            var cache = new FirstInLastOutGenuineCache();
            cache.SetCapacity(1);
            cache.InsertCacheItem(new GenuineCacheItem(DateTime.Now.AddMinutes(2), new GenuineKey()));
            cache.InsertCacheItem(new GenuineCacheItem(DateTime.Now.AddMinutes(1), new GenuineKey()));
            cache.InsertCacheItem(new GenuineCacheItem(DateTime.Now, new GenuineKey()));
            Assert.AreEqual(cache.GetCapacity(), cache.GetUtilization());
        }

        /// <summary>
        /// Verify the eviction strategy.
        /// </summary>
        [TestMethod]
        public void OnCacheOverflowCheckItemTest()
        {
            var item1 = new GenuineCacheItem(DateTime.Now, new GenuineKey());
            item1.SetCreatedTime(DateTime.Now);
            var item2 = new GenuineCacheItem(DateTime.Now.AddSeconds(1), new GenuineKey());
            item2.SetCreatedTime(DateTime.Now.AddSeconds(1));
            var item3 = new GenuineCacheItem(DateTime.Now.AddSeconds(2), new GenuineKey());
            var cache = new FirstInLastOutGenuineCache();
            cache.SetCapacity(1);
            item3.SetCreatedTime(DateTime.Now.AddSeconds(2));
            cache.InsertCacheItem(item1);
            cache.InsertCacheItem(item2);
            cache.InsertCacheItem(item3);
            Assert.IsTrue(cache.GetCache().ContainsKey(item1.GetKey()));
        }

        /// <summary>
        /// Verification that the cache only loads to capacity with the First In Last Out Eviction strategy.
        /// </summary>
        [TestMethod]
        public void OnCacheOverflowTTLTest()
        {
            var cache = new FirstInLastOutGenuineCache();
            cache.SetItemAccessedTimeToLive(TimeSpan.MaxValue);
            cache.SetCapacity(1);

            cache.InsertCacheItem(new GenuineCacheItem(DateTime.Now.AddMinutes(2), new GenuineKey()));
            cache.InsertCacheItem(new GenuineCacheItem(DateTime.Now.AddMinutes(1), new GenuineKey()));
            cache.InsertCacheItem(new GenuineCacheItem(DateTime.Now, new GenuineKey()));
            Assert.AreEqual(cache.GetCapacity(), cache.GetUtilization());
        }
        
        /// <summary>
        /// Verification that a Timespan of 0 prevents the Time Based eviction from occuring when using the First In Last Out Eviction policy.
        /// </summary>
        [TestMethod]
        public void OnCacheOverflowCreatedTTLZeroTimespanTest()
        {
            var cache = new FirstInLastOutGenuineCache();
            cache.SetItemCreatedTimeToLive(TimeSpan.Zero);
            cache.SetCapacity(2);

            var item1 = new GenuineCacheItem(DateTime.Now.AddSeconds(-1), new GenuineKey());
            item1.SetCreatedTime(DateTime.Now.AddSeconds(-1));
            var item2 = new GenuineCacheItem(DateTime.Now.AddSeconds(-3), new GenuineKey());
            item2.SetCreatedTime(DateTime.Now.AddSeconds(-3));
            var item3 = new GenuineCacheItem(DateTime.Now.AddSeconds(2), new GenuineKey());
            item3.SetCreatedTime(DateTime.Now.AddSeconds(2));
            cache.InsertCacheItem(item1);
            cache.InsertCacheItem(item2);
            item1.SetCreatedTime(DateTime.Now.AddSeconds(-1));
            item1.SetCreatedTime(DateTime.Now.AddSeconds(-1));
            cache.InsertCacheItem(item3);
            Assert.IsTrue(cache.GetCache().ContainsKey(item1.GetKey()));
        }

        /// <summary>
        /// Verification that a Timespan of 0 prevents the Time Based eviction from occuring when using the First In Last Out Eviction policy.
        /// </summary>
        [TestMethod]
        public void OnCacheOverflowAccessedTTLTest()
        {
            var cache = new FirstInLastOutGenuineCache();
            cache.SetItemAccessedTimeToLive(TimeSpan.Zero);
            cache.SetCapacity(2);

            var item1 = new GenuineCacheItem(DateTime.Now.AddSeconds(-1), new GenuineKey());
            item1.SetCreatedTime(DateTime.Now.AddSeconds(-1));
            var item2 = new GenuineCacheItem(DateTime.Now.AddSeconds(-3), new GenuineKey());
            item2.SetCreatedTime(DateTime.Now.AddSeconds(-3));
            var item3 = new GenuineCacheItem(DateTime.Now.AddSeconds(2), new GenuineKey());
            item3.SetCreatedTime(DateTime.Now.AddSeconds(2));
            cache.InsertCacheItem(item1);
            cache.InsertCacheItem(item2);
            item1.SetCreatedTime(DateTime.Now.AddSeconds(-1));
            item1.SetCreatedTime(DateTime.Now.AddSeconds(-1));
            cache.InsertCacheItem(item3);
            Assert.IsTrue(cache.GetCache().ContainsKey(item1.GetKey()));
        }

        /// <summary>
        /// Verification that the First In Last Out Cache Item item gets evicted first (Time set manually).
        /// </summary>
        [TestMethod]
        public void OnCacheOverflowCreatedTTLMaxTimespanTest()
        {
            var cache = new FirstInLastOutGenuineCache();
            cache.SetItemCreatedTimeToLive(TimeSpan.MaxValue);
            cache.SetCapacity(2);
            
            var item1 = new GenuineCacheItem(DateTime.Now, new GenuineKey());
            item1.SetCreatedTime(DateTime.Now);
            var item2 = new GenuineCacheItem(DateTime.Now.AddSeconds(1), new GenuineKey());
            item2.SetCreatedTime(DateTime.Now.AddSeconds(1));
            var item3 = new GenuineCacheItem(DateTime.Now.AddSeconds(2), new GenuineKey());
            item3.SetCreatedTime(DateTime.Now.AddSeconds(2));
            cache.InsertCacheItem(item1);
            cache.InsertCacheItem(item2);
            cache.InsertCacheItem(item3);
            Assert.IsTrue(cache.GetCache().ContainsKey(item1.GetKey()));
        }

        /// <summary>
        /// Verification that the First In Last Out Cache Item item gets evicted first (Time set manually).
        /// </summary>
        [TestMethod]
        public void OnCacheOverflowAccessedTTLMaxTimespanTest()
        {
            var cache = new FirstInLastOutGenuineCache();
            cache.SetItemAccessedTimeToLive(TimeSpan.MaxValue);
            cache.SetCapacity(1);

            var item1 = new GenuineCacheItem(DateTime.Now, new GenuineKey());
            item1.SetCreatedTime(DateTime.Now);
            var item2 = new GenuineCacheItem(DateTime.Now.AddSeconds(1), new GenuineKey());
            item2.SetCreatedTime(DateTime.Now.AddSeconds(1));
            var item3 = new GenuineCacheItem(DateTime.Now.AddSeconds(2), new GenuineKey());
            item3.SetCreatedTime(DateTime.Now.AddSeconds(2));
            cache.InsertCacheItem(item1);
            cache.InsertCacheItem(item2);
            cache.InsertCacheItem(item3);
            Assert.IsTrue(cache.GetCache().ContainsKey(item1.GetKey()));
        }

        /// <summary>
        /// Test the Constructor for the Time Based Eviction FILO Cache.
        /// </summary>
        [TestMethod]
        public void CreatedCreatedTimeBasedEvictionFILOConstrutorTest()
        {
            var timespanCache = new FirstInLastOutGenuineCache();
            timespanCache.SetItemCreatedTimeToLive(TimeSpan.FromDays(1));
            Assert.AreEqual(TimeSpan.FromDays(1), timespanCache.GetItemCreatedTimeToLive());
        }

        /// <summary>
        /// Test the Constructor for the Time Based Eviction FILO Cache.
        /// </summary>
        [TestMethod]
        public void CreatedAccessedTimeBasedEvictionFILOConstrutorTest()
        {
            var timespanCache = new FirstInLastOutGenuineCache();
            timespanCache.SetItemAccessedTimeToLive(TimeSpan.FromDays(1));
            Assert.AreEqual(TimeSpan.FromDays(1), timespanCache.GetItemAccessedTimeToLive());
        }

        /// <summary>
        /// Verification that stale objects get removed from the cache.
        /// </summary>
        [TestMethod]
        public void RemoveCreatedExpiredCacheItemsTest()
        {
            var cache = new FirstInLastOutGenuineCache();
            cache.SetItemCreatedTimeToLive(TimeSpan.FromSeconds(10));
            cache.SetCapacity(1);
            
            var item1 = new GenuineCacheItem(DateTime.Now, new GenuineKey());
            var item2 = new GenuineCacheItem(DateTime.Now, new GenuineKey());
            var item3 = new GenuineCacheItem(DateTime.Now, new GenuineKey());
            cache.InsertCacheItem(item1);
            cache.InsertCacheItem(item2);
            cache.InsertCacheItem(item3);
            cache.GetCachedItem(item1.GetKey()).SetCreatedTime(DateTime.Now.AddDays(-5));
            cache.GetCachedItem(item2.GetKey()).SetCreatedTime(DateTime.Now.AddDays(-5));
            cache.GetCachedItem(item3.GetKey()).SetCreatedTime(DateTime.Now.AddDays(-5));
            Assert.IsTrue(cache.GetUtilization() == 0);
        }

        /// <summary>
        /// Verification that stale objects get removed from the cache.
        /// </summary>
        [TestMethod]
        public void RemoveAccessedExpiredCacheItemsTest()
        {
            var cache = new FirstInLastOutGenuineCache();
            cache.SetItemAccessedTimeToLive(TimeSpan.FromSeconds(10));
            cache.SetCapacity(1);

            var item1 = new GenuineCacheItem(DateTime.Now.AddDays(-1), new GenuineKey());
            var item2 = new GenuineCacheItem(DateTime.Now.AddDays(-1), new GenuineKey());
            var item3 = new GenuineCacheItem(DateTime.Now.AddDays(-1), new GenuineKey());
            cache.InsertCacheItem(item1);
            cache.InsertCacheItem(item2);
            cache.InsertCacheItem(item3);
            Assert.IsTrue(cache.GetUtilization() == 0);
        }

        /// <summary>
        /// Test the Constructor for the Time Based Eviction FILO Cache.
        /// </summary>
        [TestMethod]
        public void AccessedTimeBasedEvictionFILOConstrutorTest()
        {
            var timespanCache = new FirstInLastOutGenuineCache();
            timespanCache.SetItemAccessedTimeToLive(TimeSpan.FromDays(1));
            Assert.AreEqual(TimeSpan.FromDays(1), timespanCache.GetItemAccessedTimeToLive());
        }
    }
}
