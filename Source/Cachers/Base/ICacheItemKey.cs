namespace CacheFactory.Cachers.Base
{
    using System;

    /// <summary>
    /// An identifier for the cache record which you can compare.
    /// </summary>
    public interface ICacheItemKey : IEquatable<ICacheItemKey>
    {
    }
}
