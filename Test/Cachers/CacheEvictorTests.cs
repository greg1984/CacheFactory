namespace CacheFactoryTest.Cachers
{
    using CacheFactory.Cachers;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using TestDTOs;

    /// <summary>
    /// A test class to ensure the Cache Evictor works correctly.
    /// </summary>
    [TestClass]
    public class CacheEvictorTests
    {
        private Dictionary<GenuineKey, GenuineCacheItem> _cache;
        private GenuineCacheItem _item1;
        private GenuineCacheItem _item2;
        private GenuineCacheItem _item3;
        private GenuineCacheItem _item4;

        [TestInitialize]
        public void MyTestInitialize()
        {
            _cache = new Dictionary<GenuineKey, GenuineCacheItem>();
            _item1 = new GenuineCacheItem(DateTime.Now.AddMinutes(-4), new GenuineKey());
            Thread.Sleep(TimeSpan.FromMilliseconds(3));
            _item2 = new GenuineCacheItem(DateTime.Now.AddMinutes(-3), new GenuineKey());
            Thread.Sleep(TimeSpan.FromMilliseconds(3));
            _item3 = new GenuineCacheItem(DateTime.Now.AddMinutes(-2), new GenuineKey());
            Thread.Sleep(TimeSpan.FromMilliseconds(3));
            _item4 = new GenuineCacheItem(DateTime.Now.AddMinutes(-1), new GenuineKey());
            _cache.Add(_item1.GetKey(), _item1);
            _cache.Add(_item2.GetKey(), _item2);
            _cache.Add(_item3.GetKey(), _item3);
            _cache.Add(_item4.GetKey(), _item4);
        }

        /// <summary>
        /// Validate that the evictor returns the correct LRU object.
        /// </summary>
        [TestMethod]
        public void GetLRUTest()
        {
            _item3.SetLastAccessedTime(DateTime.Now.AddDays(-1));

            var keyToBeEvicted = CacheEvictor<GenuineKey, GenuineCacheItem>.GetLRU(_cache);
            Assert.AreEqual(keyToBeEvicted.GetKey(), _item3.GetKey(), keyToBeEvicted + "<>" + _item3.GetKey());
        }

        /// <summary>
        /// Validate that the evictor returns the most recently inserted item.
        /// </summary>
        [TestMethod]
        public void GetLatestInsertedItemTest()
        {
            var keyToBeEvicted = CacheEvictor<GenuineKey, GenuineCacheItem>.GetLatestInsertedItem(_cache);
            Assert.AreEqual(keyToBeEvicted.GetKey(), _item4.GetKey(), keyToBeEvicted + "<>" + _item4.GetKey());
        }

        /// <summary>
        /// Validate that the evictor returns the least recently inserted item.
        /// </summary>
        [TestMethod]
        public void GetOldestInsertedItemTest()
        {
            var keyToBeEvicted = CacheEvictor<GenuineKey, GenuineCacheItem>.GetOldestInsertedItem(_cache);
            Assert.AreEqual(keyToBeEvicted.GetKey(), _item1.GetKey(), keyToBeEvicted + "<>" + _item1.GetKey());
        }
    }
}
