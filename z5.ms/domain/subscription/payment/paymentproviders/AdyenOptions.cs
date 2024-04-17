namespace z5.ms.domain.subscription.payment.paymentproviders
{
    /// <summary>Configuration options for adyen payment provider</summary>
    public class AdyenOptions
    {
        private string _adyenRedirectUrl;
        private string _adyenApiUrl;

        /// <summary>Adyen skin code. This is encoded in the token generated when preparing a subscription/purchase/donation</summary>
        public string AdyenSkinCode { get; set; }

        /// <summary>Adyen skin code. This skin is used for free trial payments that require cancellation</summary>
        public string AdyenFreeTrialSkinCode { get; set; }

        /// <summary>Adyen api payment endpoint</summary>
        public string AdyenApiUrl
        {
            get => _adyenApiUrl;
            set => _adyenApiUrl = value?.TrimEnd('/');
        }

        /// <summary>Adyen api account username used to confirm / cancel payments</summary>
        public string AdyenApiUsername { get; set; }

        /// <summary>Adyen api account password used to confirm / cancel payments</summary>
        public string AdyenApiPassword { get; set; }

        /// <summary>Locale used for adyen payment pages. Format: ll_CC where ll = language code and CC = country code. Leave blank to use default.</summary>
        public string AdyenShopperLocale { get; set; }

        /// <summary>After payment completion, the user will be redirected to this page. Parameters outlining the result will be added</summary>
        public string AdyenRedirectUrl
        {
            get => _adyenRedirectUrl;
            set => _adyenRedirectUrl = value?.TrimEnd('/');
        }

        /// <summary>Should pending payments be treated as complete when activating a subscription/purchase/donation</summary>
        public bool AdyenActivateOnPending { get; set; } = false;
    }

    /// <summary>Settings for different countries</summary>
    public class AdyenSettings
    {
        /// <summary>Adyen merchant account identifier. This is encoded in the token generated when preparing a subscription / purchase / donation</summary>
        public string MerchantAccount { get; set; }

        /// <summary>The minimum payment amount used by Adyen for free trial payments. Payment will be cancelled on callback</summary>
        public int MinimumPaymentAmount { get; set; }
    }
}