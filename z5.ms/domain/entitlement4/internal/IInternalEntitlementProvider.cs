using System.Threading.Tasks;
using z5.ms.common.abstractions;
using z5.ms.domain.entitlement4.viewmodel;

namespace z5.ms.domain.entitlement4.@internal
{
    /// <summary>
    /// Interface for the internal subscription and purchase entitlement provider
    /// </summary>
    public interface IInternalEntitlementProvider
    {
        /// <summary>Check whether a user is entitled to access an asset</summary>
        /// <param name="query">Entitlement query parameters</param>
        /// <returns>True if the user entitled to access a resources, false otherwise</returns>
        Task<Result<EntitlementResponse>> Check(GetInternalEntitlementQuery query);

        /// <summary>Get a DRM token for a DRM-protected asset</summary>
        /// <param name="query">Entitlement query parameters</param>
        /// <returns>DRM token</returns>
        Task<Result<EntitlementResponse>> GetDrm(GetInternalEntitlementQuery query);

        /// <summary>Get a CDN token for an asset</summary>
        /// <param name="query">Entitlement query parameters</param>
        /// <returns>CDN token</returns>
        Task<Result<EntitlementResponse>> GetCdn(GetInternalEntitlementQuery query);
    }
}