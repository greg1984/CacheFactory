namespace CacheFactory.Cachers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using CacheFactory.Cachers.Base;
    using CacheFactory.Cachers.CacheFilters;

    class ClassReflector<TCacheItem, TCacheItemKey, TCache, TCacheFilter>
        where TCacheItemKey : CacheItemKey
        where TCacheItem : CacheItem<TCacheItemKey>
        where TCacheFilter : ICacheFilter<TCacheItem, TCacheItemKey>
        where TCache : ICache<TCacheItem, TCacheItemKey, TCacheFilter>
    {

        /// <summary>
        /// A private function which enables us to load classes from memory.
        /// </summary>
        /// <returns>Return the instances of a specific cache type.</returns>
        public static IEnumerable<TCache> GetInstances(Type type)
        {
            return (from t in Assembly.GetExecutingAssembly().GetTypes()
                    where t.BaseType == type && t.GetConstructor(Type.EmptyTypes) != null
                    select (TCache)Activator.GetObject(t, "")).ToList();
        }
    }
}
