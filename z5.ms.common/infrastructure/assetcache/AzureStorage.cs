using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using z5.ms.common.extensions;
using z5.ms.common.infrastructure.assetcache.config;

namespace z5.ms.common.infrastructure.assetcache
{
    // TODO: difficult to test, make testable
    /// <summary>Azure blob storage access logic.</summary>
    /// <remarks>Specific to asset loading</remarks>
    public static class AzureStorage
    {
        private static readonly BlobRequestOptions BlobRequestOptions =
            new BlobRequestOptions {DisableContentMD5Validation = true};

        /// <summary>Build paths from path segments.</summary>
        /// <param name="paths">Path segments to join</param>
        /// <returns>Complete path</returns>
        public static string CombinePath(params string[] paths) 
            => paths.Aggregate(string.Empty, (cur, path) => cur + $"{path.Trim('/')}/").Trim('/');

        /// <summary>List all blobs in a container.</summary>
        /// <param name="cfg">Azure storage access settings</param>
        /// <param name="prefix">Blob name prefix</param>
        /// <returns>List of all blob names in a container</returns>
        private static async Task<IEnumerable<string>> ListBlobs(SyncOptions cfg, string prefix = "")
        {
            var blobContainer = GetClient(cfg.PublishStorageConnection).GetContainerReference(cfg.PublishContainerName);
            BlobContinuationToken token = null;
            var results = new List<string>();
            do
            {
                var blobs = await blobContainer.ListBlobsSegmentedAsync(prefix, true, BlobListingDetails.None, null, token, BlobRequestOptions, new OperationContext());
                token = blobs.ContinuationToken;
                results = results.Concat(blobs.Results.Select(x => StripContainerName(x.Uri.AbsolutePath, cfg.PublishContainerName))).ToList();
            } while (token != null);
            
            return results;
        }

        /// <summary>Download a blob.</summary>
        /// <param name="cfg">Azure storage access settings</param>
        /// <param name="path">Blob name</param>
        /// <param name="logger">Optional logger</param>
        /// <returns>Returns an open stream to the blob or null in case of failure</returns>
        public static async Task<T> GetAsset<T>(SyncOptions cfg, string path, ILogger logger = null) where T : class, IAsset, new()
        {
            try
            {
                var file = GetClient(cfg.PublishStorageConnection).GetContainerReference(cfg.PublishContainerName).GetBlobReference(Uri.UnescapeDataString(path));
                using (var stream = await file.OpenReadAsync())
                    return stream.TryDeserialize<T>(logger);
            }
            catch (StorageException)
            {
                logger?.LogError($"Failed to load asset from path : {path}");
                return null;
            }
        }

        /// <summary>Download a blob.</summary>
        /// <param name="cfg">Azure storage access settings</param>
        /// <param name="path">Blob name</param>
        /// <param name="logger">Optional logger</param>
        /// <returns>Returns an open stream to the blob or null in case of failure</returns>
        public static async Task<T> GetResourceFile<T>(SyncOptions cfg, string path, ILogger logger = null) where T : class, new()
        {
            try
            {
                var file = GetClient(cfg.ResourceStorageConnection).GetContainerReference(cfg.ResourceContainerName).GetBlobReference(Uri.UnescapeDataString(path));
                using (var stream = await file.OpenReadAsync())
                    return stream.Deserialize<T>();
            }
            catch (StorageException)
            {
                logger?.LogError($"Failed to load file from path : {path}");
                return null;
            }
        }


        /// <summary>Download a blob.</summary>
        /// <param name="connectionString">Blob storage connection string</param>
        /// <param name="containerName">Blob storage container name</param>
        /// <param name="path">Blob path</param>
        /// <param name="logger">Optional logger</param>
        /// <returns>Returns an open stream to the blob or null in case of failure</returns>
        public static async Task<Stream> GetFile(string connectionString, string containerName, string path, ILogger logger = null)
        {
            try
            {
                var file = GetClient(connectionString)
                    .GetContainerReference(containerName)
                    .GetBlobReference(Uri.UnescapeDataString(path));
                return await file.OpenReadAsync();
            }
            catch (StorageException)
            {
                logger?.LogError($"Failed to load file from path : {path}");
                return null;
            }
        }

