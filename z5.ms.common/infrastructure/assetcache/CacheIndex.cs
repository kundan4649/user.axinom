using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace z5.ms.common.infrastructure.assetcache
{
    /// <summary>An index for cache entries using an untranslated key for lookups</summary>
    /// <typeparam name="TKey"></typeparam>
    public class CacheIndex<TKey>
    {
        private readonly ConcurrentDictionary<TKey, ConcurrentDictionary<string, CacheEntry>> _index = new ConcurrentDictionary<TKey, ConcurrentDictionary<string, CacheEntry>>();

        /// <summary>Add a item to the cache</summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        protected void Add(TKey key, CacheEntry value)
        {
            var list = _index.GetOrAdd(key, k => new ConcurrentDictionary<string, CacheEntry>());
            list.AddOrUpdate(value.Id, value, (k, v) => value);
        }

        /// <summary>Remove an item from the cache</summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        protected void Remove(TKey key, CacheEntry value)
        {
            var list = _index.GetOrAdd(key, k => new ConcurrentDictionary<string, CacheEntry>());
            list.TryRemove(value.Id, out var _);
        }

        /// <summary>Fetch a list of transalted assets from the cache</summary>
        /// <param name="key"></param>
        /// <param name="translation"></param>
        /// <returns></returns>
        protected IEnumerable<IAsset> Lookup(TKey key, string translation = null)
        {
            if (!_index.TryGetValue(key, out var list)) return new List<IAsset>();
            return list.Values.Select(x => x.GetTranslation(translation)).Where(x => x != null);
        }

        /// <summary>Query if a given asset belongs to a given index key</summary>
        /// <param name="key"></param>
        /// <param name="assetId"></param>
        /// <returns></returns>
        public bool BelongsTo(string assetId, TKey key) => _index.TryGetValue(key, out var list) && list.ContainsKey(assetId);
    }

    /// <summary>A cache index using an untranslated key for lookups and a key for ordering</summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TOrder"></typeparam>
    public class OrderedCacheIndex<TKey, TOrder> where TOrder : IComparable<TOrder>
    {
        private readonly ConcurrentDictionary<TKey, SortedList<TOrder, CacheEntry>> _index = new ConcurrentDictionary<TKey, SortedList<TOrder, CacheEntry>>();

        /// <summary>Add a item to the cache</summary>
        /// <param name="key"></param>
        /// <param name="position"></param>
        /// <param name="value"></param>
        protected void Add(TKey key, TOrder position, CacheEntry value)
        {
            var list = _index.GetOrAdd(key, k => new SortedList<TOrder, CacheEntry>());
            lock (list) list.Add(position, value);
        }

        /// <summary>Remove an item from the cache</summary>
        /// <param name="key"></param>
        /// <param name="position"></param>
        protected void Remove(TKey key, TOrder position)
        {
            var list = _index.GetOrAdd(key, k => new SortedList<TOrder, CacheEntry>());
            lock (list) list.Remove(position);
        }

        /// <summary>Fetch a list of translated assets from the cache</summary>
        /// <param name="key"></param>
        /// <param name="translation"></param>
        /// <returns></returns>
        protected IEnumerable<IAsset> Lookup(TKey key, string translation = null)
        {
            if (!_index.TryGetValue(key, out var list)) return new List<IAsset>();
            IEnumerable<CacheEntry> values;
            lock (list) values = list.Select(x => x.Value).ToList();
            return values.Select(x => x.GetTranslation(translation)).Where(x => x != null);
        }
    }
}