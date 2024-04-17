using Microsoft.Extensions.Configuration;

namespace z5.ms.common.extensions
{
    /// <summary>Extensions for <see cref="IConfiguration"/></summary>
    public static class ConfigurationExtensions
    {
        /// <summary>Get a configuration section and try to bind it to a model.</summary>
        public static T GetSectionAs<T>(this IConfiguration configuration) where T : class, new() 
            => configuration.SectionExistsOfType<T>() ? configuration.GetSection(typeof(T).Name).Get<T>() : new T();

        /// <summary>Check if a section exists in the given configuration.</summary>
        public static bool SectionExistsOfType<T> (this IConfiguration configuration) where T : class 
            => configuration.GetSection(typeof(T).Name).Exists();

    }
}