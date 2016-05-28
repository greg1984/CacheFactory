namespace CacheFactoryTest.Cachers
{

    using CacheFactory.Cachers;
    using CacheFactory.Cachers.Base;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using TestDTOs;

    /// <summary>
    /// Summary description for CacheFactoryTest
    /// </summary>
    [TestClass]
    public class CacheEvictorTests
    {
        private Dictionary<GenuineKey, CacheItem<GenuineKey>> _cache;
        private CacheItem<GenuineKey> _item1;
        private CacheItem<GenuineKey> _item2;
        private CacheItem<GenuineKey> _item3;
        private CacheItem<GenuineKey> _item4;

        [TestInitialize]
        public void MyTestInitialize()
        {
            _cache = new Dictionary<GenuineKey, CacheItem<GenuineKey>>();
            _item1 = new CacheItem<GenuineKey>(DateTime.Now.AddMinutes(-4), new GenuineKey());
            Thread.Sleep(TimeSpan.FromMilliseconds(3));
            _item2 = new CacheItem<GenuineKey>(DateTime.Now.AddMinutes(-3), new GenuineKey());
            Thread.Sleep(TimeSpan.FromMilliseconds(3));
            _item3 = new CacheItem<GenuineKey>(DateTime.Now.AddMinutes(-2), new GenuineKey());
            Thread.Sleep(TimeSpan.FromMilliseconds(3));
            _item4 = new CacheItem<GenuineKey>(DateTime.Now.AddMinutes(-1), new GenuineKey());
            _cache.Add(_item1.Key, _item1);
            _cache.Add(_item2.Key, _item2);
            _cache.Add(_item3.Key, _item3);
            _cache.Add(_item4.Key, _item4);
        }

        [TestMethod]
        public void GetLRUTest()
        {
            _item3.SetLastAccessed(DateTime.Now.AddDays(-1));

            var keyToBeEvicted = CacheEvictor<CacheItem<GenuineKey>, GenuineKey>.GetLRU(_cache);
            Assert.AreEqual(keyToBeEvicted.Key, _item3.Key, keyToBeEvicted + "<>" + _item3.Key);
        }

        [TestMethod]
        public void GetLatestInsertedItemTest()
        {
            var keyToBeEvicted = CacheEvictor<CacheItem<GenuineKey>, GenuineKey>.GetLatestInsertedItem(_cache);
            Assert.AreEqual(keyToBeEvicted.Key, _item4.Key, keyToBeEvicted + "<>" + _item4.Key);
        }

        [TestMethod]
        public void GetOldestInsertedItemTest()
        {
            var keyToBeEvicted = CacheEvictor<CacheItem<GenuineKey>, GenuineKey>.GetOldestInsertedItem(_cache);
            Assert.AreEqual(keyToBeEvicted.Key, _item1.Key, keyToBeEvicted + "<>" + _item1.Key);
        }
    }
}
