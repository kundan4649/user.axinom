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
    /// <summary>Base command to modify user's watchlist</summary>
    public class WatchhistoryCommandBase : IRequest<Result<Success>>
    {
        /// <summary>User Id</summary>
        [JsonIgnore]
        public Guid? UserId { get; set; }
        
        /// <summary>Catalog item</summary>
        [JsonProperty("catalog_item", Required = Required.Always)]
        [Required]
        public CatalogItem Item { set; get; }
    }

    /// <summary>Validator for commands that modify user's watchlist</summary>
    public class WatchhistoryCommandBaseValidator<TCommand> : AbstractValidator<TCommand> where TCommand : WatchhistoryCommandBase
    {
        /// <inheritdoc />
        public WatchhistoryCommandBaseValidator(IOptions<UserErrors> errors)
        {
            RuleFor(c => c.UserId).Must(id => id.HasValue).WithMessage(errors.Value.MissingUserId.Message);
            
            RuleFor(c => c.Item).Must(item => item != null).WithMessage(errors.Value.ItemNotFound.Message);
            
            RuleFor(c => c.Item).Must(item => item == null || item.AssetId.IsAssetId()).WithMessage(errors.Value.InvalidAssetId.Message);

            RuleFor(c => c.Item).Must(item => item == null || !item.AssetId.IsAssetId() || item.AssetId.IsAssetOfType((int) item.AssetType))
                .WithMessage(errors.Value.InvalidAssetType.Message);
        }
    }
}