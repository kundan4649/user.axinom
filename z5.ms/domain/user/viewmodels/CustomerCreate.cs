using Newtonsoft.Json;
using z5.ms.domain.user.user;

namespace z5.ms.domain.user.viewmodels
{
    /// <summary>Customer creation request parameters</summary>
    public class CustomerCreate : RegisterUserCommand
    {
        /// <summary>The email address of the user</summary>
        [JsonProperty("email", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Email { get; set; }

        /// <summary>The mobile phone number of the user</summary>
        [JsonProperty("mobile", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Mobile { get; set; }

        /// <summary>The password of the user.</summary>
        [JsonProperty("password", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Password { get; set; }

        /// <summary>The first name of the user</summary>
        [JsonProperty("first_name", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string FirstName { get; set; }

        /// <summary>The first name of the user</summary>
        [JsonProperty("last_name", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string LastName { get; set; }

        /// <summary>The system of the user</summary>
        [JsonProperty("system", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string System { get; set; } = "Internal";
    }
}
