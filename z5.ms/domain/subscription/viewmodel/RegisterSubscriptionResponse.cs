﻿using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace z5.ms.domain.subscription.viewmodel
{
    /// <summary>Response with details to enable payment</summary>
    public class RegisterSubscriptionResponse
    {
        /// <summary>This is the unique identifier for the registered subscription.</summary>
        [JsonProperty("subscription_id", Required = Required.Always)]
        [Required]
        public Guid SubscriptionId { get; set; }

        /// <summary>This is an optional token that might be needed in the frontend while redirecting the user to the payment provider web page.</summary>
        [JsonProperty("token", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Token { get; set; }
    }
}
