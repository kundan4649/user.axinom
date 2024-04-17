using System.Threading.Tasks;

namespace z5.ms.common.infrastructure.assetcache
{
    /// <summary>Adapter to translate file-system assets into a managed DTO that is served to client apps</summary>
    public interface ICacheAdapter
    {
        /// <summary>Asset type of the adapter from AssetType enumeration</summary>
        int AssetType { get; }

        /// <summary>
        /// Load asset from a specified path
        /// </summary>
        /// <param name="assetPath">Path in the file system</param>
        /// <param name="assetId">Unique asset ID</param>
        /// <returns>Asset wrapper model with all asset translations</returns>
        Task<CacheEntry> Load(string assetPath, string assetId);
    }

    /// <inheritdoc />
    public class TestAdapter : ICacheAdapter
    {
        /// <inheritdoc />
        public int AssetType { get; } = 99;

        /// <inheritdoc />
        public Task<CacheEntry> Load(string assetPath, string assetId)
        {
            throw new System.NotImplementedException();
        }
    }
}