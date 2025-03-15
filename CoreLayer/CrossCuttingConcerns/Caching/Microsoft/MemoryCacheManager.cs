using CoreLayer.Utilities.IoC;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace CoreLayer.CrossCuttingConcerns.Caching.Microsoft
{
    /// <summary>
    /// //Adapter Pattern
    /// </summary>
    public class MemoryCacheManager : ICacheManager
    {
        private readonly IMemoryCache _memoryCache;

        public MemoryCacheManager()
        {
            _memoryCache = ServiceTool.ServiceProvider.GetService<IMemoryCache>();
        }

        public void Add(string key, object value, int duration)
        {
            _memoryCache.Set(key, value, TimeSpan.FromMinutes(duration));
        }

        public T Get<T>(string key)
        {
            return _memoryCache.Get<T>(key);
        }

        public object Get(string key)
        {
            return _memoryCache.Get(key);
        }

        public bool IsAdd(string key)
        {
            return _memoryCache.TryGetValue(key, out _);
        }

        public void Remove(string key)
        {
            _memoryCache.Remove(key);
        }

        public void RemoveByPattern(string pattern)
        {
            FieldInfo coherentState = typeof(MemoryCache).GetField("_coherentState", BindingFlags.NonPublic | BindingFlags.Instance);

            object coherentStateValue = coherentState.GetValue(_memoryCache);

            PropertyInfo entriesCollection = coherentStateValue.GetType().GetProperty("StringEntriesCollection", BindingFlags.NonPublic | BindingFlags.Instance);

            ICollection entriesCollectionValue = entriesCollection.GetValue(coherentStateValue) as ICollection;

            List<ICacheEntry> cacheCollectionValues = new();

            foreach (var cacheItem in entriesCollectionValue)
            {
                ICacheEntry cacheItemValue = (ICacheEntry)cacheItem.GetType().GetProperty("Value").GetValue(cacheItem, null);
                cacheCollectionValues.Add(cacheItemValue);
            }

            Regex regex = new Regex(pattern, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
            HashSet<object> keysToRemove = [.. cacheCollectionValues.Where(d => regex.IsMatch(d.Key.ToString())).Select(d => d.Key)];

            foreach (var key in keysToRemove)
            {
                _memoryCache.Remove(key);
            }
        }
    }
}
