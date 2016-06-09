namespace CacheFactoryTest
{
    using CacheFactory.CacheExceptions;
    using CacheFactory;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Linq;
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
            var cache = new CacheFactory<GenuineKey, GenuineCacheItem, FirstInFirstOutGenuineCache>().CreateCache();
            if (cache == null) Assert.Fail("First In First Out Cache resulted in null when being constructed.");
        }

        /// <summary>
        /// Testing that the FILO Cache is being constructed correctly by the Factory.
        /// </summary>
        [TestMethod]
        public void FILOConstructorTest()
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
            var cache = new CacheFactory<GenuineKey, GenuineCacheItem, LeastRecentlyUsedGenuineCache>().CreateCache();
            if (cache == null) Assert.Fail("Least Recently Used Cache resulted in null when being constructed.");
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
            var cache = new CacheFactory<GenuineKey, GenuineCacheItem, FirstInFirstOutGenuineCache>().CreateCache(true);
            if (cache == null) Assert.Fail("First In First Out Cache resulted in null when being constructed.");
        }

        /// <summary>
        /// Testing that the FILO Global Cache is being constructed correctly by the Factory.
        /// </summary>
        [TestMethod]
        public void FILOGlobalConstructorTest()
        {
            var cache = new CacheFactory<GenuineKey, GenuineCacheItem, FirstInLastOutGenuineCache>().CreateCache(true);
            if (cache == null) Assert.Fail("First In Last Out Cache resulted in null when being constructed.");
        }

        /// <summary>
        /// Testing that the LRU Global Cache is being constructed correctly by the Factory.
        /// </summary>
        [TestMethod]
        public void LRUGlobalConstructorTest()
        {
            var cache = new CacheFactory<GenuineKey, GenuineCacheItem, LeastRecentlyUsedGenuineCache>().CreateCache(true);
            if (cache == null) Assert.Fail("Least Recently Used Cache resulted in null when being constructed.");
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
        /// Testing that the same cache is returned when you try create two global caches of the same type.
        /// </summary>
        [TestMethod]
        public void FIFOMultipleGlobalConstructorTest()
        {
            try
            {
                var cache1 = new CacheFactory<GenuineKey, GenuineCacheItem, FirstInFirstOutGenuineCache>().CreateCache(true);
                var cache2 = new CacheFactory<GenuineKey, GenuineCacheItem, FirstInFirstOutGenuineCache>().CreateCache(true);
                Assert.AreEqual(cache1.GetCacheID(), cache2.GetCacheID());
            }
            catch (Exception ex)
            {
                Assert.Fail("Failed to generate the FIFO Cache." + ex);
            }
        }

        /// <summary>
        /// Testing to make sure that the cache returns the name "global_cache" when creating a global cache.
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
        /// Testing that the same cache is returned when you try create two global caches of the same type.
        /// </summary>
        [TestMethod]
        public void FILOMultipleGlobalConstructorTest()
        {
            try
            {
                var cache1 = new CacheFactory<GenuineKey, GenuineCacheItem, FirstInLastOutGenuineCache>().CreateCache(true);
                var cache2 = new CacheFactory<GenuineKey, GenuineCacheItem, FirstInLastOutGenuineCache>().CreateCache(true);
                Assert.AreEqual(cache1.GetCacheName(), cache2.GetCacheName());
            }
            catch (Exception ex)
            {
                Assert.Fail("Failed to generate the FIFO Cache." + ex);
            }
        }

        /// <summary>
        /// Testing that the same cache is returned when you try create two global caches of the same type.
        /// </summary>
        [TestMethod]
        public void CacheManagerNameChangeTest()
        {
            var cache1 = new CacheFactory<GenuineKey, GenuineCacheItem, FirstInLastOutGenuineCache>().CreateCache(true);
            var guid = Guid.NewGuid().ToString();
            CacheManager<GenuineKey, GenuineCacheItem, FirstInLastOutGenuineCache>.GetCacheByName(cache1.GetCacheName()).First().SetCacheName(guid);
            Assert.AreEqual(cache1.GetCacheName(), guid);
        }

        /// <summary>
        /// Ensures that an exception is thrown when setting a global cache to an invalid name.
        /// </summary>
        [TestMethod]
        public void SetCacheInvalidNameTest()
        {
            try
            {
                var cache1 = new CacheFactory<GenuineKey, GenuineCacheItem, FirstInLastOutGenuineCache>().CreateCache(true);
                var cache2 = new CacheFactory<GenuineKey, GenuineCacheItem, FirstInLastOutGenuineCache>().CreateCache();
                CacheManager<GenuineKey, GenuineCacheItem, FirstInLastOutGenuineCache>.AddCache(cache2);
                CacheManager<GenuineKey, GenuineCacheItem, FirstInLastOutGenuineCache>.GetCacheByName(
                    cache1.GetCacheName()).First().SetCacheName(cache2.GetCacheName());
                Assert.Fail("No Exception was thrown.");
            }
            catch (InvalidCacheNameException)
            {
                // ignore
            }
            catch (Exception ex)
            {
                Assert.Fail("Expected InvalidCacheNameException to be thrown. " + Environment.NewLine + ex);
            }
        }

        /// <summary>
        /// Testing that the same cache is returned when you try create two global caches of the same type.
        /// </summary>
        [TestMethod]
        public void LRUGlobalMultipleConstructorTest()
        {
            try
            {
                var cache1 = new CacheFactory<GenuineKey, GenuineCacheItem, LeastRecentlyUsedGenuineCache>().CreateCache(true);
                var cache2 = new CacheFactory<GenuineKey, GenuineCacheItem, LeastRecentlyUsedGenuineCache>().CreateCache(true);
                Assert.AreEqual(cache1.GetCacheName(), cache2.GetCacheName());
            }
            catch (Exception ex)
            {
                Assert.Fail("Failed to generate the FIFO Cache." + ex );
            }
        }
    }
}