        /// <summary>List all asset directories (prefixes) in a container.</summary>
        /// <param name="cfg">Azure storage access settings</param>
        /// <param name="excludePrefix">Prefix for directories to be excluded</param>
        public static async Task<IEnumerable<AssetDirectory>> GetAssetDirectories(SyncOptions cfg, string excludePrefix)
        {
            var dirTree = await GetAllBlobsGrouped(cfg);

            return dirTree
                .Where(p => p.Key.IndexOf(excludePrefix, StringComparison.Ordinal) < 0)
                .Select(p =>
                {
                    var assetId = GetBasename(p.Key);
                    var assetType = assetId.GetDirectoryAssetType();

                    return new AssetDirectory
                    {
                        AssetId = assetId,
                        AssetType = assetType,
                        Path = p.Key
                    };
                })
                .OrderBy(d => d.AssetType);
        }

        /// <summary>Read translated assets.</summary>
        /// <param name="cfg">Azure storage access settings</param>
        /// <param name="assetDir">Asset directory prefix</param>
        /// <param name="logger">Optionall logger</param>
        /// <typeparam name="T">Type of the assets</typeparam>
        public static async Task<IDictionary<string, T>> GetTranslatedAssets<T>(SyncOptions cfg, string assetDir, ILogger logger = null)
            where T : class, IAsset, new()
        {
            var translatedFiles = (await ListBlobs(cfg, $"{assetDir}/")).Where(f => f.GetAssetFileLanguage() != null);
            var translations = new Dictionary<string, T>();
            
            foreach (var file in translatedFiles)
            {
                var language = file.GetAssetFileLanguage();
                if (string.IsNullOrEmpty(language))
                    continue;

                var asset = await GetAsset<T>(cfg, file, logger);
                
                if (asset == null)
                    continue;
                
                translations.Add(language, asset);
            }

            return translations;
        }
        
        /// <summary>Return the basename of a path.</summary>
        private static string GetBasename(string path)
        {
            return path.Substring(path.LastIndexOf('/') + 1);
        }
        
        /// <summary>Return the dirname of a path.</summary>
        /// <remarks>Returns an empty string in case of a name with no path separators</remarks>
        private static string GetDirname(string path)
        {
            var i = path.LastIndexOf('/');
            return i > 0 ? path.Substring(0, i) : "";
        }
        
        /// <summary>Create a blob storage client.</summary>
        private static CloudBlobClient GetClient(string connectionString)
            => CloudStorageAccount.Parse(connectionString).CreateCloudBlobClient();

        /// <summary>Remove the container name from the relative URI of a blob.</summary>
        private static string StripContainerName(string path, string container) 
            => path.Substring(path.LastIndexOf($"{container}/", StringComparison.Ordinal) + $"{container}/".Length);

        /// <summary>List all blobs and group them by the common directory</summary>
        private static async Task<List<KeyValuePair<string, List<string>>>> GetAllBlobsGrouped(SyncOptions cfg)
        {
            var dirDict = new Dictionary<string, List<string>>();
            var files = await ListBlobs(cfg);

            foreach (var file in files)
            {
                var dirname = GetDirname(file);
                var basename = GetBasename(file);

                if (dirDict.ContainsKey(dirname))
                    dirDict[dirname].Add(basename);
                else
                    dirDict[dirname] = new List<string> {basename};
            }

            return dirDict.ToList();
        }
    }

    /// <summary>Asset file reading extensions</summary>
    public static class AssetFileExtensions
    {
        // TODO: move to AssetExtensions?
        /// <summary>Get metadata file locale in upper case, e.g. ru-RU</summary>
        public static string GetAssetFileLanguage(this string fileName)
        {
            var regex = new Regex(@".+_(?<language>[a-zA-Z]{2})\.json", RegexOptions.Compiled);
            var match = regex.Match(fileName);
            return !match.Success ? null : match.Groups["language"].Value;
        }
    }

    /// <summary>Asset directory structure</summary>
    public class AssetDirectory
    {
        /// <summary>Asset directory path</summary>
        public string Path;

        /// <summary>Asset type</summary>
        public int AssetType;

        /// <summary>Asset ID</summary>
        public string AssetId;
    }
}