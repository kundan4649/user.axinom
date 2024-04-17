using System;
using System.Threading.Tasks;
using z5.ms.common.abstractions;
using z5.ms.domain.subscription.entitlement.viewmodel;

namespace z5.ms.domain.subscription.entitlement.trailer
{
    /// <summary>
    /// Interface for the 'trailer' subscription entitlement provider
    /// </summary>
    /// <remarks>Returns only CDN tokens to trailers, no DRM</remarks>
    [Obsolete("Use entitlement2")]
    public interface ITrailerEntitlementProvider
    {
        /// <summary>Get a CDN token for an asset</summary>
        /// <returns>CDN token</returns>
        Task<Result<EntitlementResponse>> GetCdn(GetTrailerEntitlementQuery q);
    }
}