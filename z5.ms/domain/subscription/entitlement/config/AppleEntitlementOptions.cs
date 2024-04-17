using System;

namespace z5.ms.domain.subscription.entitlement.config
{
    /// <summary>
    /// Configuration options for Apple App Store based entitlements
    /// </summary>
    [Obsolete("Use entitlement2")]
    public class AppleEntitlementOptions
    {
        private string _appleStoreApiUrl;

        /// <summary>Apple store url to verify of in-app subscriptions for entitlements</summary>
        public string AppleStoreApiUrl
        {
            get => _appleStoreApiUrl;
            set => _appleStoreApiUrl = value?.TrimEnd('/');
        }

        /// <summary>Apple store api secret value to verify of in-app subscriptions for entitlements</summary>
        public string AppleStoreApiSecret { get; set; }
    }
}