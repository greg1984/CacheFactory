namespace CacheFactoryTest.Cachers.Base
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using TestDTOs;

    /// <summary>
    /// Summary description for CacheFactoryTest
    /// </summary>
    [TestClass]
    public class CacheItemKeyTests
    {
        /// <summary>
        /// A function to compare the hash value of the key to another item key.
        /// </summary>
        [TestMethod]
        public void EqualsTest()
        {
            var key1 = new GenuineKey();
            var key2 = new GenuineKey();

            Assert.IsFalse(key1.Equals(key2));
        }
    }
}
