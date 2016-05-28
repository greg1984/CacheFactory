namespace CacheFactoryTest.Cachers
{
    using CacheFactory.Cachers;
    using CacheFactory.Cachers.Base;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections.Generic;
    using TestDTOs;

    /// <summary>
    /// Tests for the Constructor for the Time Based Eviction Cache.
    /// </summary>
    [TestClass]
    public class TimeBasedEvictionCacheTests
    {
        /// <summary>
        /// Test the Constructor for the Time Based Eviction FIFO Cache.
        /// </summary>
        [TestMethod]
        public void TimeBasedEvictionFIFOConstrutorTest()
        {
            try
            {
                new TimeBasedEvictionCache<CacheItem<CacheItemKey>, CacheItemKey>(TimeSpan.FromMilliseconds(1500));
            }
            catch (Exception ex)
            {
                Assert.Fail("Exception thrown when generatinge the Time Based FIFO Eviction Cache." + ex);
            }
        }

        /// <summary>
        /// Test the Constructor for the Time Based Eviction FILO Cache.
        /// </summary>
        [TestMethod]
        public void TimeBasedEvictionFILOConstrutorTest()
        {
            try
            {
                new TimeBasedEvictionCache<CacheItem<CacheItemKey>, CacheItemKey>(TimeSpan.FromMilliseconds(1500), strategy:TimeBasedEvictionStrategies.FirstInLastOut);
            }
            catch (Exception ex)
            {
                Assert.Fail("Exception thrown when generatinge the Time Based FILO Eviction Cache." + ex);
            }
        }

        /// <summary>
        /// Test the Constructor for the Time Based Eviction LRU Cache.
        /// </summary>
        [TestMethod]
        public void TimeBasedEvictionLRUConstrutorTest()
        {
            try
            {
                new TimeBasedEvictionCache<CacheItem<CacheItemKey>, CacheItemKey>(TimeSpan.FromMilliseconds(1500), strategy: TimeBasedEvictionStrategies.LeastRecentlyUsed);
            }
            catch (Exception ex)
            {
                Assert.Fail("Exception thrown when generatinge the Time Based LRU Eviction Cache." + ex);
            }
        }

        /// <summary>
        /// Test the Constructor for the Time Based Eviction FIFO Cache
        /// </summary>
        [TestMethod]
        public void TimeBasedEvictionFIFOConstrutorTest2()
        {
            var cache = new TimeBasedEvictionCache<CacheItem<CacheItemKey>, CacheItemKey>(TimeSpan.FromMilliseconds(1500));
            if (cache == null) Assert.Fail("Time Based Eviction FIFO Cache resulted in null when being constructed..");
        }

        /// <summary>
        /// Test the Constructor for the Time Based Eviction FILO Cache
        /// </summary>
        [TestMethod]
        public void TimeBasedEvictionFILOConstrutorTest2()
        {
            var cache = new TimeBasedEvictionCache<CacheItem<CacheItemKey>, CacheItemKey>(TimeSpan.FromMilliseconds(1500), strategy: TimeBasedEvictionStrategies.FirstInLastOut);
            if (cache == null) Assert.Fail("Time Based Eviction FILO Cache resulted in null when being constructed..");
        }

        /// <summary>
        /// Test the Constructor for the Time Based Eviction LRU Cache
        /// </summary>
        [TestMethod]
        public void TimeBasedEvictionLRUConstrutorTest2()
        {
            var cache = new TimeBasedEvictionCache<CacheItem<CacheItemKey>, CacheItemKey>(TimeSpan.FromMilliseconds(1500), strategy: TimeBasedEvictionStrategies.LeastRecentlyUsed);
            if (cache == null) Assert.Fail("Time Based Eviction LRU Cache resulted in null when being constructed.");
        }

        /// <summary>
        ///  Verification that the cache only loads to capacity with the First In First Out Eviction strategy.
        /// </summary>
        [TestMethod]
        public void OnCacheOverflowFIFOTest()
        {
            var cache = new TimeBasedEvictionCache<CacheItem<GenuineKey>, GenuineKey>(TimeSpan.MaxValue, new Dictionary<GenuineKey, CacheItem<GenuineKey>>(), 1);
            cache.InsertCacheItem(new CacheItem<GenuineKey>(DateTime.Now.AddMinutes(2), new GenuineKey()));
            cache.InsertCacheItem(new CacheItem<GenuineKey>(DateTime.Now.AddMinutes(1), new GenuineKey()));
            cache.InsertCacheItem(new CacheItem<GenuineKey>(DateTime.Now, new GenuineKey()));
            Assert.AreEqual(cache.GetCapacity(), cache.GetUtilization());
        }

        /// <summary>
        /// Verification that the cache only loads to capacity with the First In Last Out Eviction strategy.
        /// </summary>
        [TestMethod]
        public void OnCacheOverflowTTLTest()
        {
            var cache = new TimeBasedEvictionCache<CacheItem<GenuineKey>, GenuineKey>(TimeSpan.MaxValue, new Dictionary<GenuineKey, CacheItem<GenuineKey>>(), 1, TimeBasedEvictionStrategies.FirstInLastOut);
            cache.InsertCacheItem(new CacheItem<GenuineKey>(DateTime.Now.AddMinutes(2), new GenuineKey()));
            cache.InsertCacheItem(new CacheItem<GenuineKey>(DateTime.Now.AddMinutes(1), new GenuineKey()));
            cache.InsertCacheItem(new CacheItem<GenuineKey>(DateTime.Now, new GenuineKey()));
            Assert.AreEqual(cache.GetCapacity(), cache.GetUtilization());
        }

        /// <summary>
        /// Verification that the cache only loads to capacity with the Least Recently Used Eviction strategy.
        /// </summary>
        [TestMethod]
        public void OnCacheOverflowLRUTest()
        {
            var cache = new TimeBasedEvictionCache<CacheItem<GenuineKey>, GenuineKey>(TimeSpan.MaxValue, new Dictionary<GenuineKey, CacheItem<GenuineKey>>(), 1, TimeBasedEvictionStrategies.LeastRecentlyUsed);
            cache.InsertCacheItem(new CacheItem<GenuineKey>(DateTime.Now.AddMinutes(2), new GenuineKey()));
            cache.InsertCacheItem(new CacheItem<GenuineKey>(DateTime.Now.AddMinutes(1), new GenuineKey()));
            cache.InsertCacheItem(new CacheItem<GenuineKey>(DateTime.Now, new GenuineKey()));
            Assert.AreEqual(cache.GetCapacity(), cache.GetUtilization());
        }

        /// <summary>
        /// Verification that the First In First Out Cache Item item gets evicted first (Time set manually).
        /// </summary>
        [TestMethod]
        public void OnCacheOverflowFIFOTest2()
        {
            var cache = new TimeBasedEvictionCache<CacheItem<GenuineKey>, GenuineKey>(TimeSpan.MaxValue, new Dictionary<GenuineKey, CacheItem<GenuineKey>>(), 1);

            var item1 = new CacheItem<GenuineKey>(DateTime.Now, new GenuineKey());
            item1.SetInsertTime(DateTime.Now);
            var item2 = new CacheItem<GenuineKey>(DateTime.Now.AddSeconds(1), new GenuineKey());
            item2.SetInsertTime(DateTime.Now.AddSeconds(1));
            var item3 = new CacheItem<GenuineKey>(DateTime.Now.AddSeconds(2), new GenuineKey());
            item3.SetInsertTime(DateTime.Now.AddSeconds(2));
            cache.InsertCacheItem(item1);
            cache.InsertCacheItem(item2);
            cache.InsertCacheItem(item3);
            Assert.IsTrue(cache.GetCache().ContainsKey(item3.Key));
        }

        /// <summary>
        /// Verification that the First In Last Out Cache Item item gets evicted first (Time set manually).
        /// </summary>
        [TestMethod]
        public void OnCacheOverflowTTLTest2()
        {
            var cache = new TimeBasedEvictionCache<CacheItem<GenuineKey>, GenuineKey>(TimeSpan.MaxValue, new Dictionary<GenuineKey, CacheItem<GenuineKey>>(), 1, TimeBasedEvictionStrategies.FirstInLastOut);

            var item1 = new CacheItem<GenuineKey>(DateTime.Now, new GenuineKey());
            item1.SetInsertTime(DateTime.Now);
            var item2 = new CacheItem<GenuineKey>(DateTime.Now.AddSeconds(1), new GenuineKey());
            item2.SetInsertTime(DateTime.Now.AddSeconds(1));
            var item3 = new CacheItem<GenuineKey>(DateTime.Now.AddSeconds(2), new GenuineKey());
            item3.SetInsertTime(DateTime.Now.AddSeconds(2));
            cache.InsertCacheItem(item1);
            cache.InsertCacheItem(item2);
            cache.InsertCacheItem(item3);
            Assert.IsTrue(cache.GetCache().ContainsKey(item1.Key));
        }

        /// <summary>
        /// Verification that the Least Recently Used Cache Item gets evicted first (Time set manually).
        /// </summary>
        [TestMethod]
        public void OnCacheOverflowLRUTest2()
        {
            var item1 = new CacheItem<GenuineKey>(DateTime.Now.AddSeconds(2), new GenuineKey());
            item1.SetInsertTime(DateTime.Now.AddSeconds(2));
            var item2 = new CacheItem<GenuineKey>(DateTime.Now.AddSeconds(3), new GenuineKey());
            item2.SetInsertTime(DateTime.Now.AddSeconds(3));
            var item3 = new CacheItem<GenuineKey>(DateTime.Now, new GenuineKey());
            item3.SetInsertTime(DateTime.Now);
            var cache = new TimeBasedEvictionCache<CacheItem<GenuineKey>, GenuineKey>(TimeSpan.MaxValue, new Dictionary<GenuineKey, CacheItem<GenuineKey>>(), 1);
            cache.InsertCacheItem(item1);
            cache.InsertCacheItem(item2);
            cache.InsertCacheItem(item3);
            Assert.IsTrue(cache.GetCache().ContainsKey(item2.Key));
        }

        /// <summary>
        /// Verification that a Timespan of 0 prevents the Time Based eviction from occuring when using the First in First Out Eviction policy.
        /// </summary>
        [TestMethod]
        public void OnCacheOverflowFIFOTest3()
        {
            var cache = new TimeBasedEvictionCache<CacheItem<GenuineKey>, GenuineKey>(TimeSpan.Zero, new Dictionary<GenuineKey, CacheItem<GenuineKey>>(), 2);

            var item1 = new CacheItem<GenuineKey>(DateTime.Now.AddSeconds(-1), new GenuineKey());
            item1.SetInsertTime(DateTime.Now.AddSeconds(-1));
            var item2 = new CacheItem<GenuineKey>(DateTime.Now.AddSeconds(-3), new GenuineKey());
            item2.SetInsertTime(DateTime.Now.AddSeconds(-3));
            var item3 = new CacheItem<GenuineKey>(DateTime.Now.AddSeconds(2), new GenuineKey());
            item3.SetInsertTime(DateTime.Now.AddSeconds(2));
            cache.InsertCacheItem(item1);
            cache.InsertCacheItem(item2);
            item1.SetInsertTime(DateTime.Now.AddSeconds(-1));
            item1.SetLastAccessed(DateTime.Now.AddSeconds(-1));
            cache.InsertCacheItem(item3);
            Assert.IsTrue(cache.GetCache().ContainsKey(item1.Key));
        }

        /// <summary>
        /// Verification that a Timespan of 0 prevents the Time Based eviction from occuring when using the First In Last Out Eviction policy.
        /// </summary>
        [TestMethod]
        public void OnCacheOverflowTTLTest3()
        {
            var cache = new TimeBasedEvictionCache<CacheItem<GenuineKey>, GenuineKey>(TimeSpan.Zero, new Dictionary<GenuineKey, CacheItem<GenuineKey>>(), 2, TimeBasedEvictionStrategies.FirstInLastOut);

            var item1 = new CacheItem<GenuineKey>(DateTime.Now.AddSeconds(-1), new GenuineKey());
            item1.SetInsertTime(DateTime.Now.AddSeconds(-1));
            var item2 = new CacheItem<GenuineKey>(DateTime.Now.AddSeconds(-3), new GenuineKey());
            item2.SetInsertTime(DateTime.Now.AddSeconds(-3));
            var item3 = new CacheItem<GenuineKey>(DateTime.Now.AddSeconds(2), new GenuineKey());
            item3.SetInsertTime(DateTime.Now.AddSeconds(2));
            cache.InsertCacheItem(item1);
            cache.InsertCacheItem(item2);
            item1.SetInsertTime(DateTime.Now.AddSeconds(-1));
            item1.SetLastAccessed(DateTime.Now.AddSeconds(-1));
            cache.InsertCacheItem(item3);
            Assert.IsTrue(cache.GetCache().ContainsKey(item1.Key));
        }

        /// <summary>
        /// Verification that a Timespan of 0 prevents the Time Based eviction from occuring when using the  Least Recently Used Eviction policy.
        /// </summary>
        [TestMethod]
        public void OnCacheOverflowLRUTest3()
        {
            var cache = new TimeBasedEvictionCache<CacheItem<GenuineKey>, GenuineKey>(TimeSpan.Zero, new Dictionary<GenuineKey, CacheItem<GenuineKey>>(), 2, TimeBasedEvictionStrategies.LeastRecentlyUsed);

            var item1 = new CacheItem<GenuineKey>(DateTime.Now.AddSeconds(-1), new GenuineKey());
            item1.SetInsertTime(DateTime.Now.AddSeconds(-1));
            var item2 = new CacheItem<GenuineKey>(DateTime.Now.AddSeconds(-3), new GenuineKey());
            item2.SetInsertTime(DateTime.Now.AddSeconds(-3));
            var item3 = new CacheItem<GenuineKey>(DateTime.Now.AddSeconds(2), new GenuineKey());
            item3.SetInsertTime(DateTime.Now.AddSeconds(2));
            cache.InsertCacheItem(item1);
            cache.InsertCacheItem(item2);
            item1.SetInsertTime(DateTime.Now.AddSeconds(-1));
            item1.SetLastAccessed(DateTime.Now.AddSeconds(-1));
            cache.InsertCacheItem(item3);
            Assert.IsTrue(cache.GetCache().ContainsKey(item1.Key));
        }

        /// <summary>
        /// Verification that stale objects get removed from the cache.
        /// </summary>
        [TestMethod]
        public void RemoveExpiredCacheItemsTest()
        {
            var cache = new TimeBasedEvictionCache<CacheItem<GenuineKey>, GenuineKey>(TimeSpan.FromSeconds(10), new Dictionary<GenuineKey, CacheItem<GenuineKey>>(), 1, TimeBasedEvictionStrategies.LeastRecentlyUsed);

            var item1 = new CacheItem<GenuineKey>(DateTime.Now, new GenuineKey());
            var item2 = new CacheItem<GenuineKey>(DateTime.Now, new GenuineKey());
            var item3 = new CacheItem<GenuineKey>(DateTime.Now, new GenuineKey());
            cache.InsertCacheItem(item1);
            cache.InsertCacheItem(item2);
            cache.InsertCacheItem(item3);
            item1.SetInsertTime(DateTime.Now.AddDays(-1));
            item2.SetInsertTime(DateTime.Now.AddDays(-1));
            item3.SetInsertTime(DateTime.Now.AddDays(-1));
            cache.RemoveExpiredCacheItems();
            Assert.IsTrue(cache.GetUtilization() == 0);
        }
    }
}
