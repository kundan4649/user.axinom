using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using z5.ms.common.abstractions;
using z5.ms.common.extensions;
using z5.ms.domain.subscription.datamodel;
using z5.ms.domain.subscription.viewmodel;

namespace z5.ms.domain.subscription.payment.paymentproviders
{
    /// <summary> Payment provider specifications </summary>
    public class PaymentProvider
    {
        /// <summary> Name (id) of the payment provider including the subprovider </summary>
        [JsonProperty("name", Required = Required.Always)]
        public string Name { get; set; }

        /// <summary> Name of the payment provider </summary>
        [JsonProperty("provider", Required = Required.Always)]
        public string Provider { get; set; }

        /// <summary> Description about the payment provider </summary>
        [JsonProperty("description", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }

        /// <summary> Indicates who implemented and handles this payment provider </summary>
        [JsonProperty("implementer", Required = Required.Always)]
        public string Implementer { get; set; }

        /// <summary> Capabilites of the payment provider </summary>
        [JsonProperty("capabilities", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public PaymentProviderCapabilities Capabilities { get; set; }

        /// <summary> Dictionary of notification options of the payment provider by notification template </summary>
        [JsonProperty("notifications", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public IDictionary<string, PaymentProviderNotification> Notifications { get; set; }

        /// <summary> Custom settings for a specific payment provider </summary>
        [JsonProperty("settings", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public JObject Settings { get; set; }

        /// <summary> Do refund actions for the payment provider</summary>
        /// <remarks>Default method for making refunds. This need to be assigned from the payment provider if specific actions needed. </remarks>
        [JsonIgnore]
        public MakeRefundDelegate MakeRefund { get; set; } = async (PaymentEntity payment, MakeRefund request) =>
        {
            await Task.CompletedTask;
            return Result<RefundEntity>.FromError(1, "Payment provider doesn't support refunds");
        };

        /// <summary> Do cancelation actions for the payment provider</summary>
        /// <remarks>Default method for subscription cancelation. This need to be assigned from the payment provider if specific actions needed. </remarks>
        [JsonIgnore]
        public CancelSubscriptionDelegate CancelSubscription { get; set; } = async (Guid subscriptionId) =>
        {
            await Task.CompletedTask;
            return new Result<Success>();
        };

        /// <summary> Check if specified notification is allowed or not to send </summary>
        public bool IsNotificationAllowed(string name, string country)
        {
            if (!Notifications.TryGetValue(name, out var notification))
                return true;

            if (notification.CountriesInclude?.Any(a => a.EqualsIgnoreCase("ALL")) ?? true)
                return !notification.CountriesExclude?.Any(a => a.EqualsIgnoreCase(country)) ?? true;

            if (notification.CountriesExclude?.Any(a => a.EqualsIgnoreCase("ALL")) ?? true)
                return notification.CountriesInclude.Any(a => a.EqualsIgnoreCase(country));

            return false;
        }

        /// <summary> Get the settings as a custom object </summary>
        public T SettingsAs<T>()
        {
            try
            {
                return Settings.ToObject<T>();
            }
            catch
            {
                throw new NotFoundException("Payment provider settings not found");
            }
        }
    }

    /// <summary> Delegate definition to make refunds for specific payment provider </summary>
    public delegate Task<Result<RefundEntity>> MakeRefundDelegate(PaymentEntity payment, MakeRefund request);

    /// <summary> Delegate definition to make refunds for specific payment provider </summary>
    public delegate Task<Result<Success>> CancelSubscriptionDelegate(Guid subscriptionId);

    /// <summary> Capabilites of the payment provider </summary>
    public class PaymentProviderCapabilities
    {
        /// <summary> Indicates the payment provider supports free trial period </summary>
        [JsonProperty("free_trial", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public bool FreeTrial { get; set; }

        /// <summary> Indicates the payment provider supports recurring billing </summary>
        [JsonProperty("renewal", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public bool Renewal { get; set; }

        /// <summary> Indicates the payment provider supports discounted payments</summary>
        [JsonProperty("promotion", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public bool Promotion { get; set; }

        /// <summary> Indicates the payment provider supports refunds</summary>
        [JsonProperty("refund", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public bool Refund { get; set; }
    }

    /// <summary> Notification options of the payment provider for a specific notification template </summary>
    public class PaymentProviderNotification
    {
        /// <summary> Countries where the specified notification is allowed to be sent </summary>
        [JsonProperty("countries_include", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public List<string> CountriesInclude { get; set; }

        /// <summary> Countries where the specified notification is NOT allowed to be sent </summary>
        [JsonProperty("countries_exclude", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public List<string> CountriesExclude { get; set; }
    }

    /// <summary> Payment provider names enum definition </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum PaymentProviderEnum
    {
        /// <summary>Adyen</summary>
        [EnumMember(Value = "adyen")]
        Adyen,

        /// <summary>Billdesk</summary>
        [EnumMember(Value = "billdesk")]
        Billdesk,

        /// <summary>Fortumo</summary>
        [EnumMember(Value = "fortumo")]
        Fortumo,

        /// <summary>Google</summary>
        [EnumMember(Value = "google")]
        Google,

        /// <summary>Apple</summary>
        [EnumMember(Value = "apple")]
        Apple,

        /// <summary>Paytm</summary>
        [EnumMember(Value = "paytm")]
        Paytm,

        /// <summary>Dialog</summary>
        [EnumMember(Value = "dialog")]
        Dialog,

        /// <summary>Dummy Payments</summary>
        [EnumMember(Value = "dummypayments")]
        DummyPayments,

        /// <summary>Crm</summary>
        [EnumMember(Value = "crm")]
        Crm,

        /// <summary>Mife</summary>
        [EnumMember(Value = "mife")]
        Mife
    }
}
