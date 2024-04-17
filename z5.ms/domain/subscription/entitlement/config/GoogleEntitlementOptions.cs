using System;

namespace z5.ms.domain.subscription.entitlement.config
{
    /// <summary>
    /// Configuration options for Google Playstore based entitlements
    /// </summary>
    [Obsolete("Use entitlement2")]
    public class GoogleEntitlementOptions
    {
        private string _googleAuthApiUrl;
        private string _googleStoreApiUrl;

        /// <summary>Authentication url to acquire authentication token from google</summary>
        public string GoogleAuthApiUrl
        {
            get => _googleAuthApiUrl;
            set => _googleAuthApiUrl = value?.TrimEnd('/');
        }

        /// <summary>Authentication request body parameters to acquire authentication token from google</summary>
        public string GoogleAuthApiBody { get; set; }

        /// <summary>Google play store url to verify of in-app subscriptions for entitlements</summary>
        public string GoogleStoreApiUrl
        {
            get => _googleStoreApiUrl;
            set => _googleStoreApiUrl = value?.TrimEnd('/');
        }
    }
}