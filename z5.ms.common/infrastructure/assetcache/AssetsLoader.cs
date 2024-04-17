using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using z5.ms.common.infrastructure.assetcache.config;

namespace z5.ms.common.infrastructure.assetcache
{
    /// <inheritdoc />
    public class AssetsLoader : IAssetsLoader
    {
        private readonly IAssetsCache _cache;
        private readonly ICacheAdaptersFactory _adaptersFactory;
        private readonly SyncOptions _settings;
        private readonly ILogger _logger;
        
        private readonly string _syncPrefix = ".$";
        private int _assetCount;
        private int _assetCountLoaded;

        /// <inheritdoc />
        public AssetsLoader(IAssetsCache cache, ICacheAdaptersFactory adaptersFactory, IOptions<SyncOptions> options, ILoggerFactory loggerFactory)
        {
            _cache = cache;
            _adaptersFactory = adaptersFactory;
            _settings = options.Value;
            _logger = loggerFactory.CreateLogger("AssetsLoader");
            
            _syncPrefix = _syncPrefix.Replace("$", _settings.AssetNameTempChar);
        }

        /// <inheritdoc />
        public async Task LoadAll()
        {
            _logger.LogInformation("Loading all assets");
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            // TODO: log connection issues
            List<AssetDirectory> assetDirectories = null;
            try
            {
                assetDirectories = (await AzureStorage.GetAssetDirectories(_settings, _syncPrefix))
                    .Where(a => AdapterRegistered(a.AssetType)).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Failed to load assets - {ex.Message}");
                Environment.Exit(1);
            }

            //add menu first, then other assets
            //var menu = assetDirectories.FirstOrDefault(d => d.AssetType == AssetType.Menu);
            //if (menu != null)
            //{
            //    _logger.LogDebug($"Adding menu asset {menu.Path}");
            //    AddAsset(menu.Path, menu.AssetId, menu.AssetType);
            //}
            
            if (!assetDirectories.Any())
            {
                stopwatch.Stop();
                _logger.LogDebug($"No assets loaded, {stopwatch.Elapsed} ms elapsed");
                return;
            }

            _assetCount = assetDirectories.Count;
            _assetCountLoaded = 0;

            //and then all remaining assets
            foreach (var dir in assetDirectories) //.Where(d => d.AssetId != menu?.AssetId && d.AssetType != AssetType.MenuList))
            {
                await _cache.AddAsset(dir.Path, dir.AssetId, dir.AssetType);
                _assetCountLoaded++;
            }

            stopwatch.Stop();
            _logger.LogInformation($"Assets loaded in {stopwatch.Elapsed}");
        }

        private bool AdapterRegistered(int assetType) => _adaptersFactory.GetAdapter(assetType) != null;

        /// <inheritdoc />
        public double Progress => _assetCount == 0 ? 0 : 100.0 * _assetCountLoaded / _assetCount;

		/// <inheritdoc />
        public int Location => _settings.CurrentStorageConnection;
    }
}