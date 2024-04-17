using System;
using System.Threading.Tasks;
using z5.ms.common.abstractions;
using z5.ms.domain.subscription.entitlement.viewmodel;

namespace z5.ms.domain.subscription.entitlement.dummy
{
    /// <summary>Interface for the Dummy entitlement provider</summary>
    /// <remarks>Dummy entitlement provider checks asset properties are correct and returns successful entitlement response</remarks>
    [Obsolete("Use entitlement2")]
    public interface IDummyEntitlementProvider
    {
        /// <summary>Check whether a user is entitled to access an asset</summary>
        /// <returns>True if the user entitled to access a resources, false otherwise</returns>
        Task<Result<EntitlementResponse>> Check(GetDummyEntitlementQuery q);

        /// <summary>Get a DRM token for a DRM-protected asset</summary>
        /// <returns>DRM token</returns>
        Task<Result<EntitlementResponse>> GetDrm(GetDummyEntitlementQuery q);

        /// <summary>Get a CDN token for an asset</summary>
        /// <returns>CDN token</returns>
        Task<Result<EntitlementResponse>> GetCdn(GetDummyEntitlementQuery q);
    }
}