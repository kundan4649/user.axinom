namespace z5.ms.common.validation.authproviders
{
    /// <summary>Authenticates notifications from adyen</summary>
    public class AdyenNotificationAuthProvider : BasicAuthProvider
    {
        /// <inheritdoc />
        public AdyenNotificationAuthProvider() : base("AdyenApiUsername", "AdyenApiPassword")
        {
        }
    }
}
