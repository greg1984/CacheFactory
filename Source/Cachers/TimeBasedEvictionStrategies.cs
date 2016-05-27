namespace CacheFactory.Cachers
{
    /// <summary>
    /// Time Based Eviction Strategy Enumerations.
    /// </summary>
    public enum TimeBasedEvictionStrategies
    {
        /// <summary>
        /// FIFO Eviction Strategy.
        /// </summary>
        FirstInFirstOut = 0,

        /// <summary>
        /// FILO Eviction Strategy.
        /// </summary>
        FirstInLastOut = 1,

        /// <summary>
        /// LRU Strategy.
        /// </summary>
        LeastRecentlyUsed = 2
    }
}
