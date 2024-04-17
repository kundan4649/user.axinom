using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace z5.ms.common.extensions
{
    /// <summary>Extension methods for dependency injection</summary>
    public static class ServiceProviderExtensions
    {
        /// <summary>Replace an existing dependency with a new scoped dependency</summary>
        public static IServiceCollection ReplaceScoped<TService, TImplementation>(this IServiceCollection services)
            where TService : class
            where TImplementation : class, TService
        {
            Remove<TService>(services);
            services.AddScoped<TService, TImplementation>();
            return services;
        }

        /// <summary>Replace an existing dependency with a new singleton dependency</summary>
        public static IServiceCollection ReplaceSingleton<TService, TImplementation>(this IServiceCollection services)
            where TService : class
            where TImplementation : class, TService
        {
            Remove<TService>(services);
            services.AddSingleton<TService, TImplementation>();
            return services;
        }

        /// <summary>Remove an existing dependency</summary>
        public static void Remove<TService>(this IServiceCollection services)
            where TService : class
        {
            foreach (var d in services.ToList())
                if (d.ServiceType == typeof(TService)) services.Remove(d);
        }
    }
}