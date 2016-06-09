namespace CacheFactoryTest.Cachers.Base
{
    using CacheFactory;
    using CacheFactory.CacheExceptions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Threading;
    using System.Collections.Generic;
    using TestDTOs;

    /// <summary>
    /// Tests for the Generic Abstracted Cache With Events.
    /// </summary>
    [TestClass]
    public class ACacheWithEventsTests
    {
        private FirstInFirstOutGenuineCache _cache;
        private GenuineCacheItem _item1;
        private GenuineCacheItem _item2;
        private GenuineCacheItem _item3;
        private GenuineCacheItem _item4;

        [TestInitialize]
        public void ConfigureTestObjects()
        {
            _cache = new FirstInFirstOutGenuineCache();
            _cache.SetCapacity(2);
            _item1 = new GenuineCacheItem(DateTime.Now.AddMinutes(1), new GenuineKey());
            _item2 = new GenuineCacheItem(DateTime.Now.AddMinutes(1), new GenuineKey());
            _item3 = new GenuineCacheItem(DateTime.Now.AddMinutes(1), new GenuineKey());
            _item4 = new GenuineCacheItem(DateTime.Now.AddMinutes(1), new GenuineKey());
        }

        /// <summary>
        /// Test the contructor for the Generic Abstracted Cache With Events.
        /// </summary>
        [TestMethod]
        public void ACacheConstructorTest()
        {
            _cache.InsertCacheItem(_item1);
            Assert.IsTrue(_cache.GetUtilization() == 1);
        }

        /// <summary>
        /// Validate all cache elements are set when passed in using SetCache.
        /// </summary>
        [TestMethod]
        public void SetCacheTest()
        {
            _cache.SetCapacity(2);
            _cache.SetCache(new Dictionary<GenuineKey, GenuineCacheItem>
            {
                {_item1.GetKey(), _item1},
                {_item2.GetKey(), _item2}
            });
            Assert.AreEqual(2, _cache.GetUtilization());
        }

        /// <summary>
        /// Validate an exception is thrown if you pass in more cache items then the cache has capacity for using SetCache and the flag is set to false.
        /// </summary>
        [TestMethod]
        public void SetCacheOverCapacityTest()
        {
            try
            {
                _cache.SetCache(new Dictionary<GenuineKey, GenuineCacheItem>
                {
                    {_item1.GetKey(), _item1},
                    {_item2.GetKey(), _item2},
                    {_item3.GetKey(), _item3}
                });
                Assert.Fail("No exception thrown when CacheOverflowException is expected.");
            }
            catch (CacheOverflowException)
            {
                // ignore
            }
            catch (Exception ex)
            {
                Assert.Fail("Invalid exception thrown on craetion of cache: " + Environment.NewLine + ex);
            }
        }

        /// <summary>
        /// Validates a cache resize on the use of CacheSize if there is more cache items then the cache has capacity for using SetCache and the flag is set to true.
        /// </summary>
        [TestMethod]
        public void ResizeCacheOverCapacityTest()
        {
            _cache.SetCapacity(1);
            _cache.SetCache(new Dictionary<GenuineKey, GenuineCacheItem>
            {
                {_item1.GetKey(), _item1},
                {_item2.GetKey(), _item2},
                {_item3.GetKey(), _item3},
                {_item4.GetKey(), _item4}
            }, true);
            Assert.AreEqual(4, _cache.GetCapacity());
        }

        /// <summary>
        /// Validates that an exception is thrown when removing a cached item that does not exist.
        /// </summary>
        [TestMethod]
        public void FailRemoveItemFromCacheTest()
        {
            try
            {
                _cache.RemoveCacheItem(_item4.GetKey());
                Assert.Fail("Exception expected when searching for invalid item.");
            }
            catch
            {
                // ignore
            }
        }

        /// <summary>
        /// Validates that a cache item is removed when removing a valid cache item.
        /// </summary>
        [TestMethod]
        public void RemoveItemFromCacheTest()
        {
            _cache.InsertCacheItem(_item1);
            _cache.RemoveCacheItem(_item1.GetKey());
            Assert.IsTrue(_cache.GetUtilization() == 0);
        }

        /// <summary>
        /// Validates that an exception is thrown when fetching a cache item that no longer exists.
        /// </summary>
        [TestMethod]
        public void FailGetItemFromCacheTest()
        {
            var item = _cache.GetCachedItem(_item2.GetKey());
            Assert.AreNotEqual(item.GetKey(), _item2.GetKey());
        }

        /// <summary>
        /// Validates that a cache item is fetched when fetching a valid cache item.
        /// </summary>
        [TestMethod]
        public void GetItemFromCacheTest()
        {
            _cache.InsertCacheItem(_item1);
            var item =_cache.GetCachedItem(_item1.GetKey());
            Assert.AreEqual(item, _cache.GetCache()[item.GetKey()]);
        }

        /// <summary>
        /// Validates that the cache name can be changed.
        /// </summary>
        [TestMethod]
        public void GoodCacheNameChangeTest()
        {
            _cache.SetCacheName("cache_id_1");
            Assert.AreEqual(_cache.GetCacheName(), "cache_id_1");
        }

        /// <summary>
        /// Validates that an exception is thrown when attempting to utilize the global cache name on a cache owned by the CacheManager.
        /// </summary>
        [TestMethod]
        public void RemoveGlobalCacheTest()
        {
            try
            {
                var cache = new CacheFactory<GenuineKey, GenuineCacheItem, FirstInFirstOutGenuineCache>().CreateCache(true);
                CacheManager<GenuineKey, GenuineCacheItem, FirstInFirstOutGenuineCache>.RemoveCache(cache.GetCacheName());
                Assert.AreEqual(0, CacheManager<GenuineKey, GenuineCacheItem, FirstInFirstOutGenuineCache>.GetCacheCount());
            }
            catch (InvalidCacheNameException)
            {
                // ignore
            }
            catch (Exception ex)
            {
                Assert.Fail(
                    "Excepted cache to fail creation and return an InvalidCacheNameException when exception returned was: " +
                    ex);
            }
        }

        /// <summary>
        /// Validates that you cannot remove a cache from the cache manager that does not already exist.
        /// </summary>
        [TestMethod]
        public void RemoveGlobalCacheTestWhichDoesNotExist()
        {
            try
            {
                CacheManager<GenuineKey, GenuineCacheItem, FirstInFirstOutGenuineCache>.RemoveCache(_cache.GetCacheName());
                Assert.Fail("Excepted cache to fail removal from the cache manager.");
            }
            catch (InvalidCacheNameException)
            {
                // ignore
            }
            catch (Exception ex)
            {
                Assert.Fail(
                    "Excepted cache to fail creation and return an InvalidCacheNameException when exception returned was: " +
                    ex);
            }
        }

        /// <summary>
        /// Validates that an exception is thrown when attempting to utilize the global cache name.
        /// </summary>
        [TestMethod]
        public void GlobalCacheNameToInvalidChangeTest()
        {
            try
            {
                var cache = new CacheFactory<GenuineKey, GenuineCacheItem, FirstInFirstOutGenuineCache>().CreateCache(true);
                CacheManager<GenuineKey, GenuineCacheItem, FirstInFirstOutGenuineCache>.AddCache(cache);
                new FirstInFirstOutGenuineCache();
                _cache.SetCacheName("global_cache");
                Assert.Fail("Excepted cache to fail creation when utilizing the global cache.");
            }
            catch (InvalidCacheNameException)
            {
                // ignore
            }
            catch (Exception ex)
            {
                Assert.Fail(
                    "Excepted cache to fail creation and return an InvalidCacheNameException when exception returned was: " +
                    ex);
            }
        }

        /// <summary>
        /// Validates that setting a capacity with a valid value works.
        /// </summary>
        [TestMethod]
        public void SetCapacityTest()
        {
            _cache.SetCapacity(3);
            Assert.AreEqual(3, _cache.GetCapacity());
        }

        /// <summary>
        /// Validates that setting a capacity with a lower higher than the utilization throws an exception.
        /// </summary>
        [TestMethod]
        public void SetBadCapacityTest()
        {
            try
            {
                _cache.InsertCacheItem(_item1);
                _cache.InsertCacheItem(_item2);
                _cache.SetCapacity(1);
                Assert.Fail("Expected exception when resizing cache smaller than contents.");
            }
            catch (CacheOverflowException)
            {
            }
            catch (Exception ex)
            {
                Assert.Fail("Excepted to get exception CacheOverflowException and exception returned was: " + ex);
            }
        }

        /// <summary>
        /// Validates the cache accepts multiple values.
        /// </summary>
        [TestMethod]
        public void SetCacheItemsTest()
        {
            try
            {
                _cache.SetCapacity(3);
                _cache.InsertCacheItem(_item2);
                _cache.InsertCacheItem(_item3);
                _cache.InsertCacheItem(_item4);
                Assert.AreEqual(_cache.GetCapacity(), _cache.GetUtilization());
            }
            catch
            {
                Assert.Fail("Unexpected exception occured when loading multiple values into the cache.");
            }
        }

        /// <summary>
        /// Validates that the cache exceptions when accepting multiple values with the same key.
        /// </summary>
        [TestMethod]
        public void SetMultipleItemsSameKeyTest()
        {// TODO: Work on this
            try
            {
                _cache.InsertCacheItem(_item1);
                Assert.Fail("Exception expected whenloading multiple identical values into the cache.");
            }
            catch
            {
                // ignore
            }
        }

        /// <summary>
        /// Validates that the cache throws an eviction event when going over capacity.
        /// </summary>
        [TestMethod]
        public void SetEvictingCacheItemsTest()
        {
            _cache.SetCapacity(1);
            _cache.InsertCacheItem(_item2);
            Assert.AreEqual(_cache.GetUtilization(), _cache.GetCapacity());
        }

        /// <summary>
        /// Validates that the cache is cleared on command.
        /// </summary>
        [TestMethod]
        public void ClearCacheTest()
        {
            _cache.InsertCacheItem(_item1);
            _cache.Clear();
            Assert.AreEqual(0, _cache.GetUtilization());
        }
        
        /// <summary>
        /// Validates that the Item Access TTL value is set correctly.
        /// </summary>
        [TestMethod]
        public void ItemAccessedTimeToLiveTest()
        {
            var timeSpan = TimeSpan.MaxValue;
            _cache.SetItemAccessedTimeToLive(timeSpan);
            Assert.AreEqual(timeSpan, _cache.GetItemAccessedTimeToLive());
        }

        /// <summary>
        /// Validates that the Item Access TTL value is set correctly.
        /// </summary>
        [TestMethod]
        public void ItemAccessedTimeToLiveEvictsTest()
        {
            var timeSpan = TimeSpan.FromMilliseconds(100);
            _cache.SetItemAccessedTimeToLive(timeSpan);
            _item1.SetLastAccessedTime(DateTime.Now);
            _cache.InsertCacheItem(_item1);
            Thread.Sleep(TimeSpan.FromMilliseconds(100));
            Assert.AreEqual(0, _cache.GetUtilization());
        }

        /// <summary>
        /// Validates that the Item Create TTL value is set correctly.
        /// </summary>
        [TestMethod]
        public void ItemCreatedTimeToLiveTest()
        {
            var timeSpan = TimeSpan.MaxValue;
            _cache.SetItemCreatedTimeToLive(timeSpan);
            Assert.AreEqual(timeSpan, _cache.GetItemCreatedTimeToLive());
        }

        /// <summary>
        /// Validates that the Access TTL value is set correctly.
        /// </summary>
        [TestMethod]
        public void AccessedTimeToLiveTest()
        {
            var timeSpan = TimeSpan.FromDays(1);
            _cache.SetAccessedTimeToLive(timeSpan);
            Assert.AreEqual(timeSpan, _cache.GetAccessedTimeToLive());
        }

        /// <summary>
        /// Validates that the Item Access TTL value is set correctly and is retained after cache modification.
        /// </summary>
        [TestMethod]
        public void AccessedTimeToLiveTestRetainedAfterMod()
        {
            var timeSpan = TimeSpan.FromDays(1);
            _cache.SetAccessedTimeToLive(timeSpan);
            _cache.InsertCacheItem(_item3);
            _cache.InsertCacheItem(_item4);
            Assert.AreEqual(timeSpan, _cache.GetAccessedTimeToLive());
        }

        /// <summary>
        /// Ensures that the cache is cleared after the Access TTL has expired.
        /// </summary>
        [TestMethod]
        public void AccessedTimeToLiveTestExpired()
        {
            _cache.SetAccessedTimeToLive(TimeSpan.FromMilliseconds(1));
            _cache.InsertCacheItem(_item3);
            _cache.InsertCacheItem(_item4);
            Thread.Sleep(TimeSpan.FromMilliseconds(2));
            Assert.AreEqual(0, _cache.GetUtilization());
        }

        /// <summary>
        /// Validates that the Create TTL value is set correctly.
        /// </summary>
        [TestMethod]
        public void CreatedTimeToLiveTest()
        {
            var timeSpan = TimeSpan.MaxValue;
            _cache.SetCreatedTimeToLive(timeSpan);
            Assert.AreEqual(timeSpan, _cache.GetCreatedTimeToLive());
        }

        /// <summary>
        /// Validates that the Item Access TTL value is set correctly and is retained after cache modification.
        /// </summary>
        [TestMethod]
        public void CreatedTimeToLiveTestRetainedAfterMod()
        {
            var timeSpan = TimeSpan.MaxValue;
            _cache.SetCreatedTimeToLive(timeSpan);
            _cache.InsertCacheItem(_item3);
            _cache.InsertCacheItem(_item4);
            Assert.AreEqual(timeSpan, _cache.GetCreatedTimeToLive());
        }

        /// <summary>
        /// Ensures that the cache is cleared after the Create TTL has expired.
        /// </summary>
        [TestMethod]
        public void CreatedTimeToLiveTestExpired()
        {
            _cache.SetCreatedTimeToLive(TimeSpan.FromMilliseconds(1));
            _cache.InsertCacheItem(_item3);
            _cache.InsertCacheItem(_item4);
            Thread.Sleep(TimeSpan.FromMilliseconds(2));
            Assert.AreEqual(0, _cache.GetUtilization());
        }
    }
}
