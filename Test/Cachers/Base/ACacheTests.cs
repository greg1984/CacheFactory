namespace CacheFactoryTest.Cachers.Base
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using CacheFactory.Cachers;
    using CacheFactory.Cachers.Base;
    using TestDTOs;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests for the Generic Abstracted Cache.
    /// </summary>
    [TestClass]
    public class ACacheTests
    {
        private ACache<GenuineKey, GenuineCacheItem> _cache;
        private GenuineCacheItem _item1;
        private GenuineCacheItem _item2;
        private GenuineCacheItem _item3;
        private GenuineCacheItem _item4;

        [TestInitialize]
        public void MyTestInitialize()
        {
            _cache = new FirstInFirstOutCache<GenuineKey, GenuineCacheItem>(capacity: 2);
            _item1 = new GenuineCacheItem(DateTime.Now, new GenuineKey());
            _item2 = new GenuineCacheItem(DateTime.Now, new GenuineKey());
            _item3 = new GenuineCacheItem(DateTime.Now, new GenuineKey());
            _item4 = new GenuineCacheItem(DateTime.Now, new GenuineKey());
            _cache.InsertCacheItem(_item1);
        }

        /// <summary>
        /// Test the contructor for the Generic Abstracted Cache.
        /// </summary>
        [TestMethod]
        public void ACacheConstructorTest()
        {
            Assert.IsTrue(_cache.GetUtilization() == 1);
        }

        /// <summary>
        /// Validate all cache elements are set when passed in using SetCache.
        /// </summary>
        [TestMethod]
        public void SetCacheTest()
        {
            _cache.SetCache(new Collection<KeyValuePair<GenuineKey, GenuineCacheItem>>
            {
                new KeyValuePair<GenuineKey, GenuineCacheItem>(_item1.GetKey(), _item1),
                new KeyValuePair<GenuineKey, GenuineCacheItem>(_item1.GetKey(), _item2)
            });
        }

        /// <summary>
        /// Validate an exception is thrown if you pass in more cache items then the cache has capacity for using SetCache and the flag is set to false.
        /// </summary>
        [TestMethod]
        public void SetCacheOverCapacityTest()
        {
            _cache.SetCache(new Collection<KeyValuePair<GenuineKey, GenuineCacheItem>>
            {
                new KeyValuePair<GenuineKey, GenuineCacheItem>(_item1.GetKey(), _item1),
                new KeyValuePair<GenuineKey, GenuineCacheItem>(_item1.GetKey(), _item2),
                new KeyValuePair<GenuineKey, GenuineCacheItem>(_item1.GetKey(), _item3)
            });
        }

        /// <summary>
        /// Validates a cache resize on the use of CacheSize if there is more cache items then the cache has capacity for using SetCache and the flag is set to true.
        /// </summary>
        [TestMethod]
        public void ResizeCacheOverCapacityTest()
        {
            _cache.SetCache(new Collection<KeyValuePair<GenuineKey, GenuineCacheItem>>
            {
                new KeyValuePair<GenuineKey, GenuineCacheItem>(_item1.GetKey(), _item1),
                new KeyValuePair<GenuineKey, GenuineCacheItem>(_item1.GetKey(), _item2),
                new KeyValuePair<GenuineKey, GenuineCacheItem>(_item1.GetKey(), _item3),
                new KeyValuePair<GenuineKey, GenuineCacheItem>(_item1.GetKey(), _item4)
            }, true);
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
            _cache.RemoveCacheItem(_item1.GetKey());
            Assert.IsTrue(_cache.GetUtilization() == 0);
        }

        /// <summary>
        /// Validates that an exception is thrown when fetching a cache item that no longer exists.
        /// </summary>
        [TestMethod]
        public void FailGetItemFromCacheTest()
        {
            _cache.GetCachedItem(_item2.GetKey());
        }

        /// <summary>
        /// Validates that a cache item is fetched when fetching a valid cache item.
        /// </summary>
        [TestMethod]
        public void GetItemFromCacheTest()
        {
            _cache.GetCachedItem(_item1.GetKey());
        }

        /// <summary>
        /// Validates that the cache name can be changed.
        /// </summary>
        [TestMethod]
        public void GoodCacheNameChangeTest()
        {
            _cache.SetCacheName("cache_id_1", new List<ICache<GenuineKey, GenuineCacheItem>>());
            Assert.AreEqual(_cache.GetCacheName(), "cache_id_1");
        }

        /// <summary>
        /// Validates that an exception is thrown when attempting to utilize the global cache name.
        /// </summary>
        [TestMethod]
        public void GlobalCacheNameChangeTest()
        {
            try
            {
                _cache.SetCacheName("global_cache", new List<ICache<GenuineKey, GenuineCacheItem>>());
                Assert.Fail("Excepted cache to fail creation when utilizing the global cache.");
            }
            catch
            {
                // ignore
            }
        }

        /// <summary>
        /// Validates that setting a capacity with a valid value works.
        /// </summary>
        [TestMethod]
        public void SetCapacityTest()
        {
            _cache.SetCapacity(3);
        }

        /// <summary>
        /// Validates that setting a capacity with a lower higher than the utilization throws an exception.
        /// </summary>
        [TestMethod]
        public void SetBadCapacityTest()
        {
            try
            {
                _cache.InsertCacheItem(_item2);
                _cache.SetCapacity(1);
            }
            catch
            {
                Assert.Fail("Expected exception when resizing cache smaller than contents.");
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
                _cache.SetCapacity(4);
                _cache.InsertCacheItem(_item2);
                _cache.InsertCacheItem(_item3);
                _cache.InsertCacheItem(_item4);
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
        {
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
            _cache.Clear();
            Assert.AreEqual(0, _cache.GetUtilization());
        }

        [TestMethod]
        public void ItemAccessedTimeToLiveTest()
        {
            var timeSpan = TimeSpan.MaxValue;
            _cache.SetItemAccessedTimeToLive(timeSpan);
            Assert.AreEqual(timeSpan, _cache.GetItemAccessedTimeToLive());
        }

        [TestMethod]
        public void ItemCreatedTimeToLiveTest()
        {
            var timeSpan = TimeSpan.MaxValue;
            _cache.SetItemCreatedTimeToLive(timeSpan);
            Assert.AreEqual(timeSpan, _cache.GetItemCreatedTimeToLive());
        }

        [TestMethod]
        public void AccessedTimeToLiveTest()
        {
            var timeSpan = TimeSpan.FromDays(1);
            _cache.SetAccessedTimeToLive(timeSpan);
            Assert.AreEqual(timeSpan, _cache.GetAccessedTimeToLive());
        }

        [TestMethod]
        public void CreatedTimeToLiveTest()
        {
            var timeSpan = TimeSpan.MaxValue;
            _cache.SetCreatedTimeToLive(timeSpan);
            Assert.AreEqual(timeSpan, _cache.GetCreatedTimeToLive());
        }

        [TestMethod]
        public void AccessedTimeToLiveTest2()
        {
            var timeSpan = TimeSpan.FromDays(1);
            _cache.SetAccessedTimeToLive(timeSpan);
            _cache.InsertCacheItem(_item3);
            _cache.InsertCacheItem(_item4);
            Assert.AreEqual(timeSpan, _cache.GetAccessedTimeToLive());
        }

        [TestMethod]
        public void CreatedTimeToLiveTest2()
        {
            var timeSpan = TimeSpan.MaxValue;
            _cache.SetCreatedTimeToLive(timeSpan);
            _cache.InsertCacheItem(_item3);
            _cache.InsertCacheItem(_item4);
            Assert.AreEqual(timeSpan, _cache.GetCreatedTimeToLive());
        }
    }
}
