namespace z5.ms.domain.subscription.payment.paymentproviders
{
    /// <summary>Configuration options for billdesk payment provider</summary>
    public class BilldeskOptions
    {
        private string _billdeskCallbackUrl;
        private string _billdeskRecurringCallbackUrl;

        /// <summary>Subscription service endpoint to get callback requests from Billdesk</summary>
        public string BilldeskCallbackUrl
        {
            get => _billdeskCallbackUrl;
            set => _billdeskCallbackUrl = value?.TrimEnd('/');
        }

        /// <summary>Url to collect recurring callback requests from Billdesk</summary>
        public string BilldeskRecurringCallbackUrl
        {
            get => _billdeskRecurringCallbackUrl;
            set => _billdeskRecurringCallbackUrl = value?.TrimEnd('/');
        }

        /// <summary>Billdesk Merchant Id to include it when generating request tokens</summary>
        public string BilldeskMerchantId { get; set; }

        /// <summary>Billdesk Security Id to include it when generating request tokens</summary>
        public string BilldeskSecurityId { get; set; }

        /// <summary>Billdesk secret key to sign and verify request tokens</summary>
        public string BilldeskSecretKey { get; set; }

        /// <summary>Switch to enable/disable billdesk request signature validation</summary>
        public bool BilldeskUseMockValidation { get; set; }
    }
}