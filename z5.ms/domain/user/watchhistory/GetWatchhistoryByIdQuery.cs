using System;
using System.ComponentModel.DataAnnotations;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using z5.ms.common;
using z5.ms.common.abstractions;
using z5.ms.domain.user.viewmodels;

namespace z5.ms.domain.user.watchhistory
{
    /// <summary>Query to get watchhistory of user filtered by asset id</summary>
    public class GetWatchhistoryByIdQuery : IRequest<Result<CatalogItem>>
    {
        /// <summary>Asset Id</summary>
        [JsonProperty("asset_id", Required = Required.Always)]
        [Required]
        public string AssetId { get; set; }
        
        /// <summary>User Id</summary>
        [JsonIgnore]
        public Guid? UserId { get; set; }
    }
    
    /// <summary>Validator for get watchhistory by id query</summary>
    public class GetWatchhistoryByIdQueryValidator : AbstractValidator<GetWatchhistoryByIdQuery>
    {
        /// <inheritdoc />
        public GetWatchhistoryByIdQueryValidator (IOptions<UserErrors> errors)
        {
            RuleFor(c => c.UserId).Must(id => id.HasValue).WithMessage(errors.Value.MissingUserId.Message);
            
            RuleFor(c => c.AssetId).Must(id => id.IsAssetId()).WithMessage(errors.Value.InvalidAssetId.Message);
        }
    }
}