using System.Collections.Generic;
using System.Linq;

namespace z5.ms.common.infrastructure.assetcache
{
    /// <summary>An index for finding assets by type</summary>
    public class TypeIndex : CacheIndex<System.Type>, ICacheUpdateListener
    {
        private readonly Dictionary<int, System.Type> _types = new Dictionary<int, System.Type>
        {
            //{ (int)AssetType.Channel, typeof(ChannelEntity) },
            //{ (int)AssetType.GenreList, typeof(GenreListEntity) },
            //{ (int)AssetType.LiveEvent, typeof(LiveEventEntity) },
            //{ (int)AssetType.Movie, typeof(MovieEntity) },
            //{ (int)AssetType.TvShow, typeof(TvShowEntity) },
            //{ (int)AssetType.EpgProgram, typeof(EpgProgramEntity) }
        };

        /// <summary>Does this index support a specific type</summary>
        public bool SupportsType<T>() => _types.Values.Contains(typeof(T));

        /// <inheritdoc />
        public void OnAdded(CacheEntry cacheEntry)
        {
            if (_types.TryGetValue(cacheEntry.Type, out var type))
                Add(type, cacheEntry);
        }

        /// <inheritdoc />
        public void OnRemoved(CacheEntry cacheEntry)
        {
            if (_types.TryGetValue(cacheEntry.Type, out var type))
                Remove(type, cacheEntry);
        }

        /// <summary>Find transalted assets by type</summary>
        /// <typeparam name="TAsset"></typeparam>
        /// <param name="translation"></param>
        /// <returns></returns>
        public List<TAsset> Lookup<TAsset>(string translation = null) where TAsset : class, IAsset =>
            Lookup(typeof(TAsset), translation).Cast<TAsset>().ToList();
    }
}