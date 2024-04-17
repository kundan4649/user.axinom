using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace z5.ms.common.infrastructure.assetcache
{
    /// <inheritdoc />
    /// <summary>A wrapper that contains all asset translations</summary>
    [JsonObject(MemberSerialization.OptIn, Title = "asset")]
    public class CacheEntry : IAsset
    {
        /// <inheritdoc />
        [JsonProperty("id", Required = Required.Always, NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }

        /// <inheritdoc />
        [JsonProperty("type", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public int Type { get; set; }

        /// <inheritdoc />
        [JsonProperty("asset_subtype", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string AssetSubtype { get; set; }

        /// <inheritdoc />
        [JsonProperty("title", Required = Required.Always)]
        public string Title { get; set; }

        // TODO: 1) 4-letter code should be replaced with 2-letter code 2) this should probably be configurable. 
        /// <summary>Default asset language, fixed to ru-RU</summary>
        private static string NeutralLanguage => "en-US";

        /// <summary>A wrapper that contains all asset translations</summary>
        private IDictionary<string, IAsset> Translations { get; }

        /// <summary>Default CacheEntry constructor that accepts asset and asset translations as parameters</summary>
        public CacheEntry(IAsset asset, IDictionary<string, IAsset> translations)
        {
            Id = asset.Id;
            Type = asset.Type;
            AssetSubtype = asset.AssetSubtype;
            Title = asset.Title;

            //TODO: if translations and neutral language code are in different case, addition will happen (case sensitive)
            //TODO: and subsequent ToDictionary will crash with exception
            if (!translations.ContainsKey(NeutralLanguage))
                translations.Add(NeutralLanguage, asset);

            Translations = translations.ToDictionary(k => k.Key, v => v.Value, StringComparer.OrdinalIgnoreCase); //.ToImmutableDictionary();
        }

        /// <summary>Get asset translation</summary>
        /// <param name="language">Optional translation language code</param>
        /// <returns>Selected or neutral asset translation</returns>
        public IAsset GetTranslation(string language = null)
        {
            if (String.IsNullOrEmpty(language))
                return Translations.ContainsKey(NeutralLanguage)
                    ? Translations[NeutralLanguage]
                    : null;

            if (Translations.ContainsKey(language))
                return Translations[language];

            return language != NeutralLanguage && Translations.ContainsKey(NeutralLanguage)
                ? Translations[NeutralLanguage]
                : null;
        }

        /// <inheritdoc />
        public override string ToString() => $"CacheEntry {Id} - {Type} - {Translations?.Count}";

        // TODO: review this, currently it's here only because of collection filtering
        /// <inheritdoc />
        public bool ApplyFilter<TFilter>(TFilter filter) where TFilter : IQueryFilter 
            => true;
    }
}