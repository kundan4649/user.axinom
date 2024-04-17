using System.Threading.Tasks;

namespace z5.ms.common.infrastructure.assetcache
{
    /// <inheritdoc />
    /// <summary>In-memory assets storage</summary>
    public interface IAssetsCache : IAssetsCacheReader
    {
        /// <summary>Clear all cached assets</summary>
        void Clear();

        /// <summary>
        /// Add asset to cache.
        /// Detects asset type from a conventional asset ID and uses a registered adapter to load asset.
        /// </summary>
        /// <param name="path">Path to asset directory</param>
        /// <param name="id">Unique asset ID</param>
        Task AddAsset(string path, string id);

        /// <summary>
        /// Add asset to cache.
        /// Uses a registered adapter for selected asset type to load asset.
        /// </summary>
        /// <param name="path">Path to asset directory</param>
        /// <param name="id">Unique asset ID</param>
        /// <param name="assetType">Asset type from AssetType enumeration</param>
        Task AddAsset(string path, string id, int assetType);

        /// <summary>Add translated asset to cache</summary>
        /// <param name="asset">Asset to add</param>
        void AddAsset(CacheEntry asset);

        /// <summary>Remove asset from cache</summary>
        /// <param name="id">Unique asset ID</param>
        void RemoveAsset(string id);
        
        /// <summary>Register new cache update subscriber</summary>
        /// <typeparam name="TListener">Type of the subscriber, implementing ICacheUpdateListener interface</typeparam>
        /// <param name="listener">Cacher update subscriber instance</param>
        void RegisterListener<TListener>(TListener listener) where TListener : ICacheUpdateListener;
    }
}