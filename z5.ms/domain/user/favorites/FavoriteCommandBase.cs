using System;
using System.ComponentModel.DataAnnotations;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using z5.ms.common;
using z5.ms.common.abstractions;
using z5.ms.domain.user.viewmodels;

namespace z5.ms.domain.user.favorites
{
    /// <summary>Base command for different favorite operations</summary>
    public class FavoriteCommandBase : IRequest<Result<Success>>
    {
        /// <summary>User Id</summary>
        [JsonProperty("user_id", Required = Required.Always)]
        [Required]
        public Guid? UserId { get; set; }
        
        /// <summary>Favorite item</summary>
        [JsonProperty("catalog_item", Required = Required.Always)]
        [Required]
        public CatalogItem Item { get; set; }
    }

    /// <summary>Validator for base favorite command operations</summary>
    public class FavoriteCommandBaseValidator<TCommand> : AbstractValidator<TCommand>
        where TCommand : FavoriteCommandBase
    {
        /// <inheritdoc />
        public FavoriteCommandBaseValidator(IOptions<UserErrors> errors)
        {
            RuleFor(c => c.UserId).Must(s => s.HasValue).WithMessage(errors.Value.MissingCustomerId.Message);

            RuleFor(c => c.Item).Must(item => item != null).WithMessage(errors.Value.FavoriteItemMissing.Message);
            
            RuleFor(c => c.Item).Must(item => item == null || item.AssetId.IsAssetId()).WithMessage(errors.Value.InvalidAssetId.Message);

            RuleFor(c => c.Item).Must(item => item == null || !item.AssetId.IsAssetId() || item.AssetId.IsAssetOfType((int) item.AssetType))
                .WithMessage(errors.Value.InvalidAssetType.Message);
        }
    }
}