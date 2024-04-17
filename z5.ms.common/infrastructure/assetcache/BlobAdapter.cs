using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using z5.ms.common.assets.common;
using z5.ms.common.infrastructure.assetcache.config;

namespace z5.ms.common.infrastructure.assetcache
{
    /// <summary>Adapter to load SubscriptionPlan assets from the file system</summary>
    public class BlobAdapter<TAsset> : ICacheAdapter where TAsset : class, IAsset, new()
    {
        private readonly ILogger _logger;
        private readonly SyncOptions _settings;
        private readonly string _assetName;

        /// <inheritdoc />
        public BlobAdapter(IOptions<SyncOptions> options, ILoggerFactory loggerFactory)
        {
            _assetName = typeof(TAsset).Name.Replace("Entity", "");
            _settings = options.Value;
            _logger = loggerFactory.CreateLogger($"{_assetName}BlobAdapter");
            _logger.LogTrace($"{_assetName} blob adapter created");
        }

        /// <inheritdoc />
        public int AssetType => (int)(AssetType)Enum.Parse(typeof(AssetType), _assetName);

        /// <inheritdoc />
        public async Task<CacheEntry> Load(string assetPath, string assetId)
        {
            _logger.LogTrace($"Loading asset {assetPath}");

            var assetFile = AzureStorage.CombinePath(assetPath, $"{assetId}.json");

            // 1. get the default asset (netural language)
            var publishedAsset = await AzureStorage.GetAsset<TAsset>(_settings, assetFile, _logger);

            if (publishedAsset == null)
            {
                _logger.LogInformation($"Failed to load asset from {assetPath}");
                return null;
            }

            // 2. get translated assets (if there are any)
            var publishedTranslations = await AzureStorage.GetTranslatedAssets<TAsset>(_settings, assetPath);

            var cacheEntry = new CacheEntry(publishedAsset, publishedTranslations.ToIassetDictionary());

            _logger.LogTrace($"Loaded asset {assetPath}");
            return cacheEntry;
        }
    }
}