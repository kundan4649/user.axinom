namespace z5.ms.common.infrastructure.assetcache
{
    /// <summary>Subscriber to add and remove notifications</summary>
    public interface ICacheUpdateListener
    {
        /// <summary>Asset added to cache</summary>
        /// <param name="asset">Added asset</param>
        void OnAdded(CacheEntry asset);

        /// <summary>Asset removed from cache</summary>
        /// <param name="asset">Removed asset</param>
        void OnRemoved(CacheEntry asset);
    }
}