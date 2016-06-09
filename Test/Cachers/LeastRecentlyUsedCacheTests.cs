namespace CacheFactoryTest.Cachers
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Threading;
    using TestDTOs;

    /// <summary>
    /// Tests for the Least Recently Used Cache.
    /// </summary>
    [TestClass]
    public class LeastRecentlyUsedCacheTests
    {
        /// <summary>
        /// Test the Constructor for the First In First Out Cache
        /// </summary>
        [TestMethod]
        public void LRUConstructorTest()
        {
            var cache = new LeastRecentlyUsedGenuineCache();
            if (cache == null) Assert.Fail("Least Recently Used Cache resulted in null when being constructed..");
        }

        /// <summary>
        /// An event which is fired when the cache is overflown.
        /// </summary>
        [TestMethod]
        public void OnCacheOverflowTestLRUEvictsCheckUtilization()
        {
            var cache = new LeastRecentlyUsedGenuineCache();
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
        public void OnCacheOverflowTestLRUEvictsCheckItem()
        {
            var item1 = new GenuineCacheItem(DateTime.Now.AddSeconds(1), new GenuineKey());
            item1.SetCreatedTime(DateTime.Now.AddSeconds(1));
            var item2 = new GenuineCacheItem(DateTime.Now, new GenuineKey());
            item2.SetCreatedTime(DateTime.Now);
            var item3 = new GenuineCacheItem(DateTime.Now.AddSeconds(2), new GenuineKey());
            var cache = new LeastRecentlyUsedGenuineCache();
            cache.SetCapacity(1);
            cache.InsertCacheItem(item1);
            cache.InsertCacheItem(item2);
            cache.InsertCacheItem(item3);
            cache.GetCachedItem(item2.GetKey()).SetLastAccessedTime(DateTime.Now);
            cache.GetCachedItem(item1.GetKey()).SetLastAccessedTime(DateTime.Now.AddSeconds(1));
            cache.GetCachedItem(item3.GetKey()).SetLastAccessedTime(DateTime.Now.AddSeconds(2));
            Assert.IsFalse(cache.GetCache().ContainsKey(item2.GetKey()));
        }

        /// <summary>
        /// Test the Constructor for the Created Time Based Eviction FIFO Cache.
        /// </summary>
        [TestMethod]
        public void CreatedTimeBasedEvictionLRUConstrutorTest()
        {
            try
            {
                var timespanCache = new LeastRecentlyUsedGenuineCache();
                timespanCache.SetItemCreatedTimeToLive(TimeSpan.FromDays(1));
            }
            catch (Exception ex)
            {
                Assert.Fail("Exception thrown when generatinge the Time Based FIFO Eviction Cache." + ex);
            }
        }

        /// <summary>
        /// Test the Constructor for the Created Time Based Eviction LRU Cache.
        /// </summary>
        [TestMethod]
        public void AccessedTimeBasedEvictionLRUConstrutorTest()
        {
            try
            {
                var timespanCache = new LeastRecentlyUsedGenuineCache();
                timespanCache.SetItemAccessedTimeToLive(TimeSpan.FromDays(1));
            }
            catch (Exception ex)
            {
                Assert.Fail("Exception thrown when generatinge the Time Based FIFO Eviction Cache." + ex);
            }
        }

        /// <summary>
        /// Verification that the Least Recently Used Cache Item gets evicted first (Time set manually).
        /// </summary>
        [TestMethod]
        public void OnCacheOverflowLRUTestContainsItemEvictsRight()
        {
            var item1 = new GenuineCacheItem(DateTime.Now.AddSeconds(2), new GenuineKey());
            item1.SetCreatedTime(DateTime.Now.AddSeconds(2));
            var item2 = new GenuineCacheItem(DateTime.Now.AddSeconds(3), new GenuineKey());
            item2.SetCreatedTime(DateTime.Now.AddSeconds(3));
            var item3 = new GenuineCacheItem(DateTime.Now, new GenuineKey());
            item3.SetCreatedTime(DateTime.Now);
            var cache = new LeastRecentlyUsedGenuineCache();
            cache.SetItemAccessedTimeToLive(TimeSpan.MaxValue);
            cache.SetCapacity(1);
            cache.InsertCacheItem(item1);
            cache.InsertCacheItem(item2);
            cache.InsertCacheItem(item3);
            Assert.IsTrue(cache.GetCache().ContainsKey(item2.GetKey()));
        }

        /// <summary>
        /// Verification that a Timespan of 0 prevents the Time Based eviction from occuring when using the  Least Recently Used Eviction policy.
        /// </summary>
        [TestMethod]
        public void OnCacheOverflowLRUZeroTTLVoidsExpiryTest()
        {
            var cache = new LeastRecentlyUsedGenuineCache();
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
            var cache = new LeastRecentlyUsedGenuineCache();
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
        /// Verification that stale objects get removed from the cache.
        /// </summary>
        [TestMethod]
        public void RemoveCreatedExpiredCacheItemsTest()
        {
            var cache = new LeastRecentlyUsedGenuineCache();
            cache.SetItemCreatedTimeToLive(TimeSpan.FromMilliseconds(1));
            cache.SetCapacity(1);

            var item1 = new GenuineCacheItem(DateTime.Now, new GenuineKey());
            var item2 = new GenuineCacheItem(DateTime.Now, new GenuineKey());
            var item3 = new GenuineCacheItem(DateTime.Now, new GenuineKey());
            cache.InsertCacheItem(item1);
            cache.InsertCacheItem(item2);
            cache.InsertCacheItem(item3);
            Thread.Sleep(TimeSpan.FromMilliseconds(3));
            Assert.IsTrue(cache.GetUtilization() == 0);
        }

        /// <summary>
        /// Verification that the cache only loads to capacity with the Least Recently Used Eviction strategy.
        /// </summary>
        [TestMethod]
        public void OnCacheOverflowLRUTest()
        {
            var cache = new LeastRecentlyUsedGenuineCache();
            cache.SetItemAccessedTimeToLive(TimeSpan.MaxValue);
            cache.SetCapacity(1);

            cache.InsertCacheItem(new GenuineCacheItem(DateTime.Now.AddMinutes(2), new GenuineKey()));
            cache.InsertCacheItem(new GenuineCacheItem(DateTime.Now.AddMinutes(1), new GenuineKey()));
            cache.InsertCacheItem(new GenuineCacheItem(DateTime.Now, new GenuineKey()));
            Assert.AreEqual(cache.GetCapacity(), cache.GetUtilization());
        }
    }
}
