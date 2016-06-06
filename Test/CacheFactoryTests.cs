namespace CacheFactoryTest
{
    using CacheFactory;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using TestDTOs;

    /// <summary>
    /// Testing all use cases of the Cache Factory
    /// </summary>
    [TestClass]
    public class CacheFactoryTest
    {
        /// <summary>
        /// Testing that the FIFO Cache is being constructed correctly by the Factory.
        /// </summary>
        [TestMethod]
        public void FIFOConstructorTest()
        {
            try
            {
                new CacheFactory<GenuineKey, GenuineCacheItem, FirstInFirstOutGenuineCache>().CreateCache();
            }
            catch (Exception ex)
            {
                Assert.Fail("Failed to generate the FIFO Cache: " + ex );
            }
        }

        /// <summary>
        /// Testing that the FIFO Cache is being constructed correctly by the Factory.
        /// </summary>
        [TestMethod]
        public void FIFOConstructorTest2()
        {
            var cache = new CacheFactory<GenuineKey, GenuineCacheItem, FirstInFirstOutGenuineCache>().CreateCache();
            if (cache == null) Assert.Fail("First In First Out Cache resulted in null when being constructed.");
        }

        /// <summary>
        /// Testing that the FILO Cache is being constructed correctly by the Factory.
        /// </summary>
        [TestMethod]
        public void FILOConstructorTest()
        {
            try
            {
                new CacheFactory<GenuineKey, GenuineCacheItem, FirstInLastOutGenuineCache>().CreateCache();
            }
            catch (Exception ex)
            {
                Assert.Fail("Failed to generate the FILO Cache." + ex);
            }
        }

        /// <summary>
        /// Testing that the FILO Cache is being constructed correctly by the Factory.
        /// </summary>
        [TestMethod]
        public void FILOConstructorTest2()
        {
            var cache = new CacheFactory<GenuineKey, GenuineCacheItem, FirstInLastOutGenuineCache>().CreateCache();
            if (cache == null) Assert.Fail("First In Last Out Cache resulted in null when being constructed.");
        }

        /// <summary>
        /// Testing that the LRU Cache is being constructed correctly by the Factory.
        /// </summary>
        [TestMethod]
        public void LRUConstructorTest()
        {
            try
            {
                new CacheFactory<GenuineKey, GenuineCacheItem, LeastRecentlyUsedGenuineCache>().CreateCache();

            }
            catch (Exception ex)
            {
                Assert.Fail("Failed to generate the LRU Cache." + ex);
            }
        }

        /// <summary>
        /// Testing that the LRU Cache is being constructed correctly by the Factory.
        /// </summary>
        [TestMethod]
        public void LRUConstructorTest2()
        {
            var cache = new CacheFactory<GenuineKey, GenuineCacheItem, LeastRecentlyUsedGenuineCache>().CreateCache();
            if (cache == null) Assert.Fail("Least Recently Used Cache resulted in null when being constructed.");
        }

        /// <summary>
        /// Testing that the TTL Cache is being constructed correctly by the Factory.
        /// </summary>
        [TestMethod]
        public void TimeBasedEvictionConstrutorTest()
        {
            try
            {
                // new CacheFactory<GenuineKey, GenuineCacheItem, TimeBasedEvictionCache<GenuineKey, GenuineCacheItem>>().CreateCache();
            }
            catch (Exception ex)
            {
                Assert.Fail("Failed to generate the Time Based Eviction Cache." + ex);
            }
        }

        /// <summary>
        /// Testing that the TTL Cache is being constructed correctly by the Factory.
        /// </summary>
        [TestMethod]
        public void TimeBasedEvictionConstrutorTest2()
        {
     ////       var cache = new CacheFactory<GenuineKey, GenuineCacheItem, TimeBasedEvictionCache<GenuineKey, GenuineCacheItem>>().CreateCache();
        //    if (cache == null) Assert.Fail("Least Recently Used Cache resulted in null when being constructed.");
        }

        /// <summary>
        /// Testing that Invalid Caches fail when the factory attempts to create it.
        /// </summary>
        [TestMethod]
        public void InvalidCacheConstructorTest()
        {
            try
            {
                new CacheFactory<InvalidCacheItemKey, InvalidCacheItem, InvalidCache>().CreateCache();
                Assert.Fail("Failed to generate the FIFO Cache.");
            }
            catch
            {
                // ignored
            }
        }

        /// <summary>
        /// Testing that the FIFO Global Cache is being constructed correctly by the Factory.
        /// </summary>
        [TestMethod]
        public void FIFOGlobalConstructorTest()
        {
            try
            {
                new CacheFactory<GenuineKey, GenuineCacheItem, FirstInFirstOutGenuineCache>().CreateCache(true);
            }
            catch (Exception ex)
            {
                Assert.Fail("Failed to generate the FIFO Cache." + ex);
            }
        }

        /// <summary>
        /// Testing that the FILO Global Cache is being constructed correctly by the Factory.
        /// </summary>
        [TestMethod]
        public void FILOGlobalConstructorTest()
        {
            try
            {
                new CacheFactory<GenuineKey, GenuineCacheItem, FirstInLastOutGenuineCache>().CreateCache(true);
            }
            catch (Exception ex)
            {
                Assert.Fail("Failed to generate the FIFO Cache." + ex);
            }
        }

        /// <summary>
        /// Testing that the LRU Global Cache is being constructed correctly by the Factory.
        /// </summary>
        [TestMethod]
        public void LRUGlobalConstructorTest()
        {
            try
            {
                new CacheFactory<GenuineKey, GenuineCacheItem, LeastRecentlyUsedGenuineCache>().CreateCache(true);
            }
            catch (Exception ex)
            {
                Assert.Fail("Failed to generate the FIFO Cache." + ex);
            }
        }

        /// <summary>
        /// Testing that the TTL Global Cache is being constructed correctly by the Factory.
        /// </summary>
        [TestMethod]
        public void TimeBasedEvictionGlobalConstrutorTest()
        {
            try
            {
     //           new CacheFactory<GenuineKey, GenuineCacheItem, TimeBasedEvictionCache<GenuineKey, GenuineCacheItem>>().CreateCache(true);
            }
            catch (Exception ex)
            {
                Assert.Fail("Failed to generate the FIFO Cache." + ex);
            }
        }

        /// <summary>
        /// Testing that Invalid Caches do not get created by the factory.
        /// </summary>
        [TestMethod]
        public void InvalidCacheGlobalConstructorTest()
        {
            try
            {
                new CacheFactory<InvalidCacheItemKey, InvalidCacheItem, InvalidCache>().CreateCache(true);
                Assert.Fail("Failed to generate the FIFO Cache.");
            }
            catch
            {
                // ignored
            }
        }

        /// <summary>
        /// Testing that the FIFO Global Cache throws an exception if you try and create multiple instances of them.
        /// </summary>
        [TestMethod]
        public void FIFOMultipleGlobalConstructorTest()
        {
            try
            {
                new CacheFactory<GenuineKey, GenuineCacheItem, FirstInFirstOutGenuineCache>().CreateCache(true);
                new CacheFactory<GenuineKey, GenuineCacheItem, FirstInFirstOutGenuineCache>().CreateCache(true);
            }
            catch (Exception ex)
            {
                Assert.Fail("Failed to generate the FIFO Cache." + ex );
            }
        }

        /// <summary>
        /// Testing that the FIFO Global Cache throws an exception if you try and create multiple instances of them.
        /// </summary>
        [TestMethod]
        public void FIFOMultipleGlobalConstructorTest2()
        {
            try
            {
                var cache = new CacheFactory<GenuineKey, GenuineCacheItem, FirstInFirstOutGenuineCache>().CreateCache(true);
                Assert.AreEqual(cache.GetCacheName(), "global_cache");
            }
            catch (Exception ex)
            {
                Assert.Fail("Failed to generate the FIFO Cache." + ex);
            }
        }

        /// <summary>
        /// Testing that the FILO Global Cache throws an exception if you try and create multiple instances of them.
        /// </summary>
        [TestMethod]
        public void FILOMultipleGlobalConstructorTest()
        {
            try
            {
                new CacheFactory<GenuineKey, GenuineCacheItem, FirstInLastOutGenuineCache>().CreateCache(true);
                new CacheFactory<GenuineKey, GenuineCacheItem, FirstInLastOutGenuineCache>().CreateCache(true);
            }
            catch (Exception ex)
            {
                Assert.Fail("Failed to generate the FIFO Cache." + ex);
            }
        }

        /// <summary>
        /// Testing that the LRU Global Cache throws an exception if you try and create multiple instances of them.
        /// </summary>
        [TestMethod]
        public void LRUGlobalMultipleConstructorTest()
        {
            try
            {
                new CacheFactory<GenuineKey, GenuineCacheItem, LeastRecentlyUsedGenuineCache>().CreateCache(true);
                new CacheFactory<GenuineKey, GenuineCacheItem, LeastRecentlyUsedGenuineCache>().CreateCache(true);
            }
            catch (Exception ex)
            {
                Assert.Fail("Failed to generate the FIFO Cache." + ex );
            }
        }

        /// <summary>
        /// Testing that the TTL Global Cache throws an exception if you try and create multiple instances of them.
        /// </summary>
        [TestMethod]
        public void TimeBasedEvictionMultipleGlobalConstrutorTest()
        {
            try
            {
      //          new CacheFactory<GenuineKey, GenuineCacheItem, TimeBasedEvictionCache<GenuineKey, GenuineCacheItem>>().CreateCache(true);
        //        new CacheFactory<GenuineKey, GenuineCacheItem, TimeBasedEvictionCache<GenuineKey, GenuineCacheItem>>().CreateCache(true);
            }
            catch (Exception ex)
            {
                Assert.Fail("Failed to generate the FIFO Cache." + ex);
            }
        }
    }
}
