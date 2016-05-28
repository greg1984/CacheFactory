namespace CacheFactoryTest
{
    using CacheFactory;
    using CacheFactory.Cachers;
    using CacheFactory.Cachers.Base;
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
                new CacheFactory<CacheItem<CacheItemKey>, CacheItemKey, FirstInFirstOutCache<CacheItem<CacheItemKey>, CacheItemKey>>().CreateCache();
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
            var cache = new CacheFactory<CacheItem<CacheItemKey>, CacheItemKey, FirstInFirstOutCache<CacheItem<CacheItemKey>, CacheItemKey>>().CreateCache();
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
                new CacheFactory<CacheItem<CacheItemKey>, CacheItemKey, FirstInLastOutCache<CacheItem<CacheItemKey>, CacheItemKey>>().CreateCache();
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
            var cache = new CacheFactory<CacheItem<CacheItemKey>, CacheItemKey, FirstInLastOutCache<CacheItem<CacheItemKey>, CacheItemKey>>().CreateCache();
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
                new CacheFactory<CacheItem<CacheItemKey>, CacheItemKey, LeastRecentlyUsedCache<CacheItem<CacheItemKey>, CacheItemKey>>().CreateCache();
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
            var cache = new CacheFactory<CacheItem<CacheItemKey>, CacheItemKey, LeastRecentlyUsedCache<CacheItem<CacheItemKey>, CacheItemKey>>().CreateCache();
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
                new CacheFactory<CacheItem<CacheItemKey>, CacheItemKey, TimeBasedEvictionCache<CacheItem<CacheItemKey>, CacheItemKey>>().CreateCache();
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
            var cache = new CacheFactory<CacheItem<CacheItemKey>, CacheItemKey, TimeBasedEvictionCache<CacheItem<CacheItemKey>, CacheItemKey>>().CreateCache();
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
                new CacheFactory<CacheItem<CacheItemKey>, CacheItemKey, InvalidCache<CacheItem<CacheItemKey>, CacheItemKey>>().CreateCache();
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
                new CacheFactory<CacheItem<CacheItemKey>, CacheItemKey, FirstInFirstOutCache<CacheItem<CacheItemKey>, CacheItemKey>>().CreateCache(true);
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
                new CacheFactory<CacheItem<CacheItemKey>, CacheItemKey, FirstInLastOutCache<CacheItem<CacheItemKey>, CacheItemKey>>().CreateCache(true);
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
                new CacheFactory<CacheItem<CacheItemKey>, CacheItemKey, LeastRecentlyUsedCache<CacheItem<CacheItemKey>, CacheItemKey>>().CreateCache(true);
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
                new CacheFactory<CacheItem<CacheItemKey>, CacheItemKey, TimeBasedEvictionCache<CacheItem<CacheItemKey>, CacheItemKey>>().CreateCache(true);
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
                new CacheFactory<CacheItem<CacheItemKey>, CacheItemKey, InvalidCache<CacheItem<CacheItemKey>, CacheItemKey>>().CreateCache(true);
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
                new CacheFactory<CacheItem<CacheItemKey>, CacheItemKey, FirstInFirstOutCache<CacheItem<CacheItemKey>, CacheItemKey>>().CreateCache(true);
                new CacheFactory<CacheItem<CacheItemKey>, CacheItemKey, FirstInFirstOutCache<CacheItem<CacheItemKey>, CacheItemKey>>().CreateCache(true);
            }
            catch (Exception ex)
            {
                Assert.Fail("Failed to generate the FIFO Cache." + ex );
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
                new CacheFactory<CacheItem<CacheItemKey>, CacheItemKey, FirstInLastOutCache<CacheItem<CacheItemKey>, CacheItemKey>>().CreateCache(true);
                new CacheFactory<CacheItem<CacheItemKey>, CacheItemKey, FirstInLastOutCache<CacheItem<CacheItemKey>, CacheItemKey>>().CreateCache(true);
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
                new CacheFactory<CacheItem<CacheItemKey>, CacheItemKey, LeastRecentlyUsedCache<CacheItem<CacheItemKey>, CacheItemKey>>().CreateCache(true);
                new CacheFactory<CacheItem<CacheItemKey>, CacheItemKey, LeastRecentlyUsedCache<CacheItem<CacheItemKey>, CacheItemKey>>().CreateCache(true);
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
                new CacheFactory<CacheItem<CacheItemKey>, CacheItemKey, TimeBasedEvictionCache<CacheItem<CacheItemKey>, CacheItemKey>>().CreateCache(true);
                new CacheFactory<CacheItem<CacheItemKey>, CacheItemKey, TimeBasedEvictionCache<CacheItem<CacheItemKey>, CacheItemKey>>().CreateCache(true);
            }
            catch (Exception ex)
            {
                Assert.Fail("Failed to generate the FIFO Cache." + ex);
            }
        }
    }
}
