using System;
using System.Linq;
using FluentValidation;
using MediatR;
using Newtonsoft.Json;
using z5.ms.common.abstractions;
using z5.ms.common.extensions;

namespace z5.ms.domain.subscription.voucher.queries
{
    /// <summary> Request body model for generating vouchers</summary>
    public class GenerateVouchersQuery : IRequest<Result<Success>>
    {
        /// <summary> Id for vouchers batch to be generated. </summary>
        [JsonProperty("batch_id", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string BatchId { get; set; }

        /// <summary> Description for vouchers batch to be generated. </summary>
        [JsonProperty("batch_description", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string BatchDescription { get; set; }

        /// <summary> Unique id of the subscription plan. </summary>
        [JsonProperty("subscription_plan_id", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string SubscriptionPlanId { get; set; }

        /// <summary> Number of vouchers to be generated. </summary>
        [JsonProperty("num_of_vouchers", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public long NumOfVouchers { get; set; }

        /// <summary> Start date of the validity period for voucher </summary>
        [JsonProperty("valid_from", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string ValidFromDatestring { get; set; }

        /// <summary> End date of the validity period for voucher </summary>
        [JsonProperty("valid_until", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string ValidUntilDatestring { get; set; }

        /// <summary> Start date of the validity period for voucher in DateTime </summary>
        [JsonIgnore]
        public DateTime ValidFrom { get; set; }

        /// <summary> End date of the validity period for voucher in DateTime</summary>
        [JsonIgnore]
        public DateTime? ValidUntil { get; set; }

        /// <summary> Switch to turn off/on usage of uppercase letters (ascii uppercase) in generating vouchers. </summary>
        [JsonProperty("uppercase", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public bool Uppercase { get; set; }

        /// <summary> Switch to turn off/on usage of lowercase  letters (ascii lowercase) in generating vouchers. </summary>
        [JsonProperty("lowercase", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public bool Lowercase { get; set; }

        /// <summary> Switch to turn off/on usage of numbers (ascii digits) in generating vouchers. </summary>
        [JsonProperty("numbers", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public bool Numbers { get; set; }

        /// <summary> Characters that not to be used in voucher generation. </summary>
        [JsonProperty("exclude_chars", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string ExcludeChars { get; set; }

        /// <summary> The pattern to be used in generation of vouchers ("PREFIX-####-####-SUFFIX" only "#" characters will be replaced according to generation rules) </summary>
        [JsonProperty("pattern", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Pattern { get; set; }

        /// <summary> Name of the CMS user who initiated generating vouchers. </summary>
        [JsonProperty("user_name", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string UserName { get; set; }
    }

    /// <summary> GenerateVouchers query parameters validator</summary>
    public class GenerateVouchersQueryValidator : AbstractValidator<GenerateVouchersQuery>
    {
        /// <inheritdoc />
        public GenerateVouchersQueryValidator()
        {
            RuleFor(m => m.BatchId).NotEmpty();
            RuleFor(m => m.BatchId.Length).LessThanOrEqualTo(12).WithMessage("Batch Id must be less than or equal to 12 characters long");
            RuleFor(m => m.BatchDescription).Length(1, 250).When(m => !string.IsNullOrEmpty(m.BatchDescription));
            RuleFor(m => m.ExcludeChars).Length(1, 255).When(m => !string.IsNullOrEmpty(m.ExcludeChars));
            RuleFor(m => m.SubscriptionPlanId).NotEmpty();
            RuleFor(m => m.NumOfVouchers).NotEmpty().Must(BeBetween1And10000).WithMessage("'num_of_vouchers' must be between 1 and 10000");
            RuleFor(m => m.Pattern).NotEmpty().Must(BeContainSixHash).WithMessage("'pattern' must contain at least 6 '#' character");
            RuleFor(m => m.Pattern.Length).LessThanOrEqualTo(100).WithMessage("Batch Description must be less than or equal to 100 characters long");
            RuleFor(m => m.UserName).NotEmpty();
            RuleFor(m => m.UserName.Length).LessThanOrEqualTo(255).WithMessage("Username must be less than or equal to 255 characters long");
            RuleFor(m => m.Lowercase || m.Numbers || m.Uppercase).NotEmpty().WithMessage("At least one of the charsets (uppercase, lowercase, numbers) must be selected");
            RuleFor(m => m.ValidFromDatestring).Must(BeEmptyOrValidFromDate).WithMessage("Please specify a valid from date");
            RuleFor(m => m.ValidUntilDatestring).Must(BeEmptyOrValidUntilDate).WithMessage("Please specify a valid until date");
            RuleFor(m => m).Must(m => m.ValidUntil == null || m.ValidUntil > m.ValidFrom).WithMessage("End date must be greater than start date");
        }

        private static bool BeEmptyOrValidFromDate(GenerateVouchersQuery query, string dateString)
        {
            var isValidDate = false;
            if (dateString.IsNullOrEmpty())
            {
                isValidDate = true;
                query.ValidFrom = DateTime.UtcNow.Date;
            }
            else if (DateTime.TryParse(dateString, out var date))
            {
                isValidDate = true;
                query.ValidFrom = date;
            }

            return isValidDate;
        }

        private static bool BeEmptyOrValidUntilDate(GenerateVouchersQuery query, string dateString)
        {
            var isValidDate = false;
            if (dateString.IsNullOrEmpty())
            {
                isValidDate = true;
            }
            else if (DateTime.TryParse(dateString, out var date))
            {
                isValidDate = true;
                query.ValidUntil = date;
            }

            return isValidDate;
        }

        private static bool BeContainSixHash(string str)
            => (str?.Count(a => a.Equals('#')) ?? 0) >= 6;

        private static bool BeBetween1And10000(long number)
            => number > 0 && number <= 10000;
    }
}
