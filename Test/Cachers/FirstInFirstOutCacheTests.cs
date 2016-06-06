namespace CacheFactoryTest.Cachers
{
    using CacheFactory.Cachers;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using TestDTOs;

    /// <summary>
    /// Test for the First In First Out Cache.
    /// </summary>
    [TestClass]
    public class FirstInFirstOutCacheTests
    {
        /// <summary>
        /// Test the Constructor for the First In First Out Cache
        /// </summary>
        [TestMethod]
        public void FIFOConstructorTest()
        {
            try
            {
                new FirstInFirstOutCache<GenuineKey, GenuineCacheItem>();
            }
            catch (Exception ex)
            {
                Assert.Fail("Exception thrown when generating the FIFO Cache." + ex);
            }
        }

        /// <summary>
        /// Test the Constructor for the First In First Out Cache
        /// </summary>
        [TestMethod]
        public void FIFOConstructorTest2()
        {
            var cache = new FirstInFirstOutCache<GenuineKey, GenuineCacheItem>();
            if (cache == null) Assert.Fail("First In First Out Cache resulted in null when being constructed.");
        }

        /// <summary>
        /// Ensure that the cache will only load to capacity.
        /// </summary>
        [TestMethod]
        public void OnCacheOverflowTest()
        {
            var cache = new FirstInFirstOutCache<GenuineKey, GenuineCacheItem>(new Dictionary<GenuineKey, GenuineCacheItem>(), 1);
            cache.InsertCacheItem(new GenuineCacheItem(DateTime.Now.AddMinutes(2), new GenuineKey()));
            cache.InsertCacheItem(new GenuineCacheItem(DateTime.Now.AddMinutes(1), new GenuineKey()));
            cache.InsertCacheItem(new GenuineCacheItem(DateTime.Now, new GenuineKey()));
            Assert.AreEqual(cache.GetCapacity(), cache.GetUtilization());
        }

        /// <summary>
        /// Test the Constructor for the Time Based Eviction FIFO Cache.
        /// </summary>
        [TestMethod]
        public void CreatedTimeBasedEvictionFIFOConstrutorTest()
        {
            try
            {
                var timespanCache = new FirstInFirstOutGenuineCache();
                timespanCache.SetItemCreatedTimeToLive(TimeSpan.FromDays(1));
            }
            catch (Exception ex)
            {
                Assert.Fail("Exception thrown when generatinge the Time Based FIFO Eviction Cache." + ex);
            }
        }

        /// <summary>
        /// Test the Constructor for the Time Based Eviction FIFO Cache.
        /// </summary>
        [TestMethod]
        public void AccessedTimeBasedEvictionFIFOConstrutorTest()
        {
            try
            {
                var timespanCache = new FirstInFirstOutGenuineCache();
                timespanCache.SetItemAccessedTimeToLive(TimeSpan.FromDays(1));
            }
            catch (Exception ex)
            {
                Assert.Fail("Exception thrown when generatinge the Time Based FIFO Eviction Cache." + ex);
            }
        }

        /// <summary>
        /// Verify the eviction strategy.
        /// </summary>
        [TestMethod]
        public void OnCacheOverflowTest2()
        {
            var item1 = new GenuineCacheItem(DateTime.Now, new GenuineKey());
            item1.SetCreatedTime(DateTime.Now);
            var item2 = new GenuineCacheItem(DateTime.Now.AddSeconds(1), new GenuineKey());
            item2.SetCreatedTime(DateTime.Now.AddSeconds(1));
            var item3 = new GenuineCacheItem(DateTime.Now.AddSeconds(2), new GenuineKey());
            var cache = new FirstInFirstOutCache<GenuineKey, GenuineCacheItem>(new Dictionary<GenuineKey, GenuineCacheItem>(), 1);
            item3.SetCreatedTime(DateTime.Now.AddSeconds(2));
            cache.InsertCacheItem(item1);
            cache.InsertCacheItem(item2);
            cache.InsertCacheItem(item3);
            Assert.IsTrue(cache.GetCache().ContainsKey(item3.GetKey()), "Keys in collection: " + cache.GetCache().SelectMany(m => m.Value.GetKey().Key.ToString()));
        }
        
        /// <summary>
        ///  Verification that the cache only loads to capacity with the First In First Out Eviction strategy.
        /// </summary>
        [TestMethod]
        public void OnAccessedTimeBasedCacheOverflowFIFOTest()
        {
            var cache = new FirstInFirstOutGenuineCache();
            cache.SetItemAccessedTimeToLive(TimeSpan.MaxValue);
            cache.SetCapacity(1);
            cache.InsertCacheItem(new GenuineCacheItem(DateTime.Now.AddMinutes(2), new GenuineKey()));
            cache.InsertCacheItem(new GenuineCacheItem(DateTime.Now.AddMinutes(1), new GenuineKey()));
            cache.InsertCacheItem(new GenuineCacheItem(DateTime.Now, new GenuineKey()));
            Assert.AreEqual(cache.GetCapacity(), cache.GetUtilization());
        }
        
        /// <summary>
        /// Verification that a Timespan of 0 prevents the Time Based eviction from occuring when using the First in First Out Eviction policy.
        /// </summary>
        [TestMethod]
        public void OnCacheOverflowFIFOTest3()
        {
            var cache = new FirstInFirstOutGenuineCache();
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
            item1.SetLastAccessedTime(DateTime.Now.AddSeconds(-1));
            cache.InsertCacheItem(item3);
            Assert.IsTrue(cache.GetCache().ContainsKey(item1.GetKey()));
        }
        /// <summary>
        /// Verification that stale objects get removed from the cache.
        /// </summary>
        [TestMethod]
        public void RemoveAccessedExpiredCacheItemsTest()
        {
            var cache = new FirstInFirstOutGenuineCache();
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
        /// Verification that the First In First Out Cache Item item gets evicted first (Time set manually).
        /// </summary>
        [TestMethod]
        public void OnCacheOverflowFIFOTest2()
        {
            var cache = new FirstInFirstOutGenuineCache();
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
            Assert.IsTrue(cache.GetCache().ContainsKey(item3.GetKey()));
        }
    }
}
