using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using z5.ms.common.extensions;
using z5.ms.common.infrastructure.assetcache;

namespace z5.ms.common.assets.common
{
    /// <inheritdoc cref="IAsset" />
    /// <inheritdoc cref="IFilterable" />
    /// <summary>Asset list item, displayed in listings</summary>
    /// <remarks>Entities are filterable and searchable</remarks>
    [JsonObject(MemberSerialization.OptIn, Title = "asset")]
    public class EntityBase : IAsset
    {
        private JRaw _extended;
        private JRaw _image;

        /// <inheritdoc />
        [JsonProperty("id", Required = Required.Always, NullValueHandling = NullValueHandling.Ignore)]
        [Required]
        public virtual string Id { get; set; }

        /// <inheritdoc />
        [JsonProperty("asset_type", Required = Required.Always, NullValueHandling = NullValueHandling.Ignore)]
        public virtual int Type { get; set; }

        /// <summary>Sub type of the asset, e.g. "Movie" or "Music" for movie assets</summary>
        [JsonProperty("asset_subtype", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string AssetSubtype { get; set; }

        /// <summary>Human readable title for this item</summary>
        [JsonProperty("title", Required = Required.Always)]
        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string Title { get; set; }

        /// <summary>Original title, not translated</summary>
        [JsonProperty("original_title", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        [StringLength(50, MinimumLength = 1)]
        public string OriginalTitle { get; set; }

        /// <summary>Asset business type, e.g. Free, Ad</summary>
        /// <remarks>Items with business_type=Ad require client app to display ad to the customer before wathing the movie</remarks>
        [JsonProperty("business_type", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(StringEnumConverter))]
        public BusinessType? BusinessType { get; set; }

        /// <summary>The playback duration of this item in seconds if available.</summary>
        [JsonProperty("duration", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public int? Duration { get; set; }

        /// <summary>The genres of this item if available</summary>
        [JsonProperty("genres", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public List<Genre> Genres { get; set; }
        
        /// <summary>Asset tags</summary>
        [JsonProperty("tags", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Tags { get; set; }
        
        /// <summary>Asset description</summary>
        [JsonProperty("description", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }
        
        /// <summary>Asset short description</summary>
        [JsonProperty("short_description", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string ShortDescription { get; set; }

        /// <summary>List of assigned asset images</summary>
        [JsonProperty("image", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public JRaw Image
        {
            get => _image;
            set
            {
                _image = value;
                ImageProperties = value == null ? null : JsonConvert.DeserializeObject<ImageProperties>(value.ToString());
            }
        }

        /// <summary>Filterable image properties. A subset of key/value pairs are deserialized for filtering / output mapping</summary>
        public ImageProperties ImageProperties { get; set; }

        /// <summary>Licensing details</summary>
        [JsonProperty("licensing", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public Licensing Licensing { get; set; }
        
        /// <summary>List of extended properties (key/value pairs) for an asset. These properties are not used by MS and are simply passed through to the response models</summary>
        [JsonProperty("extended", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public JRaw Extended
        {
            get => _extended;
            set
            {
                _extended = value;
                ExtendedProperties = value == null ? null : JsonConvert.DeserializeObject<ExtendedProperties>(value.ToString());
            }
        }

        /// <summary>Filterable extended properties. A subset of key/value pairs are deserialized for filtering</summary>
        public ExtendedProperties ExtendedProperties { get; set; }
        
        /// <summary>If the viewer can skip for episodes in this season the intro or recap</summary>
        [JsonProperty("skip_available", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public bool SkipAvailable { get; set; }

        /// <summary>The start position of the episodes intro in the format 'HH:MM:SS'</summary>
        [JsonProperty("intro_start", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string IntroStart { get; set; }
       
        /// <summary>The end position of the episodes intro in the format 'HH:MM:SS'</summary>
        [JsonProperty("intro_end", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string IntroEnd { get; set; }
       
        /// <summary>The start position of the episodes recap in the format 'HH:MM:SS'</summary>
        [JsonProperty("recap_start", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string RecapStart { get; set; }
       
        /// <summary>The end position of the episodes recap in the format 'HH:MM:SS'</summary>
        [JsonProperty("recap_end", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string RecapEnd { get; set; }

        /// <summary>The start position of the end credits in the format 'HH:MM:SS'</summary>
        [JsonProperty("end_credits_start", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string EndCreditsStart { get; set; }

        /// <summary>Asset id matches an id from specified IDs list</summary>
        public bool HasListedId(IList<string> ids)
            => ids == null || ids.IsNullOrEmpty() || ids.Any(Id.Equals);
        
        /// <summary>Asset has one of genres that were specified in a parameters list</summary>
        public bool HasAnyGenre(IList<string> genreIds)
            => genreIds == null || genreIds.IsNullOrEmpty() || Genres != null && Genres.Select(g => g.Id).Intersects(genreIds);

        /// <summary>Asset has one of tags that were specified in a parameters list</summary>
        public bool HasAnyTag(IList<string> tagIds)
            => tagIds == null || tagIds.IsNullOrEmpty() || Tags != null && Tags.Intersects(tagIds);

        /// <summary>Asset has valid subtype</summary>
        public bool HasValidSubtype(string subtype)
            =>  string.IsNullOrEmpty(subtype) ||  AssetSubtype != null && AssetSubtype.Equals(subtype, StringComparison.OrdinalIgnoreCase);

        /// <summary>Check if asset title conains a search term</summary>
        public bool TitleContains(string searchTerm)
            => Title.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) > -1;

        /// <summary>Check if asset title starts with a search term</summary>
        public bool TitleStartsWith(string searchTerm)
            => Title.StartsWith(searchTerm, StringComparison.OrdinalIgnoreCase);

        /// <summary>Checks if access to an asset is allowed according to the license terms</summary>
        /// <remarks>If country is not provided, then license will be checked for any country</remarks>
        public bool LicenseIsValid(string country, DateTime queryTime)
            => string.IsNullOrEmpty(country)
                ? Licensing?.IsAllowed(queryTime) ?? true 
                : Licensing?.IsAllowed(country, queryTime) ?? true;

        /// <summary>Asset is licensed in a specified country</summary>
        /// <remarks>Returns true if country is not provided</remarks>
        public bool HasListedCountry(string country)
            => string.IsNullOrEmpty(country) || (Licensing?.IsAllowed(country) ?? true);
        
        /// <inheritdoc />
        public virtual bool ApplyFilter<TFilter>(TFilter filter) where TFilter : IQueryFilter 
            => true;
        
        /// <inheritdoc />
        public override string ToString() => $"[Item {Id}]";
    }

    /// <summary>String extensions for entities</summary>
    public static class StringExtensions {

        /// <summary>Extension method to check string contains a substring</summary>
        public static bool ContainsString(this string s, string substring, bool caseSensitive = false)
        {
            if (!string.IsNullOrWhiteSpace(s) && !string.IsNullOrWhiteSpace(substring))
                return s.IndexOf(substring, caseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase) > -1;
            
            return false;
        } 
    }

    /// <summary>List extensions for entities</summary>
    public static class ListExtensions
    {
        /// <summary>Extension method to check list of string contains a substring in any item before colon character</summary>
        public static bool ContainsAnyBeforeColon(this List<string> l, string text)
        {
            return l?.Any(a => a.Split(':')[0]?.IndexOf(text, StringComparison.OrdinalIgnoreCase) > -1) ?? false;
        }
    }
}