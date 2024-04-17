using System;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace z5.ms.common
{
    /// <summary>A unique asset interface</summary>
    /// <remarks>https://wiki.axinom.com/display/PA/Data+Model</remarks>
    [JsonObject(MemberSerialization.OptIn, Title = "asset")]
    public interface IAsset : IFilterable
    {
        /// <summary>Unique asset ID</summary>
        [JsonProperty("id", Required = Required.Always, NullValueHandling = NullValueHandling.Ignore)]
        string Id { get; set; }

        /// <summary>Asset type</summary>
        [JsonProperty("type", Required = Required.Always, NullValueHandling = NullValueHandling.Ignore)]
        int Type { get; set; }
        
        /// <summary>Asset subtype</summary>
        [JsonProperty("asset_subtype", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        string AssetSubtype { get; set; }

        /// <summary>Human readable title for this asset</summary>
        [JsonProperty("title", Required = Required.Always)]
        string Title { get; set; }
    }

    /// <summary>
    /// Interface to add filtering capabilities to an asset.
    /// </summary>
    public interface IFilterable
    {
        /// <summary>Verify if filter applies to the entity</summary>
        bool ApplyFilter<TFilter>(TFilter filter) where TFilter : IQueryFilter;
    }

    /// <summary>Extension methods for validation of conventional AssetID-formatted strings based on ASsetType enumeration</summary>
    public static class AssetExtensions
    {
        /// <summary>AssetID Regular expression pattern</summary>
        private const string AssetIdPattern = "^[A-Za-z0-9]+-(?<type>[0-9]+)-.+";

        private static readonly Regex AssetIdRegex = new Regex(AssetIdPattern, RegexOptions.Compiled);

        /// <summary>Get asset type enum value from assetID string</summary>
        /// <param name="assetId">Conventional AssetID-formatted string</param>
        /// <returns>AssetType value or null, if assetID has invalid format</returns>
        private static int? GetAssetType(this string assetId)
        {
            if (String.IsNullOrEmpty(assetId))
                return null;

            var match = AssetIdRegex.Match(assetId);
            if (!match.Success)
                return null;

            if (int.TryParse(match.Groups["type"].Value, out var res))
                return res;

            return null;
        }

        /// <summary>Check if assetID is valid AssetID string</summary>
        /// <param name="assetId">Conventional AssetID-formatted string</param>
        /// <returns>True, if assetID is valid AssetID string</returns>
        public static bool IsAssetId(this string assetId)
        {
            if (String.IsNullOrEmpty(assetId))
                return false;

            var match = AssetIdRegex.Match(assetId);
            if (!match.Success)
                return false;

            return true;
        }

        /// <summary>Check if assetID belongs to asset of specified type</summary>
        /// <param name="assetId">Conventional AssetID-formatted string</param>
        /// <param name="assetType">Target asset type</param>
        /// <returns>>True, if assetID belongs to asset of specified type</returns>
        public static bool IsAssetOfType(this string assetId, int assetType)
        {
            if (string.IsNullOrEmpty(assetId))
                return false;

            var match = AssetIdRegex.Match(assetId);
            if (!match.Success)
                return false;

            if (!int.TryParse(match.Groups["type"].Value, out var res))
                return false;

            return res == assetType;
        }

        /// <summary>Get asset type enum value from assetID string</summary>
        /// <param name="assetId">Conventional AssetID-formatted string</param>
        /// <param name="defaultValue">default value to use, if assetID has invalid format (defaults to AssetType.Unknown)</param>
        /// <returns>AssetType value or specified default Value, if assetID has invalid format</returns>
        public static int GetAssetTypeOrDefault(this string assetId, int defaultValue = 99)
        {
            try
            {
                return GetAssetType(assetId) ?? defaultValue;
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>Get asset type from asset directory name</summary>
        /// <param name="directoryName">Asset directory name</param>
        /// <returns>Asset type value or AssetType.Unknown</returns>
        public static int GetDirectoryAssetType(this string directoryName)
        {
            try
            {
                return GetAssetType(directoryName) ?? 99;
            }
            catch
            {
                return 99;
            }
        }

        /// <summary>
        /// Get a new clone of specified asset
        /// </summary>
        /// <typeparam name="TAsset"></typeparam>
        /// <param name="asset"></param>
        /// <returns></returns>
        public static TAsset GetAssetClone<TAsset>(this TAsset asset)
        {
            var settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            };
            return JsonConvert.DeserializeObject<TAsset>(JsonConvert.SerializeObject(asset, settings), settings);
        }
    }
}