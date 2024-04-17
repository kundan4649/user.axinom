using System.Threading.Tasks;
using z5.ms.common.abstractions;

namespace z5.ms.domain.entitlement4.common
{
    /// <summary>Common service to validate assets for all entitlement providers</summary>
    public interface IEntitlementAssetValidator
    {
        /// <summary>Validate asset licensing</summary>
        Task<Result<EntitlementAssetValidationResult>> ValidateAsset(string assetId, string country = null);

        /// <summary>Validate the drm key of an asset and licensing</summary>
        Task<Result<EntitlementAssetValidationResult>> ValidateDrmAsset(string assetId, string keyId, string country = null);
    }
}
