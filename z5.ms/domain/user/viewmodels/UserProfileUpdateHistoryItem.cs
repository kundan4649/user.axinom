using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace z5.ms.domain.user.viewmodels
{
    public class UserProfileUpdateHistoryItem
    {
        /// <summary>The unique ID of the AccessValidationByIP item</summary>
        [JsonProperty("Id", Required = Required.Always)]
        public Guid Id { get; set; }

        /// <summary>User id of the user profile</summary>
        [JsonProperty("UserId", Required = Required.Always)]
        public Guid UserId { get; set; }

        /// <summary>Email id of the user profile just before the update</summary>
        [JsonProperty("EmailId", Required = Required.Default)]
        public string EmailId { get; set; }

        /// <summary>Mobile number of the user profile just before the update</summary>
        [JsonProperty("MobileNumber", Required = Required.Default)]
        public string MobileNumber { get; set; }

        /// <summary>User profile update request payload</summary>
        [JsonProperty("RequestPayload", Required = Required.Default)]
        public string RequestPayload { get; set; }

        /// <summary>Source ip address from which the update profile request is received</summary>
        [JsonProperty("IpAddress", Required = Required.Default)]
        public string IpAddress { get; set; }

        /// <summary>Country code from which the update profile request is received</summary>
        [JsonProperty("CountryCode", Required = Required.Default)]
        public string CountryCode { get; set; }

        /// <summary>Update profile request received date</summary>
        [JsonProperty("DateCreated", Required = Required.Always)]
        public DateTime DateCreated { get; set; }

        /// <summary>Password updated for the user</summary>
        [JsonIgnore]
        public bool PasswordUpdated { get; set; }
    }
}