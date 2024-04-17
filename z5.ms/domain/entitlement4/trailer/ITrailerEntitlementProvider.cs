using System.Threading.Tasks;
using z5.ms.common.abstractions;
using z5.ms.domain.entitlement4.viewmodel;

namespace z5.ms.domain.entitlement4.trailer
{
    /// <summary>
    /// Interface for the 'trailer' subscription entitlement provider
    /// </summary>
    /// <remarks>Returns only CDN tokens to trailers, no DRM</remarks>
    public interface ITrailerEntitlementProvider
    {
        /// <summary>Get a CDN token for an asset</summary>
        /// <param name="assetId">ID of the asset to check</param>
        /// <param name="token">The token to use for entitlement check, it may be a user token (internal subscription/purchase), Google Playstore token or an Apple App Store token</param>
        /// <param name="clientIp">Client IP address</param>
        /// <returns>CDN token</returns>
        Task<Result<EntitlementResponse>> GetCdn(string assetId, string token, string clientIp);
    }
}