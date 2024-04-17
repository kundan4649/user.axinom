using System;
using System.ComponentModel.DataAnnotations;
using FluentValidation;
using MediatR;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using z5.ms.common;
using z5.ms.common.abstractions;
using z5.ms.domain.entitlement4.viewmodel;

namespace z5.ms.domain.entitlement4.common
{
    /// <summary>
    /// Base class for the different entitlement queries
    /// </summary>
    public class GetEntitlementQuery : IRequest<Result<EntitlementResponse>>
    {
        /// <inheritdoc />
        public GetEntitlementQuery(EntitlementRequest entitlementRequest, Guid userId, string clientIp)
        {
            AssetId = entitlementRequest.AssetId;
            RequestType = entitlementRequest.RequestType;
            UserId = userId;
            Token = entitlementRequest.Token;
            KeyId = entitlementRequest.KeyId;
            Persistent = entitlementRequest.Persistent;
            Country = entitlementRequest.Country;
            DeviceId = entitlementRequest.DeviceId;
            ClientIp = clientIp;
        }

        /// <summary>IP of the requesting client</summary>
        [JsonProperty("client_ip", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string ClientIp { get; set; }

        /// <summary>ID of the queried asset</summary>
        [JsonProperty("asset_id", Required = Required.Always)]
        [Required]
        public string AssetId { get; set; }
    
        /// <summary>Type of the query:
        ///  * `check` - just a check to detremine if a user is entitled (no token generation)
        ///  * `drm` - request a DRM token
        ///  * `cdn` - request all available CDN tokens
        /// </summary>
        [JsonProperty("request_type", Required = Required.Always)]
        [Required]
        [JsonConverter(typeof(StringEnumConverter))]
        public EntitlementRequestType RequestType { get; set; }

        /// <summary>Id of the user which extracted from user auth jwt token</summary>
        [JsonIgnore]
        public Guid UserId { get; set; }

        /// <summary>The token to use for entitlement check, it may be a user token (internal subscription/purchase), Google Playstore token or an Apple App Store token</summary>
        [JsonProperty("token", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Token { get; set; }

        /// <summary>The content key ID of the protected video stream</summary>
        [JsonProperty("key_id", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string KeyId { get; set; }
    
        /// <summary>If the entitlement message should contain the flag to make the DRM license a persistent one.</summary>
        [JsonProperty("persistent", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public bool Persistent { get; set; }

        /// <summary>The current country of the user. Required for playback of premium content with licensing restrictions.</summary>
        [JsonProperty("country", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string Country { get; set; }

        /// <summary>The unique device ID of the current device of the user. Required for playback of premium content.</summary>
        [JsonProperty("device_id", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string DeviceId { get; set; }
    }

    /// <summary>
    /// Base class for entitlement query validators
    /// </summary>
    public class GetEntitlementQueryValidator<TQuery> : AbstractValidator<TQuery> where TQuery : GetEntitlementQuery
    {
        /// <inheritdoc />
        public GetEntitlementQueryValidator()
        {
            RuleFor(q => q.AssetId).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .Must(id => id.IsAssetId()).WithMessage("Please specify a valid asset ID");
            RuleFor(q => q.KeyId).NotEmpty()
                .When(q => q.RequestType == EntitlementRequestType.Drm)
                .WithMessage("You must provide key_id when requesting a DRM token");
            RuleFor(q => q.ClientIp).NotEmpty()
                .When(q => q.RequestType == EntitlementRequestType.Cdn)
                .WithMessage("You must provide a client IP when requesting a CDN token");
        }
    }
}