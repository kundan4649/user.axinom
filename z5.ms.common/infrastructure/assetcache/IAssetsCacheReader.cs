using System;
using System.Collections.Generic;

namespace z5.ms.common.infrastructure.assetcache
{
    /// <summary>An interface to read assets from in-memory assets storage</summary>
    public interface IAssetsCacheReader
    {
        /// <summary>Get cloned asset value</summary>
        /// <param name="assetId">Unique asset ID</param>
        /// <param name="language">Optional asset translation language</param>
        /// <returns>Single asset or null</returns>
        IAsset Get(string assetId, string language = null);

        /// <summary>Get value of cloned asset of type TAsset</summary>
        /// <typeparam name="TAsset">Asset type from AssetType enumeration</typeparam>
        /// <param name="assetId">Unique asset ID</param>
        /// <param name="language">Optional asset translation language</param>
        /// <returns>Single asset or null</returns>
        TAsset Get<TAsset>(string assetId, string language = null) where TAsset : class, IAsset;

        /// <summary>Select all assets in cache</summary>
        /// <param name="language">Optional asset translation language</param>
        /// <returns>All assets in cache</returns>
        IEnumerable<IAsset> Select(string language = null);

        /// <summary>Select all assets in cache of type TAsset</summary>
        /// <typeparam name="TAsset">Asset type from AssetType enumeration</typeparam>
        /// <param name="language">Optional asset translation language</param>
        /// <returns>all assets in cache of type TAsset</returns>
        IEnumerable<TAsset> Select<TAsset>(string language = null) where TAsset : class, IAsset;

        /// <summary>Select all assets in cache that satisfy predicate conditions</summary>
        /// <param name="prediate">A function to check certain asset properties</param>
        /// <param name="language">Optional asset translation language</param>
        /// <returns>Collection of matched assets</returns>
        IEnumerable<IAsset> Select(Func<IAsset, bool> prediate, string language = null);

        /// <summary>Select all assets of type TAsset in cache that satisfy predicate conditions</summary>
        /// <typeparam name="TAsset">Asset type from AssetType enumeration</typeparam>
        /// <param name="predicate">A function to check certain asset properties</param>
        /// <param name="language">Optional asset translation language</param>
        /// <returns>Collection of matched assets of type T</returns>
        IEnumerable<TAsset> Select<TAsset>(Func<TAsset, bool> predicate, string language = null) where TAsset : class, IAsset;

        /// <summary>Select all assets of type TAsset in cache that satisfy predicate conditions</summary>
        /// <typeparam name="TAsset">Asset type from AssetType enumeration</typeparam>
        /// <param name="prediate">A function to check certain asset properties</param>
        /// <param name="sort">Sorting parameter</param>
        /// <param name="paging">Paging parameter</param>
        /// <param name="language">Optional asset translation language</param>
        /// <returns>Collection of matched assets of type T</returns>
        IEnumerable<TAsset> Select<TAsset>(Func<TAsset, bool> prediate, SortParam sort, PagingParam paging, string language = null) where TAsset : class, IAsset;
        
        /// <summary>Select all assets of type TAsset in cache that satisfy predicate conditions</summary>
        /// <typeparam name="TAsset">Asset type from AssetType enumeration</typeparam>
        /// <param name="filter">Filter parameter</param>
        /// <param name="sort">Sorting parameter</param>
        /// <param name="paging">Paging parameter</param>
        /// <returns>Collection of matched assets of type T</returns>
        (IEnumerable<TAsset> assets, int total) Select<TAsset>(IQueryFilter filter, SortParam sort, PagingParam paging) where TAsset : class, IAsset, IFilterable;
        
        ///// <summary>
        ///// Select all asset tags for selected asset property from all cached assets.
        ///// E.g. SelectTags(x => x.Genres, language) to select all genres of all cached assets.
        ///// </summary>
        ///// <typeparam name="TAsset">Asset type from AssetType enumeration</typeparam>
        ///// <param name="selectFunc">A selector function to pick certain asset properties of type IEnumerable&lt;AssetTag&gt;</param>
        ///// <param name="language">Optional asset translation language</param>
        ///// <returns>A list of assets tags for selected asset property</returns>
        //IEnumerable<AssetTag> SelectTags<TAsset>(Func<TAsset, IEnumerable<AssetTag>> selectFunc, string language = null) where TAsset : class, IAsset;

        /// <summary>Find first asset that matches predicate conditions</summary>
        /// <typeparam name="TAsset">Asset type from AssetType enumeration</typeparam>
        /// <param name="prediate">A function to check certain asset properties</param>
        /// <param name="language">Optional asset translation language</param>
        /// <returns>Single asset of type TAsset or null</returns>
        TAsset FirstOrDefault<TAsset>(Func<TAsset, bool> prediate, string language = null) where TAsset : class, IAsset;

        /// <summary>Find number of assets in cache that satisfy predicate conditions</summary>
        /// <typeparam name="TAsset">Asset type from AssetType enumeration</typeparam>
        /// <param name="prediate">A function to check certain asset properties</param>
        /// <param name="language">Optional asset translation language</param>
        /// <returns>Number of matching assets</returns>
        int Count<TAsset>(Func<TAsset, bool> prediate, string language = null) where TAsset : class, IAsset;

        /// <summary>Check if asset is present in cache</summary>
        /// <param name="assetId">Unique asset ID</param>
        /// <returns>true, if asset exists in cache</returns>
        bool Exists(string assetId);
    }
}