using System;
using System.Threading.Tasks;
using z5.ms.common.abstractions;
using z5.ms.domain.subscription.entitlement.viewmodel;

namespace z5.ms.domain.subscription.entitlement.apple
{
    /// <summary>
    /// Interface for the Apple App Store subscription entitlement provider
    /// </summary>
    [Obsolete("Use entitlement2")]
    public interface IAppleEntitlementProvider
    {
        /// <summary>Check whether a user is entitled to access an asset</summary>
        /// <returns>True if the user entitled to access a resources, false otherwise</returns>
        Task<Result<EntitlementResponse>> Check(GetAppleEntitlementQuery q);

        /// <summary>Get a DRM token for a DRM-protected asset</summary>
        /// <returns>DRM token</returns>
        Task<Result<EntitlementResponse>> GetDrm(GetAppleEntitlementQuery q);

        /// <summary>Get a CDN token for an asset</summary>
        /// <returns>CDN token</returns>
        Task<Result<EntitlementResponse>> GetCdn(GetAppleEntitlementQuery q);
    }
}