using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace z5.ms.common.infrastructure.assetcache
{
    /// <inheritdoc />
    public class AssetCacheReader : IAssetsCacheReader
    {
        private readonly TypeIndex _typeIndex;

        /// <inheritdoc />
        public AssetCacheReader(TypeIndex typeIndex)
        {
            _typeIndex = typeIndex;
        }

        /// <summary>General purpose logger</summary>
        protected ILogger Logger;
        
        /// <summary>In memory asset store</summary>
        protected readonly ConcurrentDictionary<string, CacheEntry> Assets = new ConcurrentDictionary<string, CacheEntry>();
        
        /// <inheritdoc />
        public IAsset Get(string assetId, string language = null) => Get<IAsset>(assetId, language);

        /// <inheritdoc />
        public TAsset Get<TAsset>(string assetId, string language = null) where TAsset : class, IAsset
            => Assets.TryGetValue(assetId, out var translatableAsset)
                ? ((TAsset)translatableAsset.GetTranslation(language))?.GetAssetClone()
                : null;

        /// <inheritdoc />
        public IEnumerable<IAsset> Select(string language = null) 
            => Select<IAsset>(arg => true, language);

        /// <inheritdoc />
        public IEnumerable<TAsset> Select<TAsset>(string language = null) where TAsset : class, IAsset 
            => Select<TAsset>(arg => true, language);

        /// <inheritdoc />
        public IEnumerable<IAsset> Select(Func<IAsset, bool> prediate, string language = null) 
            => Select<IAsset>(prediate, language);

        private IEnumerable<TAsset> GetAssetTranslations<TAsset>(string language = null) where TAsset : class, IAsset
            => _typeIndex != null && _typeIndex.SupportsType<TAsset>() ? _typeIndex.Lookup<TAsset>(language)
                    : Assets
                        .Select(a => a.Value.GetTranslation(language))
                        .OfType<TAsset>();

        /// <inheritdoc />
        public IEnumerable<TAsset> Select<TAsset>(Func<TAsset, bool> predicate, string language = null)
            where TAsset : class, IAsset
            => GetAssetTranslations<TAsset>(language)
                .Where(predicate)
                .Select(a => a.GetAssetClone());

        /// <inheritdoc />
        public IEnumerable<TAsset> Select<TAsset>(Func<TAsset, bool> prediate, SortParam sort, PagingParam paging,
            string language = null)
            where TAsset : class, IAsset
            => GetAssetTranslations<TAsset>(language)
                .Where(prediate)
                .SortBy(sort)
                .PageBy(paging, out var _)
                .Select(a => a.GetAssetClone());

        /// <inheritdoc />
        public (IEnumerable<TAsset> assets, int total) Select<TAsset>(IQueryFilter filter, SortParam sort, PagingParam paging)
            where TAsset : class, IAsset, IFilterable
        {
            var result = GetAssetTranslations<TAsset>(filter.Translation)
                .FilterBy(filter)
                .SortBy(sort)
                .PageBy(paging, out var total)
                .Select(a => a.GetAssetClone());

            return (result, total);
        }

        /// <inheritdoc />
        public TAsset FirstOrDefault<TAsset>(Func<TAsset, bool> prediate, string language = null)
            where TAsset : class, IAsset
            => Select(prediate, language).FirstOrDefault();

        /// <inheritdoc />
        public int Count<TAsset>(Func<TAsset, bool> prediate, string language = null) where TAsset : class, IAsset
            => _typeIndex != null && _typeIndex.SupportsType<TAsset>() ? _typeIndex.Lookup<TAsset>(language).Count(a => prediate(a))
                : Assets
                    .Select(a => a.Value.GetTranslation(language))
                    .Count(a => a != null && a.GetType() == typeof(TAsset) && prediate((TAsset)a));

        /// <inheritdoc />
        public bool Exists(string assetId) => Assets.ContainsKey(assetId);
    }

    /// <inheritdoc cref="IAssetsCache" />
    /// <summary>
    /// Default in-memory assets storage.
    /// Uses adapters to load assets from various asset sources (ext4, NTFS, Azure blob storage, etc.)
    /// </summary>
    public class AssetsCache : AssetCacheReader, IAssetsCache
    {
        private readonly ICacheAdaptersFactory _cacheAdapters;
        
        //private readonly Dictionary<int, ICacheAdapter> _adapters = new Dictionary<int, ICacheAdapter>();
        private readonly IList<ICacheUpdateListener> _listeners = new List<ICacheUpdateListener>();

        /// <summary>Default AssetsCache constructor with injected dependencies</summary>
        public AssetsCache(ICacheAdaptersFactory cacheAdapters, ILoggerFactory loggerFactory, CacheIndexRegistry indexRegistry = null, TypeIndex typeIndex = null) : base(typeIndex)
        {
            _cacheAdapters = cacheAdapters;
            Logger = loggerFactory.CreateLogger("AssetsCache");

            Logger.LogDebug("AssetCache created.");

            indexRegistry?.Indexes.ToList().ForEach(RegisterListener);
        }

        /// <inheritdoc />
        public void Clear() { Assets.Clear(); }

        /// <inheritdoc />
        public virtual async Task AddAsset(string path, string id) { await AddAsset(path, id, id.GetDirectoryAssetType()); }

        /// <inheritdoc />
        public async Task AddAsset(string path, string id, int assetType)
        {
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(path) || assetType == 99)
            {
                Logger.LogInformation($"Failed to add asset - invalid data (id: {id} path: {path} type: {assetType})");
                return;
            }

            Logger.LogTrace($"Adding asset {id} of type {assetType} at {path}");

            try
            {
                var adapter = _cacheAdapters.GetAdapter(assetType);
                if (adapter == null)
                {
                    Logger.LogInformation($"Failed to load asset {id} from {path} - loader not registered");
                    return;
                }

                var asset = await adapter.Load(path, id);
                if (asset == null)
                {
                    Logger.LogInformation($"Failed to load asset {id} from {path} with loader {adapter.GetType().Name}");
                    return;
                }

                AddAsset(asset);
                Logger.LogDebug($"Added asset {id}");
            }
            catch (Exception ex)
            {
                Logger.LogInformation(ex, $"Failed to load asset {id} from {path}");
            }
        }

        /// <inheritdoc />
        public virtual void AddAsset(CacheEntry asset)
        {
            Assets.AddOrUpdate(asset.Id, asset, (key, value) => asset);

            foreach (var listener in _listeners)
            {
                try
                {
                    listener.OnAdded(asset);
                }
                catch (Exception ex)
                {
                    Logger.LogWarning(ex, "Exception in cache updates listener");
                }
            }
        }

        /// <inheritdoc />
        public virtual void RemoveAsset(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                Logger.LogInformation("Failed to remove asset - invalid asset data");
                return;
            }

            Logger.LogTrace($"Removing asset {id}");

            try
            {
                if (!Assets.TryRemove(id, out var translatableAsset) || translatableAsset == null)
                {
                    Logger.LogInformation($"Failed to remove asset {id}");
                    return;
                }
                
                Logger.LogDebug($"Removed asset {id}");

                foreach (var listener in _listeners)
                {
                    try
                    {
                        listener.OnRemoved(translatableAsset);
                    }
                    catch (Exception ex)
                    {
                        Logger.LogWarning(ex, "Exception in cache updates listener");
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogInformation(ex, $"Failed to remove asset {id}");
            }
        }

        /// <inheritdoc />
        public void RegisterListener<TListener>(TListener listener) where TListener : ICacheUpdateListener
        {
            Logger.LogDebug($"Adding cache update listener: {listener.GetType().Name}.");
            _listeners.Add(listener);
        }
    }
}