using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using z5.ms.common.infrastructure.assetcache.config;

namespace z5.ms.common.infrastructure.assetcache
{
    /// <inheritdoc />
    /// <summary>Assets cache registration module</summary>
    public class CacheModule : IComponentModule
    {
        /// <inheritdoc />
        public IServiceCollection Register(IServiceCollection services, IConfiguration config, IMapperConfigurationExpression mapperConfig)
        {
            //Sync options
            services.Configure<SyncOptions>(config);

            //Assets cache
            services.TryAddSingleton<IAssetsCache, AssetsCache>();
            services.TryAddSingleton<IAssetsCacheReader>(x => x.GetService<IAssetsCache>());
            
            //Cache adapters factory and loader
            services.RegisterCacheLoader<AssetsLoader>();
            services.RegisterCacheAdaptersFactory();

            return services;
        }
    }
}
