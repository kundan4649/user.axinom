namespace z5.ms.domain.subscription.viewmodel
{
    // TODO: break into multiple classes
    /// <summary>
    /// Configuration options for the subscription service
    /// </summary>
    public class SubscriptionServiceOptions
    {
        private string _googleAuthApiUrl;
        private string _googleStoreApiUrl;
        private string _catalogApiUrl;
        private string _userServiceBaseUrl;
        private string _subscriptionServiceBaseUrl;
        private string _frontEndUrl;
        private string _appleStoreApiUrl;

        /// <summary>Default value for a user's system type</summary>
        public string DefaultSystemType { get; set; } = "Internal";

        /// <summary>Apple store url to verify of in-app subscriptions for entitlements</summary>
        public string AppleStoreApiUrl
        {
            get => _appleStoreApiUrl;
            set => _appleStoreApiUrl = value?.TrimEnd('/');
        }

        /// <summary>Apple store api secret value to verify of in-app subscriptions for entitlements</summary>
        public string AppleStoreApiSecret { get; set; }

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

        /// <summary>Catalog api url to get media asset information for entitlements</summary>
        public string CatalogApiUrl
        {
            get => _catalogApiUrl;
            set => _catalogApiUrl = value?.TrimEnd('/');
        }

        /// <summary>File path of the private key file that provided by BuyDrm</summary>
        public string BuyDrmCertificateFilePath { get; set; }

        /// <summary>Axinom Drm service communication key to sign entitlement messages</summary>
        public string AxinomDrmCommunicationKey { get; set; }

        /// <summary>Communication key id indicates the ID of the Communication Key that was used to sign an Axinom DRM License Server Message</summary>
        public string AxinomDrmCommunicationKeyId { get; set; }
        
        /// <summary>Cdn token to send it as check entitlement response</summary>
        public string EntitlementCdnToken { get; set; }

        /// <summary>Secret for internal api authentication</summary>
        public string InternalApiSecret { get; set; }

        /// <summary>This url will be used to query information from the user service.</summary>
        public string UserServiceBaseUrl
        {
            get => _userServiceBaseUrl;
            set => _userServiceBaseUrl = value?.TrimEnd('/');
        }

        /// <summary>This url will be used as the base for payment callback urls. It must be an absolute url without v1/etc..</summary>
        public string SubscriptionServiceBaseUrl
        {
            get => _subscriptionServiceBaseUrl;
            set => _subscriptionServiceBaseUrl = value?.TrimEnd('/');
        }

        /// <summary>Front end url for insertion into notifications</summary>
        public string FrontEndUrl
        {
            get => _frontEndUrl;
            set => _frontEndUrl = value?.TrimEnd('/');
        }

        /// <summary>Feature switch enables service bus events </summary>
        // TODO: Remove feature switch
        public bool MsEventsFeatureSwitch { get; set; }

        /// <summary>Allow multiple subscriptions per user</summary>
        public bool MultipleSubscriptionsPerUser { get; set; }

        /// <summary>Feature switch enables geoblocking for entitlement service </summary>
        public bool GeoBlockingFeatureSwitch { get; set; }

        /// <summary>Comma separated list of payment providers that promotions are enabled for</summary>
        public string EnablePromotionsForPaymentProviders { get; set; }

        /// <summary>To define the time period for users can be used free trial once</summary>
        public int? FreeTrialInactivityMonth { get; set; } = null;
    }
}
