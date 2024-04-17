using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using z5.ms.common;
using z5.ms.common.assets.common;

namespace z5.ms.domain.catalog.file
{
    /// <inheritdoc cref="EntityBase" />
    /// <summary>FileAsset published model</summary>
    /// <remarks>FileAsset price is either VoD price group of price of paid content section (set by CMS)</remarks>
    [JsonObject(MemberSerialization.OptIn, Title = "asset")]
    public class FileEntity : EntityBase, IAggregateRoot
    {
        /// <summary>Age rating for parental ratings</summary>
        [JsonProperty("age_rating", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string AgeRating { get; set; }

        /// <summary>Country of origin</summary>
        [JsonProperty("countries", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Countries { get; set; }

        /// <summary>Release date</summary>
        [JsonProperty("released", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? ReleaseDate { get; set; }

        /// <summary>Relative asset URL</summary>
        [JsonProperty("url", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Url { get; set; }
    }
}