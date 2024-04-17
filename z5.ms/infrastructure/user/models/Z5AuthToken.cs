using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using z5.ms.common.validation;

namespace z5.ms.infrastructure.user.models
{
    /// <summary>
    /// Authentication token model Z5 specific token(included subscriptions for that specific user)
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Z5AuthToken : AuthToken
    {
        /// <inheritdoc />
        public Z5AuthToken(AuthToken data, JArray subscriptions, string currentCountry)
        {
            UserId = data.UserId;
            System = data.System;
            UserEmail = data.UserEmail;
            UserMobile = data.UserMobile;
            ActivationDate = data.ActivationDate;
            CreatedDate = data.CreatedDate;
            RegistrationCountry = data.RegistrationCountry;
            Subscriptions = subscriptions;
            CurrentCountry = currentCountry;
        }

        /// <summary>Subscriptions of the user</summary>
        [JsonProperty("subscriptions", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public JArray Subscriptions { get; set; }

        /// <summary>Subscriptions of the user</summary>
        [JsonProperty("current_country", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string CurrentCountry { get; set; }
    }
}
