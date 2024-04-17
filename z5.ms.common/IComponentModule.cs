using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace z5.ms.common
{
    /// <summary>Component module</summary>
    [SuppressMessage("ReSharper", "UnusedMethodReturnValue.Global")]
    public interface IComponentModule<in TOptions> where TOptions : class, IComponentRegistrationOptions
    {
        /// <summary>Register BC module services with custom options</summary>
        IServiceCollection Register(IServiceCollection services, IConfiguration config,
            ConfigureControllerFeatureProvider provider, IMapperConfigurationExpression mapperConfig = null,
            TOptions options = null);
    }

    /// <summary>Component module</summary>
    [SuppressMessage("ReSharper", "UnusedMethodReturnValue.Global")]
    public interface IComponentModule
    {
        /// <summary>Register BC module services</summary>
        IServiceCollection Register(IServiceCollection services, IConfiguration config, IMapperConfigurationExpression mapperConfig = null);
    }

    /// <summary>BC component registration options</summary>
    public interface IComponentRegistrationOptions
    {
    }

    /// <inheritdoc />
    /// <summary>Default component module registration options</summary>
    public class ComponentRegistrationOptions : IComponentRegistrationOptions
    {
        /// <summary>Default registration options</summary>
        public static ComponentRegistrationOptions Default = new ComponentRegistrationOptions();
    }
}