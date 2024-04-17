using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace z5.ms.domain.subscription.viewmodel
{
    /// <summary>Request to register a donation</summary>
    public class RegisterDonation
    {
        /// <summary>The ID of the donation plan to use for the payment</summary>
        [JsonProperty("donation_plan_id")]
        [Required(ErrorMessage = "donation_plan_id is required")]
        public string DonationPlanId { get; set; }

        /// <summary>The amount of money that was paid</summary>
        [JsonProperty("amount")]
        [Required(ErrorMessage = "amount is required")]
        [Range(1, int.MaxValue, ErrorMessage = "amount has a minimum value of 1")]
        public double Amount { get; set; }

        /// <summary>The UI language as two letter "ISO 639-1" code. Default is English (en).</summary>
        [JsonProperty("language")]
        [StringLength(2, MinimumLength = 2, ErrorMessage = "language must be 2 characters")]
        public string Language { get; set; }

    }
}
