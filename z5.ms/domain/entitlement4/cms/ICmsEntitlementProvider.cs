using System.Threading.Tasks;
using z5.ms.common.abstractions;
using z5.ms.domain.entitlement4.viewmodel;

namespace z5.ms.domain.entitlement4.cms
{
    /// <summary>
    /// Interface for the CMS entitlement provider
    /// </summary>
    /// <remarks>Since we trust CMS, we don't really need to check for entitlement - CMS will get everything it requests</remarks>
    public interface ICmsEntitlementProvider
    {
        /// <summary>Check whether a user is entitled to access an asset</summary>
        /// <param name="assetId">ID of the asset to check</param>
        /// <param name="token">The token to use for entitlement check, it may be a user token (internal subscription/purchase), Google Playstore token or an Apple App Store token</param>
        /// <returns>True if the user entitled to access a resources, false otherwise</returns>
        Task<Result<EntitlementResponse>> Check(string assetId, string token);

        /// <summary>Get a DRM token for a DRM-protected asset</summary>
        /// <param name="assetId">ID of the asset to check</param>
        /// <param name="keyId"></param>
        /// <param name="token">The token to use for entitlement check, it may be a user token (internal subscription/purchase), Google Playstore token or an Apple App Store token</param>
        /// <param name="persistent">Should the DRM license be persistent</param>
        /// <returns>DRM token</returns>
        Task<Result<EntitlementResponse>> GetDrm(string assetId, string keyId, string token, bool persistent = false);

        /// <summary>Get a CDN token for an asset</summary>
        /// <param name="assetId">ID of the asset to check</param>
        /// <param name="token">The token to use for entitlement check, it may be a user token (internal subscription/purchase), Google Playstore token or an Apple App Store token</param>
        /// <param name="clientIp">Client IP address</param>
        /// <returns>CDN token</returns>
        Task<Result<EntitlementResponse>> GetCdn(string assetId, string token, string clientIp);
    }
}