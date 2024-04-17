using System.Collections.Generic;
using FluentValidation;
using MediatR;
using Newtonsoft.Json;
using z5.ms.common;
using z5.ms.common.abstractions;
using z5.ms.common.assets.common;
using z5.ms.domain.entitlement4.viewmodel;

namespace z5.ms.domain.entitlement4.common
{
    /// <summary>Base class for the collection entitlement queries </summary>
    public class GetCollectionEntitlementQuery : IRequest<Result<List<CollectionEntitlementItem>>>
    {
        /// <inheritdoc />
        public GetCollectionEntitlementQuery(EntitlementRequest request, string clientIp)
        {
            ClientIp = clientIp;
            EntitlementRequest = request;
        }
        
        /// <summary>IP of the requesting client</summary>
        [JsonProperty("client_ip", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public string ClientIp { get; set; }

        /// <summary>The entitlement request object</summary>
        public EntitlementRequest EntitlementRequest { get; set; }
    }
    
    /// <summary>
    /// Base class for entitlement query validators
    /// </summary>
    public class GetCollectionEntitlementQueryValidator<TQuery> : AbstractValidator<TQuery> where TQuery : GetCollectionEntitlementQuery
    {
        /// <inheritdoc />
        public GetCollectionEntitlementQueryValidator()
        {
            RuleFor(q => q.EntitlementRequest.AssetId).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .Must(id => id.IsAssetId()).WithMessage("Please specify a valid asset ID");
            RuleFor(q => q.EntitlementRequest.AssetId.GetAssetTypeOrDefault(99)).Must(type => (short) type == (short) AssetType.Collection)
                .WithName("asset_id").WithMessage("Only Collection asset type supported");
        }
    }
}