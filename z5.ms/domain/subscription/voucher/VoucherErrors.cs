using z5.ms.common.abstractions;

namespace z5.ms.domain.subscription.voucher
{
    /// <summary>Errors used by Voucher Module</summary>
    public class VoucherErrors
    {
        /// <summary>Invalid voucher code</summary>
        public Error InvalidVoucher { get; } = new Error { Code = 3150, Message = "Invalid voucher code" };

        /// <summary>Failed to generate enough vouchers with this criteria, generated batch removed.</summary>
        public Error VoucherGenerationFailed { get; } = new Error { Code = 3151, Message = "Failed to generate enough vouchers with this criteria, generated batch removed." };

        /// <summary>Payment provider doesn't support vouchers</summary>
        public Error PaymentProviderNotSupported { get; } = new Error { Code = 3152, Message = "Payment provider doesn't support vouchers" };

        /// <summary>Voucher code is expired or not activated yet</summary>
        public Error VoucherExpired { get; } = new Error { Code = 3153, Message = "Voucher code is expired or not activated yet" };

        /// <summary>Voucher code is already redeemed</summary>
        public Error VoucherRedeemed { get; } = new Error { Code = 3154, Message = "Voucher code is already redeemed" };

        /// <summary>Batch already exists</summary>
        public Error BatchExists { get; } = new Error { Code = 3155, Message = "Batch already exists" };

        /// <summary>It's not possible to generate enough vouchers with this criteria</summary>
        public Error VoucherGenerationValidationFailed { get; } = new Error { Code = 3156, Message = "It's not possible to generate enough vouchers with this criteria" };

        /// <summary>Specified subscription plan doesn't exist</summary>
        public Error SubscriptionPlanNotFound { get; } = new Error { Code = 3157, Message = "Subscription plan not found" };

    }
}
