using System;
using System.Threading.Tasks;
using z5.ms.common.abstractions;
using z5.ms.domain.subscription.entitlement.viewmodel;

namespace z5.ms.domain.subscription.entitlement.google
{
    /// <summary>
    /// Interface for the Google Playstore subscription entitlement provider
    /// </summary>
    [Obsolete("Use entitlement2")]
    public interface IGoogleEntitlementProvider
    {
        /// <summary>Check whether a user is entitled to access an asset</summary>
        /// <returns>True if the user entitled to access a resources, false otherwise</returns>
        Task<Result<EntitlementResponse>> Check(GetGoogleEntitlementQuery q);

        /// <summary>Get a DRM token for a DRM-protected asset</summary>
        /// <returns>DRM token</returns>
        Task<Result<EntitlementResponse>> GetDrm(GetGoogleEntitlementQuery q);

        /// <summary>Get a CDN token for an asset</summary>
        /// <returns>CDN token</returns>
        Task<Result<EntitlementResponse>> GetCdn(GetGoogleEntitlementQuery q);
    }
}