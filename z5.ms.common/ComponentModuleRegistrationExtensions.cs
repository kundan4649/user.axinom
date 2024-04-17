using System;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace z5.ms.common
{
    /// <summary>Component module extensions</summary>
    public static class ComponentModuleRegistrationExtensions
    {
        /// <summary>Register BC module services with custom options</summary>
        /// <example>
        /// services.AddModule&lt;CatalogModule&gt;(new CatalogRegistrationOptions());
        /// </example>
        public static void AddModule<TModule, TOptions>(this IServiceCollection services, IConfiguration config,
            ConfigureControllerFeatureProvider featureProvider, IMapperConfigurationExpression mapperConfig, TOptions options)
            where TModule : class, IComponentModule<TOptions>
            where TOptions : class, IComponentRegistrationOptions
        {
            var module = (TModule) Activator.CreateInstance(typeof(TModule));
            if (module == null)
                throw new Exception("invalid module");

            //Add configuration singleton (in addition to Options)
            services.TryAddSingleton(config);

            //Setup options with DI
            services.AddOptions();

            module.Register(services, config, featureProvider, mapperConfig, options);
        }

        /// <summary>Register BC module services</summary>
        public static void AddModule<TModule>(this IServiceCollection services, IConfiguration config, IMapperConfigurationExpression mapperConfig = null)
            where TModule : class, IComponentModule
        {
            var module = (TModule) Activator.CreateInstance(typeof(TModule));
            if (module == null)
                throw new Exception("invalid module");

            module.Register(services, config, mapperConfig);
        }
    }
}