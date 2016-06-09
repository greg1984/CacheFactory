namespace CacheFactory.CacheExceptions
{
    using System;

    public class InvalidCacheNameException : Exception
    {
        public InvalidCacheNameException(string cacheName, string failureReason) : base("Could not create cache " + cacheName, new ArgumentException(failureReason)) { }
    }
}
