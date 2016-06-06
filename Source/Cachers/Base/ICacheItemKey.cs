using System;

namespace CacheFactory.Cachers.Base
{
    /// <summary>
    /// An identifier for the cache record which you can compare.
    /// </summary>
    public interface ICacheItemKey : IEquatable<ICacheItemKey>
    {
    }
}
