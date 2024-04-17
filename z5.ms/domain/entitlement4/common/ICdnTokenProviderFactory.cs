using System.Collections.Generic;

namespace z5.ms.domain.entitlement4.common
{
    /// <summary>
    /// Singleton CDN provider factory for registered CDN providers.
    /// </summary>
    public interface ICdnTokenProviderFactory
    {
        /// <summary>Register a CND token provider</summary>
        /// <param name="cdnTokenProvider">CDN token provider to register</param>
        void RegisterCdnTokenProvider(ICdnTokenProvider cdnTokenProvider);

        /// <summary>Get a specific registered CDN token provider</summary>
        /// <param name="providerName">Name/type of the provider</param>
        ICdnTokenProvider GetCdnProvider(string providerName);

        /// <summary>Get all registered CDN token providers</summary>
        IEnumerable<ICdnTokenProvider> GetAllCdnProviders();
    }
}