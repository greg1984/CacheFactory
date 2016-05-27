namespace CacheFactory
{
    /// <summary>
    /// The different cache types that have been loaded into the library by default.
    /// </summary>
    public enum CacheTypes
    {
        /// <summary>
        /// Enumerator for the FIFO Cache.
        /// </summary>
        FirstInFirstOut = 0,

        /// <summary>
        /// Enumerator for the FILO Cache.
        /// </summary>
        FirstInLastOut = 1,

        /// <summary>
        /// Enumerator for the LRU Cache.
        /// </summary>
        LeastRecentlyUsed = 2,

        /// <summary>
        /// Enumerator for the TTL Cache.
        /// </summary>
        TimeBasedEviction = 3
    }
}
