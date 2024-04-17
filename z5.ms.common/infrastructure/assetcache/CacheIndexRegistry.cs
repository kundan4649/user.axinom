using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace z5.ms.common.infrastructure.assetcache
{
    /// <summary>Registry of cache indexes for automated index intialisation with cache</summary>
    public class CacheIndexRegistry
    {
        /// <summary>The list of indexes</summary>
        public List<ICacheUpdateListener> Indexes { get; set; } = new List<ICacheUpdateListener>();
    }

    /// <summary>ServiceCollection extension methods for registration of cache indexes with dependency injection container</summary>
    public static class ServiceCollectionCacheindexRegistryExtensions
    {
        private static CacheIndexRegistry _registry;

        /// <summary>Add a cache index to be registered for dependency injection and added as a listener to the assets cache</summary>
        /// <param name="services"></param>
        /// <param name="index"></param>
        public static void AddCacheIndex(this IServiceCollection services, ICacheUpdateListener index)
        {
            if (_registry == null)
            {
                _registry = new CacheIndexRegistry();
                services.AddSingleton(_registry);
            }

            services.AddSingleton(index.GetType(), index);
            _registry.Indexes.Add(index);
        }
        /// <summary>Add a cache index to be registered for dependency injection and added as a listener to the assets cache</summary>
        /// <param name="services"></param>
        public static void AddCacheIndex<TIndex>(this IServiceCollection services)
            where TIndex : ICacheUpdateListener, new() => AddCacheIndex(services, new TIndex());
    }
}