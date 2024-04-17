using System;
using System.Threading.Tasks;
using z5.ms.common.abstractions;

namespace z5.ms.domain.subscription.entitlement.common
{
    /// <summary>
    /// Common service to validate assets for all entitlement providers
    /// </summary>
    [Obsolete("Use entitlement2")]
    public interface IEntitlementAssetValidator
    {
        /// <summary>
        /// Validate asset properties are meet the entitlement criterias
        /// </summary>
        /// <param name="assetId"></param>
        /// <param name="country"></param>
        /// <param name="authenticated"></param>
        /// <returns></returns>
        Task<Result<EntitlementAssetValidationResult>> ValidateAsset(string assetId, string country, bool authenticated = false);

        /// <summary>
        /// Validate asset properties are meet the entitlement criterias for DRM requests
        /// </summary>
        /// <param name="assetId"></param>
        /// <param name="keyId"></param>
        /// <param name="country"></param>
        /// <param name="authenticated"></param>
        /// <returns></returns>
        Task<Result<EntitlementAssetValidationResult>> ValidateDrmAsset(string assetId, string keyId, string country, bool authenticated = false);
    }
}
