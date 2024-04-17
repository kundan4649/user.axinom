using System.Collections.Generic;
using z5.ms.common.assets.common;
using z5.ms.domain.entitlement4.viewmodel;

namespace z5.ms.domain.entitlement4.common
{
    /// <summary> Entitlement validation result for asset validator </summary>
    public class EntitlementAssetValidationResult
    {
        /// <summary>Id of requested asset</summary>
        public string AssetId { get; set; }

        /// <summary>Business type of the asset</summary>
        public BusinessType? BusinessType { get; set; }
        
        /// <summary>Video urls of the asset </summary>
        public Video Video { get; set; }

        /// <summary>Content key ID DRM protected video</summary>
        public string ContentKeyId { get; set; }

        /// <summary>Languages of the asset</summary>
        public List<string> Languages { get; set; }
    }
}
