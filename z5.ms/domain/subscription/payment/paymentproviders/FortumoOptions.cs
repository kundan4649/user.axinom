namespace z5.ms.domain.subscription.payment.paymentproviders
{
    /// <summary>Configuration options for fortumo payment provider</summary>
    public class FortumoOptions
    {
        private string _fortumoCancelationUrl;

        /// <summary>The url to send cancelation requests to cancel recurring subscriptions from Fortumo's system</summary>
        public string FortumoCancelationUrl
        {
            get => _fortumoCancelationUrl;
            set => _fortumoCancelationUrl = value?.TrimEnd('/');
        }
    }
}