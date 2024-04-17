using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace z5.ms.domain.user.viewmodels
{
    /// <summary>PartnerSettingsMaster item view model</summary>
    public class PartnerSettingsMasterItem
    {
        /// <summary>The unique ID of the PartnerSettingsMaster item</summary>
        [JsonProperty("Id", Required = Required.Always)]
        public Guid Id { get; set; }

        /// <summary>Partner Id of the request</summary>
        [JsonProperty("PartnerId", Required = Required.Default)]
        public string PartnerId { get; set; }

        /// <summary>Partner name of the request</summary>
        [JsonProperty("PartnerName", Required = Required.Always)]
        public string PartnerName { get; set; }

        /// <summary>Json content of the specific partner</summary>
        [JsonProperty("Json", Required = Required.Always)]
        public string Json { get; set; }
    }
}