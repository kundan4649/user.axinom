using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace z5.ms.domain.user.viewmodels
{
    /// <summary>AccessValidationByIPItem item view model</summary>
    public class AccessValidationByIPItem
    {
        /// <summary>The unique ID of the AccessValidationByIP item</summary>
        [JsonProperty("Id", Required = Required.Always)]
        public Guid Id { get; set; }

        /// <summary>Source ip address of the request</summary>
        [JsonProperty("IpAddress", Required = Required.Always)]
        public string IpAddress { get; set; }

        /// <summary>Requested end point</summary>
        [JsonProperty("RequestEndPoint", Required = Required.Always)]
        public string RequestEndPoint { get; set; }

        /// <summary>Number of hits received from the Ip for the end point</summary>
        [JsonProperty("RequestCount", Required = Required.Default)]
        public int RequestCount { get; set; }

        /// <summary>Date on which last request is received from the Ip for the end point</summary>
        [JsonProperty("DateCreated", Required = Required.Default)]
        public DateTime DateCreated { get; set; }

        /// <summary>Date on which last request is received from the Ip for the end point</summary>
        [JsonProperty("LastRequestReceivedDate", Required = Required.Default)]
        public DateTime LastRequestReceivedDate { get; set; }

        /// <summary>Date on which last request is received from the Ip for the end point</summary>
        [JsonProperty("RequestBlockedDate", Required = Required.Default)]
        public DateTime? RequestBlockedDate { get; set; }
    }
}