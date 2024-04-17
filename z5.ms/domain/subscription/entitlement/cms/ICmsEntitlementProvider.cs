using System;
using System.Threading.Tasks;
using z5.ms.common.abstractions;
using z5.ms.domain.subscription.entitlement.viewmodel;

namespace z5.ms.domain.subscription.entitlement.cms
{
    /// <summary>
    /// Interface for the CMS entitlement provider
    /// </summary>
    /// <remarks>Since we trust CMS, we don't really need to check for entitlement - CMS will get everything it requests</remarks>
    [Obsolete("Use entitlement2")]
    public interface ICmsEntitlementProvider
    {
        /// <summary>Check whether a user is entitled to access an asset</summary>
        /// <returns>True if the user entitled to access a resources, false otherwise</returns>
        Task<Result<EntitlementResponse>> Check(GetCmsEntitlementQuery q);

        /// <summary>Get a DRM token for a DRM-protected asset</summary>
        /// <returns>DRM token</returns>
        Task<Result<EntitlementResponse>> GetDrm(GetCmsEntitlementQuery q);

        /// <summary>Get a CDN token for an asset</summary>
        /// <returns>CDN token</returns>
        Task<Result<EntitlementResponse>> GetCdn(GetCmsEntitlementQuery q);
    }
}