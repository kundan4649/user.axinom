namespace z5.ms.domain.subscription.payment.paymentproviders
{
    /// <summary>
    /// Configuration options for Apple App Store
    /// </summary>
    public class AppleOptions
    {
        private string _appleStoreApiUrl;

        /// <summary>Apple store url to verify of in-app subscriptions</summary>
        public string AppleStoreApiUrl
        {
            get => _appleStoreApiUrl;
            set => _appleStoreApiUrl = value?.TrimEnd('/');
        }

        /// <summary>Apple store api secret value to verify of in-app subscriptions</summary>
        public string AppleStoreApiSecret { get; set; }
    }
}