using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace z5.ms.common.infrastructure.assetcache
{
    /// <summary>Cache adapters factory</summary>
    /// <remarks>Provides registered adapter instances for specified asset types</remarks>
    public interface ICacheAdaptersFactory
    {
        /// <summary>Register new asset adapter</summary>
        /// <param name="adapter">Asset cache adapter</param>
        void RegisterAdapter(ICacheAdapter adapter);
        
        /// <summary>Get registered adapter for specified asset type</summary>
        ICacheAdapter GetAdapter(int assetType);
    }
    
    /// <inheritdoc />
    public class CacheAdaptersFactory : ICacheAdaptersFactory
    {
        private readonly IDictionary<int, ICacheAdapter> _adapters = new Dictionary<int, ICacheAdapter>();
        
        /// <inheritdoc />
        public void RegisterAdapter(ICacheAdapter adapter)
        {
            _adapters[adapter.AssetType] = adapter;
        }

        /// <inheritdoc />
        public ICacheAdapter GetAdapter(int assetType)
        {
            _adapters.TryGetValue(assetType, out var adapter);
            return adapter;
        }
    }
    
    /// <summary>Cache dependencies registration extensions</summary>
    public static class CacheRegistrationExtensions
    {
        /// <summary>Register singleton adapters fctory instnace</summary>
        public static void RegisterCacheAdaptersFactory(this IServiceCollection services)
        {
            services.AddSingleton<ICacheAdaptersFactory, CacheAdaptersFactory>();
        }
        
        /// <summary>Configure all registered adapters in cache adapters factory</summary>
        public static IServiceProvider ConfigureCacheAdapters(this IServiceProvider services)
        {
            var factory = services.GetService<ICacheAdaptersFactory>();
            var adapters = services.GetServices<ICacheAdapter>();
            foreach (var adapter in adapters)
                factory.RegisterAdapter(adapter); //signleton instances, same as factory itself

            return services;
        }
        
        /// <summary>Register scoped adapter instnace</summary>
        public static void RegisterCacheAdapter<TAdapter>(this IServiceCollection services)
            where TAdapter : class, ICacheAdapter
        {
            services.AddSingleton<ICacheAdapter, TAdapter>();
        }
        
        /// <summary>Register singleton cache loader instnace</summary>
        public static void RegisterCacheLoader<TLoader>(this IServiceCollection services)
            where TLoader : class, IAssetsLoader
        {
            services.AddSingleton<IAssetsLoader, TLoader>();
        }
    }
    
    /// <summary>Extensions specific to adapters and asset loading</summary>
    //[Obsolete("Use AsListItem interface method instead! This glues all subfeatures of catalog domain together!", false)]
    public static class AdapterExtensions
    {
        /// <summary>Convert a list of translated assets to IAsset dictionary for creating a CacheEntry </summary>
        public static IDictionary<string, IAsset> ToIassetDictionary<TAsset>(this IEnumerable<KeyValuePair<string, TAsset>> translatedAssets) 
            where TAsset : IAsset
            => translatedAssets.ToDictionary(k => k.Key, v => (IAsset) v.Value);
    }
}