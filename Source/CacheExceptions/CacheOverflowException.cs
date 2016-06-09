namespace CacheFactory.CacheExceptions
{
    using System;

    public class CacheOverflowException : Exception
    {
        public CacheOverflowException(string cacheName, string failureReason) : base("Could not create cache " + cacheName, new ArgumentException(failureReason)) { }
    }
}
